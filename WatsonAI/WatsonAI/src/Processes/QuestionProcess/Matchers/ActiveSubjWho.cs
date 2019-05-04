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

    private IEnumerable<Entity> answers = null;
    private string response = null;

    public ActiveSubjWho(CommonPatterns cp, KnowledgeQuery query) {
      this.cp = cp;
      this.query = query;
    }

    public bool MatchOn(Parse tree)
    {
      //var dobjQuestionNounPhrase = (Branch("VP") > cp.NounPhrase).Flatten();
      //var dobjQuestionVerbPhrase = cp.VerbPhrase;
      //var dobjQueryN = (cp.Top >= dobjQuestionNounPhrase).Flatten();
      //var dobjQueryV = (cp.Top >= dobjQuestionVerbPhrase).Flatten();

      var subjQuestionNounPhrase = (Branch("SQ") > cp.NounPhrase).Flatten();
      var subjQuestionVerbPhrase = (Branch("SQ") > cp.VerbPhrase).Flatten();
      var subjQueryN = (cp.Top >= subjQuestionNounPhrase).Flatten();
      var subjQueryV = (cp.Top >= subjQuestionVerbPhrase).Flatten();
      var spNs = subjQueryN.Match(tree);
      var spVs = subjQueryV.Match(tree);

      //-------

      var whoQuestion = Branch("SBARQ") > Branch("WHNP");
      Debug.WriteLineIf(whoQuestion.Match(tree).HasValue, "Who Question");
      var activeSubjQuestion = (Branch("SQ") > (Branch("VP") > Branch("NP"))).Flatten();
      Debug.WriteLineIf(activeSubjQuestion.Match(tree).HasValue, "Active Subj Question");
      var activeSubjWho = cp.Top >= And(whoQuestion, activeSubjQuestion);


      var isActiveSubjWho = activeSubjWho.Match(tree).HasValue;

      if (isActiveSubjWho)
      {
        var entities = (cp.Top >= cp.NounPhrase).Match(tree).Value;
        var verbs = (cp.Top >= cp.VerbPhrase).Match(tree).Value;
        answers = GenerateAnswers(spNs.Value, spVs.Value);
      }

      Debug.WriteLineIf(isActiveSubjWho, "Active Subj WhoWhat Question");

      return isActiveSubjWho;
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
