using OpenNLP.Tools.Parser;
using System;
using System.Collections.Generic;
using System.Linq;

using static WatsonAI.Patterns;

namespace WatsonAI
{
  /// <summary>
  /// Implements a pre-process and post-process that remove and add, respectively, pronouns.
  /// </summary>
  public class PronounsProcess : IPreProcess, IPostProcess
  {
    private readonly Character character;
    private readonly Memory memory;
    private readonly Parser parser;
    private readonly List<Character> characters;


    public PronounsProcess(Character character, List<Character> characters, Parser parser)
    {
      this.character = character;
      this.characters = characters;
      this.parser = parser;
    }

    public PronounsProcess(Character character, List<Character> characters, Memory memory, Parser parser)
    {
      this.character = character;
      this.characters = characters;
      this.memory = memory;
      this.parser = parser;
    }

    /// <summary>
    /// Implements a pre-process that replaces pronouns with the character name in the input stream.
    /// </summary>
    /// <param name="tokens">A reference to a list of tokens to act on.</param>
    public void PreProcess(ref List<string> tokens)
    {
      var replacing = new List<Tuple<List<string>, List<string>>>();

      replacing.AddRange(SimplePronounReplacements());
      replacing.AddRange(ItPronounReplacements(tokens));
      replacing.AddRange(CharacterPronounReplacements(tokens));

      ReplaceWords(replacing, tokens);

      //TODO: Else here is a good place to introduce the failstate.
    }

    /// <summary>
    /// Returns all the simple patterns for a replacing pronouns in the tokens.
    /// </summary>
    /// <remarks>
    /// I define simple to mean not needing to query the structure of the tokens: the replacement is
    /// independent of the input tokens.
    /// </remarks>
    /// <param name="tokens">The tokens to check for nouns in.</param>
    /// <returns>
    /// List of tuples containing a list of tokens to match against, and list of tokens to replace.
    /// </returns>
    public List<Tuple<List<string>, List<string>>> SimplePronounReplacements()
    {
      var replacing = new List<Tuple<List<string>, List<string>>>
      {
        Tuple.Create(new List<string> { "do", "you" }, new List<string> { "does", this.character.Name }),
        Tuple.Create(new List<string> { "you", "are" }, new List<string> { this.character.Name, "is" }),
        Tuple.Create(new List<string> { "are", "you" }, new List<string> { "is", this.character.Name }),
        Tuple.Create(new List<string> { "do", "I" }, new List<string> { "does", "Watson" }),
        Tuple.Create(new List<string> { "I", "am" }, new List<string> { "Watson", "is" }),
        Tuple.Create(new List<string> { "I", "'m" }, new List<string> { "Watson", "is" }),
        Tuple.Create(new List<string> { "am", "I" }, new List<string> { "is", "Watson" }),
        Tuple.Create(new List<string> { "your" }, new List<string> { this.character.Name, "'s" }),
        Tuple.Create(new List<string> { "you" }, new List<string> { this.character.Name }),
        Tuple.Create(new List<string> { "I" }, new List<string> { "Watson" }),
        Tuple.Create(new List<string> { "me" }, new List<string> { "Watson" }),
        Tuple.Create(new List<string> { "my" }, new List<string> { "Watson", "'s" }),
        Tuple.Create(new List<string> { "mine" }, new List<string> { "Watson", "'s" })
      };
      return replacing;
    }

    /// <summary>
    /// Returns all the patterns for a replacing it-based pronouns in the tokens.
    /// </summary>
    /// <param name="tokens">The tokens to check for nouns in.</param>
    /// <returns>
    /// List of tuples containing a list of tokens to match against, and list of tokens to replace.
    /// </returns>
    public List<Tuple<List<string>, List<string>>> ItPronounReplacements(List<string> tokens)
    {
      var replacing = new List<Tuple<List<string>, List<string>>>();
      
      string entity = "";
      var replacingItWord = CheckForItWord(tokens, out entity);

      if (replacingItWord)
      {
        replacing.Add(Tuple.Create(new List<string> { "it" }, new List<string> { "the", entity }));
      }
      return replacing;
    }

