using System.Text;
using OpenNLP.Tools.Parser;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using static WatsonAI.Patterns;

namespace WatsonAI
{
  public class PassiveDobjAdj : IEntityMatcher
  {
    private readonly CommonPatterns cp;
    private readonly KnowledgeQuery query;
    private readonly Associations associations;
    private readonly Thesaurus thesaurus;

    private IEnumerable<Entity> answers = null;
    private string response = null;

    public PassiveDobjAdj(CommonPatterns cp, KnowledgeQuery query, Associations associations, Thesaurus thesaurus)
    {
      this.cp = cp;
      this.query = query;
      this.associations = associations;
      this.thesaurus = thesaurus;
    }

    public bool MatchOn(Parse tree)
    {
      var question = (cp.Top >= (Branch("S") > (Branch("SBAR")))).Flatten();
      var adjQuestion = (cp.Top >= (Branch("S") > (Branch("VP") > Branch("ADJP")))).Flatten().Flatten();
      var adjQuestionPattern = And(question, adjQuestion);
      var isAdjQuestion = adjQuestionPattern.Match(tree).HasValue;

      if (isAdjQuestion)
      {
        var entityPattern = (cp.Top >= (Branch("S") > (Branch("VP") > cp.AdjPhrase))).Flatten().Flatten().Flatten();
        var entities = entityPattern.Match(tree).Value;

        var verbPattern = (cp.Top >= (Branch("S") > cp.VerbPhrase)).Flatten().Flatten();
        var verbs = verbPattern.Match(tree).Value;
        answers = GenerateAnswers(entities.Distinct(), verbs.Distinct());
        if (answers.Any())
        {
          var verbWordPattern = (cp.Top >= (Branch("S") > (Branch("VP") > cp.SimpleVerb))).Flatten().Flatten();
          var verbWord = verbWordPattern.Match(tree).Value.First().Value;

          var entityWordPattern = (cp.Top >= (Branch("S") > (Branch("VP") > Branch("ADJP")))).Flatten().Flatten();
          var entityWord = entityWordPattern.Match(tree).Value.First().Value;

          var answer = associations.UncheckedNameEntity(answers.First());
          //var responseParts = new string[] { "the", answer };
          //response = string.Join(" ", responseParts);
          response = "the " + entityWord + " " + verbWord + " " + answer + " ";
          Debug.WriteLine("Response: " + response);
        }
      }
      return isAdjQuestion && answers.Any();
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
