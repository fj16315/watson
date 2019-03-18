using System;
using System.Collections.Generic;
using System.Text;
using FsCheck;
using FsCheck.Xunit;
using Xunit;

using WatsonAI;

namespace WatsonTest
{
  [Properties(Arbitrary = new System.Type[] { typeof(Generators) })]
  public class AssociationsTests
  {
    [Property]
    public bool TryGetEntity(NonNull<string> word, Entity entity)
    {
      var entities = new Dictionary<string, Entity>();
      entities.Add(word.Get, entity);
      var verbs = new Dictionary<string, Verb>();
      var assoc = new Associations(entities, verbs);
      return assoc.TryGetEntity(word.Get, out var newEntity);
    }

    [Property]
    public bool TryGetVerb(NonNull<string> word, Verb verb)
    {
      var verbs = new Dictionary<string, Verb>();
      verbs.Add(word.Get, verb);
      var entities = new Dictionary<string, Entity>();
      var assoc = new Associations(entities, verbs);
      return assoc.TryGetVerb(word.Get, out var newVerb);
    }

    [Property]
    public bool TryNameEntity(Entity entity, NonNull<string> word)
    {
      var entities = new Dictionary<string, Entity>();
      entities.Add(word.Get, entity);
      var verbs = new Dictionary<string, Verb>();
      var assoc = new Associations(entities, verbs);
      return assoc.TryNameEntity(entity, out var newWord);
    }

    [Property]
    public bool TryNameVerb(Verb verb, NonNull<string> word)
    {
      var verbs = new Dictionary<string, Verb>();
      verbs.Add(word.Get, verb);
      var entities = new Dictionary<string, Entity>();
      var assoc = new Associations(entities, verbs);
      return assoc.TryGetVerb(word.Get, out var newWord);
    }
  }
}
