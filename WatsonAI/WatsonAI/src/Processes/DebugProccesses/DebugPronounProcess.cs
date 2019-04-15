using System;
using System.Collections.Generic;
using System.Linq;

namespace WatsonAI
{
  public class DebugPronounProcess
  {
    private readonly PronounsRemovalProcess pronounPreProcess;
    private readonly PronounsAddingProcess pronounPostProcess;

    /// <summary>
    /// Construct a new DebugPronounProcess with character actress.
    /// </summary>
    public DebugPronounProcess(List<Character> characters, Parser parser)
    {
      var character = new Character("actress", false, Gender.Female);
      this.pronounPreProcess = new PronounsRemovalProcess(character, characters, parser);
      this.pronounPostProcess = new PronounsAddingProcess(character, parser);
    }

    /// <summary>
    /// 'pp' will replace personal pronouns in a sentence with the appropriate replacements
    /// </summary>
    /// <param name="stream">The stream.</param>
    /// <returns>Stream with the input sentence with the personal pronouns.</returns>
    public Stream Process(Stream stream)
    {
      if (stream.ConsumeIf("ppre".Equals))
      {
        List<string> remainingInput;
        stream.RemainingInput(out remainingInput, Read.Consume);
        pronounPreProcess.Process(stream);
        stream.AppendOutput(string.Join(" ", stream.Input));
      }
      if (stream.ConsumeIf("ppos".Equals))
      {
        List<string> remainingInput;
        stream.RemainingInput(out remainingInput, Read.Consume);
        pronounPostProcess.Process(stream);
        stream.AppendOutput(string.Join(" ", stream.Input));
      }
      return stream;
    }
  }
}
