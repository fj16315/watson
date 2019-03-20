using System.Collections.Generic;

namespace WatsonAI
{
  /// <summary>
  /// Stores associations between an <see cref="WatsonAI.Entity"/> or
  /// <see cref="WatsonAI.Verb"/> and its respective <see cref="string"/>.
  /// </summary>
  public class Associations
  {
    private readonly Dictionary<Entity, string> entities;
    private readonly Dictionary<Verb, string> verbs;

    /// <summary>
    /// Initializes a new instance of the <see cref="T:WatsonAI.Associations"/>
    /// class with no associations.
    /// </summary>
    public Associations()
    {
      entities = new Dictionary<Entity, string>();
      verbs = new Dictionary<Verb, string>();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:WatsonAI.Associations"/>
    /// class from dictionaries containing the associations.
    /// </summary>
    /// <param name="entities">The associations of <see cref="WatsonAI.Entity"/>.</param>
    /// <param name="verbs">The associations of <see cref="WatsonAI.Verb"/>.</param>
    public Associations(Dictionary<Entity, string> entities, Dictionary<Verb, string> verbs)
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
      foreach (var pair in entities)
      {
        if (pair.Value == word)
        {
          entity = pair.Key;
          return true;
        }
      }
      entity = new Entity();
      return false;
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
      foreach (var pair in verbs)
      {
        if (pair.Value == word)
        {
          verb = pair.Key;
          return true;
        }
      }
      verb = new Verb();
      return false;
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
      => entities.TryGetValue(entity, out name);

    /// <summary>
    /// Tries to name the <see cref="WatsonAI.Verb"/>.
    /// </summary>
    /// <returns><c>true</c>, if a name was found for <paramref name="verb"/>,
    /// <c>false</c> otherwise.</returns>
    /// <param name="verb">The <see cref="WatsonAI.Verb"/> to name.</param>
    /// <param name="name">The <see cref="string"/> in which the name is
    /// stored.</param>
    public bool TryNameVerb(Verb verb, out string name)
      => verbs.TryGetValue(verb, out name);

    /// <summary>
    /// Associates a name with an <see cref="WatsonAI.Entity"/>.
    /// </summary>
    /// <returns><c>true</c>, if the entity name was added, <c>false</c>
    /// otherwise.</returns>
    /// <param name="entity">Entity.</param>
    /// <param name="name">Name.</param>
    public bool AddEntityName(Entity entity, string name)
    {
      if (entities.ContainsKey(entity) || entities.ContainsValue(name))
      {
        return false;
      }

      entities.Add(entity, name);
      return true;
    }

    /// <summary>
    /// Associates a name with an <see cref="WatsonAI.Verb"/>.
    /// </summary>
    /// <returns><c>true</c>, if the verb name was added, <c>false</c>
    /// otherwise.</returns>
    /// <param name="verb">Verb.</param>
    /// <param name="name">Name.</param>
    public bool AddVerbName(Verb verb, string name)
    {
      if (verbs.ContainsKey(verb) || verbs.ContainsValue(name))
      {
        return false;
      }

      verbs.Add(verb, name);
      return true;
    }

    /// <summary>
    /// Removes the name of the entity.
    /// </summary>
    /// <returns><c>true</c>, if the entity name was removed, <c>false</c>
    /// otherwise.</returns>
    /// <param name="entity">Entity.</param>
    public bool RemoveEntityName(Entity entity)
    {
      return entities.Remove(entity);
    }

    /// <summary>
    /// Removes the name of the verb.
    /// </summary>
    /// <returns><c>true</c>, if the verb name was removed, <c>false</c>
    /// otherwise.</returns>
    /// <param name="verb">Verb.</param>
    public bool RemoveVerbName(Verb verb)
    {
      return verbs.Remove(verb);
    }
  }
}
