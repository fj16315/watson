using OpenNLP.Tools.Parser;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WatsonAI
{
  public class QuestionProcessDebugPrints
  {
    private Parser parser;
    private KnowledgeQuery query;
    private Knowledge knowledge;
    private Thesaurus thesaurus;
    private Associations associations;

    /// <summary>
    /// Text engine for debuging the specified Parser.
    /// </summary>
    /// <param name="parser">The parser to use.</param>
    public QuestionProcessDebugPrints(Parser parser, Knowledge knowledge, Thesaurus thesaurus, Associations associations)
    {
      this.parser = parser;
      this.knowledge = knowledge;
      this.thesaurus = thesaurus;
      this.associations = associations;
      this.query = new KnowledgeQuery(knowledge);
    }

    public void PrintVerbs(Stream stream)
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

    public void PrintEntities(Stream stream)
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
  }
}
