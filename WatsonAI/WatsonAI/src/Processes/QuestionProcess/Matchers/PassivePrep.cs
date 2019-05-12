using OpenNLP.Tools.Parser;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using static WatsonAI.Patterns;

namespace WatsonAI
{
  public class PassivePrep : IEntityMatcher
  {
    private readonly CommonPatterns cp;
    private readonly KnowledgeQuery query;
    private readonly Associations associations;

    private IEnumerable<Entity> answers = null;
    private string response = null;

    public PassivePrep(CommonPatterns cp, KnowledgeQuery query, Associations associations)
    {
      this.cp = cp;
      this.query = query;
      this.associations = associations;
    }

    public bool MatchOn(Parse tree)
    {
      var whoQuestion = (cp.Top >= (Branch("SBARQ") > Branch("WHNP"))).Flatten();
      //Debug.WriteLineIf(whoQuestion.Match(tree).HasValue, "Who Question");
      var passiveDobjQuestion = cp.Top >= (Branch("SQ") > (Branch("VP") > (Branch("PP") > Branch("NP")))).Flatten().Flatten();
      //Debug.WriteLineIf(passiveSubjQuestion.Match(tree).HasValue, "Active Subj Question");
      var passiveDobjWho = And(whoQuestion, passiveDobjQuestion);


      var isPassiveDobjWho = passiveDobjWho.Match(tree).HasValue;
      Debug.WriteLineIf(isPassiveDobjWho, "Passive Prep");

      if (isPassiveDobjWho)
      {
        var entityPattern = (cp.Top >= (Branch("SQ") > (Branch("VP") > (Branch("PP") > cp.NounPhrase)))).Flatten().Flatten().Flatten().Flatten();
        var entities = entityPattern.Match(tree).Value;

        var verbPattern = (cp.Top >= (Branch("SQ") > cp.VerbPhrase)).Flatten().Flatten();

        var verbs = verbPattern.Match(tree).Value;
        answers = GenerateAnswers(entities.Distinct(), verbs.Distinct());
        if (answers.Any())
        {
          var verbWordPattern = (cp.Top >= (Branch("SQ") > Branch("VP"))).Flatten();
          var restOfQuestion = verbWordPattern.Match(tree).Value.First().Value;

          var answer = associations.UncheckedNameEntity(answers.First());
          var responseParts = new string[] { "The", answer, restOfQuestion };
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
        answers.AddRange(query.GetSubjAnswers(v, e));
      }
      return answers;
    }
  }
}
