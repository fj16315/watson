using FsCheck;
using FsCheck.Xunit;

using WatsonAI;

namespace WatsonTest
{
  [Properties(Arbitrary = new System.Type[] { typeof(Generators) })]
  public class AssociationsTests
  {
    [Property]
    public bool TryGetEntityPresent(Entity entity, NonNull<string> word)
    {
      var assoc = new Associations();
      assoc.AddEntityName(entity, word.Get);
      return assoc.TryGetEntity(word.Get, out var newEntity);
    }

    [Property]
    public bool TryGetVerbPresent(Verb verb, NonNull<string> word)
    {
      var assoc = new Associations();
      assoc.AddVerbName(verb, word.Get);
      return assoc.TryGetVerb(word.Get, out var newVerb);
    }

    [Property]
    public bool TryNameEntityPresent(Entity entity, NonNull<string> word)
    {
      var assoc = new Associations();
      assoc.AddEntityName(entity, word.Get);
      return assoc.TryNameEntity(entity, out var newWord);
    }

    [Property]
    public bool TryNameVerbPresent(Verb verb, NonNull<string> word)
    {
      var assoc = new Associations();
      assoc.AddVerbName(verb, word.Get);
      return assoc.TryGetVerb(word.Get, out var newWord);
    }

    [Property]
    public bool TryGetEntityEmpty(NonNull<string> word)
    {
      var assoc = new Associations();
      return assoc.TryGetEntity(word.Get, out var entity) == false;
    }

    [Property]
    public bool TryGetVerbEmpty(NonNull<string> word)
    {
      var assoc = new Associations();
      return assoc.TryGetVerb(word.Get, out var verb) == false;
    }

    [Property]
    public bool TryNameEntityEmpty(Entity entity)
    {
      var assoc = new Associations();
      return assoc.TryNameEntity(entity, out var name) == false;
    }

    [Property]
    public bool TryNameVerbEmpty(Verb verb)
    {
      var assoc = new Associations();
      return assoc.TryNameVerb(verb, out var name) == false;
    }
  }
}
