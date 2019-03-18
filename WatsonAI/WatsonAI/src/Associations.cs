using System;
using System.Collections.Generic;

namespace WatsonAI
{
  /// <summary>
  /// Stores associations between an <see cref="WatsonAI.Entity"/> or
  /// <see cref="WatsonAI.Verb"/> and its respective <see cref="string"/>.
  /// </summary>
  public class Associations
  {
    private readonly Dictionary<string, Entity> entities;
    private readonly Dictionary<string, Verb> verbs;

    /// <summary>
    /// Initializes a new instance of the <see cref="T:WatsonAI.Associations"/>
    /// class from data in <paramref name="path"/>
    /// </summary>
    /// <param name="path">Path to the data.</param>
    public Associations(string path)
    {
      throw new NotImplementedException();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:WatsonAI.Associations"/>
    /// class from dictionaries containing the associations.
    /// </summary>
    /// <param name="entities">The associations of <see cref="WatsonAI.Entity"/>.</param>
    /// <param name="verbs">The associations of <see cref="WatsonAI.Verb"/>.</param>
    public Associations(Dictionary<string, Entity> entities, Dictionary<string, Verb> verbs)
    {
      this.entities = entities;
      this.verbs = verbs;
    }

    /// <summary>
    /// Tries to get the <see cref="WatsonAI.Entity"/> associated with
    /// <paramref name="word"/>.
    /// </summary>
    /// <returns><c>true</c>, if an <see cref="WatsonAI.Entity"/> was present,
    /// <c>false</c> otherwise.</returns>
    /// <param name="word">The word to look up.</param>
    /// <param name="entity">The <see cref="WatsonAI.Entity"/> in which the
    /// result will be stored.</param>
    public bool TryGetEntity(string word, out Entity entity)
    {
      return entities.TryGetValue(word, out entity);
    }

    /// <summary>
    /// Tries to get the <see cref="WatsonAI.Verb"/> associated with
    /// <paramref name="word"/>.
    /// </summary>
    /// <returns><c>true</c>, if a <see cref="WatsonAI.Verb"/> was present,
    /// <c>false</c> otherwise.</returns>
    /// <param name="word">The word to look up.</param>
    /// <param name="verb">The <see cref="WatsonAI.Verb"/> in which the
    /// result will be stored.</param>
    public bool TryGetVerb(string word, out Verb verb)
    {
      return verbs.TryGetValue(word, out verb);
    }

    /// <summary>
    /// Tries to name the <see cref="WatsonAI.Entity"/>.
    /// </summary>
    /// <returns><c>true</c>, if a name was found for <paramref name="entity"/>,
    /// <c>false</c> otherwise.</returns>
    /// <param name="entity">The <see cref="WatsonAI.Entity"/> to name.</param>
    /// <param name="name">The <see cref="string"/> in which the name is
    /// stored.</param>
    public bool TryNameEntity(Entity entity, out string name)
    {
      foreach (var pair in entities)
      {
        if (pair.Value == entity)
        {
          name = pair.Key;
          return true;
        }
      }
      name = null;
      return false;
    }

    /// <summary>
    /// Tries to name the <see cref="WatsonAI.Verb"/>.
    /// </summary>
    /// <returns><c>true</c>, if a name was found for <paramref name="verb"/>,
    /// <c>false</c> otherwise.</returns>
    /// <param name="verb">The <see cref="WatsonAI.Verb"/> to name.</param>
    /// <param name="name">The <see cref="string"/> in which the name is
    /// stored.</param>
    public bool TryNameVerb(Verb verb, out string name)
    {
      foreach (var pair in verbs)
      {
        if (pair.Value == verb)
        {
          name = pair.Key;
          return true;
        }
      }
      name = null;
      return false;
    }
  }
}
