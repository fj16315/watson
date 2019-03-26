using System;
using System.Collections.Generic;
using System.Linq;

namespace WatsonAI
{
  public class QuestionProcess : IProcess
  {
    private Parser parser;
    private Knowledge kg;
    private Thesaurus thesaurus;
    private Associations associations;

    /// <summary>
    /// Text engine for debuging the specified Parser.
    /// </summary>
    /// <param name="parse">The parser to use.</param>
    public QuestionProcess(Parser parse, Knowledge kg, Thesaurus thesaurus, Associations associations)
    {
      this.parser = parse;
      this.kg = kg;
      this.thesaurus = thesaurus;
      this.associations = associations;
    }

    public Stream Process(Stream stream)
    {
      //PrintVerbs(stream);
      //PrintEntities(stream);

      var tree = parser.Parse(stream.RemainingInput);

      var noun = new EntityName(associations, thesaurus);
      var verb = new VerbName(associations, thesaurus);
      var nounPhrase = new Flatten<Entity>(new Descendant<IEnumerable<Entity>>(new Branch("NP"), noun));
      var verbPhrase = new Flatten<Verb>(new Descendant<IEnumerable<Verb>>(new Branch("VP"), verb));

      var top = new Branch("TOP");

      var subjQuestionNounPhrase = new Flatten<Entity>(new Children<IEnumerable<Entity>>(new Branch("SQ"), nounPhrase));
      var subjQuestionVerbPhrase = new Flatten<Verb>(new Children<IEnumerable<Verb>>(new Branch("SQ"), verbPhrase));

      var dobjQuestionNounPhrase = new Flatten<Entity>(new Children<IEnumerable<Entity>>(new Branch("VP"), nounPhrase));
      var dobjQuestionVerbPhrase = verbPhrase;

      var subjQueryN = new Flatten<Entity>(new Descendant<IEnumerable<Entity>>(top, subjQuestionNounPhrase));
      var subjQueryV = new Flatten<Verb>(new Descendant<IEnumerable<Verb>>(top, subjQuestionVerbPhrase));

      var dobjQueryN = new Flatten<Entity>(new Descendant<IEnumerable<Entity>>(top, dobjQuestionNounPhrase));
      var dobjQueryV = new Flatten<Verb>(new Descendant<IEnumerable<Verb>>(top, dobjQuestionVerbPhrase));

      // Deal with active case
      var spNs = subjQueryN.Match(tree);
      var spVs = subjQueryV.Match(tree);
      if (spNs.HasValue && spVs.HasValue)
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
      if (dpNs.HasValue && dpVs.HasValue)
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
          if (answers.Count != 0)
          {
            stream.AppendOutput(GeneratePassiveResponse(entityName, verbName, answers));
          }
        }
      }
      return stream;
    }

    private void PrintVerbs(Stream stream)
    {
      var tree = this.parser.Parse(stream.RemainingInput);

      var verb = new VerbName(associations, thesaurus);
      var top = new Branch("TOP");

      var query = new Descendant<IEnumerable<Verb>>(top, verb);

      var verbs = query.Match(tree);

      Console.WriteLine("Verbs: ");
      if (verbs.HasValue)
      {
        foreach (var v in verbs.Value.SelectMany(x => x).Distinct())
        {
          string verbName;
          associations.TryNameVerb(v, out verbName);
          Console.WriteLine($"{v} {verbName}");
        }
      }
    }
    
    private void PrintEntities(Stream stream)
    {
      var tree = this.parser.Parse(stream.RemainingInput);

      var entity = new EntityName(associations, thesaurus);
      var top = new Branch("TOP");

      var query = new Flatten<Entity>(new Descendant<IEnumerable<Entity>>(top, entity));

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
