using System;
using System.Collections.Generic;
using System.Text;

namespace WatsonAI
{
  /// <summary>
  /// Engine for processing greetings at the start of the input.
  /// </summary>
  public class GreetingsEngine : IRule
  {
    /// <summary>
    /// Checks for a greeting at the start of the string. 
    /// Removes it from the remaining input and adds a greeting to the output.
    /// </summary>
    /// <param name="io">The InputOutput state struct.</param>
    /// <returns>Mutated InputOutput state struct.</returns>
    public InputOutput Process(InputOutput io)
    {
      if (io.remainingInput.StartsWith("hello", StringComparison.OrdinalIgnoreCase))
      {
        io.remainingInput = io.remainingInput.Substring("hello".Length);
        io.output = "Hello watson! " + io.output;
      }
      return io;
    }
  }
}
