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

    public bool TryGetEntity(string word, out Entity entity)
    {
      return entities.TryGetValue(word, out entity);
    }

    public bool TryGetVerb(string word, out Verb verb)
    {
      return verbs.TryGetValue(word, out verb);
    }

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
