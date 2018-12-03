using System;
using System.Collections;
using System.Collections.Generic;

namespace GameAI
{
  /// <summary>
  /// Defines a relationship between two nodes in the knowledge graph.
  /// </summary>
  public struct Relationship
  {
    /// <summary>
    /// Relationship.
    /// </summary>
    /// <remarks>
    /// Stored in one-hot encoding to allow for or-ing of multiple relationships for efficient storage.
    /// </remarks>
    [Flags]
    public enum Relationships : int
    {
      None = 0, Contains = 1,
      Owns = 2, Wants = 4
    };

    /// <summary>
    /// The relationship between two nodes in the knowledge graph, stored as the bitwise or of the Relationships.
    /// </summary>
    public Relationships relationship
    {
      get;
      set;
    }

    /// <summary>
    /// Unpack relationships into a list.
    /// </summary>
    /// <returns>
    /// List containing all relationships between the two nodes.
    /// </returns>
    public List<Relationships> relationships()
    {
      var relationships = new List<Relationships>();
      BitArray bitArray = new BitArray((int)relationship);
      for (var i = 0; i < sizeof(int); i++)
      {
        if (bitArray[i] == true)
        {
          relationships.Add((Relationships)Math.Pow(2, i));
        }
      }
      return relationships;
    }
  }
}
