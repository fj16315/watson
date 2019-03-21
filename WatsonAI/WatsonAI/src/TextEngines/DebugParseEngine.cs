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
    private Parser parser;

    /// <summary>
    /// Text engine for debuging the parser.
    /// </summary>
    public DebugParseEngine()
    {
      this.parser = new Parser();
    }

    /// <summary>
    /// Text engine for debuging the specified Parser.
    /// </summary>
    /// <param name="parse">The parser to use.</param>
    public DebugParseEngine(Parser parse)
    {
      this.parser = parse;
    }

    /// <summary>
    /// Typing 'debugparse x' will print the parse tree for x.
    /// </summary>
    /// <param name="io">The InputOutput state struct.</param>
    /// <returns>Output with parsetree appended when appropriate.</returns>
    public InputOutput Process(InputOutput io)
    {
      if (io.remainingInput.Trim().StartsWith("!p ", StringComparison.OrdinalIgnoreCase))
      {
        io.remainingInput = io.remainingInput.Substring("!p ".Length);
        var parse = this.parser.Parse(io.remainingInput).Show();
        io.remainingInput = "";
        io.output = parse;
      }
      return io;
    }
  }
}
