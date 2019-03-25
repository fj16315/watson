using System;
using System.Collections.Generic;
using System.Text;

namespace WatsonAI
{
  /// <summary>
  /// Text engine that allows for printing the parse tree in console.
  /// </summary>
  public class DebugParseEngine : IProcess
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
    /// <param name="stream">The InputOutput state struct.</param>
    /// <returns>Output with parsetree appended when appropriate.</returns>
    public Stream Process(Stream stream)
    {
      if (stream.RemainingInput.GetEnumerator().Current.Equals("!p", StringComparison.OrdinalIgnoreCase))
      {
        stream.Consume();
        var parse = this.parser.Parse(stream.RemainingInput).Show();
        stream.AppendOutput(parse);
      }
      return stream;
    }
  }
}
