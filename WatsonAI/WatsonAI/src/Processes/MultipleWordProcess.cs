using System;
using System.Collections.Generic;
using System.Text;

namespace WatsonAI
{
  public class MultipleWordProcess : IProcess
  {

    private List<Tuple<string, string>> words;

    public MultipleWordProcess()
    {
      words = new List<Tuple<string, string>>();
      words.Add(new Tuple<string, string>("rat", "poison"));
      words.Add(new Tuple<string, string>("dining", "room"));
      words.Add(new Tuple<string, string>("master", "bedroom"));
      words.Add(new Tuple<string, string>("sleeping", "aid"));
      words.Add(new Tuple<string, string>("barbital", "tolerance"));
      words.Add(new Tuple<string, string>("fast", "acting"));
      words.Add(new Tuple<string, string>("slow", "acting"));

    }

    public Stream Process(Stream stream)
    {
      var clone = stream.Clone();
      List<string> remainingInput;
      clone.RemainingInput(out remainingInput, Read.Peek);
      var newInput = new List<string>();
      for (int i = 0; i < remainingInput.Count; i++)
      {
        var s = remainingInput[i];
        if (i != remainingInput.Count-1)
        {
          if (s.Contains("-"))
          {
            string secondWord = s.Split("-")[1];
            s = s.Split("-")[0];
            remainingInput.Insert(i + 1, secondWord);
          }
          //Loop through list of multiple words
          foreach (var w in words)
          {
            if (s.Equals(w.Item1))
            {
              var nextS = remainingInput[i + 1];
              if (nextS.Equals(w.Item2))
              {
                //remainingInput[remainingInput.IndexOf(s)] = s + "_" + nextS;
                s = s + "_" + nextS;
                //remainingInput.RemoveAt(remainingInput.IndexOf(s) + 1);
                i++;
                
              }
            }
          }
            //If current string = first word of current word
              //If next string = second word of current word
                //Replace whitespace between words with underscore
        }
        newInput.Add(s);
      }
   
      stream.SetInput(newInput);
   
      return stream;
    }
  }
}
