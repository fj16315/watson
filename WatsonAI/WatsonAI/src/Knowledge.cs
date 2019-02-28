using System;
using System.Collections.Generic;
using System.Linq;

namespace WatsonAI
{
  public class Knowledge
  {
    private readonly List<VerbPhrase> verbPhrases;
    private int entityCount;

    public Knowledge()
    {
      verbPhrases = new List<VerbPhrase>();
      entityCount = 0;
    }

    void AddVerbPhrase(VerbPhrase verbPhrase)
    {
      verbPhrases.Add(verbPhrase);
      var highestEntity = verbPhrase.GetValents()
                                    .Select(v => (int)v.entity)
                                    .Max();
      if (highestEntity > entityCount)
      {
        entityCount = highestEntity;
      }
    }

    bool RemoveVerbPhrase(VerbPhrase verbPhrase)
    {
      // TODO: Check if this changes entityCount
      return verbPhrases.Remove(verbPhrase);
    }

    IEnumerable<VerbPhrase> GetVerbPhrases()
      => verbPhrases;

    // TODO: Make wrapper class for uint (Relation.cs: 8)
    IEnumerable<VerbPhrase> FilterBy(uint verb)
      => GetVerbPhrases().Where(
        verbPhrase => verbPhrase.verb == verb
      );

    IEnumerable<Entity> GetEntities()
      => Enumerable.Range(0, entityCount)
         .Select(n => new Entity(n));
  }
}