    /// <summary>
    /// Returns all the patterns for a replacing character-based pronouns in the tokens.
    /// </summary>
    /// <param name="tokens">The tokens to check for characters in.</param>
    /// <returns>
    /// List of tuples containing a list of tokens to match against, and list of tokens to replace.
    /// </returns>
    public List<Tuple<List<string>, List<string>>> CharacterPronounReplacements(List<string> tokens)
    {
      var replacing = new List<Tuple<List<string>, List<string>>>();

      var inputCharacters = FindCharactersInInput(tokens);

      if (inputCharacters.Any())
      {
        var inputCharacter = inputCharacters.First();
        if (inputCharacter.Gender == Gender.Male)
        {
          replacing.Add(Tuple.Create(new List<string> { "him" }, new List<string> { inputCharacter.Name }));
          replacing.Add(Tuple.Create(new List<string> { "his" }, new List<string> { inputCharacter.Name, "'s" }));
          replacing.Add(Tuple.Create(new List<string> { "he" }, new List<string> { inputCharacter.Name }));
        }
        else if (inputCharacter.Gender == Gender.Female)
        {
          // This needs to support her and hers
          replacing.Add(Tuple.Create(new List<string> { "her" }, new List<string> { inputCharacter.Name }));
          replacing.Add(Tuple.Create(new List<string> { "hers" }, new List<string> { inputCharacter.Name, "'s" }));
          replacing.Add(Tuple.Create(new List<string> { "she" }, new List<string> { inputCharacter.Name }));
        }
        if (inputCharacter.Gender == Gender.Other || inputCharacter.Gender == Gender.Male || inputCharacter.Gender == Gender.Female)
        {
          replacing.Add(Tuple.Create(new List<string> { "them" }, new List<string> { inputCharacter.Name }));
          replacing.Add(Tuple.Create(new List<string> { "their" }, new List<string> { inputCharacter.Name, "'s" }));
          replacing.Add(Tuple.Create(new List<string> { "their", "'s" }, new List<string> { inputCharacter.Name, "'s" }));
          if (inputCharacters.Count == 1)
          {
            replacing.Add(Tuple.Create(new List<string> { "they", "are" }, new List<string> { inputCharacter.Name, "is" }));
            replacing.Add(Tuple.Create(new List<string> { "they" }, new List<string> { inputCharacter.Name }));
            replacing.Add(Tuple.Create(new List<string> { "are", "they" }, new List<string> { "is", inputCharacter.Name }));
          }
        }
      }
      return replacing;
    }

    /// <summary>
    /// Implements a post-process that replaces character names with pronouns in the ouput stream.
    /// </summary>
    /// <param name="tokens">A reference to a list of tokens to act on.</param>
    public void PostProcess(ref List<string> tokens)
    {
      var replacing = new List<Tuple<List<string>, List<string>>>
      {
        Tuple.Create(new List<string> { "Watson" }, new List<string> { "you" }),
        Tuple.Create(new List<string> { "the", character.Name }, new List<string> { "me" }),
        Tuple.Create(new List<string> { character.Name }, new List<string> { "me" })
      };

      ReplaceWords(replacing, tokens);
    }

    /// <summary>
    /// Applies a list of replacements to the list of tokens. 
    /// </summary>
    /// <param name="replacements">
    /// A list of tuples, each of which has a list of tokens to match against, 
    /// and a list of tokens to replace it with.
    /// </param>
    /// <param name="tokens"></param>
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

    /// <summary>
    /// Finds any character names in the list of tokens.
    /// </summary>
    /// <param name="tokens">The list of tokens to search.</param>
    /// <returns>A list of character names in the tokens.</returns>
    private List<Character> FindCharactersInInput(List<string> tokens)
    {
      var storyCharacters = this.characters;
      var inputCharacters = new List<Character>();
      foreach (var token in tokens)
      {
        foreach (var character in storyCharacters)
        {
          if (token.Equals(character.Name, StringComparison.OrdinalIgnoreCase))
          {
            inputCharacters.Add(character);
          }
        }
      }
      return inputCharacters;
    }

    /// <summary>
    /// Checks for "it" token and finds the correct replacement in the sentence or in memory.
    /// </summary>
    /// <param name="tokens">The list of tokens to check for "it" word.</param>
    /// <param name="word">Out parameter, the noun found to replace "it".</param>
    /// <returns>True if a noun replacement for "it" was found.</returns>
    private bool CheckForItWord(List<string> tokens, out string word)
    {
      if (tokens.Contains("it"))
      {
        var sentenceUpToIt = tokens.Take(tokens.FindIndex(x => x == "it"));
        if (FindItWord(sentenceUpToIt, out word))
        {
          return true;
        }
        else if (this.memory != null)
        {
          var response = memory.GetLastResponse();
          var responseTokens = parser.Tokenize(response);
          if (FindItWord(responseTokens, out word))
          {
            return true;
          }
          var inputTokens = parser.Tokenize(memory.GetLastInput());
          if (FindItWord(inputTokens, out word))
          {
            return true;
          }
        }
      }
      word = "";
      return false;
    }

    /// <summary>
    /// Finds a noun that an it references.
    /// </summary>
    /// <param name="tokens">The tokens to search for noun.</param>
    /// <param name="word">Out paramater, the noun found to replace "it".</param>
    /// <returns>True if a noun was found.</returns>
    private bool FindItWord(IEnumerable<string> tokens, out string word)
    {
      Parse parse;
      var parseExists = parser.Parse(tokens, out parse);

      if (parseExists)
      {
        var top = Branch("TOP");

        var subject = top >= Branch("NN");

        var entity = subject.Match(parse);

        if (entity.HasValue)
        {
          word = entity.Value.First().Value;
          return true;
        }
      }
      word = "";
      return false;
    }
  }
}
