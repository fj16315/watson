using System;

namespace GameAI
{
  /// <summary>
  /// Stores the relationships between two nodes in <see cref="GameAI.KnowledgeGraph"/>.
  /// </summary>
  public struct Relationships
  {
    /// <summary>
    /// The internal enum used by <see cref="GameAI.Relationships"/>.
    /// </summary>
    /// <remarks>
    /// Stored in one-hot encoding to allow for or-ing of multiple relationships.
    /// Named against standard because of clash with class name.
    /// </remarks>
    [Flags]
    public enum Flags : int
    {
      None = 0, Contains = 1,
      Owns = 2, Wants = 4
    };

    /// <summary>
    /// The member of <see cref="GameAI.Relationships"/> that stores the relationships (as <see cref="GameAI.Relationships.Flags"/>).
    /// </summary>
    public Flags relationships
    {
      get;
      set;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:GameAI.Relationships"/> struct.
    /// </summary>
    /// <param name="flags">The relationships for this new instance.</param>
    public Relationships(Flags flags)
    {
      relationships = flags;
    }

    /// <summary>
    /// Checks if there are no relationships.
    /// </summary>
    /// <returns><c>true</c>, if <see cref="GameAI.Relationships.relationships"/> is <see cref="GameAI.Relationships.Flags.None"/>, <c>false</c> otherwise.</returns>
    public bool IsNone()
      => this.relationships == Flags.None;

    /// <summary>
    /// Checks if all of the relationships in <c>that</c> are also present in <c>this</c>.
    /// </summary>
    /// <returns><c>true</c> if all of the relationships in <c>that</c> are present in <c>this</c>.</returns>
    /// <param name="that">The <see cref="GameAI.Relationships"/> which should be contained in <c>this</c>.</param>
    public bool Contains(Relationships that)
      => (this & that) == that;

    /// <summary>
    /// Determines whether a specified instance of <see cref="GameAI.Relationships"/> is equal to another specified <see cref="GameAI.Relationships"/>.
    /// </summary>
    /// <param name="lhs">The first <see cref="GameAI.Relationships"/> to compare.</param>
    /// <param name="rhs">The second <see cref="GameAI.Relationships"/> to compare.</param>
    /// <returns><c>true</c> if <c>lhs</c> and <c>rhs</c> are equal; otherwise, <c>false</c>.</returns>
    public static bool operator ==(Relationships lhs, Relationships rhs)
      => lhs.relationships == rhs.relationships;

    /// <summary>
    /// Determines whether a specified instance of <see cref="GameAI.Relationships"/> is not equal to another specified <see cref="GameAI.Relationships"/>.
    /// </summary>
    /// <param name="lhs">The first <see cref="GameAI.Relationships"/> to compare.</param>
    /// <param name="rhs">The second <see cref="GameAI.Relationships"/> to compare.</param>
    /// <returns><c>true</c> if <c>lhs</c> and <c>rhs</c> are not equal; otherwise, <c>false</c>.</returns>
    public static bool operator !=(Relationships lhs, Relationships rhs)
      => !(lhs == rhs);

    /// <summary>
    /// The union (<c>and</c>) of two <see cref="GameAI.Relationships"/>.
    /// </summary>
    /// <returns>A new <see cref="GameAI.Relationships"/> containing the union of the relationships in <c>lhs</c> and <c>rhs</c>.</returns>
    /// <param name="lhs">The left hand side of the expression.</param>
    /// <param name="rhs">The right hand side of the expression.</param>
    public static Relationships operator &(Relationships lhs, Relationships rhs)
      => new Relationships(lhs.relationships & rhs.relationships);

    /// <summary>
    /// The inclusive disjuction (<c>or</c>) of two <see cref="GameAI.Relationships"/>.
    /// </summary>
    /// <returns>A new <see cref="GameAI.Relationships"/> containing the inclusive disjuction of the relationships in <c>lhs</c> and <c>rhs</c>.</returns>
    /// <param name="lhs">The left hand side of the expression.</param>
    /// <param name="rhs">The right hand side of the expression.</param>
    public static Relationships operator |(Relationships lhs, Relationships rhs)
      => new Relationships(lhs.relationships | rhs.relationships);

    // /// <summary>
    // /// The exclusive disjuction (<c>xor</c>) of two <see cref="GameAI.Relationships"/>.
    // /// </summary>
    // /// <returns>A new <see cref="GameAI.Relationships"/> containing the exclusive disjucntion of the relationships in <c>lhs</c> and <c>rhs</c>.</returns>
    // /// <param name="lhs">The left hand side of the expression.</param>
    // /// <param name="rhs">The right hand side of the expression.</param>
    // public static Relationships operator ^(Relationships lhs, Relationships rhs)
    //   => new Relationships(lhs.relationships ^ rhs.relationships);

    /// <summary>
    /// Determines whether the specified <see cref="object"/> is equal to the current <see cref="T:GameAI.Relationships"/>.
    /// </summary>
    /// <param name="obj">The <see cref="object"/> to compare with the current <see cref="T:GameAI.Relationships"/>.</param>
    /// <returns><c>true</c> if the specified <see cref="object"/> is equal to the current
    /// <see cref="T:GameAI.Relationships"/>; otherwise, <c>false</c>.</returns>
    public override bool Equals(object obj)
    {
      if ( obj is Relationships rel)
      {
        return this == rel;
      }
      return false;
    }

    /// <summary>
    /// Serves as a hash function for a <see cref="T:GameAI.Relationships"/> object.
    /// </summary>
    /// <returns>A hash code for this instance that is suitable for use in hashing algorithms and data structures such as a hash table.</returns>
    public override int GetHashCode()
      => (int) relationships;
  }
}
