using System;
using System.Collections.Generic;
using System.Text;

namespace WatsonAI
{
  public class MultipleWordRemovalProcess : IProcess
  {

    public Stream Process(Stream stream)
    {
      var clone = stream.Clone();
      if (clone.Output != null)
      {
        var output = clone.Output[0];
        var tokenisedOutput = output.Split(new string[] { " " }, StringSplitOptions.None);
        var newOutput = new List<string>();
        for (int i = 0; i < tokenisedOutput.Length; i++)
        {
          string[] newWords;
          var word = tokenisedOutput[i];

          if (word.Contains("_"))
          {
            newWords = word.Split(new string[] { "_" }, StringSplitOptions.None);
            newOutput.AddRange(newWords);
          }
          else newOutput.Add(word);
        }

        var finalOutput = new List<string>();
        finalOutput.Add(String.Join(" ", newOutput));
        stream.SetOutput(finalOutput);
      }
      return stream;
    
    }
  }
}
