using System.Collections;

namespace GameAI
{
  public class KnowledgeGraph
  {
    private Relationships[,] adj_matrix;

    public KnowledgeGraph( int size )
    {
      adj_matrix = new Relationships[size,size];
    }

    public int GetSize()
      => adj_matrix.GetLength(0);

    public ref Relationships RelationshipsFromTo(int from, int to)
      => ref adj_matrix[from, to];

    public OutEdgeIter RelationshipsFrom(int from)
      => new OutEdgeIter(from, this);

    public InEdgeIter RelationshipsTo(int to)
      => new InEdgeIter(to, this);
  }

  public class OutEdgeIter : IEnumerator
  {
    private readonly int from;
    private readonly ref KnowledgeGraph graph;
    int to;

    public OutEdgeIter( int _from, ref KnowledgeGraph _graph )
    {
      from = _from;
      graph = _graph;
      Reset();
    }

    object IEnumerator.Current
      => graph.RelationshipsFromTo(from, to);

    bool IEnumerator.MoveNext()
    {
      if ( to >= graph.GetSize() ) { return false; }
      to += 1;
      return true;
    }

    void IEnumerator.Reset()
    {
      to = -1;
    }
  }

  public class InEdgeIter : IEnumerator
  {
    private readonly int to;
    private readonly ref KnowledgeGraph graph;
    int from;

    public InEdgeIter( int _to, ref KnowledgeGraph _graph )
    {
      to = _to;
      graph = _graph;
      Reset();
    }

    object IEnumerator.Current
      => graph.RelationshipsFromTo(from, to);

    bool IEnumerator.MoveNext()
    {
      if ( from >= graph.GetSize() ) { return false; }
      from += 1;
      return true;
    }

    void IEnumerator.Reset()
    {
      from = -1;
    }
  }
}
