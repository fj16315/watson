using System;
using System.Collections.Generic;
using System.Text;

namespace WatsonAI
{
  /// <summary>
  /// Textual engine for what to return when no other output has been generated.
  /// </summary>
  public class FallbackProcess : IProcess
  {

    private List<string> randomFallbacks;

    public FallbackProcess() {
      this.randomFallbacks = new List<string>();
    }

    public Stream Process(Stream stream)
    {


      randomFallbacks.AddRange(new List<string>
      {
        "I dont know.",
        "Can you remain on task please?",
        "We have got more urgent matters at hand.",
        "I would rather be talking about the murder.",
        "Please dont go off topic.",
        "Dont you have more important things to discuss?",
      });


      if (stream.Output().Length ==0)
      {
        Random rnd = new Random();

        stream.AppendOutput(randomFallbacks[rnd.Next(randomFallbacks.Count-1)]);

      }
      return stream;
    }
  }
}
