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

    /// <summary>
    /// Creates a new Story object and generates the story
    /// </summary>
    public Story()
    {
      knowledgeBuilder = new KnowledgeBuilder();
      GenerateStory();
      this.Knowledge = knowledgeBuilder.Knowledge();
      this.Associations = knowledgeBuilder.Associations();
    }

    /// <summary>
    /// Uses hard-coded List<string> for entities and verbs, then adds simple verb phrases
    /// built from the entities and verbs
    /// </summary>
    private void GenerateStory()
    {
      var entities = new List<string> { "cat", "earl", "dog" };
      var verbs = new List<string> { "kill", "owe", "consume" };
      knowledgeBuilder.AddEntities(entities);
      knowledgeBuilder.AddVerbs(verbs);

      knowledgeBuilder.AddSimplePhrase("cat", "kill", "earl");
      knowledgeBuilder.AddSimplePhrase("dog", "owe", "cat");
      knowledgeBuilder.AddSimplePhrase("earl", "consume", "dog");
    }
  }
}
