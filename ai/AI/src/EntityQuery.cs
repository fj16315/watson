using System;
using edu.stanford.nlp.trees;
using edu.stanford.nlp.ling;

using System.Collections;
using System.Collections.Generic;

namespace GameAI
{
  public class EntityQuery
  {
    public IndexedWord query(TypedDependenciesList tdlist, KnowledgeGraph kg)
    {
      var root = tdlist.GetRoot();
      var start = tdlist.WithRelationFrom(root, "nsubj");
      return start;
    }

    public IEnumerator<Entity> fullQuery(TypedDependenciesList tdlist, KnowledgeGraph kg, IndexedWord subject)
    {
      var verb = tdlist.WithRelationTo("nsubj", subject);
      if (verb == null)
      {

      }
      else
      {
        var noun = tdlist.WithRelationTo("dobj", verb);
        if (noun == null)
        {

        }
        else
        {

        }
      }
    }

  }

  
}
