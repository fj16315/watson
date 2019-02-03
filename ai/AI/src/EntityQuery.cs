using edu.stanford.nlp.ling;

using System.Collections.Generic;
using System.Linq;

//using System;

namespace GameAI
{
  public class Query
  {
    private readonly Parser parser;
    //public Func<string, object> logger;

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
      var obj_verbs =
        from verb in tdlist.AllWithRelationTo("nsubj", subj)
        from obj in tdlist.AllWithRelationFrom(verb, "dobj")
        select new ObjectVerbPair(obj, verb);
        
      //foreach (var foo in obj_verbs)
      //{
      //  logger($"verb: {foo.verb}, obj:{foo.obj}");
      //}

      if (obj_verbs.Any())
      {
        //logger("  Not empty!");
        return obj_verbs.SelectMany(
          vo => RunClause(assocs, tdlist, kg, vo.obj)
                  .SelectMany(kg.RelationTo)
                  .Where(
                    arg => assocs.Describes(vo.verb.word(), arg.relation)
                        && assocs.Describes(vo.obj.word(), arg.to) )
                  .Select(arg => arg.from));

                    //arg => {
                    //var verb_bool = assocs.Describes(vo.verb.word(), arg.relation);
                    ////var subj_bool = assocs.Describes(subj.word(), arg.from);
                    //var obj_bool = assocs.Describes(vo.obj.word(), arg.to);
                    //logger($"  v {vo.verb.word()} == {(int)arg.relation}? {verb_bool}");
                    ////logger($"  {subj.word()} == {(int)arg.from}? {subj_bool}");
                    //logger($"  o {vo.obj.word()} == {(int)arg.to}? {obj_bool}");
                    //return verb_bool && obj_bool;
                    //})
      }
      //logger("  Empty!");
      return kg.AllEntities();
    }
  }

  public struct ObjectVerbPair
  {
    public IndexedWord obj { get; }

    public IndexedWord verb { get; }

    public ObjectVerbPair(IndexedWord obj, IndexedWord verb)
    {
      this.obj = obj;
      this.verb = verb;
    }
  }
}
