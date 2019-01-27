using System;
using System.Collections;
using System.Collections.Generic;



namespace GameAI
{
  /// <summary>
  /// Represents the information known by an NPC
  /// </summary>
  [Serializable()]
  public class KnowledgeGraph
  {
    /// <summary>
    /// An adjacency matrix used for representing the <see cref="GameAI.KnowledgeGraph"/>.
    /// </summary>
    private Relationships[,] adj_matrix;

    // TODO: Remove this.
    /// <summary>
    /// A dictionary lookup for entities.
    /// </summary>
    public Dictionary<Entity,String> entity_names;

    /// <summary>
    /// Initializes a new instance of the <see cref="GameAI.KnowledgeGraph"/> class.
    /// </summary>
    /// <param name="node_count">The number of nodes in the graph.</param>
    public KnowledgeGraph(int node_count)
    {
      adj_matrix = new Relationships[node_count,node_count];
    }

    /// <summary>
    /// Gets the number of nodes in the graph.
    /// </summary>
    /// <returns>The number of nodes in the graph.</returns>
    /// <remarks>The number of nodes is the same as the size of the adjacency matrix.</remarks>
    public int GetNodeCount()
      => adj_matrix.GetLength(0);

    /// <summary>
    /// Returns the relationships from one node to another.
    /// </summary>
    /// <returns>The <see cref="GameAI.Relationships"/> from one node to another.</returns>
    /// <param name="from">The node from which the relationships come.</param>
    /// <param name="to">The node to which the relationships go.</param>
    public ref Relationships RelationshipsFromTo(Entity from, Entity to)
      => ref adj_matrix[(int)from, (int)to];

    /// <summary>
    /// Returns the relationships from a given node.
    /// </summary>
    /// <returns>An <see cref="GameAI.OutEdgeIter"/> containing the <see cref="GameAI.Relationships"/> from the given node, <paramref name="from"/>.</returns>
    /// <param name="from">The node from which the relationships come.</param>
    public OutEdgeIter RelationshipsFrom(Entity from)
      => new OutEdgeIter(from, this);

    /// <summary>
    /// Returns the relationships to a given node.
    /// </summary>
    /// <returns>An <see cref="GameAI.InEdgeIter"/> containing the <see cref="GameAI.Relationships"/> to the given node, <paramref name="to"/>.</returns>
    /// <param name="to">The node to which the relationships go.</param>
    public InEdgeIter RelationshipsTo(Entity to)
      => new InEdgeIter(to, this);

    /// <summary>
    /// Returns all of the <see cref="GameAI.Relationships"/> stored in this
    /// instance of the graph.
    /// </summary>
    /// <returns>An <see cref="System.Collections.Generic.IEnumerator{(Entity,Relationships,Entity)}"/>.</returns>
    /// <remarks>The items are of the form <c>(from, relationship, to)</c></remarks>
    public IEnumerator<(Entity,Relationships,Entity)> AllRelationships()
    {
      for (int from_i = 0; from_i < GetNodeCount(); ++from_i)
      {
        var from = new Entity(from_i);
        for (int to_i = 0; to_i < GetNodeCount(); ++to_i)
        {
          var to = new Entity(to_i);
          yield return (from, RelationshipsFromTo(from, to), to);
        }
      }
    }
  }

  /// <summary>
  /// A container for the <see cref="GameAI.Relationships"/> coming from an
  /// <see cref="GameAI.Entity"/> and going to another.
  /// </summary>
  public class OutEdgeIter : IEnumerable<Relationships>
  {
    /// <summary>
    /// The node from which the <see cref="GameAI.Relationships"/> are coming.
    /// </summary>
    private readonly Entity from;
    /// <summary>
    /// The <see cref="GameAI.KnowledgeGraph"/> containing the <see cref="GameAI.Relationships"/>.
    /// </summary>
    private readonly KnowledgeGraph graph;

    /// <summary>
    /// Initializes a new instance of the <see cref="GameAI.OutEdgeIter"/> class.
    /// </summary>
    /// <param name="from">The node from which the <see cref="GameAI.Relationships"/> come.</param>
    /// <param name="graph">The <see cref="KnowledgeGraph"/> containing the <see cref="GameAI.Relationships"/>.</param>
    public OutEdgeIter(Entity from, KnowledgeGraph graph)
    {
      this.from = from;
      this.graph = graph;
    }

