using System;
using System.Collections.Generic;
using System.Text;

namespace WatsonAI
{
  public class Story
  {
    private readonly KnowledgeBuilder knowledgeBuilder;
    public Knowledge Knowledge { get; }
    public Associations Associations { get; }

    public Story()
    {
      knowledgeBuilder = new KnowledgeBuilder();
      GenerateStory();
      this.Knowledge = knowledgeBuilder.Knowledge();
      this.Associations = knowledgeBuilder.Associations();
    }

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
