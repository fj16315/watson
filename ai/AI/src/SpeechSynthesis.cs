using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameAI
{
  public static class SpeechSynthesis
  {
    public static string Synthesise(TypedDependenciesList tdList, IEnumerable<Entity> entities, Associations ass)
    {
      string sentence = "The " + ass.NameOf(entities.GetEnumerator().Current) + " " + tdList.GetRoot() + " the " + tdList.WithRelationFrom(tdList.GetRoot(), "dobj");
      return sentence;
    }
  }
}
