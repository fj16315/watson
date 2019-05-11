using OpenNLP.Tools.Parser;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using static WatsonAI.Patterns;

namespace WatsonAI
{
  public class ActiveSubjWho : IEntityMatcher
  {
    private readonly CommonPatterns cp;
    private readonly KnowledgeQuery query;
    private readonly Associations associations;

    private IEnumerable<Entity> answers = null;
    private string response = null;

    public ActiveSubjWho(CommonPatterns cp, KnowledgeQuery query, Associations associations) {
      this.cp = cp;
      this.query = query;
      this.associations = associations;
    }

    public bool MatchOn(Parse tree)
    {
      var whoQuestion = (cp.Top >= (Branch("SBARQ") > Branch("WHNP"))).Flatten();
      //Debug.WriteLineIf(whoQuestion.Match(tree).HasValue, "Who Question");
      var activeSubjQuestion = (cp.Top >= ((Branch("SQ") > (Branch("VP") > Branch("NP"))))).Flatten().Flatten();
      //Debug.WriteLineIf(activeSubjQuestion.Match(tree).HasValue, "Active Subj Question");
      var isWho = whoQuestion.Match(tree).HasValue;
      var isActive = activeSubjQuestion.Match(tree).HasValue;
      Console.WriteLine("isWho: " + isWho);
      Console.WriteLine("isActive: " + isActive);
      var activeSubjWho = And(whoQuestion, activeSubjQuestion);


      var isActiveSubjWho = activeSubjWho.Match(tree).HasValue;
      Debug.WriteLineIf(isActiveSubjWho, "Active Subj WhoWhat Question");

      if (isActiveSubjWho)
      {
        var entityPattern = (cp.Top >= (Branch("SQ") > (Branch("VP") > cp.NounPhrase))).Flatten().Flatten().Flatten();
        var entities = entityPattern.Match(tree).Value;
        foreach (var e in entities)
        {
          string answer = "";
          associations.TryNameEntity(e, out answer);
          Console.WriteLine("Entities for activesubjwho: " + answer);
        }

        var verbPattern = (cp.Top >= (Branch("SQ") > cp.VerbPhrase)).Flatten().Flatten();
        var verbs = verbPattern.Match(tree).Value;
        foreach (var v in verbs)
        {
          string answer = "";
          associations.TryNameVerb(v, out answer);
          Console.WriteLine("Verbs for activesubjwho: " + answer);
        }
        answers = GenerateAnswers(entities.Distinct(), verbs.Distinct());
        foreach (var a in answers)
        {
          string answer = "";
          associations.TryNameEntity(a, out answer);
          Console.WriteLine("Answers for activesubjwho: " + answer);
        }
        if (answers.Any())
        {
          var verbWordPattern = (cp.Top >= (Branch("SQ") > Branch("VP"))).Flatten();
          var verbWord = verbWordPattern.Match(tree).Value.First().Value;
          response = "The " + associations.UncheckedNameEntity(answers.First()) + " " + verbWord + ".";
          Debug.WriteLine("Response: " + response);
        }
      }

      return isActiveSubjWho && answers.Any();
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
