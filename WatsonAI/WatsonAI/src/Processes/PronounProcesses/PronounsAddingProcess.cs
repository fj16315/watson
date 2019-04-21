using System;
using System.Collections.Generic;
using System.Linq;

namespace WatsonAI
{
  /// <summary>
  /// Implements a process that adds pronouns.
  /// </summary>
  /// <remarks>
  /// The string matching in this isn't an optimal complexity algorithm, 
  /// but it's mostly one pass and I think it should be fast enough for the small message sizes.
  /// </remarks>
  public class PronounsAddingProcess : IProcess
  {
    private readonly Character character;
    private readonly Parser parser;

    public PronounsAddingProcess(Character character, Parser parser)
    {
      this.character = character;
      this.parser = parser;
    }

    /// <summary>
    /// Implements a process that replaces character names with pronouns in the ouput stream.
    /// </summary>
    /// <param name="stream">A stream to act on.</param>
    public Stream Process(Stream stream)
    {
      var replacing = new List<Tuple<List<string>, List<string>>>
      {
        Tuple.Create(new List<string> { "Watson" }, new List<string> { "you" }),
        Tuple.Create(new List<string> { "the", character.Name }, new List<string> { "me" }),
        Tuple.Create(new List<string> { character.Name }, new List<string> { "me" })
      };

      ReplaceWords(replacing, stream.Output);

      return stream;
    }

    /// <summary>
    /// Applies a list of replacements to the list of tokens. 
    /// </summary>
    /// <param name="replacements">
    /// A list of tuples, each of which has a list of tokens to match against, 
    /// and a list of tokens to replace it with.
    /// </param>
    /// <param name="tokens">A list of tokens to replace in.</param>
    private void ReplaceWords(List<Tuple<List<string>, List<string>>> replacements, List<string> tokens)
    {
      for (int i = 0; i < tokens.Count; i++)
      {
        foreach (var replacement in replacements)
        {
          var originalPhrase = replacement.Item1;
          var replacementPhrase = replacement.Item2;
          if (i + originalPhrase.Count < tokens.Count)
          {
            var tokenSection = tokens.GetRange(i, originalPhrase.Count);
            if (AllWordsEqual(originalPhrase, tokenSection))
            {
              tokens.RemoveRange(i, originalPhrase.Count);
              tokens.InsertRange(i, replacementPhrase);
            }
          }
        }
      }
    }

    /// <summary>
    /// Checks whether two lists of strings are equal, ignoring case on each string.
    /// </summary>
    /// <param name="firstTokens">The first list of strings.</param>
    /// <param name="secondTokens">The second list of strings.</param>
    /// <returns>True if the two lists are equal, ignoring the case on each string.</returns>
    private bool AllWordsEqual(List<string> firstTokens, List<string> secondTokens)
     => firstTokens.Zip(secondTokens, (x, y) => x.Equals(y, StringComparison.OrdinalIgnoreCase)).All(x => x);
  }
}
