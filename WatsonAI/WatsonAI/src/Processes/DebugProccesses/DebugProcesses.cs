using System;
using System.Collections.Generic;
using System.Text;

namespace WatsonAI
{
  /// <summary>
  /// Process that applies all the debug processes.
  /// </summary>
  public class DebugProcesses : IProcess
  {
    private DebugParseProcess debugParse;
    private DebugThesaurusProcess debugThesaurus;

    /// <summary>
    /// Process that contains all debugging processes.
    /// </summary>
    /// <param name="parser">Instance of the parser to use.</param>
    /// <param name="thesaurus">Instance of the thesaurus to use.</param>
    public DebugProcesses(Parser parser, Thesaurus thesaurus)
    {
      this.debugParse = new DebugParseProcess(parser);
      this.debugThesaurus = new DebugThesaurusProcess(thesaurus);
    }

    /// <summary>
    /// Typing '!' will cause it to check against all the debug engines.
    /// </summary>
    /// <param name="stream">The stream.</param>
    /// <returns>Debug processes applied to output.</returns>
    public Stream Process(Stream stream)
    {
      if (stream.NextToken().Equals("!", StringComparison.OrdinalIgnoreCase))
      {
        stream.Consume();
        stream.ProcessWith(debugParse);
        stream.ProcessWith(debugThesaurus);
      }
      return stream;
    }
  }
}
