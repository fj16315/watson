using OpenNLP.Tools.Parser;
using System.Collections.Generic;

namespace WatsonAI
{
  public class QuestionProcess : IProcess
  {
    private Parser parser;
    private KnowledgeQuery query;
    private CommonPatterns cp;

    private readonly List<IEntityMatcher> entityMatchers;
    private readonly List<IBoolMatcher> boolMatchers;
    private readonly List<IMatcher> matchers;

    /// <summary>
    /// Text engine for debuging the specified Parser.
    /// </summary>
    /// <param name="parser">The parser to use.</param>
    public QuestionProcess(Parser parser, Knowledge knowledge, Thesaurus thesaurus, Associations associations)
    {
      this.parser = parser;
      cp = new CommonPatterns(thesaurus, associations);
      query = new KnowledgeQuery(knowledge);
      entityMatchers = new List<IEntityMatcher>
      {
        new ActiveSubjWho(cp, query, associations),
        new ActiveDobjWho(cp, query, associations),
        new PassiveDobjWho(cp, query, associations,thesaurus),
        new Where(cp, query, associations, thesaurus),
        new PassiveAdj(cp, query, associations, thesaurus),
        new ActiveAdj(cp,query,associations),
        new PassiveNoun(cp,query,associations,thesaurus),
        new PassivePrep(cp, query, associations),
        new AdverbDobj(cp, query, associations),
        new PassiveDobjAdj(cp, query, associations, thesaurus),
        new DobjNoun(cp, query, associations)
      };
      boolMatchers = new List<IBoolMatcher>
      {
        new ActiveBoolean(cp, query, associations, thesaurus),
        new PassiveBoolean(cp, query, associations, thesaurus)
      };
      matchers = new List<IMatcher>();
      matchers.AddRange(entityMatchers);
      matchers.AddRange(boolMatchers);
    }

    public Stream Process(Stream stream)
    {
      List<string> remainingInput;
      if (!stream.RemainingInput(out remainingInput, Read.Peek))
      {
        return stream;
      }

      Parse tree;
      if (!parser.Parse(remainingInput, out tree)) return stream;

      foreach (var m in matchers)
      {
        if (m.MatchOn(tree))
        {
          stream.AppendOutput(m.GenerateResponse());
          return stream;

        }
      }

      return stream;
    }

    public IEnumerable<Entity> GetEntityAnswers(string input)
    {
      var answers = new List<Entity>();

      Parse tree;
      if (parser.Parse(input, out tree))
      {
        foreach (var m in entityMatchers)
        {
          if (m.MatchOn(tree))
          {
            answers.AddRange(m.GetAnswers());
          }
        }
      }
      return answers;
    }

    public bool GetBooleanAnswer(string input)
    {
      var answer = false;

      Parse tree;
      if (parser.Parse(input, out tree))
      {
        foreach (var m in boolMatchers)
        {
          if (m.MatchOn(tree))
          {
            answer = answer || m.GetAnswer();
          }
        }
      }
      return answer;
    }
  }
}
