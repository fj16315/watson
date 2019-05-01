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

    /// <summary>
    /// Text engine for debuging the specified Parser.
    /// </summary>
    /// <param name="parse">The parser to use.</param>
    public QuestionProcess(Parser parse, Knowledge knowledge, Thesaurus thesaurus, Associations associations)
    {
      this.parser = parse;
      this.knowledge = knowledge;
      this.thesaurus = thesaurus;
      this.associations = associations;
      this.query = new KnowledgeQuery(knowledge);
    }

    public Stream Process(Stream stream)
    {
      List<string> remainingInput;
      if (!stream.RemainingInput(out remainingInput, Read.Peek))
      {
        return stream;
      }

      Parse tree;
      if (!parser.Parse(remainingInput, out tree))
      {
        return stream;
      }

      var noun = new EntityName(associations, thesaurus);
      var verb = new VerbName(associations, thesaurus);
      var nounPhrase = (Branch("NP") >= noun).Flatten();
      var verbPhrase = (Branch("VP") >= verb).Flatten();


      var top = Branch("TOP");

      var subjQuestionNounPhrase = (Branch("SQ") > nounPhrase).Flatten();
      var subjQuestionVerbPhrase = (Branch("SQ") > verbPhrase).Flatten();

      var subjQueryN = (top >= subjQuestionNounPhrase).Flatten();
      var subjQueryV = (top >= subjQuestionVerbPhrase).Flatten();

      var whoWhatQ = top >= (Word(thesaurus, "who") | Word(thesaurus, "what"));
      var whereQ = top >= Word(thesaurus, "where");

      if (whereQ.Match(tree).HasValue)
      {
        var nouns = subjQueryN.Match(tree);
        if (nouns.HasValue && nouns.Value.Any())
        {
          var contains = associations.UncheckedGetVerb("contain");
          foreach (var n in nouns.Value.Distinct())
          {
            var answers = query.GetSubjAnswers(contains, n);
            string entityName;
            associations.TryNameEntity(n, out entityName);
            if (answers.Count != 0)
            {
              stream.AppendOutput(GenerateActiveResponse(entityName, "is in", answers));
            }
          }
        }
      }
      if (whoWhatQ.Match(tree).HasValue)
      {
        var dobjQuestionNounPhrase = (Branch("VP") > nounPhrase).Flatten();
        var dobjQuestionVerbPhrase = verbPhrase;

        var dobjQueryN = (top >= dobjQuestionNounPhrase).Flatten();
        var dobjQueryV = (top >= dobjQuestionVerbPhrase).Flatten();

        // Deal with active case
        var spNs = subjQueryN.Match(tree);
        var spVs = subjQueryV.Match(tree);
        if (spNs.HasValue && spVs.HasValue && spNs.Value.Any() && spVs.Value.Any())
        {
          var pairs = from n in spNs.Value
                      from v in spVs.Value
                      select Tuple.Create(n, v);

          foreach (var p in pairs.Distinct())
          {
            var e = p.Item1;
            var v = p.Item2;
            var answers = query.GetDobjAnswers(v, e);
            string verbName;
            associations.TryNameVerb(v, out verbName);
            string entityName;
            associations.TryNameEntity(e, out entityName);
            if (answers.Count != 0)
            {
              stream.AppendOutput(GenerateActiveResponse(entityName, verbName, answers));
            }
          }
        }

        // Deal with passive case
        var dpNs = dobjQueryN.Match(tree);
        var dpVs = dobjQueryV.Match(tree);
        if (dpNs.HasValue && dpVs.HasValue && dpNs.Value.Any() && dpVs.Value.Any())
        {
          var pairs = from n in dpNs.Value
                      from v in dpVs.Value
                      select Tuple.Create(n, v);

          foreach (var p in pairs.Distinct())
          {
            var e = p.Item1;
            var v = p.Item2;
            var answers = query.GetSubjAnswers(v, e);
            string verbName;
            associations.TryNameVerb(v, out verbName);
            string entityName;
            associations.TryNameEntity(e, out entityName);
            if (answers.Any())
            {
              stream.AppendOutput(GeneratePassiveResponse(entityName, verbName, answers));
            }
          }
        }
      }
      return stream;
    }

    private void PrintVerbs(Stream stream)
    {
      List<string> remainingInput;
      if (!stream.RemainingInput(out remainingInput, Read.Peek))
      {
        return;
      }

      Parse tree;
      if (parser.Parse(remainingInput, out tree))
      {
        var verb = new VerbName(associations, thesaurus);
        var top = new Branch("TOP");

        var query = (top >= verb).Flatten();

        var verbs = query.Match(tree);

        Console.WriteLine("Verbs: ");
        if (verbs.HasValue)
        {
          foreach (var v in verbs.Value.Distinct())
          {
            string verbName;
            associations.TryNameVerb(v, out verbName);
            Console.WriteLine($"{v} {verbName}");
          }
        }
      }
    }
    
    private void PrintEntities(Stream stream)
    {
      List<string> remainingInput;
      if (!stream.RemainingInput(out remainingInput, Read.Peek))
      {
        return;
      }

      Parse tree;
      if (parser.Parse(remainingInput, out tree))
      {
        var entity = new EntityName(associations, thesaurus);
        var top = new Branch("TOP");

        var query = (top >= entity).Flatten();

        var entities = query.Match(tree);

        Console.WriteLine("Entities: ");
        if (entities.HasValue)
        {
          foreach (var e in entities.Value.Distinct())
          {
            string entityName;
            associations.TryNameEntity(e, out entityName);
            Console.WriteLine($"{e} {entityName}");
          }
        }
      }
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
