using System;
using System.Collections.Generic;
using System.Linq;

namespace WatsonAI
{
  public class DebugPronounProcess
  {
    private readonly PronounsProcess pronounProcess;


    public DebugPronounProcess()
    {
      this.pronounProcess = new PronounsProcess(new Character("actress", false));

    }
    /// <summary>
    /// 'pp' will replace personal pronouns in a sentence with the appropriate replacements
    /// </summary>
    /// <param name="stream">The stream.</param>
    /// <returns>Stream with the input sentence with the personal pronouns.</returns>
    public Stream Process(Stream stream)
    {
      if (stream.ConsumeIf("pp".Equals))
      {
        List<string> remainingInput;
        stream.RemainingInput(out remainingInput, Read.Consume);
        pronounProcess.PreProcess(ref remainingInput);
        stream.AppendOutput(string.Join(" ", remainingInput));
      }
      return stream;
    }
  }
}
