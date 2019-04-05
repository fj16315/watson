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

    private string[] randomFallbacks;

    public FallbackProcess() {
      randomFallbacks = new string[]
      {
        "I dont know.",
        "Can you remain on task please?",
        "We have got more urgent matters at hand.",
        "I would rather be talking about the murder.",
        "Please dont go off topic.",
        "Dont you have more important things to discuss?",
      };
    }

    public Stream Process(Stream stream)
    {
      var input = new List<string>();
      bool flag = false;
      if (stream.Output.Contains<string>("!u"))
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
          stream.AppendOutput("I really couldn't answer that.");
        }
        else
        {
          stream.AppendOutput("You're talking nonesense, can we get back on track please?");
        }
      }
      else if (!stream.Output.Any())
      {
        foreach (var o in stream.Output)
        {
          Console.WriteLine(o);
        }
        stream.ClearOutput();
        stream.AppendOutput("I don't know, but someone else might.");
      }
      flag = false;
      return stream;
    }
  }
}
