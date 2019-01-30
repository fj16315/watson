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

    // I am so sorry for how this looks, this needs urgent restructuring.
    //   - Jago Taylor
    private IEnumerable<Entity> RunClause(Associations assocs, TypedDependenciesList tdlist, KnowledgeGraph kg, IndexedWord subj)
    {
      IEnumerable<(IndexedWord verb, IndexedWord obj)> verb_objs =
        from verb in tdlist.AllWithRelationTo("nsubj", subj)
        from obj in tdlist.AllWithRelationFrom(verb, "dobj")
        select (verb, obj);

      if (verb_objs.Any())
      {
        return verb_objs.SelectMany(
          vo => RunClause(assocs, tdlist, kg, vo.obj)
                  .SelectMany(kg.RelationTo)
                  .Where(
                    arg => assocs.Describes(vo.verb.word(), arg.rel)
                        && assocs.Describes(subj.word(), arg.from))
                  .Select(arg => arg.from));

      }
      return kg.AllEntities();
    }

  }

}
