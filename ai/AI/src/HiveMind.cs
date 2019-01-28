using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameAI 
{
  class HiveMind
  {
    private KnowledgeGraph mainGraph;
    private List<KnowledgeGraph> subGraphs;

    private HiveMind(KnowledgeGraph mainGraph, List<KnowledgeGraph> subGraphs)
    {
      this.mainGraph = mainGraph;
      this.subGraphs = subGraphs;
    }

    /*public static Universe CheckedNew(KnowledgeGraph mainGraph, List<KnowledgeGraph> subGraphs)
      => subGraphs.Select(g => IsSubGraph(g, mainGraph)).All(x => x) ? new Universe(mainGraph, subGraphs) : null;*/

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
        (main, sub) => main.Item2.Contains(sub.Item2)).All(x => x);
    }
  }

  [Serializable]
  class Associations
  {
    private string[] entityNames;

    private string[] relationNames;

    public Dictionary<string,List<Entity>> entityWords { get; }

    public Dictionary<string,List<Relation>> relationWords { get; }

    public Associations(int entityCount, int relationCount)
    {
      this.entityNames = new string[entityCount];
      this.entityNames = new string[relationCount];
      this.entityWords = new Dictionary<string, List<Entity>>();
      this.relationWords = new Dictionary<string, List<Relation>>();
    }

    public string NameOf(Entity entity)
      => this.entityNames[(int) entity];

    public string NameOf(Relation relation)
      => this.relationNames[(int) relation];
  }
}
