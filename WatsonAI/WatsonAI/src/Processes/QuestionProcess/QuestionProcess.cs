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
        new PassiveDobjWho(cp, query, associations),
        new Where(cp, query, associations, thesaurus)
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
      if (!stream.RemainingInput(out remainingInput)) return stream;
      Parse tree;
      if (!parser.Parse(remainingInput, out tree)) return stream;

      foreach (var m in entityMatchers)
      {
        if (m.MatchOn(tree))
        {
          stream.AppendOutput(m.GenerateResponse());
          return stream;
        }
      }

      return stream;
    }

      //if (IsWhereQuestion(tree))
      //{
      //  string response;
      //  if (GenerateWhereResponse(tree, out response)) stream.AppendOutput(response);
      //}

      //if (IsPassiveDobjWhoWhatQuestion(tree))
      //{
      //  string response;
      //  if (GeneratePassiveWhoWhatResponse(tree, out response)) stream.AppendOutput(response);
      //}

      //if (IsActiveSubjWhoWhatQuestion(tree))
      //{
      //  string response;
      //  if (GenerateActiveWhoWhatResponse(tree, out response)) stream.AppendOutput(response);
      //}

      //return stream;

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

    //private IEnumerable<Entity> GetEntityAnswers(Parse tree)
    //{
    //  if (IsWhereQuestion(tree))
    //  {
    //    return GetWhereEntityAnswers(tree);
    //  }

    //  if (IsWhoOrWhatQuestion(tree))
    //  {
    //    if (IsActiveSubjWhoWhatQuestion(tree)) return GetActiveWhoWhatAnswers(tree);
    //    if (IsPassiveDobjWhoWhatQuestion(tree)) return GetPassiveWhoWhatAnswers(tree);
    //  }

    //  return Enumerable.Empty<Entity>();
    //}

    //private List<Entity> GetWhereEntityAnswers(Parse tree)
    //{
    //  var nouns = subjQueryN.Match(tree);

    //  if (nouns.HasValue && nouns.Value.Any())
    //  {
    //    var contains = associations.UncheckedGetVerb("contain");
    //    var answers = new List<Entity>();
    //    foreach (var n in nouns.Value.Distinct())
    //    {
    //      answers.AddRange(query.GetSubjAnswers(contains, n));
    //    }
    //    return answers;
    //  }
    //  return new List<Entity>();
    //}

    //private List<Entity> GetActiveWhoWhatAnswers(Parse tree)
    //{
    //  var pairs = from n in subjQueryN.Match(tree).Value
    //              from v in subjQueryV.Match(tree).Value
    //              select Tuple.Create(n, v);

    //  var answers = new List<Entity>();
    //  foreach (var p in pairs.Distinct())
    //  {
    //    var e = p.Item1;
    //    var v = p.Item2;
    //    answers.AddRange(query.GetDobjAnswers(v, e));
    //  }
    //  return answers;
    //}

    //private List<Entity> GetPassiveWhoWhatAnswers(Parse tree)
    //{
    //  var pairs = from n in dobjQueryN.Match(tree).Value
    //              from v in dobjQueryV.Match(tree).Value
    //              select Tuple.Create(n, v);

    //  var answers = new List<Entity>();
    //  foreach (var p in pairs.Distinct())
    //  {
    //    var e = p.Item1;
    //    var v = p.Item2;
    //    answers.AddRange(query.GetSubjAnswers(v, e));
    //  }
    //  return answers;
    //}

    //public bool GetBooleanAnswer(string input)
    //{
    //  Parse tree;
    //  if (parser.Parse(input, out tree))
    //  {
    //    return GetBooleanAnswer(tree);
    //  }
    //  return false; 
    //}

    //private bool GetBooleanAnswer(Parse tree)
    //{
    //  var qSubj= (top >= (Branch("SQ") > nounPhrase)).Flatten().Flatten();
    //  var qVerb = (top >= verbPhrase).Flatten();
    //  var qObj = (top >= (Branch("VP") > (Branch("NP") >= noun))).Flatten().Flatten().Flatten();

    //  if (IsYesNoQuestion(tree))
    //  {
    //    var sentenceSubj = qSubj.Match(tree);
    //    var sentenceVerb = qVerb.Match(tree);
    //    var sentenceObj = qObj.Match(tree);
    //    if (sentenceVerb.HasValue && sentenceSubj.HasValue && sentenceObj.HasValue 
    //      && sentenceVerb.Value.Any() && sentenceSubj.Value.Any() && sentenceObj.Value.Any())
    //    {
    //      return query.GetBoolAnswer(sentenceVerb.Value.First(), sentenceSubj.Value.First(), sentenceObj.Value.First());
    //    }
    //  }

    //  return false;
    //}

    //private bool IsYesNoQuestion(Parse tree)
    //{
    //  var genericVerb = Branch("VB") | Branch("VBD") | Branch("VBG") | Branch("VBN") | Branch("VBP") | Branch("VBZ");
    //  var yesNoQuestion = (top >= (Branch("SQ") > genericVerb)).Flatten();

    //  var isYesNoQuestion = yesNoQuestion.Match(tree).HasValue;
    //  Debug.WriteLineIf(isYesNoQuestion, "YesNo Question");
    //  return isYesNoQuestion;
    //}

    //private bool IsWhereQuestion(Parse tree)
    //{
    //  var whereQ = top >= Word(thesaurus, "where");

    //  var isWhereQ = whereQ.Match(tree).HasValue;
    //  Debug.WriteLineIf(isWhereQ, "Where Question");
    //  return isWhereQ;
    //}

    //private bool IsWhoOrWhatQuestion(Parse tree)
    //{
    //  var whoWhatQ = top >= (Word(thesaurus, "who") | Word(thesaurus, "what"));
    //  var isWhoWhatQ = whoWhatQ.Match(tree).HasValue;
    //  Debug.WriteLineIf(isWhoWhatQ, "WhoWhat Question");
    //  return isWhoWhatQ;
    //}

    //private bool IsActiveSubjWhoWhatQuestion(Parse tree)
    //{
    //  var spNs = subjQueryN.Match(tree);
    //  var spVs = subjQueryV.Match(tree);
    //  var isActiveWhoWhatQuestion = spNs.HasValue && spVs.HasValue && spNs.Value.Any() && spVs.Value.Any();
    //  Debug.WriteLineIf(isActiveWhoWhatQuestion, "Active WhoWhat Question");
    //  return isActiveWhoWhatQuestion;
    //}

    //private bool IsPassiveDobjWhoWhatQuestion(Parse tree)
    //{
    //  var dpNs = dobjQueryN.Match(tree);
    //  var dpVs = dobjQueryV.Match(tree);
    //  var isPassiveWhoWhatQuestion = dpNs.HasValue && dpVs.HasValue && dpNs.Value.Any() && dpVs.Value.Any();
    //  Debug.WriteLineIf(isPassiveWhoWhatQuestion, "Passive WhoWhat Question");
    //  return isPassiveWhoWhatQuestion;
    //}

    //private bool GenerateWhereResponse(Parse tree, out string response)
    //{
    //  var answers = GetWhereEntityAnswers(tree);

    //  var nouns = subjQueryN.Match(tree);
    //  if (nouns.HasValue && nouns.Value.Any())
    //  {
    //    foreach (var n in nouns.Value.Distinct())
    //    {
    //      string entityName;
    //      associations.TryNameEntity(n, out entityName);
    //      if (answers.Count != 0)
    //      {
    //        response = GenerateActiveResponse(entityName, "is in", answers);
    //        return true;
    //      }
    //    }
    //  }
    //  response = "";
    //  return false;
    //}

    //private bool GeneratePassiveWhoWhatResponse(Parse tree, out string response)
    //{
    //  var answers = GetPassiveWhoWhatAnswers(tree).ToList();

    //  var pairs = from n in dobjQueryN.Match(tree).Value
    //              from v in dobjQueryV.Match(tree).Value
    //              select Tuple.Create(n, v);

    //  foreach (var p in pairs.Distinct())
    //  {
    //    var e = p.Item1;
    //    var v = p.Item2;

    //    string verbName;
    //    associations.TryNameVerb(v, out verbName);
    //    string entityName;
    //    associations.TryNameEntity(e, out entityName);

    //    if (answers.Any())
    //    {
    //      response = GeneratePassiveResponse(entityName, verbName, answers);
    //      return true;
    //    }
    //  }
    //  response = "";
    //  return false;
    //}

    //private bool GenerateActiveWhoWhatResponse(Parse tree, out string response)
    //{
    //  var answers = GetActiveWhoWhatAnswers(tree).ToList();

    //  var pairs = from n in dobjQueryN.Match(tree).Value
    //              from v in dobjQueryV.Match(tree).Value
    //              select Tuple.Create(n, v);

    //  foreach (var p in pairs.Distinct())
    //  {
    //    var e = p.Item1;
    //    var v = p.Item2;

    //    string verbName;
    //    associations.TryNameVerb(v, out verbName);
    //    string entityName;
    //    associations.TryNameEntity(e, out entityName);
    //    if (answers.Count != 0)
    //    {
    //      response = GenerateActiveResponse(entityName, verbName, answers);
    //      return true;
    //    }
    //  }
    //  response = "";
    //  return false;
    //}

    //private string GeneratePassiveResponse(string noun, string verb, List<Entity> answers)
    //{
    //  string entityName;
    //  associations.TryNameEntity(answers.FirstOrDefault(), out entityName);
    //  var response = "The " + entityName + " " + verb;
    //  response += " the " + noun;
    //  return response;
    //}

    //private string GenerateActiveResponse(string noun, string verb, List<Entity> answers)
    //{
    //  var response = "The " + noun + " " + verb;
    //  foreach (Entity entityAnswer in answers.Distinct())
    //  {
    //    if (answers.IndexOf(entityAnswer) == 0)
    //    {
    //      string entityName;
    //      associations.TryNameEntity(entityAnswer, out entityName);
    //      response += " the " + entityName;
    //    }
    //    else
    //    {
    //      string entityName;
    //      associations.TryNameEntity(entityAnswer, out entityName);
    //      response += " and the " + entityName;
    //    }
    //  }
    //  return response;
    //}
  }

}
