using OpenNLP.Tools.Parser;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using static WatsonAI.Patterns;

namespace WatsonAI
{
  public class DobjNoun : IEntityMatcher
  {
    private readonly CommonPatterns cp;
    private readonly KnowledgeQuery query;
    private readonly Associations associations;

    private IEnumerable<Entity> answers = null;
    private string response = null;

    public DobjNoun(CommonPatterns cp, KnowledgeQuery query, Associations associations)
    {
      this.cp = cp;
      this.query = query;
      this.associations = associations;
    }

    public bool MatchOn(Parse tree)
    {
      var whoQuestion = (cp.Top >= (Branch("SBARQ") > Branch("WHNP"))).Flatten();
      //Debug.WriteLineIf(whoQuestion.Match(tree).HasValue, "Who Question");
      var passiveDobjQuestion = cp.Top >= (And(Branch("SQ") > cp.SimpleVerb, Branch("SQ") > Branch("NP")));
      //Debug.WriteLineIf(passiveSubjQuestion.Match(tree).HasValue, "Active Subj Question");
      var passiveDobjWho = And(whoQuestion, passiveDobjQuestion);
      var notVerb = cp.Top >= (Branch("SQ") > Branch("VP"));


      var isPassiveDobjWho = passiveDobjWho.Match(tree).HasValue && !notVerb.Match(tree).HasValue;
      Debug.WriteLineIf(isPassiveDobjWho, "Passive Dobj WhoWhat Question");

      if (isPassiveDobjWho)
      {
        var entityPattern = (cp.Top >= (Branch("SQ") > cp.NounPhrase)).Flatten().Flatten();
        var entities = entityPattern.Match(tree).Value;

        var verbs = new List<Verb>();
        foreach (var e in entities)
        {
          var temp = "";
          var tempVerb = new Verb();
          associations.TryNameEntity(e, out temp);
          if (associations.TryGetVerb(temp, out tempVerb))
          {
            verbs.Add(tempVerb);
          }
        }
        answers = GenerateAnswers(entities.Distinct(), verbs.Distinct());
        if (answers.Any())
        {
          var entity = "";
          var verb = "";
          associations.TryNameEntity(entities.First(), out entity);
          associations.TryNameVerb(verbs.First(), out verb);
          var answer = associations.UncheckedNameEntity(answers.First());
          var responseParts = new string[] { "The", entity, verb, answer };
          response = string.Join(" ", responseParts);
          Debug.WriteLine("Response: " + response);
        }
      }

      return isPassiveDobjWho && answers.Any();
    }

    public string GenerateResponse()
    {
      return response;
    }

    public IEnumerable<Entity> GetAnswers()
    {
      return answers;
    }

    private List<Entity> GenerateAnswers(IEnumerable<Entity> entities, IEnumerable<Verb> verbs)
    {
      var pairs = from e in entities
                  from v in verbs
                  select Tuple.Create(e, v);

      var answers = new List<Entity>();
      foreach (var p in pairs.Distinct())
      {
        var e = p.Item1;
        var v = p.Item2;
        answers.AddRange(query.GetDobjAnswers(v, e));
      }
      return answers;
    }
  }
}
