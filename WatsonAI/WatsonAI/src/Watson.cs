using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace WatsonAI
{
  public class Watson : IWatson
  {
    private Parser parser;
    private Thesaurus thesaurus; 


    public Watson(string stringToPath) {
      this.parser = new Parser(stringToPath);
      this.thesaurus = new Thesaurus(stringToPath);
    }

    /// <summary>
    /// Run the AI on some input speech.
    /// </summary>
    /// <param name="input">The input given by the player. </param>
    /// <returns>Response to the player.</returns>
    public string Run(string input)
    {
      var io = new InputOutput(input);

      var greetings = new GreetingsEngine();
      var debugParse = new DebugParseEngine(parser);
      var fallback = new FallbackEngine();
      var thesaurusDebug = new ThesaurusDebugEngine(thesaurus);

      var output = io
        .Process(greetings)
        .Process(debugParse)
        .Process(thesaurusDebug)

        .Process(fallback);
      return output.output;
    }

    /// <summary>
    /// Run the AI on some input speech from a character.
    /// </summary>
    /// <remarks> Currently ignores character. </remarks>
    /// <param name="input">The input given by the player. </param>
    /// <param name="character">The character the player is talking to.</param>
    /// <returns>Response to the player.</returns>
    public string Run(string input, Character character)
    {
      return Run(input);
    }
  }

  /// <summary>
  /// Specifying the interface for how the AI will process input strings.
  /// </summary>
  public interface IWatson
  {
    /// <summary>
    /// Run the AI on some input speech.
    /// </summary>
    /// <param name="input">The input given by the player. </param>
    /// <returns>Response to the player.</returns>
    string Run(string input);

    /// <summary>
    /// Run the AI on some input speech from a character.
    /// </summary>
    /// <param name="input">The input given by the player. </param>
    /// <param name="character">The character the player is talking to.</param>
    /// <returns>Response to the player.</returns>
    string Run(string input, Character character);
  }
}
