using System;
using System.Collections.Generic;

namespace GameAI
{
  /// <summary>
  /// Stores the Relation between two nodes in <see cref="GameAI.KnowledgeGraph"/>.
  /// </summary>
  [Serializable()]
  public struct Relation
  {
    /// <summary>
    /// The member of <see cref="GameAI.Relation"/> that stores the Relation (as <see cref="GameAI.Relation.Flags"/>).
    /// </summary>
    private int relation;

    public Relation(int r)
    {
      this.relation = r;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:GameAI.Relation"/> struct.
    /// </summary>
    /// <param name="flags">The Relation for this new instance.</param>
    public Relation(List<SingleRelation> singleRelations)
    {
      this.relation = 0;

      foreach (var r in singleRelations)
      {
        this |= r.AsRelation();
      }
    }

    /// <summary>
    /// Checks if there are no Relation.
    /// </summary>
    /// <returns><c>true</c>, if <see cref="GameAI.Relation.Relation"/> is <see cref="GameAI.Relation.Flags.None"/>, <c>false</c> otherwise.</returns>
    public bool IsNone()
      => this.relation == 0;

    /// <summary>
    /// Checks if all of the Relation in <c>that</c> are also present in <c>this</c>.
    /// </summary>
    /// <returns><c>true</c> if all of the Relation in <c>that</c> are present in <c>this</c>.</returns>
    /// <param name="that">The <see cref="GameAI.Relation"/> which should be contained in <c>this</c>.</param>
    public bool Contains(Relation that)
      => (this & that) == that;

    /// <summary>
    /// Determines whether a specified instance of <see cref="GameAI.Relation"/> is equal to another specified <see cref="GameAI.Relation"/>.
    /// </summary>
    /// <param name="lhs">The first <see cref="GameAI.Relation"/> to compare.</param>
    /// <param name="rhs">The second <see cref="GameAI.Relation"/> to compare.</param>
    /// <returns><c>true</c> if <c>lhs</c> and <c>rhs</c> are equal; otherwise, <c>false</c>.</returns>
    public static bool operator ==(Relation lhs, Relation rhs)
      => lhs.relation == rhs.relation;

    /// <summary>
    /// Determines whether a specified instance of <see cref="GameAI.Relation"/> is not equal to another specified <see cref="GameAI.Relation"/>.
    /// </summary>
    /// <param name="lhs">The first <see cref="GameAI.Relation"/> to compare.</param>
    /// <param name="rhs">The second <see cref="GameAI.Relation"/> to compare.</param>
    /// <returns><c>true</c> if <c>lhs</c> and <c>rhs</c> are not equal; otherwise, <c>false</c>.</returns>
    public static bool operator !=(Relation lhs, Relation rhs)
      => !(lhs == rhs);

    /// <summary>
    /// The union (<c>and</c>) of two <see cref="GameAI.Relation"/>.
    /// </summary>
    /// <returns>A new <see cref="GameAI.Relation"/> containing the union of the Relation in <c>lhs</c> and <c>rhs</c>.</returns>
    /// <param name="lhs">The left hand side of the expression.</param>
    /// <param name="rhs">The right hand side of the expression.</param>
    public static Relation operator &(Relation lhs, Relation rhs)
      => new Relation(lhs.relation & rhs.relation);

    /// <summary>
    /// The inclusive disjuction (<c>or</c>) of two <see cref="GameAI.Relation"/>.
    /// </summary>
    /// <returns>A new <see cref="GameAI.Relation"/> containing the inclusive disjuction of the Relation in <c>lhs</c> and <c>rhs</c>.</returns>
    /// <param name="lhs">The left hand side of the expression.</param>
    /// <param name="rhs">The right hand side of the expression.</param>
    public static Relation operator |(Relation lhs, Relation rhs)
      => new Relation(lhs.relation | rhs.relation);

    // /// <summary>
    // /// The exclusive disjuction (<c>xor</c>) of two <see cref="GameAI.Relation"/>.
    // /// </summary>
    // /// <returns>A new <see cref="GameAI.Relation"/> containing the exclusive disjucntion of the Relation in <c>lhs</c> and <c>rhs</c>.</returns>
    // /// <param name="lhs">The left hand side of the expression.</param>
    // /// <param name="rhs">The right hand side of the expression.</param>
    // public static Relation operator ^(Relation lhs, Relation rhs)
    //   => new Relation(lhs.Relation ^ rhs.Relation);

    /// <summary>
    /// Determines whether the specified <see cref="object"/> is equal to the current <see cref="T:GameAI.Relation"/>.
    /// </summary>
    /// <param name="obj">The <see cref="object"/> to compare with the current <see cref="T:GameAI.Relation"/>.</param>
    /// <returns><c>true</c> if the specified <see cref="object"/> is equal to the current
    /// <see cref="T:GameAI.Relation"/>; otherwise, <c>false</c>.</returns>
    public override bool Equals(object obj)
    {
      var rel = obj as Relation?;
      if (rel != null)
      {
        return this == rel;
      }
      return false;
    }

    /// <summary>
    /// Returns a copy of the wrapped <see cref="int"/>.
    /// </summary>
    /// <returns>The wrapped <see cref="int"/>.</returns>
    /// <param name="r">The wrapping <see cref="GameAI.Relation"/>.</param>
    public static explicit operator int(Relation r)
      => r.relation;

    public SingleRelation? AsSingleRelation()
    {
      for (int r = 0; r < sizeof(int) * 8; ++r)
      {
        if (relation == (1 << r))
        {
          return new SingleRelation(r);
        }
      }
      return null;
    }

    public List<SingleRelation> SingleRelations()
    {
      var singleRelations = new List<SingleRelation>();
      for (int r = 0; r < sizeof(int) * 8; ++r)
      {
        if (relation == (1 << r))
        {
          singleRelations.Add(new SingleRelation(r));
        }

      }
      return singleRelations;
    }

    /// <summary>
    /// Serves as a hash function for a <see cref="T:GameAI.Relation"/> object.
    /// </summary>
    /// <returns>A hash code for this instance that is suitable for use in hashing algorithms and data structures such as a hash table.</returns>
    public override int GetHashCode()
      => this.relation;
  }

  /// <summary>
  /// Wrapper type around <see cref="int"/> for safety.
  /// </summary>
  public struct SingleRelation
  {
    private int _n;

    /// <summary>
    /// Initializes a new instance of the <see cref="GameAI.SingleRelation"/> struct.
    /// </summary>
    /// <param name="n">The <see cref="int"/> to wrap.</param>
    public SingleRelation(int n) 
    {
      _n = n;
    }

    /// <summary>
    /// Returns a copy of the wrapped <see cref="int"/>.
    /// </summary>
    /// <returns>The wrapped <see cref="int"/>.</returns>
    /// <param name="r">The wrapping <see cref="GameAI.SingleRelation"/>.</param>
    public static explicit operator int(SingleRelation r)
      => r._n;

    public Relation AsRelation()
      => new Relation(1 << _n);
  }
}

