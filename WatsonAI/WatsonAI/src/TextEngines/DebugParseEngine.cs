using System;
using System.Collections.Generic;
using System.Text;

namespace WatsonAI
{
  /// <summary>
  /// Text engine that allows for printing the parse tree in console.
  /// </summary>
  public class DebugParseEngine : IRule
  {
    /// <summary>
    /// Typing 'debugparse x' will print the parse tree for x.
    /// </summary>
    /// <param name="io">The InputOutput state struct.</param>
    /// <returns>Output with parsetree appended when appropriate.</returns>
    public InputOutput Process(InputOutput io)
    {
      if (io.remainingInput.Trim().StartsWith("debugparse", StringComparison.OrdinalIgnoreCase))
      {
        io.remainingInput = io.remainingInput.Substring("debugparse".Length);
        var parse = new Parser().Parse(io.remainingInput).Show();
        io.remainingInput = "";
        io.output = io.output + parse;
      }
      return io;
    }
  }
}
