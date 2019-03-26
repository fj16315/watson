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
      var entities = new EntityBuilder { "cat", "earl", "dog" };
      var verbs = new VerbBuilder { "kill", "owe", "consume" };
      knowledgeBuilder = new KnowledgeBuilder(entities, verbs)
      {
        {"cat", "kill", "earl" },
        {"dog", "owe", "cat" },
        {"earl", "consume", "dog" }
      };
    }

      //Demo story:
      /*
      {"actress", "poison", "earl"};
      {"countess", "marry", "earl"};
      {"earl", "marry", "countess"};
      {"earl", "owe", "gangster"};
      {"earl", "employ", "butler"};
      {"actress", "inherit", "money"};
      {"earl", "prevent", "promotion"};
      {"butler", "launder", "money"};
      {"study", "contain", "will"};
      {"earl", "own", "belongings"};
      {"belongings", "contain", "letter"};
      {"colonel", "resent", "earl"};
      {"butler", "do", "books"};
      {"gangster", "blackmail", "butler"};
      {"butler", "own", "arsenic"};
      {"earl", "use", "barbital"};
      {"belongings", "contain", "nightshade"};
      */
  }
}
