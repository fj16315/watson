using OpenNLP.Tools.Parser;
using System;
using System.Collections.Generic;
using System.Linq;

using static WatsonAI.Patterns;

namespace WatsonAI
{
  public class QuestionProcess : IProcess
  {
    private Parser parser;
    private KnowledgeQuery query;
    private Knowledge knowledge;
    private Thesaurus thesaurus;
    private Associations associations;

    // Common Patterns
    private readonly Branch top;
    private readonly EntityName noun;
    private readonly VerbName verb;
    private readonly Pattern<IEnumerable<Entity>> nounPhrase;
    private readonly Pattern<IEnumerable<Verb>> verbPhrase;

    private readonly Pattern<IEnumerable<Entity>> dobjQuestionNounPhrase;
    private readonly Pattern<IEnumerable<Verb>> dobjQuestionVerbPhrase;
    private readonly Pattern<IEnumerable<Entity>> dobjQueryN;
    private readonly Pattern<IEnumerable<Verb>> dobjQueryV;

    private readonly Pattern<IEnumerable<Entity>> subjQuestionNounPhrase;
    private readonly Pattern<IEnumerable<Verb>> subjQuestionVerbPhrase;
    private readonly Pattern<IEnumerable<Entity>> subjQueryN;
    private readonly Pattern<IEnumerable<Verb>> subjQueryV;

    /// <summary>
    /// Text engine for debuging the specified Parser.
    /// </summary>
    /// <param name="parser">The parser to use.</param>
    public QuestionProcess(Parser parser, Knowledge knowledge, Thesaurus thesaurus, Associations associations)
    {
      this.parser = parser;
      this.knowledge = knowledge;
      this.thesaurus = thesaurus;
      this.associations = associations;
      this.query = new KnowledgeQuery(knowledge);

      top = Branch("TOP");
      noun = new EntityName(associations, thesaurus);
      verb = new VerbName(associations, thesaurus);
      nounPhrase = (Branch("NP") >= noun).Flatten();
      verbPhrase = (Branch("VP") >= verb).Flatten();

      dobjQuestionNounPhrase = (Branch("VP") > nounPhrase).Flatten();
      dobjQuestionVerbPhrase = verbPhrase;
      dobjQueryN = (top >= dobjQuestionNounPhrase).Flatten();
      dobjQueryV = (top >= dobjQuestionVerbPhrase).Flatten();

      subjQuestionNounPhrase = (Branch("SQ") > nounPhrase).Flatten();
      subjQuestionVerbPhrase = (Branch("SQ") > verbPhrase).Flatten();
      subjQueryN = (top >= subjQuestionNounPhrase).Flatten();
      subjQueryV = (top >= subjQuestionVerbPhrase).Flatten();
    }

    public Stream Process(Stream stream)
    {
      List<string> remainingInput;
      if (!stream.RemainingInput(out remainingInput)) return stream;
      Parse tree;
      if (!parser.Parse(remainingInput, out tree)) return stream; 

      if (IsWhereQuestion(tree))
      {
        string response;
        if (GenerateWhereResponse(tree, out response)) stream.AppendOutput(response);
      }

      if (IsPassiveWhoWhatQuestion(tree))
      {
        string response;
        if (GeneratePassiveWhoWhatResponse(tree, out response)) stream.AppendOutput(response);
      }

      if (IsActiveWhoWhatQuestion(tree))
      {
        string response;
        if (GenerateActiveWhoWhatResponse(tree, out response)) stream.AppendOutput(response);
      }

      return stream;
    }

    public IEnumerable<Entity> GetEntityAnswers(string input)
    {
      Parse tree;
      if (parser.Parse(input, out tree))
      {
        return GetEntityAnswers(tree);
      }
      return Enumerable.Empty<Entity>();
    }

    private IEnumerable<Entity> GetEntityAnswers(Parse tree)
    {
      if (IsWhereQuestion(tree))
      {
        return GetWhereEntityAnswers(tree);
      }

      if (IsWhoOrWhatQuestion(tree))
      {
        if (IsActiveWhoWhatQuestion(tree)) return GetActiveWhoWhatAnswers(tree);
        if (IsPassiveWhoWhatQuestion(tree)) return GetPassiveWhoWhatAnswers(tree);
      }

      return Enumerable.Empty<Entity>();
    }

    private List<Entity> GetWhereEntityAnswers(Parse tree)
    {
      var nouns = subjQueryN.Match(tree);

      if (nouns.HasValue && nouns.Value.Any())
      {
        var contains = associations.UncheckedGetVerb("contain");
        var answers = new List<Entity>();
        foreach (var n in nouns.Value.Distinct())
        {
          answers.AddRange(query.GetSubjAnswers(contains, n));
        }
        return answers;
      }
      return new List<Entity>();
    }

    private List<Entity> GetActiveWhoWhatAnswers(Parse tree)
    {
      var pairs = from n in subjQueryN.Match(tree).Value
                  from v in subjQueryV.Match(tree).Value
                  select Tuple.Create(n, v);

      var answers = new List<Entity>();
      foreach (var p in pairs.Distinct())
      {
        var e = p.Item1;
        var v = p.Item2;
        answers.AddRange(query.GetDobjAnswers(v, e));
      }
      return answers;
    }

    private List<Entity> GetPassiveWhoWhatAnswers(Parse tree)
    {
      var pairs = from n in dobjQueryN.Match(tree).Value
                  from v in dobjQueryV.Match(tree).Value
                  select Tuple.Create(n, v);

      var answers = new List<Entity>();
      foreach (var p in pairs.Distinct())
      {
        var e = p.Item1;
        var v = p.Item2;
        answers.AddRange(query.GetSubjAnswers(v, e));
      }
      return answers;
    }