    /// <summary>
    /// Implementing IEnumerable<Relationships>.
    /// </summary>
    public IEnumerator<Relationships> GetEnumerator()
    {
      for (int to = 0; to < this.graph.GetNodeCount(); ++to)
      {
        yield return this.graph.RelationshipsFromTo(from, new Entity(to));
      }
    }

    /// <summary>
    /// Implementing IEnumerable.
    /// </summary>
    IEnumerator IEnumerable.GetEnumerator()
      => this.GetEnumerator();
  }

  /// <summary>
  /// A container for the <see cref="GameAI.Relationships"/> going to an
  /// <see cref="GameAI.Entity"/> and coming from another.
  /// </summary>
  public class InEdgeIter : IEnumerable<Relationships>
  {
    /// <summary>
    /// The node to which the <see cref="GameAI.Relationships"/> are going.
    /// </summary>
    private readonly Entity to;
    /// <summary>
    /// The <see cref="GameAI.KnowledgeGraph"/> containing the <see cref="GameAI.Relationships"/>.
    /// </summary>
    private readonly KnowledgeGraph graph;

    /// <summary>
    /// Initializes a new instance of the <see cref="GameAI.InEdgeIter"/> class.
    /// </summary>
    /// <param name="to">The node to which the <see cref="GameAI.Relationships"/> go.</param>
    /// <param name="graph">The <see cref="KnowledgeGraph"/> containing the <see cref="GameAI.Relationships"/>.</param>
    public InEdgeIter(Entity to, KnowledgeGraph graph)
    {
      this.to = to;
      this.graph = graph;
    }

    /// <summary>
    /// Implementing IEnumerable<Relationships>.
    /// </summary>
    public IEnumerator<Relationships> GetEnumerator()
    {
      for (int from = 0; from < this.graph.GetNodeCount(); ++from)
      {
        yield return this.graph.RelationshipsFromTo(new Entity(from), to);
      }
    }

    /// <summary>
    /// Implementing IEnumerable.
    /// </summary>
    IEnumerator IEnumerable.GetEnumerator()
      => this.GetEnumerator();

  }

  /// <summary>
  /// Quick class for building <see cref="GameAI.KnowledgeGraph"/>.
  /// </summary>
  public class KnowledgeGraphBuilder
  {
    private KnowledgeGraph kg;

    /// <summary>
    /// Begins initialisation of a new <see cref="GameAI.KnowledgeGraph"/>.
    /// </summary>
    /// <param name="size">The sixe of the <see cref="GameAI.KnowledgeGraph"/>.</param>
    public KnowledgeGraphBuilder(int size)
    {
      this.kg = new KnowledgeGraph(size);
    }

    // TODO: Remove this!
    /// <summary>
    /// Provide a mapping for entities to their names.
    /// </summary>
    /// <returns><c>this</c>.</returns>
    /// <param name="a">A dictionary mapping entities to names.</param>
    public KnowledgeGraphBuilder AddEntityNames(Dictionary<Entity,String> entity_names)
    {
      this.kg.entity_names = entity_names;
      return this;
    }

    /// <summary>
    /// Adds a <see cref="GameAI.Relationships"/>, <paramref name="rel"/>, from <paramref name="a"/> to <paramref name="b"/>.
    /// </summary>
    /// <returns><c>this</c>.</returns>
    /// <param name="a">The first node.</param>
    /// <param name="rel">The relationships</param>
    /// <param name="b">The second node.</param>
    public KnowledgeGraphBuilder AddEdge(Entity a, Relationships rel, Entity b)
    {
      this.kg.RelationshipsFromTo(a, b) = rel;
      return this;
    }

    /// <summary>
    /// Builds the instance of <see cref="GameAI.KnowledgeGraph"/>.
    /// </summary>
    /// <returns>A new <see cref="GameAI.KnowledgeGraph"/>.</returns>
    public KnowledgeGraph Build()
      => this.kg;
  }
}
