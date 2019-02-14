using GameAI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
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
    private readonly EditorModel model;

    public GraphReader(EditorModel model)
    {
      this.model = model;
    }

    /// <summary>
    /// Loads the graph into the model, clobbers existing data.
    /// </summary>
    public void LoadGraph()
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
        var universe = (Universe) formatter.Deserialize(stream);
        stream.Close();

        model.entities = EntitiesFromUniverse(universe);
        model.entityNames = EntityNamesFromUniverse(universe);
        model.relations = RelationsFromUniverse(universe);
        model.relationNames = RelationNamesFromUniverse(universe);
      }
    }

    /// <summary>
    /// Returns a list of entities stored in the universe.
    /// </summary>
    /// <param name="universe"></param>
    /// <returns></returns>
    private static List<Entity> EntitiesFromUniverse(Universe universe)
    {
      var entities = new List<Entity>();
      
      foreach (var entity in universe.knowledgeGraph.AllEntities())
      {
        entities.Add(entity);
        
      }

      return entities;
    }

    /// <summary>
    /// Returns a mapping from entities to their names that are stored in the universe.
    /// </summary>
    /// <param name="universe"></param>
    /// <returns></returns>
    private static Dictionary<Entity, string> EntityNamesFromUniverse(Universe universe)
    {
      var entityNames = new Dictionary<Entity,string>();
      for (int i = 0; i < universe.associations.entityNames.Length; i++)
      {
        entityNames[new Entity(i)] = universe.associations.entityNames[i];
      }

      return entityNames;
    }

    /// <summary>
    /// Returns a mapping from entities to a list of relations that is stored in the universe.
    /// </summary>
    /// <param name="universe"></param>
    /// <returns></returns>
    private Dictionary<Entity, List<RelationDestinationRow>> RelationsFromUniverse(Universe universe)
    {
      var relations = new Dictionary<Entity, List<RelationDestinationRow>>();
          
      foreach (var entity in universe.knowledgeGraph.AllEntities())
      {
        relations[entity] = new List<RelationDestinationRow>();
        foreach (var edge in universe.knowledgeGraph.RelationFrom(entity))
        {
          foreach (var singleRelation in edge.relation.SingleRelations())
          {
            relations[entity].Add(new RelationDestinationRow(edge.to, singleRelation));
          }
        }
      }

      return relations;
    }
    
    /// Returns a mapping from single relations to their names that are stored in the universe.
    private static Dictionary<SingleRelation, string> RelationNamesFromUniverse(Universe universe)
    {
      var relationNames = new Dictionary<SingleRelation,string>();
      for (int i = 0; i < universe.associations.relationNames.Length; i++)
      {
        relationNames[new SingleRelation(i)] = universe.associations.relationNames[i];
      }

      return relationNames;
    }
  }
}
