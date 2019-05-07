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
      List<string> remainingInput;
      clone.RemainingInput(out remainingInput, Read.Peek);
      var newInput = new List<string>();
      for( int i = 0; i< remainingInput.Count; i++)
      {
        string[] newWords;
        var word = remainingInput[i];
        if (word.Contains("_")) {
          newWords = word.Split("_");
          newInput.AddRange(newWords);
        }
        else newInput.Add(word);
      }
      stream.SetInput(newInput);
      return stream;
    
    }
  }
}
