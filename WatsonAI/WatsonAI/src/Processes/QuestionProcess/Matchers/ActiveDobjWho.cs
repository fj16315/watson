using OpenNLP.Tools.Parser;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using static WatsonAI.Patterns;

namespace WatsonAI
{
  public class ActiveDobjWho : IEntityMatcher
  {
    private readonly CommonPatterns cp;
    private readonly KnowledgeQuery query;
    private readonly Associations associations;

    private IEnumerable<Entity> answers = null;
    private string response = null;

    public ActiveDobjWho(CommonPatterns cp, KnowledgeQuery query, Associations associations) {
      this.cp = cp;
      this.query = query;
      this.associations = associations;
    }

    public bool MatchOn(Parse tree)
    {
      var whoQuestion = (cp.Top >= (Branch("SBARQ") > Branch("WHNP"))).Flatten();
      //Debug.WriteLineIf(whoQuestion.Match(tree).HasValue, "Who Question");
      var activeDobjQuestion = cp.Top >= (And(Branch("SQ") > Branch("NP"), And(Branch("SQ") > cp.SimpleVerb, Branch("SQ") > Branch("VP"))));
      //Debug.WriteLineIf(activeSubjQuestion.Match(tree).HasValue, "Active Subj Question");
      var activeDobjWho = And(whoQuestion, activeDobjQuestion);


      var isActiveDobjWho = activeDobjWho.Match(tree).HasValue;
      Debug.WriteLineIf(isActiveDobjWho, "Active Dobj WhoWhat Question");

      if (isActiveDobjWho)
      {
        var entityPattern = (cp.Top >= (Branch("SQ") > cp.NounPhrase)).Flatten().Flatten();
        var entities = entityPattern.Match(tree).Value;

        var verbPattern = (cp.Top >= (Branch("SQ") > cp.VerbPhrase)).Flatten().Flatten();

        var verbs = verbPattern.Match(tree).Value;
        answers = GenerateAnswers(entities.Distinct(), verbs.Distinct());
        if (answers.Any())
        {
          var verbWordPattern = (cp.Top >= (Branch("SQ") > Branch("VP"))).Flatten();
          var verbWord = verbWordPattern.Match(tree).Value.First().Value;

          var preVerbWordPattern = (cp.Top >= (Branch("SQ") > cp.SimpleVerb)).Flatten();
          var preVerbWord = preVerbWordPattern.Match(tree).Value.First().Value;

          var entityWordPattern = (cp.Top >= (Branch("SQ") > Branch("NP"))).Flatten();
          var entityWord = entityWordPattern.Match(tree).Value.First().Value;

          var answer = associations.UncheckedNameEntity(answers.First());
          //var responseParts = new string[] { entityWord, preVerbWord, verbWord, "the", answer };
          var responseParts = new string[] { "the", answer };
          response = string.Join(" ", responseParts);
          Debug.WriteLine("Response: " + response);
        }
      }

      return isActiveDobjWho && answers.Any();
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
