using System;
using System.Collections.Generic;

namespace WatsonAI
{
  /// <summary>
  /// Creates a Knowledge object and its Associations based on hard-coded story
  /// </summary>
  public static class Story
  {
    private static readonly KnowledgeBuilder universeKnowledgeBuilder;
    public static List<Character> Characters { get; }
    /*public static Knowledge Knowledge
    {
      get
      {
        return universeKnowledgeBuilder.Knowledge;
      }
    }
    public static Associations Associations
    {
      get
      {
        return universeKnowledgeBuilder.Associations;
      }
    }*/
    public static Knowledge Knowledge { get; }
    public static Associations Associations { get; }

    static Story()
    {
      var entities = new EntityBuilder {
        "actress", "butler", "countess", "earl",
        "gangster", "money", "promotion", "money",
        "study", "will", "belonging", "letter",
        "colonel", "book", "arsenic", "barbital",
        "nightshade", "murderer", "dead", "music", "chocolate",
        "policeman"
      };
      var verbs = new VerbBuilder {
        "poison", "marry", "owe", "employ",
        "inherit", "prevent", "launder", "contain",
        "own", "resent", "do", "blackmail",
        "use", "kill", "be", "love", "like"};
      universeKnowledgeBuilder = new KnowledgeBuilder(entities, verbs)
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
        {"earl", "own", "belonging"},
        {"belonging", "contain", "letter"},
        {"colonel", "resent", "earl"},
        {"butler", "do", "book"}, // butler do book?
        {"gangster", "blackmail", "butler"},
        {"butler", "own", "arsenic"},
        {"earl", "use", "barbital"},
        {"belonging", "contain", "nightshade"},
      };

      Knowledge = universeKnowledgeBuilder.Knowledge;
      Associations = universeKnowledgeBuilder.Associations;
      Characters = new List<Character>();
      //Console.WriteLine("Associations assigned to Story");

      var characterKnowledgeBuilders = new List<KnowledgeBuilder>();

      Characters.Add(new Character("actress", true));
      Characters.Add(new Character("countess", false));
      Characters.Add(new Character("colonel", false));
      Characters.Add(new Character("gangster", false));
      Characters.Add(new Character("policeman", false));
      Characters.Add(new Character("butler", false));

      foreach (var c in Characters)
      {
        var characterKnowledgeBuilder = new KnowledgeBuilder(entities, verbs);
        characterKnowledgeBuilders.Add(characterKnowledgeBuilder);
      }

      characterKnowledgeBuilders[0].Add("actress", "poison", "earl");
      characterKnowledgeBuilders[0].Add("earl", "love", "countess");
      characterKnowledgeBuilders[0].Add("countess", "love", "earl");
      characterKnowledgeBuilders[0].Add("earl", "like", "music");
      characterKnowledgeBuilders[0].Add("earl", "be", "dead");
      characterKnowledgeBuilders[0].Add("actress", "kill", "earl");
      characterKnowledgeBuilders[0].Add("countess", "marry", "earl");
      characterKnowledgeBuilders[0].Add("earl", "marry", "countess");
      characterKnowledgeBuilders[0].Add("earl", "employ", "butler");
      characterKnowledgeBuilders[0].Add("actress", "inherit", "money");
      characterKnowledgeBuilders[0].Add("study", "contain", "will");
      characterKnowledgeBuilders[0].Add("earl", "own", "belonging");
      characterKnowledgeBuilders[0].Add("earl", "use", "barbital");
      Characters[0].Knowledge = characterKnowledgeBuilders[0].Knowledge;

      characterKnowledgeBuilders[1].Add("earl", "love", "countess");
      characterKnowledgeBuilders[1].Add("countess", "love", "earl");
      characterKnowledgeBuilders[1].Add("earl", "like", "music");
      characterKnowledgeBuilders[1].Add("earl", "be", "dead");
      characterKnowledgeBuilders[1].Add("countess", "marry", "earl");
      characterKnowledgeBuilders[1].Add("earl", "marry", "countess");
      characterKnowledgeBuilders[1].Add("earl", "employ", "butler");
      characterKnowledgeBuilders[1].Add("countess", "inherit", "money");
      characterKnowledgeBuilders[1].Add("study", "contain", "will");
      characterKnowledgeBuilders[1].Add("earl", "own", "belonging");
      characterKnowledgeBuilders[1].Add("earl", "use", "barbital");
      characterKnowledgeBuilders[1].Add("countess", "love", "chocolate");
      characterKnowledgeBuilders[1].Add("belonging", "contain", "letter");
      characterKnowledgeBuilders[1].Add("belonging", "contain", "nightshade");
      Characters[1].Knowledge = characterKnowledgeBuilders[1].Knowledge;

      Characters[2].Knowledge = universeKnowledgeBuilder.Knowledge;
      Characters[3].Knowledge = universeKnowledgeBuilder.Knowledge;
      Characters[4].Knowledge = universeKnowledgeBuilder.Knowledge;
      Characters[5].Knowledge = universeKnowledgeBuilder.Knowledge;
    }
  }
}
