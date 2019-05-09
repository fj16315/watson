using System;
using System.Collections.Generic;
using System.Text;

namespace WatsonAI
{
  public class MultipleWordProcess : IProcess
  {

    private List<List<string>> words;

    public MultipleWordProcess()
    {
      words = new List<List<string>>()
      {
        new List<string>()
        {
          "rat", "poison"
        },
        new List<string>()
        {
          "dining", "room"
        },
        new List<string>()
        {
          "master", "bedroom"
        },
        new List<string>()
        {
          "sleeping", "aid"
        },
        new List<string>()
        {
          "barbital", "tolerance"
        },
        new List<string>()
        {
          "fast", "acting"
        },
        new List<string>()
        {
          "slow", "acting"
        }
      };

    }

    public Stream Process(Stream stream)
    {
      var clone = stream.Clone();
      List<string> remainingInput;
      clone.RemainingInput(out remainingInput, Read.Peek);
      foreach (var t in remainingInput)
      {
        Console.WriteLine(t);
      }
      var newInput = new List<string>();
      var newS = "";
      for (int i = 0; i < remainingInput.Count; i++) //For each word in the input
      {
        var s = remainingInput[i];
        newS = "";
        if (i != remainingInput.Count-1) //If it's not the last one
        {
          if (s.Contains("-")) // If it contains a hyphen
          {
            //Split the hyphenated word into two different words
            string secondWord = s.Split('-')[1];
            s = s.Split('-')[0];
            remainingInput.Insert(i + 1, secondWord);
          }
          bool found = false;
          int increment = 0;
          foreach (var ws in words) //For each set of words we want to underscore
          {
            if (!found)
            {
              for (int j = 0; j < ws.Count; j++) //For each word in the multiple word set
              {
                if ((i + j >= remainingInput.Count) || (!remainingInput[i + j].Equals(ws[j])))
                {
                  if (words.IndexOf(ws) == words.Count-1)
                  {
                    newS = s;
                  }
                  else
                  {
                    newS = "";
                  }
                  j = ws.Count;
                }
                else //If the current word matches the current word in the word set
                {
                  if (j == ws.Count-1)
                  {
                    newS = newS + remainingInput[i + j];
                    found = true;
                    increment = ws.Count-1;
                  }
                  else
                  {
                    if ((words.IndexOf(ws) == 5) || (words.IndexOf(ws) == 6))
                    {
                      newS = newS + remainingInput[i + j] + "-";
                    }
                    else
                    {
                      newS = newS + remainingInput[i + j] + "_";
                    }
                  }
                }
              }
            }
          }
          i += increment;
          newInput.Add(newS);
        }
        else
        {
          newInput.Add(remainingInput[i]);
        }
      }
      foreach (var t in newInput)
      {
        Console.WriteLine(t);
      }
      stream.SetInput(newInput);
   
      return stream;
    }
  }
}
