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
      if (io.output.Length == 0)
      {
        io.output = "What are you talking about!";
      }
      return io;
    }
  }
}
