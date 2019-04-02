using System;
using System.Linq;

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
      if (!stream.Output.Any())
      {
        var rnd = new Random();
        var index = rnd.Next(randomFallbacks.Length);
        stream.AppendOutput(randomFallbacks[index]);
      }
      return stream;
    }
  }
}
