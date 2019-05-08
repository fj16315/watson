using OpenNLP.Tools.Parser;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace WatsonAI
{
  /// <summary>
  /// Engine for processing greetings at the start of the input.
  /// </summary>
  public class SpellCheckProcess : IProcess
  {
    private SymSpell symSpell;
    private readonly Parser parser;

    /// <summary>
    ///Initialises the SymSpell object and loads the dictionary into it
    /// </summary>
    /// <param name="symSpell">SymSpell object with the dictionary initialised</param>
    /// <param name="parser">Parser.</param>
    public SpellCheckProcess(SymSpell symSpell, Parser parser) 
    {
      this.symSpell = symSpell;
      this.parser = parser;
    }

    /// <summary>
    /// Initialises SymSpell object and loads the dictionary
    /// </summary>
    public SpellCheckProcess(Parser parser)
    {
      this.parser = parser;
      int initialCapacity = 549313;
      int maxEditDistanceDictionary = 2;
      symSpell = new SymSpell(initialCapacity, maxEditDistanceDictionary);
      string directory = Path.Combine(Directory.GetCurrentDirectory(), "res", "dictionary", "frequency_dictionary.txt");
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
        if (suggestion.distance == 1 && stream.Input.Contains("?"))
        {
          //Purposely left empty
        }
        else if (suggestion.distance != 0)
        {
          string corrected = suggestion.term;
          if (stream.Input.Contains("?")) corrected += "?";
          corrected = corrected.First().ToString().ToUpper() + corrected.Substring(1);
          return Stream.Tokenise(parser, corrected);
        }
      }
      return stream;
    }
  }
}
