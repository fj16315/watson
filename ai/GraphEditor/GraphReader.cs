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
  /// Loads knowledge graphs and associations(not implemented) from a file.
  /// </summary>
  public class GraphReader
  {
    private List<Entity> nodes;
    private Dictionary<Entity, string> nodeNames;

    private Dictionary<Entity, List<RelationDestinationRow>> relations;
    private Dictionary<SingleRelation, string> relationNames;

    public GraphReader(List<Entity> nodes, Dictionary<Entity, string> nodeNames, 
      Dictionary<Entity, List<RelationDestinationRow>> relations, Dictionary<SingleRelation, string> relationNames)
    {
      this.nodes = nodes;
      this.nodeNames = nodeNames;
      this.relations = relations;
      this.relationNames = relationNames;
    }

    public KnowledgeGraph LoadGraph()
    {
      Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog
      {
        DefaultExt = ".universe",
        Filter = "Universe - Knowledge Graph and Associations (.universe)|*.universe"
      };

      bool? result = dlg.ShowDialog();

      if (result == true)
      {
        string filename = dlg.FileName;

        IFormatter formatter = new BinaryFormatter();  
        Stream stream = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read);  
        KnowledgeGraph knowledgeGraph = (KnowledgeGraph) formatter.Deserialize(stream);  
        stream.Close();

        return knowledgeGraph;
      }
      throw new IOException("Failed reading graph file.");
    }
  }
}