    public bool GetBooleanAnswer(string input)
    {
      Parse tree;
      if (parser.Parse(input, out tree))
      {
        return GetBooleanAnswer(tree);
      }
      return false; 
    }

    private bool GetBooleanAnswer(Parse tree)
    {
      var qSubj= (top >= (Branch("SQ") > nounPhrase)).Flatten().Flatten();
      var qVerb = (top >= verbPhrase).Flatten();
      var qObj = (top >= (Branch("VP") > (Branch("NP") >= noun))).Flatten().Flatten().Flatten();

      if (IsYesNoQuestion(tree))
      {
        var sentenceSubj = qSubj.Match(tree);
        var sentenceVerb = qVerb.Match(tree);
        var sentenceObj = qObj.Match(tree);
        if (sentenceVerb.HasValue && sentenceSubj.HasValue && sentenceObj.HasValue 
          && sentenceVerb.Value.Any() && sentenceSubj.Value.Any() && sentenceObj.Value.Any())
        {
          return query.GetBoolAnswer(sentenceVerb.Value.First(), sentenceSubj.Value.First(), sentenceObj.Value.First());
        }
      }

      return false;
    }

    private bool IsYesNoQuestion(Parse tree)
    {
      var genericVerb = Branch("VB") | Branch("VBD") | Branch("VBG") | Branch("VBN") | Branch("VBP") | Branch("VBZ");
      var yesNoQuestion = (top >= (Branch("SQ") > genericVerb)).Flatten();
      return yesNoQuestion.Match(tree).HasValue;
    }

    private bool IsWhereQuestion(Parse tree)
    {
      var whereQ = top >= Word(thesaurus, "where");
      return whereQ.Match(tree).HasValue;
    }

    private bool IsWhoOrWhatQuestion(Parse tree)
    {
      var whoWhatQ = top >= (Word(thesaurus, "who") | Word(thesaurus, "what"));
      return whoWhatQ.Match(tree).HasValue;
    }

    private bool IsActiveWhoWhatQuestion(Parse tree)
    {
      var spNs = subjQueryN.Match(tree);
      var spVs = subjQueryV.Match(tree);
      return spNs.HasValue && spVs.HasValue && spNs.Value.Any() && spVs.Value.Any();
    }

    private bool IsPassiveWhoWhatQuestion(Parse tree)
    {
      var dpNs = dobjQueryN.Match(tree);
      var dpVs = dobjQueryV.Match(tree);
      return dpNs.HasValue && dpVs.HasValue && dpNs.Value.Any() && dpVs.Value.Any();
    }

    private bool GenerateWhereResponse(Parse tree, out string response)
    {
      var answers = GetWhereEntityAnswers(tree);

      var nouns = subjQueryN.Match(tree);
      if (nouns.HasValue && nouns.Value.Any())
      {
        foreach (var n in nouns.Value.Distinct())
        {
          string entityName;
          associations.TryNameEntity(n, out entityName);
          if (answers.Count != 0)
          {
            response = GenerateActiveResponse(entityName, "is in", answers);
            return true;
          }
        }
      }
      response = "";
      return false;
    }

    private bool GeneratePassiveWhoWhatResponse(Parse tree, out string response)
    {
      var answers = GetPassiveWhoWhatAnswers(tree).ToList();

      var pairs = from n in dobjQueryN.Match(tree).Value
                  from v in dobjQueryV.Match(tree).Value
                  select Tuple.Create(n, v);

      foreach (var p in pairs.Distinct())
      {
        var e = p.Item1;
        var v = p.Item2;

        string verbName;
        associations.TryNameVerb(v, out verbName);
        string entityName;
        associations.TryNameEntity(e, out entityName);

        if (answers.Any())
        {
          response = GeneratePassiveResponse(entityName, verbName, answers);
          return true;
        }
      }
      response = "";
      return false;
    }

    private bool GenerateActiveWhoWhatResponse(Parse tree, out string response)
    {
      var answers = GetActiveWhoWhatAnswers(tree).ToList();

      var pairs = from n in dobjQueryN.Match(tree).Value
                  from v in dobjQueryV.Match(tree).Value
                  select Tuple.Create(n, v);

      foreach (var p in pairs.Distinct())
      {
        var e = p.Item1;
        var v = p.Item2;

        string verbName;
        associations.TryNameVerb(v, out verbName);
        string entityName;
        associations.TryNameEntity(e, out entityName);
        if (answers.Count != 0)
        {
          response = GenerateActiveResponse(entityName, verbName, answers);
          return true;
        }
      }
      response = "";
      return false;
    }

    private string GeneratePassiveResponse(string noun, string verb, List<Entity> answers)
    {
      string entityName;
      associations.TryNameEntity(answers.FirstOrDefault(), out entityName);
      var response = "The " + entityName + " " + verb;
      response += " the " + noun;
      return response;
    }

    private string GenerateActiveResponse(string noun, string verb, List<Entity> answers)
    {
      var response = "The " + noun + " " + verb;
      foreach (Entity entityAnswer in answers.Distinct())
      {
        if (answers.IndexOf(entityAnswer) == 0)
        {
          string entityName;
          associations.TryNameEntity(entityAnswer, out entityName);
          response += " the " + entityName;
        }
        else
        {
          string entityName;
          associations.TryNameEntity(entityAnswer, out entityName);
          response += " and the " + entityName;
        }
      }
      return response;
    }
  }

}
