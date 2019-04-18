using OpenNLP.Tools.Parser;
using System;
using System.IO;
using System.Collections.Generic;
using MSpell;




namespace WatsonAI
{
  /// <summary>
  /// Engine for processing greetings at the start of the input.
  /// </summary>
  public class SpellCheckProcess : IProcess
  {
 
    private SymSpell symSpell;


    public SpellCheckProcess() {
  
      //create the symspell object
      int initialCapacity = 549313;
      int maxEditDistanceDictionary = 2;
      this.symSpell = new SymSpell(initialCapacity, maxEditDistanceDictionary);


      //load dictionary
      string directory = Path.Combine("..", "WatsonAI", "bin", "Debug", "netcoreapp2.1", "res", "dictionary", "frequency_dictionary.txt");
      int termIndex = 0;
      int countIndex = 1;

      if (!symSpell.LoadDictionary(directory, termIndex, countIndex)) 
      {
        Console.WriteLine("File not found!");
        Console.ReadKey();
        return;
      }

     

    }

    /// <summary>
    /// and adds a greeting to the output.
    /// </summary>
    /// <param name="stream">The input stream.</param>
    /// <returns>Mutated stream.</returns>
    public Stream Process(Stream stream)
    {

      //lookup suggestions for multi-word input strings (supports compound splitting & merging)
      int maxEditDistanceLookup = 2; //max edit distance per lookup (per single word, not per whole input string)
      var suggestions = symSpell.LookupCompound(stream.nonTokenisedInput, maxEditDistanceLookup);

      //display suggestions, edit distance and term frequency
      foreach (var suggestion in suggestions)
      {
        if (suggestion.distance ==1 && stream.Input.Contains("?"))
        {
          //Purposely left empty
        }
        else if (suggestion.distance != 0)
        {
          stream.AssignSpecialCaseHandler(this);
          if (stream.Input.Contains("?")) {
            stream.AppendOutput(suggestion.term + "?");
          }
          else 
          {
             stream.AppendOutput(suggestion.term);
          }

        }
      }

      return stream;

    }

  }
}
