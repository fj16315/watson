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
    private static SymSpell symSpell;

    static SpellCheckProcess() {
      int initialCapacity = 549313;
      int maxEditDistanceDictionary = 2;
      symSpell = new SymSpell(initialCapacity, maxEditDistanceDictionary);

      string directory = Path.Combine("..","WatsonAI", "res", "dictionary", "frequency_dictionary.txt");
      int termIndex = 0;
      int countIndex = 1;

      if (!symSpell.LoadDictionary(directory, termIndex, countIndex)) 
      {
        System.Diagnostics.Debug.WriteLine("File not found");

        return;
      }
    }
    /// <summary>
    /// checks the spelling of the input stream
    /// </summary>
    /// <param name="stream">The input stream.</param>
    /// <returns>Stream Suggestion.</returns>
    public Stream Process(Stream stream)
    {
      int maxEditDistanceLookup = 2; 
      var suggestions = symSpell.LookupCompound(stream.nonTokenisedInput, maxEditDistanceLookup);

      foreach (var suggestion in suggestions)
      {
        if (suggestion.distance ==1 && stream.Input.Contains("?"))
        {
          //Purposely left empty
        }
        else if (suggestion.distance != 0)
        {
          stream.AssignSpecialCaseHandler(this);
          if (stream.Input.Contains("?")) stream.AppendOutput(suggestion.term + "?");
          else stream.AppendOutput(suggestion.term);

        }
      }
      return stream;
    }

  }
}
