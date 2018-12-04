using System;
using System.Collections;
using System.Collections.Generic;

namespace GameAI
{
  /// <summary>
  /// Defines all relationships between two nodes in the knowledge graph.
  /// </summary>
  public struct Relationships
  {
    /// <summary>
    /// Relationship.
    /// </summary>
    /// <remarks>
    /// Stored in one-hot encoding to allow for or-ing of multiple relationships for efficient storage.
    /// Named against standard because of clash with class name.
    /// </remarks>
    [Flags]
    public enum RelationshipFlags : int
    {
      None = 0, Contains = 1,
      Owns = 2, Wants = 4
    };

    /// <summary>
    /// The relationships between two nodes in the knowledge graph, stored as the bitwise or of the Relationships.
    /// </summary>
    public RelationshipFlags relationships
    {
      get;
      set;
    }
  }
}
