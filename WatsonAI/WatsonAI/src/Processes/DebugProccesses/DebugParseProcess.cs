using System;
using System.Collections.Generic;
using System.Text;

namespace WatsonAI
{
  /// <summary>
  /// Process that allows for printing the parse tree in console.
  /// </summary>
  public class DebugParseProcess : IProcess
  {
    private Parser parser;

    /// <summary>
    /// Process for debuging the parser.
    /// </summary>
    public DebugParseProcess()
    {
      this.parser = new Parser();
    }

    /// <summary>
    /// Text engine for debuging the specified Parser.
    /// </summary>
    /// <param name="parse">The parser to use.</param>
    public DebugParseProcess(Parser parse)
    {
      this.parser = parse;
    }

    /// <summary>
    /// 'p' will print the parse tree for the string that follows.
    /// </summary>
    /// <param name="stream">The stream.</param>
    /// <returns>Stream with parse tree appended to output if appropriate.</returns>
    public Stream Process(Stream stream)
    {
      if (stream.NextToken().Equals("p", StringComparison.OrdinalIgnoreCase))
      {
        stream.Consume();
        var parse = this.parser.Parse(stream.RemainingInput).Show();
        stream.AppendOutput(parse);
      }
      return stream;
    }
  }
}
