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

    private readonly Dictionary<string, Entity> entityNames;
    private readonly Dictionary<string, Verb> verbNames;

    /// <summary>
    /// Initializes a new instance of the <see cref="T:WatsonAI.Associations"/>
    /// class with no associations.
    /// </summary>
    public Associations()
    {
      entities = new Dictionary<Entity, string>();
      verbs = new Dictionary<Verb, string>();
      entityNames = new Dictionary<string, Entity>();
      verbNames = new Dictionary<string, Verb>();
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
      this.entityNames = new Dictionary<string, Entity>();
      foreach (var entity in entities.Keys)
      {
        this.entityNames.Add(entities[entity], entity);
      }
      this.verbNames = new Dictionary<string, Verb>();
      foreach (var verb in verbs.Keys)
      {
        this.verbNames.Add(verbs[verb], verb);
      }
    }

    /// <summary>
    /// Returns a list of words currently associated to entities.
    /// </summary>
    /// <returns>A list of words currently associated to entities.</returns>
    public IEnumerable<string> EntityNames()
      => entityNames.Keys;

    /// <summary>
    /// Returns a list of words currently associated to verbs.
    /// </summary>
    /// <returns>A list of words currently associated to verbs.</returns>
    public IEnumerable<string> VerbNames()
      => verbNames.Keys;

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
      => entityNames.TryGetValue(word, out entity);

    /// <summary>
    /// Gets the entity of given name (unsafe)
    /// </summary>
    /// <returns>The entity of given name.</returns>
    /// <param name="word">The word to look up.</param>
    public Entity UncheckedGetEntity(string word)
      => entityNames[word];

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
      => verbNames.TryGetValue(word, out verb);

    /// <summary>
    /// Gets the verb of given name (unsafe)
    /// </summary>
    /// <returns>The verb of given name.</returns>
    /// <param name="word">The word to look up.</param>
    public Verb UncheckedGetVerb(string word)
      => verbNames[word];

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

      entityNames.Add(name, entity);
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

      verbNames.Add(name, verb);
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
      return entityNames.Remove(entities[entity]) && entities.Remove(entity);
    }

    /// <summary>
    /// Removes the name of the verb.
    /// </summary>
    /// <returns><c>true</c>, if the verb name was removed, <c>false</c>
    /// otherwise.</returns>
    /// <param name="verb">Verb.</param>
    public bool RemoveVerbName(Verb verb)
    {
      return verbNames.Remove(verbs[verb]) && verbs.Remove(verb);
    }
  }
}
