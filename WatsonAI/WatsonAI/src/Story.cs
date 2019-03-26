using System;
using System.Collections.Generic;
using System.Text;

namespace WatsonAI
{
  /// <summary>
  /// Creates a Knowledge object and its Associations based on hard-coded story
  /// </summary>
  public class Story
  {
    private readonly KnowledgeBuilder knowledgeBuilder;
    public Knowledge Knowledge { get; }
    public Associations Associations { get; }
    public List<Character> Characters { get; }

    /// <summary>
    /// Creates a new Story object and generates the story
    /// </summary>
    public Story()
    {
      knowledgeBuilder = new KnowledgeBuilder();
      GenerateStory();
      this.Knowledge = knowledgeBuilder.Knowledge();
      this.Associations = knowledgeBuilder.Associations();
      this.Characters = new List<Character>();
    }

    /// <summary>
    /// Uses hard-coded List<string> for entities and verbs, then adds simple verb phrases
    /// built from the entities and verbs
    /// </summary>C
    private void GenerateStory()
    {
      var entities = new List<string> { "cat", "earl", "dog" };
      var verbs = new List<string> { "kill", "owe", "consume" };
      knowledgeBuilder.AddEntities(entities);
      knowledgeBuilder.AddVerbs(verbs);

      knowledgeBuilder.AddSimplePhrase("cat", "kill", "earl");
      knowledgeBuilder.AddSimplePhrase("dog", "owe", "cat");
      knowledgeBuilder.AddSimplePhrase("earl", "consume", "dog");

      //Characters.Add(new Character("cat", true));
      //Characters.Add(new Character("dog", false));
      //Characters.Add(new Character("earl", false));

      //Demo story:
      /*
      knowledgeBuilder.AddSimplePhrase("actress", "poison", "earl");
      knowledgeBuilder.AddSimplePhrase("countess", "marry", "earl");
      knowledgeBuilder.AddSimplePhrase("earl", "marry", "countess");
      knowledgeBuilder.AddSimplePhrase("earl", "owe", "gangster");
      knowledgeBuilder.AddSimplePhrase("earl", "employ", "butler");
      knowledgeBuilder.AddSimplePhrase("actress", "inherit", "money");
      knowledgeBuilder.AddSimplePhrase("earl", "prevent", "promotion");
      knowledgeBuilder.AddSimplePhrase("butler", "launder", "money");
      knowledgeBuilder.AddSimplePhrase("study", "contain", "will");
      knowledgeBuilder.AddSimplePhrase("earl", "own", "belongings");
      knowledgeBuilder.AddSimplePhrase("belongings", "contain", "letter");
      knowledgeBuilder.AddSimplePhrase("colonel", "resent", "earl");
      knowledgeBuilder.AddSimplePhrase("butler", "do", "books");
      knowledgeBuilder.AddSimplePhrase("gangster", "blackmail", "butler");
      knowledgeBuilder.AddSimplePhrase("butler", "own", "arsenic");
      knowledgeBuilder.AddSimplePhrase("earl", "use", "barbital");
      knowledgeBuilder.AddSimplePhrase("belongings", "contain", "nightshade");
      */
    }
  }
}
