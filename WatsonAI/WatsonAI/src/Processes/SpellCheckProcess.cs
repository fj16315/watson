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


    public SpellCheckProcess() {
      var file = File.ReadAllLines(Path.Combine("..", "WatsonAI", "bin", "Debug", "netcoreapp2.1", "res", "dictionary", "words.txt"));
      this.english = new List<string>(file);
      this.spellChecker = new SpellChecker();
      this.english.Add("?");
      this.spellChecker.Train(english);
      // To get all word in the dictionry within 1 edit distance
    
    }




    /// <summary>
    /// Checks for a greeting at the start of the string. 
    /// and adds a greeting to the output.
    /// </summary>
    /// <param name="stream">The input stream.</param>
    /// <returns>Mutated stream.</returns>
    public Stream Process(Stream stream)
    {
      var suggestions = new List<string>();
      var streamNew = stream.Clone();
      string word;
      streamNew.NextToken(out word);
      foreach(var token in stream.Input)
      {
        if (!spellChecker.IsSpellingCorrect(token))
        {
          var corrections = spellChecker.GetSuggestedWords(token, 1);
          foreach (var correction in corrections)
          {
            suggestions.Add(correction.Word);
          }
        }
      }

      foreach ( var suggestion in suggestions)
      {
        System.Diagnostics.Debug.WriteLine(suggestion);
      }


      return stream;
    }
  }
}
