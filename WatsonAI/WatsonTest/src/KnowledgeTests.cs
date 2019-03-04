using FsCheck;
using FsCheck.Xunit;
using Xunit;

using WatsonAI;

using System.Linq;

namespace WatsonTest
{
  [Properties(Arbitrary = new System.Type[] { typeof(Generators) })]
  public class KnowledgeTests
  {
    [Property]
    public bool RemoveVerbPhrases(VerbPhrase[] verbPhrases)
    {
      var knowledge = new Knowledge();
      foreach (var verbPhrase in verbPhrases)
      {
        knowledge.AddVerbPhrase(verbPhrase);
      }
      return verbPhrases.Select( knowledge.RemoveVerbPhrase )
               .All(b => b);
    }

    [Property]
    public bool GetVerbPhrases(VerbPhrase[] verbPhrases)
    {
      var knowledge = new Knowledge();
      foreach (var verbPhrase in verbPhrases)
      {
        knowledge.AddVerbPhrase(verbPhrase);
      }
      return Enumerable.Zip(
        knowledge.GetVerbPhrases()
          .OrderBy(vp => vp.verb),
        verbPhrases.OrderBy(vp => vp.verb),
        (a, b) => a == b
      ).All(b => b);
    }

    [Fact]
    public void EmptyVerbPhrases()
    {
      var knowledge = new Knowledge();
      Assert.Empty(knowledge.GetVerbPhrases());
    }

    [Fact]
    public void EmptyEntities()
    {
      var knowledge = new Knowledge();
      Assert.Empty(knowledge.GetEntities());
    }

    [Property]
    public bool ReaddSameEntity(VerbPhrase verbPhrase)
    {
      var knowledge = new Knowledge();
      knowledge.AddVerbPhrase(verbPhrase);
      knowledge.AddVerbPhrase(verbPhrase);

      var entities = verbPhrase.GetValents()
                       .Select(valent => valent.entity)
                       .Distinct()
                       .OrderBy(e => (uint)e);

      return Enumerable.Zip(
        knowledge.GetEntities().OrderBy(e => (uint)e),
        entities,
        (a, b) => a == b
      ).All(b => b);
    }

    [Property]
    public bool RemoveAllVerbPhrases(Knowledge knowledge)
    {
      var verbPhrases = knowledge.GetVerbPhrases().ToArray();
      foreach (var verbPhrase in verbPhrases)
      {
        knowledge.RemoveVerbPhrase(verbPhrase);
      }
      return !knowledge.GetVerbPhrases().Any()
          && !knowledge.GetEntities().Any();
    }
  }
}
