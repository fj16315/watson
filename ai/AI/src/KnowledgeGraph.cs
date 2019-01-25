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
  }

  /// <summary>
  /// An iterator over the <see cref="GameAI.Relationships"/> coming out of a node.
  /// </summary>
  public class OutEdgeIter : IEnumerator, IEnumerable
  {
    /// <summary>
    /// The node from which the <see cref="GameAI.Relationships"/> are coming.
    /// </summary>
    private readonly Entity _from;
    /// <summary>
    /// The <see cref="GameAI.KnowledgeGraph"/> containing the <see cref="GameAI.Relationships"/>.
    /// </summary>
    private readonly KnowledgeGraph _graph;
    /// <summary>
    /// The node to which the current <see cref="GameAI.Relationships"/> goes.
    /// </summary>
    int _to;

    /// <summary>
    /// Implementing IEnumerable.
    /// </summary>
    public IEnumerator GetEnumerator()
    {
      return this;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="GameAI.OutEdgeIter"/> class.
    /// </summary>
    /// <param name="from">The node from which the <see cref="GameAI.Relationships"/> come.</param>
    /// <param name="graph">The <see cref="KnowledgeGraph"/> containing the <see cref="GameAI.Relationships"/>.</param>
    public OutEdgeIter(Entity from, KnowledgeGraph graph)
    {
      _from = from;
      _graph = graph;
      (this as IEnumerator).Reset();
    }

    /// <summary>
    /// The <see cref="GameAI.Relationships"/> from the initial node to the current node.
    /// </summary>
    /// <value>The <see cref="GameAI.Relationships"/> from the initial node to the current node.</value>
    object IEnumerator.Current
      => _graph.RelationshipsFromTo(_from, new Entity(_to));

    /// <summary>
    /// Advances to the next <see cref="GameAI.Relationships"/>.
    /// </summary>
    /// <returns><c>true</c> if a next <see cref="GameAI.Relationships"/> existed, <c>false</c> otherwise.</returns>
    bool IEnumerator.MoveNext()
    {
      if ( _to >= _graph.GetNodeCount() ) { return false; }
      _to += 1;
      return true;
    }

    /// <summary>
    /// Resets the node to which the <see cref="GameAI.Relationships"/> go.
    /// </summary>
    void IEnumerator.Reset()
    {
      _to = -1;
    }
  }

  /// <summary>
  /// An iterator over the <see cref="GameAI.Relationships"/> going into a node.
  /// </summary>
  public class InEdgeIter : IEnumerator, IEnumerable<Relationships>
  {
    /// <summary>
    /// The node to which the <see cref="GameAI.Relationships"/> are going.
    /// </summary>
    private readonly Entity _to;
    /// <summary>
    /// The <see cref="GameAI.KnowledgeGraph"/> containing the <see cref="GameAI.Relationships"/>.
    /// </summary>
    private readonly KnowledgeGraph _graph;
    /// <summary>
    /// The node from which the current <see cref="GameAI.Relationships"/> comes.
    /// </summary>
    int _from;

    /// <summary>
    /// Implementing IEnumerable.
    /// </summary>
    public IEnumerator<Relationships> GetEnumerator()
    {
      return this as IEnumerable<Relationships>;
    }

    /// <summary>
    /// Implementing IEnumerable.
    /// </summary>
    IEnumerator IEnumerable.GetEnumerator()
    {
      return this;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="GameAI.InEdgeIter"/> class.
    /// </summary>
    /// <param name="to">The node to which the <see cref="GameAI.Relationships"/> go.</param>
    /// <param name="graph">The <see cref="KnowledgeGraph"/> containing the <see cref="GameAI.Relationships"/>.</param>
    public InEdgeIter(Entity to, KnowledgeGraph graph)
    {
      _to = to;
      _graph = graph;
      (this as IEnumerator).Reset();
    }

    /// <summary>
    /// The <see cref="GameAI.Relationships"/> from the current node to the initial node.
    /// </summary>
    /// <value>The <see cref="GameAI.Relationships"/> from the current node to the initial node.</value>
    object IEnumerator.Current
      => _graph.RelationshipsFromTo(new Entity(_from), _to);

    /// <summary>
    /// Advances to the next <see cref="GameAI.Relationships"/>.
    /// </summary>
    /// <returns><c>true</c> if a next <see cref="GameAI.Relationships"/> existed, <c>false</c> otherwise.</returns>
    bool IEnumerator.MoveNext()
    {
      if ( _from >= _graph.GetNodeCount() ) { return false; }
      _from += 1;
      return true;
    }

    /// <summary>
    /// Resets the node from which the <see cref="GameAI.Relationships"/> come.
    /// </summary>
    void IEnumerator.Reset()
    {
      _from = -1;
    }
  }

  /// <summary>
  /// Quick class for building <see cref="GameAI.KnowledgeGraph"/>.
  /// </summary>
  public class KnowledgeGraphBuilder
  {
    private KnowledgeGraph _kg;

    /// <summary>
    /// Begins initialisation of a new <see cref="GameAI.KnowledgeGraph"/>.
    /// </summary>
    /// <param name="size">The sixe of the <see cref="GameAI.KnowledgeGraph"/>.</param>
    public KnowledgeGraphBuilder(int size)
    {
      _kg = new KnowledgeGraph(size);
    }

    /// <summary>
    /// Provide a mapping for entities to their names.
    /// </summary>
    /// <returns><c>this</c>.</returns>
    /// <param name="a">A dictionary mapping entities to names.</param>
    public KnowledgeGraphBuilder AddEntityNames(Dictionary<Entity,String> entity_names)
    {
      _kg.entity_names = entity_names;
      return this;
    }

    /// <summary>
    /// Adds a <see cref="GameAI.Relationships"/>, <paramref name="rel"/>, from <paramref name="a"/> to <paramref name="b"/>.
    /// </summary>
    /// <returns><c>this</c>.</returns>
    /// <param name="a">The first node.</param>
    /// <param name="b">The second node.</param>
    /// <param name="rel">The relationships</param>
    public KnowledgeGraphBuilder AddEdge(Entity a, Entity b, Relationships rel)
    {
      _kg.RelationshipsFromTo(a, b) = rel;
      return this;
    }

    /// <summary>
    /// Builds the instance of <see cref="GameAI.KnowledgeGraph"/>.
    /// </summary>
    /// <returns>A new <see cref="GameAI.KnowledgeGraph"/>.</returns>
    public KnowledgeGraph Build()
      => _kg;
  }
}
