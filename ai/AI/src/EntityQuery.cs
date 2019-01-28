using edu.stanford.nlp.ling;

using System.Collections.Generic;
using System.Linq;

namespace GameAI
{
  public class Query
  {
    private readonly Parser parser;

    public Query(Parser parser)
    {
      this.parser = parser;
    }

    public IEnumerable<Entity> Run(Associations assocs, string sentence, KnowledgeGraph kg)
    {
      var tree = parser.Parse(sentence);
      var tdList = parser.DependenciesFrom(tree);
      var root = tdList.GetRoot();
      var start = tdList.WithRelationFrom(root, "nsubj");
      return RunClause(assocs, tdList, kg, start);
    }

    private IEnumerable<Entity> RunClause(Associations assocs, TypedDependenciesList tdlist, KnowledgeGraph kg, IndexedWord subj)
    {
      var verb = tdlist.WithRelationTo("nsubj", subj);
      if (verb != null)
      {
        var obj = tdlist.WithRelationFrom(verb, "dobj");
        if (obj != null)
        {
          return RunClause(assocs, tdlist, kg, obj)
            .SelectMany(kg.RelationTo)
            .Where(
              arg => assocs.Describes(verb.word(), arg.rel)
                  && assocs.Describes(subj.word(), arg.from))
            .Select(arg => arg.from);
        }
      }
      return kg.AllEntities();
    }

  }

}
