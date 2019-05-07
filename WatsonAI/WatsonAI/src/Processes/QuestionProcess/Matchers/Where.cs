using OpenNLP.Tools.Parser;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using static WatsonAI.Patterns;

namespace WatsonAI
{
  public class Where : IEntityMatcher
  {
    private readonly CommonPatterns cp;
    private readonly KnowledgeQuery query;
    private readonly Associations associations;
    private readonly Thesaurus thesaurus;

    private IEnumerable<Entity> answers = null;
    private string response = null;

    public Where(CommonPatterns cp, KnowledgeQuery query, Associations associations, Thesaurus thesaurus) {
      this.cp = cp;
      this.query = query;
      this.associations = associations;
      this.thesaurus = thesaurus;
    }

    public bool MatchOn(Parse tree)
    {
      var whereQuestion = cp.Top >= Word(thesaurus, "where");
      //Debug.WriteLineIf(whoQuestion.Match(tree).HasValue, "Who Question");
      var question = cp.Top >= (Branch("SQ") >= Branch("VP"));
      //Debug.WriteLineIf(activeSubjQuestion.Match(tree).HasValue, "Active Subj Question");
      var whereQuestionPattern = And(whereQuestion, question);


      var isWhereQuestion = whereQuestionPattern.Match(tree).HasValue;
      Debug.WriteLineIf(isWhereQuestion, "Where Question");

      if (isWhereQuestion)
      {
        var entityPattern = (cp.Top >= (Branch("SQ") >= cp.NounPhrase)).Flatten().Flatten();
        var entities = entityPattern.Match(tree).Value;

        Verb verb;
        var containsInAssociations = associations.TryGetVerb("contain", out verb);

        if (!containsInAssociations) return false;
        answers = GenerateAnswers(entities.Distinct(), new List<Verb> { verb });
        if (answers.Any()) { 
          var answer = associations.UncheckedNameEntity(answers.First());
          var entity = associations.UncheckedNameEntity(entities.First());
          //var responseParts = new string[] { entityWord, preVerbWord, verbWord, "the", answer };
          var responseParts = new string[] { "the", entity, "is", "in", "the", answer };
          response = string.Join(" ", responseParts);
          Debug.WriteLine("Response: " + response);
        }
      }

      return isWhereQuestion;
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
