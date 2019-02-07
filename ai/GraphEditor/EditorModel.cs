using GameAI;
using System.Collections.Generic;
using System.Linq;

namespace GraphEditor
{
  /// <summary>
  /// The editor model class deals with the core non-UI functionality of the editor.
  /// </summary>
  public class EditorModel
  {
    public List<Entity> entities { get; set; }
    public Dictionary<Entity, string> entityNames { get; }

    public Dictionary<Entity, List<RelationDestinationRow>> relations { get; }
    public Dictionary<SingleRelation, string> relationNames { get; }

    public FreeValueManager relationValueManager { get; }
    public FreeValueManager entityValueManager { get; }

    public EditorModel()
    {
      this.entities = new List<Entity>();
      this.entityNames = new Dictionary<Entity, string>();
      this.relations = new Dictionary<Entity, List<RelationDestinationRow>>();
      this.relationNames = new Dictionary<SingleRelation, string>(); 
      this.relationValueManager = new FreeValueManager();
      this.entityValueManager = new FreeValueManager();
    }

    /// <summary>
    /// Adds a new entity to the knowledge graph.
    /// </summary>
    /// <param name="entity">The entity to be added.</param>
    public void AddNewEntity(Entity entity)
    {
      if (!this.entities.Contains(entity))
      {
        this.entities.Add(entity);
        this.relations.Add(entity, new List<RelationDestinationRow>());
      }
    }

    /// <summary>
    /// Deletes an entity from the knowledge graph.
    /// </summary>
    /// <param name="deletedEntity">The entity to be deleted.</param>
    public void DeleteEntity(Entity deletedEntity)
    {
      this.entities.RemoveAt((int)deletedEntity);
      foreach (List<RelationDestinationRow> relations in this.relations.Values)
      {
        relations.RemoveAll(r => r.destination.Equals(deletedEntity));
      }
    }

    /// <summary>
    /// Adds a new relationship mapping from a source entity to a destination entity within
    /// the graph of some specified single relation.
    /// </summary>
    /// <param name="source">The source entity.</param>
    /// <param name="relation">The single relation.</param>
    /// <param name="destination">The destination entity.</param>
    public void AddNewRelationshipMapping(Entity source, SingleRelation relation, Entity destination)
    {
      RelationDestinationRow relationMapping = new RelationDestinationRow(destination, relation);
      this.relations[source].Add(relationMapping);
    }

    /// <summary>
    /// Deletes a relationship mapping between a source entity and some destination entity within
    /// the graph of some specified single relation.
    /// </summary>
    /// <param name="source">The source entity.</param>
    /// <param name="deletedRelation"></param>
    public void DeleteRelationshipMapping(Entity source, RelationDestinationRow deletedRelation)
      => this.relations[source].Remove(deletedRelation);

    /// <summary>
    /// Checks if the entity is contained within the graph.
    /// </summary>
    /// <param name="entity">The entity in to check in the graph for.</param>
    /// <returns>True if the entity is in the graph.</returns>
    public bool ContainsEntity(Entity entity)
      => this.entities.Contains(entity);

    /// <summary>
    /// Saves the current graph and associations into a file.
    /// Opens file explorer box to specify file name.
    /// </summary>
    public void SaveGraph()
    {
      this.CrunchGraph();
      var graphWriter = new GraphWriter(this.entities, this.entityNames, this.relations, this.relationNames);
      graphWriter.SaveGraph();
    }

    /// <summary>
    /// Removes all the gaps in the numbering from the ids of possible entities and relations.
    /// </summary>
    private void CrunchGraph()
    {
      this.CrunchEntities();
      this.CrunchRelations();
    }

    /// <summary>
    /// Gets rid of gaps in the entity numberings.
    /// </summary>
    private void CrunchEntities()
    {
      this.entities = this.entities.OrderBy(e => (int)e).ToList();
      for (int i = 0; i < this.entities.Count; i++)
      {
        var previous = this.entities[i];
        if ((int)previous != i)
        {
          var newEntity = new Entity(i);
          this.entityNames[newEntity] = this.entityNames[previous];
          this.relations[newEntity] = this.relations[previous];
          foreach (var e in this.entities)
          {
            for (int r = 0; r < this.relations[e].Count; r++)
            {
              var relationRow = this.relations[newEntity][r];
              if (relationRow.destination.Equals(previous))
              {
                this.relations[newEntity][r] = new RelationDestinationRow(newEntity, relationRow.relation);
              }
            }
          }
        }
      }
    }

    /// <summary>
    /// Gets rid of gaps in the relation numberings.
    /// </summary>
    private void CrunchRelations()
    {
      var relations = this.relationNames.Keys.OrderBy(r => (int)r).ToList();
      for (int i = 0; i < relations.Count; i++)
      {
        var previous = relations[i];
        var newRelation = new SingleRelation(i);
        if ((int)previous != i)
        {
          foreach (var e in this.entities)
          {
            for (int r = 0; r < this.relations[e].Count; r++)
            {
              var rel = this.relations[e][r];
              if (rel.relation.Equals(previous))
              {
                this.relations[e][r] = new RelationDestinationRow(rel.destination, newRelation);
              }
            }
          }
        }
      }
    }
  }
}
