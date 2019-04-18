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

    private SpellChecker spellChecker;
    private List<string> english;
    private SymSpell symSpell;


    public SpellCheckProcess() {
      //var file = File.ReadAllLines(Path.Combine("..", "WatsonAI", "bin", "Debug", "netcoreapp2.1", "res", "dictionary", "words.txt"));
      //this.english = new List<string>(file);
      //this.spellChecker = new SpellChecker();
      //this.english.Add("?");
      //this.spellChecker.Train(english);
      // To get all word in the dictionry within 1 edit distance

      //create the symspell object
      int initialCapacity = 82765;
      int maxEditDistanceDictionary = 2;
      this.symSpell = new SymSpell(initialCapacity, maxEditDistanceDictionary);


      //load dictionary
      string directory = Path.Combine("..", "WatsonAI", "bin", "Debug", "netcoreapp2.1", "res", "dictionary", "frequency_dictionary_en_82_765.txt");
      int termIndex = 0;
      int countIndex = 1;


      if (!symSpell.LoadDictionary(directory, termIndex, countIndex)) 
      {
        Console.WriteLine("File not found!");
        Console.ReadKey();
        return;
      }
      this.symSpell.CreateDictionaryEntry("?", 30000000);
     

    }


    /// <summary>

    /// and adds a greeting to the output.
    /// </summary>
    /// <param name="stream">The input stream.</param>
    /// <returns>Mutated stream.</returns>
    public Stream Process(Stream stream)
    {
      //  var suggestions = new List<string>();
      //  var streamNew = stream.Clone();
      //  foreach(var token in streamNew.Input)
      //  {
      //    if (!spellChecker.IsSpellingCorrect(token))
      //    {
      //      stream.AssignSpecialCaseHandler(this);
      //      var corrections = spellChecker.GetSuggestedWords(token, 1);


      //    }
      //  }

      //  return stream;

      //lookup suggestions for multi-word input strings (supports compound splitting & merging)

      int maxEditDistanceLookup = 2; //max edit distance per lookup (per single word, not per whole input string)

      var suggestions = symSpell.LookupCompound(stream.nonTokenisedInput, maxEditDistanceLookup);

      //display suggestions, edit distance and term frequency
      foreach (var suggestion in suggestions)
      {

        if (suggestion.distance != 0)
        {
          stream.AssignSpecialCaseHandler(this);
          if (stream.Input.Contains("?")) {
            Console.WriteLine("Did you mean " + "'" + suggestion.term + "?" + "'");
          }
          else 
          {
            Console.WriteLine("Did you mean " + "'" + suggestion.term + "'");
          }

        }
      }

      return stream;

    }

  }
}
