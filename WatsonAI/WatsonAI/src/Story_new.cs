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

    // HS: Single line comments are entities/verbs/relations that I think
    //     might need hiding/omitting from the graphs.
    //     Multi line comments are entities/verbs/relations that I am unsure
    //     how to represent.
    static Story()
    {
      var entities = new EntityBuilder {
        "actress", "butler", "countess", "earl",
        "gangster", "scrap", "nightshade", "belongings", /*"fast-acting",*/
        "blackcurrants", /*"dining room",*/ "letter", /*"master bedroom",*/
        "arsenic", /*"rat poison",*/ "kitchen", "plants", "nervous", /*"barbital tolerance",*/
        "barbital", /*"sleeping aid",*/, "bathroom", "book", "estate", "promotion",
        "war", "note", "will", "desk", "study", /*"slow-acting",*/"herbology"
      };
      var verbs = new VerbBuilder {
        "study", "have", "about", "contain", "own", "poison", "on", "fight",
        "resent", "want", "prevent", "marry", "use", "employ", "owe", "meet",
        "send", "receive"
      };
      var universeKnowledgeBuilder = new KnowledgeBuilder(entities, verbs)
      {
        //{"actress", "be", "murderer"},
        /*{"actress", "be", "earl's daughter"},*/
        {"actress", "study", "herbology"},
        {"actress", "have", "scrap"},
        {"scrap", "about", "nightshade"},
        {"belongings", "contain", "nightshade"},
        {"colonel", "own", "belongings"},
        /*{"nightshade", "is", "fast-acting"},*/ 
        /*{"nightshade", "look like", "blackcurrants"},*/
        /*{"dining room", "contain", "blackcurrants"},*/
        //{"nightshade", "poison", "earl"},
        /*{"actress", "gets", "contents of will"},*/
        {"will", "on", "desk"},
        {"study", "contain", "desk"},
        {"study", "contain", "will"},
        {"earl", "fight", "war"},
        /*{"earl", "friends with", "colonel"},*/ //Probably just a like relation
        {"colonel", "fight", "war"},
        {"colonel", "resent", "earl"},
        {"colonel", "want", "promotion"},
        {"earl", "prevent", "promotion"},
        {"note", "about", "promotion"},
        {"earl", "marry", "countess"},
        {"countess", "marry", "earl"},
        /*{"countess", "wants to sell", "estate"},*/
        {"earl", "use", "barbital"},
        {"book", "about", "barbital"},
        /*{"barbital", "is", "sleeping aid"},*/
        {"bathroom", "contain", "barbital"},
        /*{"earl", "have", "barbital tolerance"},*/
        {"earl", "employ", "butler"},
        /*{"earl", "stealing from", "earl"},*/
        {"butler", "is", "nervous"},
        /*{"butler", "allergic to", "plants"},*/
        /*{"butler", "have", "rat poison"},*/
        /*{"kitchen", "contain", "rat poison"},*/
        {"earl", "owe", "gangster"},
        {"earl", "meet", "gangster"},
        {"gangster", "meet", "earl"},
        /*{"gangster", "kill with", "arsenic"},*/
        /*{"arsenic", "is", "fast-acting"},*/
        /*{"arsenic, "used as", "rat poison"},*/
        /*{"master bedroom", "contain", "letter"},*/
        {"gangster", "send", "letter"}, 
        {"earl", "receive", "letter"}
      };
      Associations = universeKnowledgeBuilder.Associations;
      Knowledge = universeKnowledgeBuilder.Knowledge;

      Characters = new Dictionary<Names, Character>
      {
        {Names.ACTRESS, new Character("actress", true)},
        {Names.COUNTESS, new Character("countess", false)},
        {Names.COLONEL, new Character("colonel", false)},
        {Names.GANGSTER, new Character("gangster", false)},
        {Names.POLICE, new Character("policeman", false)},
        {Names.BUTLER, new Character("butler", false)},
      };

      var characterKnowledgeBuilders = new List<KnowledgeBuilder>
      {
        // Actress
        new KnowledgeBuilder(entities, verbs)
        {},
        // Countess
        new KnowledgeBuilder(entities, verbs)
        {},
        // Colonel
        new KnowledgeBuilder(entities, verbs)
        {},
        // Gangster
        new KnowledgeBuilder(entities, verbs)
        {},
        // Police
        new KnowledgeBuilder(entities, verbs)
        {},
        // Butler
        new KnowledgeBuilder(entities, verbs)
        {}
      };

      foreach (var c in Characters)
      {
        c.Value.Knowledge = characterKnowledgeBuilders[(int) c.Key].Knowledge;
      }
    }
  }
}