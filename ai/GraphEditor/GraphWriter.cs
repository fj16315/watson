using GameAI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace GraphEditor
{
  /// <summary>
  /// Writes knowledge graphs and associations(not implemented) to a file.
  /// </summary>
  public class GraphWriter
  {
    private List<Entity> nodes;
    private Dictionary<Entity, string> nodeNames;

    private Dictionary<Entity, List<RelationDestinationRow>> relations;
    private Dictionary<SingleRelation, string> relationNames;

    public GraphWriter(List<Entity> nodes, Dictionary<Entity, string> nodeNames, 
      Dictionary<Entity, List<RelationDestinationRow>> relations, Dictionary<SingleRelation, string> relationNames)
    {
      this.nodes = nodes;
      this.nodeNames = nodeNames;
      this.relations = relations;
      this.relationNames = relationNames;
    }

    public void SaveGraph()
    {
      Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog
      {
        FileName = "UntitledGraph",
        DefaultExt = ".universe",
        Filter = "Universe - Knowledge Graph and Associations (.universe)|*.universe"
      };

      bool? result = dlg.ShowDialog();

      if (result == true)
      {
        string filename = dlg.FileName;

        KnowledgeGraph knowledgeGraph = this.BuildGraph();

        IFormatter formatter = new BinaryFormatter();
        Stream stream = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.None);
        formatter.Serialize(stream, knowledgeGraph);
        stream.Close();
      }
    }
    
    private KnowledgeGraph BuildGraph()
    {
      KnowledgeGraphBuilder builder = new KnowledgeGraphBuilder(this.nodes.Count);
      foreach (Entity source in this.nodes)
      {
        foreach (Entity possibleDestination in this.nodes)
        {
          var relation = new Relation();
          foreach (RelationDestinationRow r in this.relations[source])
          {
            if (r.destination.Equals(possibleDestination))
            {
              relation |= r.relation.AsRelation();
            }
          }
          if (!relation.IsNone())
          {
            builder.AddEdge(source, relation, possibleDestination);
          }
        }
      }

      return builder.Build();
    }
  }
}
