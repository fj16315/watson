using System;
using System.Collections.Generic;
using System.Text;

namespace WatsonAI
{
  /// <summary>
  /// Textual engine for what to return when no other output has been generated.
  /// </summary>
  public class FallbackEngine : IRule
  {
    /// <summary>
    /// Generic response if no other output has been generated.
    /// </summary>
    /// <remarks>Should always be the last process.</remarks>
    /// <param name="io">The InputOutput state struct.</param>
    /// <returns>The InputOutput state object with generic output as output.</returns>
    public InputOutput Process(InputOutput io)
    {
      List<string> randomFallbacks = new List<string>();

      randomFallbacks.AddRange(new List<string>
      {
        "I dont know.",
        "Can you remain on task please?",
        "We have got more urgent matters at hand.",
        "I would rather be talking about the murder.",
        "Please dont go off topic.",
        "Dont you have more important things to discuss?",
      });


      if (io.output.Length == 0)
      {
        Random rnd = new Random();

        io.output = randomFallbacks[rnd.Next(randomFallbacks.Count-1);

      }
      return io;
    }
  }
}
