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

    // HS: Single line comments are entities/verbs/relations that I think
    //     might need hiding/omitting from the graphs.
    //     Multi line comments are entities/verbs/relations that I am unsure
    //     how to represent.
    static Story()
    {
      entities = new EntityBuilder {
        "actress", "butler", "countess", "earl",
        "gangster", "colonel", "scrap", "nightshade", "belongings", /*"fast-acting",*/
        "blackcurrants", /*"dining room",*/ "letter", /*"master bedroom",*/
        "arsenic", /*"rat poison",*/ "kitchen", "plants", "nervous", /*"barbital tolerance",*/
        "barbital", /*"sleeping aid",*/ "bathroom", "book", "estate", "promotion",
        "war", "note", "will", "desk", "study", /*"slow-acting",*/"herbology"
      };
      var verbs = new VerbBuilder {
        "study", "have", "about", "contain", "own", "poison", "on", "fight",
        "resent", "want", "prevent", "marry", "use", "employ", "owe", "meet",
        "send", "receive", "be"
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
        {"study", "contain", "book"},
        /*{"barbital", "is", "sleeping aid"},*/
        {"bathroom", "contain", "barbital"},
        /*{"earl", "have", "barbital tolerance"},*/
        {"earl", "employ", "butler"},
        /*{"butler", "stealing from", "earl"},*/
        {"butler", "be", "nervous"},
        /*{"butler", "allergic to", "plants"},*/
        /*{"butler", "have", "rat poison"},*/
        /*{"kitchen", "contain", "rat poison"},*/
        {"earl", "owe", "gangster"},
        {"earl", "meet", "gangster"},
        {"gangster", "meet", "earl"},
        /*{"gangster", "kill with", "arsenic"},*/
        /*{"arsenic", "is", "slow-acting"},*/
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
        {Names.BUTLER, new Character("butler", false)},
        {Names.COLONEL, new Character("colonel", false)},
        {Names.COUNTESS, new Character("countess", false)},
        {Names.GANGSTER, new Character("gangster", false)},
        {Names.POLICE, new Character("policeman", false)},
      };

      //var characterKnowledgeBuilders = new List<KnowledgeBuilder>
      var characterKnowledgeBuilders = new Dictionary<Names, KnowledgeBuilder>
      {
        {
          Names.ACTRESS,
          new KnowledgeBuilder(entities, verbs)
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
            {"earl", "fight", "war"},
            /*{"earl", "friends with", "colonel"},*/ //Probably just a like relation
            {"colonel", "fight", "war"},
            {"earl", "marry", "countess"},
            {"countess", "marry", "earl"},
            /*{"barbital", "is", "sleeping aid"},*/
            /*{"arsenic, "used as", "rat poison"},*/
            {"earl", "employ", "butler"}
          }
        },
        {
          Names.BUTLER,
          new KnowledgeBuilder(entities, verbs)
          {
            /*{"nightshade", "look like", "blackcurrants"},*/
            /*{"dining room", "contain", "blackcurrants"},*/
            {"will", "on", "desk"},
            {"study", "contain", "desk"},
            {"study", "contain", "will"},
            {"earl", "fight", "war"},
            /*{"earl", "friends with", "colonel"},*/ //Probably just a like relation
            {"colonel", "fight", "war"},
            /*{"countess", "gets", "contents of will"},*/
            {"colonel", "want", "promotion"},
            {"earl", "marry", "countess"},
            {"countess", "marry", "earl"},
            /*{"countess", "wants to sell", "estate"},*/
            {"book", "about", "barbital"},
            {"study", "contain", "book"},
            {"earl", "employ", "butler"},
            // /*{"butler", "stealing from", "earl"},*/ // Not sure how to represent _and_ needs hiding/omitting in this graph
            {"butler", "be", "nervous"},
            /*{"butler", "allergic to", "plants"},*/
            /*{"butler", "have", "rat poison"},*/
            /*{"kitchen", "contain", "rat poison"},*/
            {"earl", "owe", "gangster"},
            {"earl", "meet", "gangster"},
            {"gangster", "meet", "earl"},
            /*{"gangster", "kill with", "arsenic"},*/
            /*{"arsenic, "used as", "rat poison"},*/
            /*{"master bedroom", "contain", "letter"},*/
            {"gangster", "send", "letter"},
            {"earl", "receive", "letter"}
          }
        },
        {
          Names.COLONEL,
          new KnowledgeBuilder(entities, verbs)
          {
            /*{"actress", "be", "earl's daughter"},*/
            {"colonel", "own", "belongings"},
            /*{"nightshade", "is", "fast-acting"},*/
            /*{"dining room", "contain", "blackcurrants"},*/
            /*{"countess", "gets", "contents of will"},*/
            {"will", "on", "desk"},
            {"study", "contain", "desk"},
            {"study", "contain", "will"},
            {"earl", "fight", "war"},
            /*{"earl", "friends with", "colonel"},*/ //Probably just a like relation
            {"colonel", "fight", "war"},
            {"colonel", "resent", "earl"},
            {"colonel", "want", "promotion"},
            {"earl", "prevent", "promotion"},
            {"earl", "marry", "countess"},
            {"countess", "marry", "earl"},
            {"earl", "employ", "butler"},
            /*{"butler", "have", "rat poison"},*/
             /*{"arsenic, "used as", "rat poison"},*/
            {"earl", "owe", "gangster"}
          }
        },
        {
          Names.COUNTESS,
          new KnowledgeBuilder(entities, verbs)
          {
            {"actress", "study", "herbology"},
            /*{"dining room", "contain", "blackcurrants"},*/
            /*{"countess", "gets", "contents of will"},*/
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
            /*{"barbital", "is", "sleeping aid"},*/
            /*{"earl", "have", "barbital tolerance"},*/
            {"earl", "employ", "butler"},
            {"butler", "be", "nervous"},
            /*{"butler", "have", "rat poison"},*/
            /*{"kitchen", "contain", "rat poison"},*/
            {"earl", "meet", "gangster"},
            {"gangster", "meet", "earl"},
            /*{"arsenic, "used as", "rat poison"},*/
            /*{"master bedroom", "contain", "letter"},*/
            {"earl", "receive", "letter"}
          }
        },
        {
          Names.EARL,
          new KnowledgeBuilder(entities, verbs)
        },
        {
          Names.GANGSTER,
          new KnowledgeBuilder(entities, verbs)
          {
            /*{"nightshade", "is", "fast-acting"},*/
            /*{"dining room", "contain", "blackcurrants"},*/
            /*{"earl", "friends with", "colonel"},*/ //Probably just a like relation
            {"colonel", "fight", "war"},
            {"earl", "marry", "countess"},
            {"countess", "marry", "earl"},
            /*{"butler", "stealing from", "earl"},*/
            {"butler", "be", "nervous"},
            {"earl", "owe", "gangster"},
            {"earl", "meet", "gangster"},
            {"gangster", "meet", "earl"},
            /*{"gangster", "kill with", "arsenic"},*/
            /*{"arsenic", "is", "slow-acting"},*/
            /*{"arsenic, "used as", "rat poison"},*/
            /*{"master bedroom", "contain", "letter"},*/
            {"gangster", "send", "letter"},
            {"earl", "receive", "letter"}
          }
        },
        {
          Names.POLICE,
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
          }
        }
      };

      foreach (var c in Characters)
      {
        c.Value.Knowledge = characterKnowledgeBuilders[c.Key].Knowledge;
      }
    }
  }
}