using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WatsonAI
{
  public static class SpeechSynthesis
  {
    // Sorry for commenting this out, Freddie. With the changes to the parser,
    // this no longer works. The parser is not yet implemented but should have
    // similar functionality.
    //
    //public static string Synthesise(TypedDependenciesList tdList, IEnumerable<Entity> entities, Associations ass)
    //{
    //  string sentence;
    //  IEnumerator<Entity> enumerator = entities.GetEnumerator();
    //  /*List<Entity> eList = new List<Entity>();
    //  eList.Add(entities.GetEnumerator().Current);
    //  while(entities.GetEnumerator().MoveNext())
    //  {
    //    Console.WriteLine("Moving on");
    //    eList.Add(entities.GetEnumerator().Current);
    //  }*/
    //  if (enumerator.Current.Equals(null))
    //  {
    //    sentence = "I'm sorry I don't know.";
    //  }
    //  else
    //  {
    //    //Console.WriteLine(eList.Count());
    //    //sentence = "The " + ass.NameOf(eList.ElementAt(0));
    //    sentence = "The " + ass.NameOf(enumerator.Current);
    //    /*foreach (Entity e in eList) {
    //      if (e.Equals(eList.ElementAt(0)) == false)
    //      {
    //        sentence += " and the " + ass.NameOf(e);
    //      }
    //    }*/
    //    while (enumerator.MoveNext())
    //    {
    //      sentence += " and the " + ass.NameOf(enumerator.Current);
    //    }
    //    sentence += " " + tdList.GetRoot().word() + " the " + tdList.WithRelationFrom(tdList.GetRoot(), "dobj").word() + ".";
    //  }
    //  return sentence;
    //}
  }
}
