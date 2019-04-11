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
      string entity = "";
      var replacingItWord = CheckForItWord(tokens, out entity);
      var inputCharacters = FindCharactersInInput(tokens);

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

      if (replacingItWord)
      {
        replacing.Add(Tuple.Create(new List<string> { "it" }, new List<string> { "the", entity }));
      }
      if (inputCharacters.Any())
      {
        var inputCharacter = inputCharacters.First();
        if (inputCharacter.Gender == Gender.Male)
        {
          replacing.Add(Tuple.Create(new List<string> { "him" }, new List<string> { inputCharacter.Name }));
          replacing.Add(Tuple.Create(new List<string> { "his" }, new List<string> { inputCharacter.Name, "'s" }));
          replacing.Add(Tuple.Create(new List<string> { "he" }, new List<string> { inputCharacter.Name }));
        }
        if (inputCharacter.Gender == Gender.Female)
        {
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
      ReplaceWords(replacing, tokens);

      //TODO: Else here is a good place to introduce the failstate.
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

    private bool AllWordsEqual(List<string> firstTokens, List<string> secondTokens)
     => firstTokens.Zip(secondTokens, (x, y) => x.Equals(y, StringComparison.OrdinalIgnoreCase)).All(x => x);

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
