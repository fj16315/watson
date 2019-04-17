using OpenNLP.Tools.Parser;
using System;
using System.IO;
using System.Collections.Generic;
using PlatformSpellCheck;


namespace WatsonAI
{
  /// <summary>
  /// Engine for processing greetings at the start of the input.
  /// </summary>
  public class SpellCheckProcess : IProcess
  {
    private SpellChecker spellChecker;
   


    public SpellCheckProcess() {
      System.Diagnostics.Debug.WriteLine(SpellChecker.IsPlatformSupported());
      this.spellChecker = new SpellChecker();
      var suggestion = spellChecker.Check("helo");
      System.Diagnostics.Debug.WriteLine(suggestion);

    }




    /// <summary>
    /// Checks for a greeting at the start of the string. 
    /// and adds a greeting to the output.
    /// </summary>
    /// <param name="stream">The input stream.</param>
    /// <returns>Mutated stream.</returns>
    public Stream Process(Stream stream)
    {
      var streamNew = stream.Clone();
      string word;
      streamNew.NextToken(out word);

      return stream;
    }
  }
}
