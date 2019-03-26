namespace WatsonAI
{
  /// <summary>
  /// Creates a Knowledge object and its Associations based on hard-coded story
  /// </summary>
  public static class Story
  {
    private static readonly KnowledgeBuilder knowledgeBuilder;
    public static Knowledge Knowledge
    {
      get
      {
        return knowledgeBuilder.Knowledge;
      }
    }
    public static Associations Associations
    {
      get
      {
        return knowledgeBuilder.Associations;
      }
    }

    static Story()
    {
      var entities = new EntityBuilder {
        "actress", "butler", "countess", "earl",
        "gangster", "money", "promotion", "money",
        "study", "will", "belonging", "letter",
        "colonel", "book", "arsenic", "barbital",
        "nightshade", "murderer", "dead", "music", "chocolate"
      };
      var verbs = new VerbBuilder {
        "poison", "marry", "owe", "employ",
        "inherit", "prevent", "launder", "contain",
        "own", "resent", "do", "blackmail",
        "use", "kill", "be", "love", "like"};
      knowledgeBuilder = new KnowledgeBuilder(entities, verbs)
      {
        //{"actress", "be", "murderer"},
        {"earl", "love", "countess"},
        {"countess", "love", "earl"},
        {"countess", "love", "chocolate"},
        {"earl", "like", "music"},
        {"actress", "poison", "earl"},
        {"earl", "be", "dead"},
        //{"actress", "kill", "earl"},
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
    }
  }
}
