using System;
using System.Collections.Generic;

namespace WatsonAI
{
  /// <summary>
  /// Creates a Knowledge object and its Associations based on hard-coded story
  /// </summary>
  public static class Story
  {
    public enum Names : int { ACTRESS, BUTLER, COLONEL, COUNTESS, EARL, GANGSTER, POLICE };
    public static Dictionary<Names, Character> Characters { get; }
    public static Knowledge Knowledge;
    public static Associations Associations;
    public static EntityBuilder entities;
    public static VerbBuilder verbs;

    static Story()
    {
      entities = new EntityBuilder {
        "actress", "butler", "countess", "earl",
        "gangster", "money", "promotion", "money",
        "study", "will", "belonging", "letter",
        "colonel", "book", "arsenic", "barbital",
        "nightshade", "murderer", "dead", "music", "chocolate",
        "house", "earth", "england", "universe", "dorset", "policeman"
      };
      verbs = new VerbBuilder {
        "poison", "marry", "owe", "employ",
        "inherit", "prevent", "launder", "contain",
        "own", "resent", "do", "blackmail",
        "use", "kill", "be", "love", "like"
      };
      var universeKnowledgeBuilder = new KnowledgeBuilder(entities, verbs)
      {
        //{"actress", "be", "murderer"},
        {"earl", "love", "countess"},
        {"countess", "love", "earl"},
        {"countess", "love", "chocolate"},
        {"earl", "like", "music"},
        {"actress", "poison", "earl"},
        {"earl", "be", "dead"},
        {"actress", "kill", "earl"},
        {"countess", "marry", "earl"},
        {"earl", "marry", "countess"},
        {"earl", "owe", "gangster"},
        {"earl", "employ", "butler"},
        {"actress", "inherit", "money"},
        {"earl", "prevent", "promotion"},
        {"butler", "launder", "money"},
        {"study", "contain", "will"},
        {"house", "contain", "study"},
        {"dorset", "contain", "house"},
        {"england", "contain", "house"},
        {"earth", "contain", "england"},
        {"universe", "contain", "england"},
        {"earl", "own", "belonging"},
        {"belonging", "contain", "letter"},
        {"colonel", "resent", "earl"},
        {"butler", "do", "book"}, // butler do book?
        {"gangster", "blackmail", "butler"},
        {"butler", "own", "arsenic"},
        {"earl", "use", "barbital"},
        {"belonging", "contain", "nightshade"},
      };
      Associations = universeKnowledgeBuilder.Associations;
      Knowledge = universeKnowledgeBuilder.Knowledge;

      Characters = new Dictionary<Names, Character>
      {
        {Names.ACTRESS, new Character("actress", true, Gender.Female)},
        {Names.COUNTESS, new Character("countess", false, Gender.Female)},
        {Names.COLONEL, new Character("colonel", false, Gender.Male)},
        {Names.EARL, new Character("earl", false, Gender.Male)},
        {Names.GANGSTER, new Character("gangster", false, Gender.Male)},
        {Names.POLICE, new Character("policeman", false, Gender.Male)},
        {Names.BUTLER, new Character("butler", false, Gender.Male)},
      };

      //var characterKnowledgeBuilders = new List<KnowledgeBuilder>
      var characterKnowledgeBuilders = new Dictionary<Names, KnowledgeBuilder>
      {
        {Names.ACTRESS,
        new KnowledgeBuilder(entities, verbs)
        {
          {"actress", "poison", "earl"},
          {"earl", "love", "countess"},
          {"countess", "love", "earl"},
          {"earl", "like", "music"},
          {"earl", "be", "dead"},
          {"actress", "kill", "earl"},
          {"countess", "marry", "earl"},
          {"earl", "marry", "countess"},
          {"earl", "employ", "butler"},
          {"actress", "inherit", "money"},
          {"study", "contain", "will"},
          {"earl", "own", "belonging"},
          {"earl", "use", "barbital"}
        }},
        {Names.BUTLER,
        new KnowledgeBuilder(entities, verbs)
        {
          {"earl", "love", "countess"},
          {"countess", "love", "earl"},
          {"earl", "like", "music"},
          {"earl", "be", "dead"},
          {"countess", "marry", "earl"},
          {"earl", "marry", "countess"},
          {"earl", "employ", "butler"},
          {"actress", "inherit", "money"},
          {"study", "contain", "will"},
          {"earl", "own", "belonging"},
          {"earl", "use", "barbital"},
          {"countess", "love", "chocolate"},
          {"belonging", "contain", "letter"},
          {"belonging", "contain", "nightshade"}
        }},
        {Names.COLONEL,
        new KnowledgeBuilder(entities, verbs)
        {
          {"actress", "poison", "earl"},
          {"earl", "love", "countess"},
          {"countess", "love", "earl"},
          {"earl", "like", "music"},
          {"earl", "be", "dead"},
          {"actress", "kill", "earl"},
          {"countess", "marry", "earl"},
          {"earl", "marry", "countess"},
          {"earl", "employ", "butler"},
          {"actress", "inherit", "money"},
          {"study", "contain", "will"},
          {"earl", "own", "belonging"},
          {"earl", "use", "barbital"}
        }},
        {Names.COUNTESS,
        new KnowledgeBuilder(entities, verbs)
        {
          {"actress", "poison", "earl"},
          {"earl", "love", "countess"},
          {"countess", "love", "earl"},
          {"earl", "like", "music"},
          {"earl", "be", "dead"},
          {"actress", "kill", "earl"},
          {"countess", "marry", "earl"},
          {"earl", "marry", "countess"},
          {"earl", "employ", "butler"},
          {"actress", "inherit", "money"},
          {"study", "contain", "will"},
          {"earl", "own", "belonging"},
          {"earl", "use", "barbital"}
        }},
        {Names.EARL,
        new KnowledgeBuilder(entities, verbs)},
        {Names.GANGSTER,
        new KnowledgeBuilder(entities, verbs)
        {
          {"actress", "poison", "earl"},
          {"earl", "love", "countess"},
          {"countess", "love", "earl"},
          {"earl", "like", "music"},
          {"earl", "be", "dead"},
          {"actress", "kill", "earl"},
          {"countess", "marry", "earl"},
          {"earl", "marry", "countess"},
          {"earl", "employ", "butler"},
          {"actress", "inherit", "money"},
          {"study", "contain", "will"},
          {"earl", "own", "belonging"},
          {"earl", "use", "barbital"}
        }},
        {Names.POLICE,
        new KnowledgeBuilder(entities, verbs)
        {
          {"actress", "poison", "earl"},
          {"earl", "love", "countess"},
          {"countess", "love", "earl"},
          {"earl", "like", "music"},
          {"earl", "be", "dead"},
          {"actress", "kill", "earl"},
          {"countess", "marry", "earl"},
          {"earl", "marry", "countess"},
          {"earl", "employ", "butler"},
          {"actress", "inherit", "money"},
          {"study", "contain", "will"},
          {"earl", "own", "belonging"},
          {"earl", "use", "barbital"}
        }}
      };

      foreach (var c in Characters)
      {
        c.Value.Knowledge = characterKnowledgeBuilders[c.Key].Knowledge;
      }
    }
  }
}