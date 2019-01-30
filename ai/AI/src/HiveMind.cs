using System;
using System.Collections.Generic;
using System.Linq;

namespace GameAI 
{
  public class HiveMind
  {
    private KnowledgeGraph mainGraph;
    private List<KnowledgeGraph> subGraphs;

    private HiveMind(KnowledgeGraph mainGraph, List<KnowledgeGraph> subGraphs)
    {
      this.mainGraph = mainGraph;
      this.subGraphs = subGraphs;
    }

    /*public static Universe CheckedNew(KnowledgeGraph mainGraph, List<KnowledgeGraph> subGraphs)
      => subGraphs.All(g => IsSubGraph(g, mainGraph)) ? new Universe(mainGraph, subGraphs) : null;*/

    public static HiveMind CheckedNew(KnowledgeGraph mainGraph, List<KnowledgeGraph> subGraphs)
    {
      foreach (var subGraph in subGraphs)
      {
        if (!IsSubGraph(subGraph, mainGraph))
        {
          return null;
        }
      }
      return new HiveMind(mainGraph, subGraphs);
    }

    // "This works" - Jago 2019
    private static bool IsSubGraph(KnowledgeGraph subGraph, KnowledgeGraph mainGraph)
    {
      return Enumerable.Zip(
        mainGraph.AllRelations(),
        subGraph.AllRelations(),
        (main, sub) => main.relation.Contains(sub.relation)).All(x => x);
    }
  }

  [Serializable]
  public class Associations
  {
    private readonly string[] entityNames;

    private readonly string[] relationNames;

    public Dictionary<string,List<Entity>> entityWords { get; }

    public Dictionary<string,List<Relation>> relationWords { get; }

    public Associations(int entityCount, int relationCount)
    {
      entityNames = new string[entityCount];
      relationNames = new string[relationCount];
      entityWords = new Dictionary<string, List<Entity>>();
      relationWords = new Dictionary<string, List<Relation>>();
    }

    public string NameOf(Entity entity)
      => entityNames[(int) entity];

    public string NameOf(Relation relation)
      => relationNames[(int) relation];

    public bool Describes(string word, Entity entity)
      => entityWords[word].Contains(entity);

    public bool Describes(string word, Relation relation)
      => relationWords[word].Contains(relation);
  }
}
