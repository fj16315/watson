using System;
using System.Linq;
using System.Collections.Generic;

namespace WatsonAI
{
  /// <summary>
  /// Textual engine for what to return when no other output has been generated.
  /// </summary>
  public class FallbackProcess : IProcess
  {
    private string[] randomNonesenseResponses;
    private string[] randomUniverseResponses;
    private string[] randomEntitiesResponses;

    /// <summary>
    /// Initialises random responses to the different fallbacks
    /// </summary>
    public FallbackProcess() {
      randomNonesenseResponses = new string[]
      {
        "Can you remain on task please?",
        "We have got more urgent matters at hand.",
        "I would rather be talking about the murder.",
        "Please dont go off topic.",
        "Dont you have more important things to discuss?",
      };

      randomUniverseResponses = new string[]
      {
        "I don't know, but someone else might.",
        "Other people in the house will know.",
        "Well I'm not sure about that but ask the others in the house."
      };

      randomEntitiesResponses = new string[]
      {
        "I really couldn't answer that.",
        "I'm not sure about that.",
        "Well no-one in the house could answer that!"
      };
    }

    /// <summary>
    /// Process determines which category of fallback it is,
    /// then returns an appropriate random response
    /// </summary>
    /// <param name="stream"></param>
    /// <returns></returns>
    public Stream Process(Stream stream)
    {
      var rnd = new Random();
      var nonesenseIndex = rnd.Next(randomNonesenseResponses.Length);
      var universeIndex = rnd.Next(randomUniverseResponses.Length);
      var entitiesIndex = rnd.Next(randomEntitiesResponses.Length);
      var input = new List<string>();
      bool flag = false;
      if (stream.Output.Contains("!u"))
      {
        stream.ClearOutput();
        stream.RemainingInput(out input, Read.Peek);
        foreach (var e in Story.entities.Names)
        {
          if (input.Contains(e))
          {
            flag = true;
          }
        }
        if (flag)
        {
          stream.AppendOutput(randomEntitiesResponses[entitiesIndex]);
        }
        else
        {
          stream.AppendOutput(randomNonesenseResponses[nonesenseIndex]);
        }
      }
      else if (!stream.Output.Any())
      {
        foreach (var o in stream.Output)
        {
          Console.WriteLine(o);
        }
        stream.ClearOutput();
        stream.AppendOutput(randomUniverseResponses[universeIndex]);
      }
      return stream;
    }
  }
}
