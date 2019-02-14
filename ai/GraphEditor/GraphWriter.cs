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
  /// Writes knowledge graphs and associations to a file.
  /// </summary>
  public class GraphWriter
  {
    private List<Entity> entities;
    private Dictionary<Entity, string> entityNames;

    private Dictionary<Entity, List<RelationDestinationRow>> relations;
    private Dictionary<SingleRelation, string> relationNames;

    public GraphWriter(List<Entity> entity, Dictionary<Entity, string> entityNames, 
      Dictionary<Entity, List<RelationDestinationRow>> relations, Dictionary<SingleRelation, string> relationNames)
    {
      this.entities = entity;
      this.entityNames = entityNames;
      this.relations = relations;
      this.relationNames = relationNames;
    }

    /// <summary>
    /// Saves the current knowledge graph and associations in a file.
    /// Opens a new fileexplorer box to fine the file path to save.
    /// Expects graph to contain no gaps in numbering of entities or relations.
    /// </summary>
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

        var knowledgeGraph = this.BuildGraph();
        var associations = this.BuildAssociations();
        var universe = new Universe(knowledgeGraph, associations);

        IFormatter formatter = new BinaryFormatter();
        Stream stream = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.None);
        formatter.Serialize(stream, universe);
        stream.Close();
      }
    }
    
    /// <summary>
    /// Builds the current knowledge graph from constituent parts stored in the model.
    /// </summary>
    /// <returns>The current knowledge graph.</returns>
    private KnowledgeGraph BuildGraph()
    {
      var builder = new KnowledgeGraphBuilder(this.entities.Count);
      foreach (Entity source in this.entities)
      {
        foreach (Entity possibleDestination in this.entities)
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

    private Associations BuildAssociations()
    {
      var associations = new Associations(this.entityNames.Keys.Count, this.relationNames.Keys.Count);
      foreach (var relation in this.relationNames.Keys)
      {
        associations.SetNameOf(relation, this.relationNames[relation]);
      }
      foreach (var entity in this.entityNames.Keys)
      {
        associations.SetNameOf(entity, this.entityNames[entity]);
      }
      return associations;
    }
  }
}
