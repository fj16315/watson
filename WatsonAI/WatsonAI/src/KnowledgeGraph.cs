using System;
using System.Collections;
using System.Collections.Generic;

namespace WatsonAI
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
    private Relation[,] adj_matrix;

    /// <summary>
    /// Initializes a new instance of the <see cref="GameAI.KnowledgeGraph"/> class.
    /// </summary>
    /// <param name="node_count">The number of nodes in the graph.</param>
    public KnowledgeGraph(int node_count)
    {
      adj_matrix = new Relation[node_count,node_count];
    }

    /// <summary>
    /// Gets the number of nodes in the graph.
    /// </summary>
    /// <returns>The number of nodes in the graph.</returns>
    /// <remarks>The number of nodes is the same as the size of the adjacency matrix.</remarks>
    public int GetNodeCount()
      => adj_matrix.GetLength(0);

    /// <summary>
    /// Returns the Relation from one node to another.
    /// </summary>
    /// <returns>The <see cref="GameAI.Relation"/> from one node to another.</returns>
    /// <param name="from">The node from which the Relation come.</param>
    /// <param name="to">The node to which the Relation go.</param>
    public Relation RelationFromTo(Entity from, Entity to)
      => adj_matrix[(int)from, (int)to];

    public void SetRelationFromTo(Entity from, Relation rel, Entity to)
    {
      adj_matrix[(int)from, (int)to] = rel;
    }

    /// <summary>
    /// Returns the Relation from a given node.
    /// </summary>
    /// <returns>An <see cref="GameAI.OutEdgeIter"/> containing the <see cref="GameAI.Relation"/> from the given node, <paramref name="from"/>.</returns>
    /// <param name="from">The node from which the Relation come.</param>
    public OutEdgeIter RelationFrom(Entity from)
      => new OutEdgeIter(from, this);

    /// <summary>
    /// Returns the Relation to a given node.
    /// </summary>
    /// <returns>An <see cref="GameAI.InEdgeIter"/> containing the <see cref="GameAI.Relation"/> to the given node, <paramref name="to"/>.</returns>
    /// <param name="to">The node to which the Relation go.</param>
    public InEdgeIter RelationTo(Entity to)
      => new InEdgeIter(to, this);

    /// <summary>
    /// Returns all of the <see cref="GameAI.Relation"/> stored in this
    /// instance of the graph.
    /// </summary>
    /// <returns>An <see cref="GameAI.AllRelationsIter"/>.</returns>
    /// <remarks>The items are of the form <c>(from, relationship, to)</c></remarks>
    public AllRelationsIter AllRelations()
      => new AllRelationsIter(this);

    /// <summary>
    /// Returns all of the entities in this graph.
    /// </summary>
    /// <returns>The entities.</returns>
    public AllEntitiesIter AllEntities()
      => new AllEntitiesIter(GetNodeCount());
  }

  /// <summary>
  /// A container for the <see cref="GameAI.Relation"/> coming from an
  /// <see cref="GameAI.Entity"/> and going to another.
  /// </summary>
  public class OutEdgeIter : IEnumerable<Edge>
  {
    /// <summary>
    /// The node from which the <see cref="GameAI.Relation"/> are coming.
    /// </summary>
    private readonly Entity from;
    /// <summary>
    /// The <see cref="GameAI.KnowledgeGraph"/> containing the <see cref="GameAI.Relation"/>.
    /// </summary>
    private readonly KnowledgeGraph graph;

    /// <summary>
    /// Initializes a new instance of the <see cref="GameAI.OutEdgeIter"/> class.
    /// </summary>
    /// <param name="from">The node from which the <see cref="GameAI.Relation"/> come.</param>
    /// <param name="graph">The <see cref="KnowledgeGraph"/> containing the <see cref="GameAI.Relation"/>.</param>
    public OutEdgeIter(Entity from, KnowledgeGraph graph)
    {
      this.from = from;
      this.graph = graph;
    }

    /// <summary>
    /// Implementing IEnumerable<Relation>.
    /// </summary>
    public IEnumerator<Edge> GetEnumerator()
    {
      for (int to_i = 0; to_i < graph.GetNodeCount(); ++to_i)
      {
        var to = new Entity(to_i);
        var edge = new Edge
        {
          from = from,
          to = to,
          relation = graph.RelationFromTo(from, to)
        };
        yield return edge;
      }
    }

    /// <summary>
    /// Implementing IEnumerable.
    /// </summary>
    IEnumerator IEnumerable.GetEnumerator()
      => this.GetEnumerator();
  }

  /// <summary>
  /// A container for the <see cref="GameAI.Relation"/> going to an
  /// <see cref="GameAI.Entity"/> and coming from another.
  /// </summary>
  public class InEdgeIter : IEnumerable<Edge>
  {
    /// <summary>
    /// The node to which the <see cref="GameAI.Relation"/> are going.
    /// </summary>
    private readonly Entity to;
    /// <summary>
    /// The <see cref="GameAI.KnowledgeGraph"/> containing the <see cref="GameAI.Relation"/>.
    /// </summary>
    private readonly KnowledgeGraph graph;

    /// <summary>
    /// Initializes a new instance of the <see cref="GameAI.InEdgeIter"/> class.
    /// </summary>
    /// <param name="to">The node to which the <see cref="GameAI.Relation"/> go.</param>
    /// <param name="graph">The <see cref="KnowledgeGraph"/> containing the <see cref="GameAI.Relation"/>.</param>
    public InEdgeIter(Entity to, KnowledgeGraph graph)
    {
      this.to = to;
      this.graph = graph;
    }

    /// <summary>
    /// Implementing IEnumerable<Relation>.
    /// </summary>
    public IEnumerator<Edge> GetEnumerator()
    {
      for (int from_i = 0; from_i < graph.GetNodeCount(); ++from_i)
      {
        var from = new Entity(from_i);
        var edge = new Edge
        {
          from = from,
          to = to,
          relation = graph.RelationFromTo(from, to)
        };
        yield return edge;
      }
    }

    /// <summary>
    /// Implementing IEnumerable.
    /// </summary>
    IEnumerator IEnumerable.GetEnumerator()
      => GetEnumerator();

  }

  public class AllRelationsIter : IEnumerable<Edge>
  {
    private readonly KnowledgeGraph graph;

    public AllRelationsIter(KnowledgeGraph graph)
    {
      this.graph = graph;
    }

    public IEnumerator<Edge> GetEnumerator()
    {
      for (int from_i = 0; from_i < graph.GetNodeCount(); ++from_i)
      {
        var from = new Entity(from_i);
        for (int to_i = 0; to_i < graph.GetNodeCount(); ++to_i)
        {
          var to = new Entity(to_i);
          var edge = new Edge
          {
            from = from,
            to = to,
            relation = graph.RelationFromTo(from, to)
          };
          yield return edge;
        }
      }
    }

    IEnumerator IEnumerable.GetEnumerator()
      => this.GetEnumerator();
  }

  public class AllEntitiesIter : IEnumerable<Entity>
  {
    private readonly int nodeCount;

    public AllEntitiesIter(int nodeCount)
    {
      this.nodeCount = nodeCount;
    }

    public IEnumerator<Entity> GetEnumerator()
    {
      for (int node = 0; node < nodeCount; ++node)
      {
        yield return new Entity(node);
      }
    }

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

    /// <summary>
    /// Adds a <see cref="GameAI.Relation"/>, <paramref name="rel"/>, from <paramref name="a"/> to <paramref name="b"/>.
    /// </summary>
    /// <returns><c>this</c>.</returns>
    /// <param name="a">The first node.</param>
    /// <param name="rel">The Relation</param>
    /// <param name="b">The second node.</param>
    public KnowledgeGraphBuilder AddEdge(Entity a, Relation rel, Entity b)
    {
      this.kg.SetRelationFromTo(a, rel, b);
      return this;
    }

    /// <summary>
    /// Builds the instance of <see cref="GameAI.KnowledgeGraph"/>.
    /// </summary>
    /// <returns>A new <see cref="GameAI.KnowledgeGraph"/>.</returns>
    public KnowledgeGraph Build()
      => this.kg;
  }

  public struct Edge
  {
    public Entity from { get; set; }

    public Entity to { get; set; }

    public Relation relation { get; set; }
  }
}

