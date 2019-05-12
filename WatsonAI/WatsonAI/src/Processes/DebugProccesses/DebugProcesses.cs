using System.Collections.Generic;

namespace WatsonAI
{
  /// <summary>
  /// Process that applies all the debug processes.
  /// </summary>
  public class DebugProcesses : IProcess
  {
    private DebugParseProcess debugParse;
    private DebugThesaurusProcess debugThesaurus;
    private DebugPronounProcess debugPronounProcess;

    /// <summary>
    /// Process that contains all debugging processes.
    /// </summary>
    /// <param name="parser">Instance of the parser to use.</param>
    /// <param name="thesaurus">Instance of the thesaurus to use.</param>
    public DebugProcesses(List<Character> characters, Parser parser, Thesaurus thesaurus)
    {
      debugParse = new DebugParseProcess(parser);
      debugThesaurus = new DebugThesaurusProcess(thesaurus);
      debugPronounProcess = new DebugPronounProcess(characters, parser);
    }

    /// <summary>
    /// Typing '!' will cause it to check against all the debug engines.
    /// </summary>
    /// <param name="stream">The stream.</param>
    /// <returns>Debug processes applied to output.</returns>
    public Stream Process(Stream stream)
    {
      if (stream.ConsumeIf("!".Equals))
      {
        debugParse.Process(stream);
        debugThesaurus.Process(stream);
        debugPronounProcess.Process(stream);
        stream.AssignSpecialCaseHandler(this);
      }
      return stream;
    }
  }
}
