using System;
using System.Collections.Generic;
using System.Text;

namespace WatsonAI
{
  public class MultipleWordProcess : IProcess
  {
    public MultipleWordProcess()
    {

    }

    public Stream Process(Stream stream)
    {
      var clone = stream.Clone();
      List<string> remainingInput;
      clone.RemainingInput(out remainingInput, Read.Peek);
      foreach (var s in remainingInput)
      {
        if (remainingInput.IndexOf(s) != remainingInput.Count-1)
        {
          //Loop through list of multiple words
            //If current string = first word of current word
              //If next string = second word of current word
                //Replace whitespace between words with underscore
        }
      }
      return stream;
    }
  }
}
