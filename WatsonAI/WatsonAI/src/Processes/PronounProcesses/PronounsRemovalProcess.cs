using System;
using System.Collections.Generic;
using System.Linq;

namespace WatsonAI
{
  /// <summary>
  /// Implements a pre-process and post-process that remove and add, respectively, pronouns.
  /// </summary>
  /// <remarks>
  /// The string matching in this isn't an optimal complexity algorithm, 
  /// but it's mostly one pass and I think it should be fast enough for the small message sizes.
  /// </remarks>
  public class PronounsRemovalProcess : IProcess
  {
    private readonly Character character;
    private readonly Memory memory;
    private readonly Parser parser;
    private readonly List<Character> characters;
    private IPronounHandler clarificationHandler;
    private bool AwaitingClarification { get { return clarificationHandler != null; } }

    public PronounsRemovalProcess(Character character, List<Character> characters, Memory memory, Parser parser)
    {
      this.character = character;
      this.characters = characters;
      this.memory = memory;
      this.parser = parser;
    }

    /// <summary>
    /// Implements a process that replaces pronouns with the character name in the input stream.
    /// </summary>
    /// <param name="tokens">A reference to a list of tokens to act on.</param>
    public Stream Process(Stream stream)
    {
      if (this.AwaitingClarification)
      {
        stream = clarificationHandler.HandleClarification(stream);
        this.clarificationHandler = null;
        return stream;
      }

      var replacing = new List<ReplacementRule>();

      IPronounHandler simpleHandler = new SimplePronounHandler(this.character, "Watson");
      replacing.AddRange(simpleHandler.GenerateReplacements(stream));

      IPronounHandler itHandler = new ItPronounHandler(this.parser, this.memory);
      replacing.AddRange(itHandler.GenerateReplacements(stream));
      if (itHandler.RequiresClarification())
      {
        this.clarificationHandler = itHandler;
        stream = itHandler.RequestClarification(stream);
        stream.AssignSpecialCaseHandler(this);
      }

      IPronounHandler characterHandler = new CharacterPronounHandler(this.characters, this.memory, this.parser);
      replacing.AddRange(characterHandler.GenerateReplacements(stream));
      if (characterHandler.RequiresClarification())
      {
        this.clarificationHandler = characterHandler;
        stream = itHandler.RequestClarification(stream);
        stream.AssignSpecialCaseHandler(this);
      }

      ReplaceWords(replacing, stream.Input);

      return stream;
    }

    /// <summary>
    /// Applies a list of replacements to the list of tokens. 
    /// </summary>
    /// <param name="replacements">
    /// A list of tuples, each of which has a list of tokens to match against, 
    /// and a list of tokens to replace it with.
    /// </param>
    /// <param name="tokens"></param>
    private void ReplaceWords(List<ReplacementRule> replacements, List<string> tokens)
    {
      for (int i = 0; i < tokens.Count; i++)
      {
        foreach (var replacement in replacements)
        {
          if (i + replacement.OriginalTokens.Count < tokens.Count)
          {
            var tokenSection = tokens.GetRange(i, replacement.OriginalTokens.Count);
            if (AllWordsEqual(replacement.OriginalTokens, tokenSection))
            {
              tokens.RemoveRange(i, replacement.OriginalTokens.Count);
              tokens.InsertRange(i, replacement.ReplacementTokens);
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

    private enum PronounType {
      None, Object, Person
    }
  }
}
