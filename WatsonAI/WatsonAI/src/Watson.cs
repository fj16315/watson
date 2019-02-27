using System;
using System.Collections.Generic;
using System.Text;

namespace WatsonAI
{
  public class Watson : IWatson
  {
    private Parser parser = new Parser();

    /// <summary>
    /// Run the AI on some input speech.
    /// </summary>
    /// <param name="input">The input given by the player. </param>
    /// <returns>Response to the player.</returns>
    public string Run(string input)
    {
      var parse = parser.Parse(input);
      return parse.Show();
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
