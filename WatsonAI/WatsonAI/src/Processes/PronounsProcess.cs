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
    private readonly Memory inputMemory;
    private readonly Parser parser;


    public PronounsProcess(Character character, Parser parser)
    {
      this.character = character;
      this.parser = parser;
    }

    public PronounsProcess(Character character, Memory inputMemory, Parser parser)
    {
      this.character = character;
      this.inputMemory = inputMemory;
      this.parser = parser;
    }

    /// <summary>
    /// Implements a pre-process that replaces pronouns with the character name in the input stream.
    /// </summary>
    /// <param name="tokens">A reference to a list of tokens to act on.</param>
    public void PreProcess(ref List<string> tokens)
    {
      var replacingIt = false;
      string entity = "";
      if (tokens.Contains("it"))
      {
        if (FindItWord(tokens, out entity))
        {
          replacingIt = true;
        }
      }

      for (int i = 0; i < tokens.Count; i++)
      {
        if (i < tokens.Count - 1)
        {
          ReplaceWords(new List<string> { "you", "are" }, new List<string> { this.character.Name, "is" }, tokens, i);
          ReplaceWords(new List<string> { "are", "you" }, new List<string> { "is", this.character.Name }, tokens, i);

          ReplaceWords(new List<string> { "I", "am" }, new List<string> { "Watson", "is" }, tokens, i);
          ReplaceWords(new List<string> { "I", "'m" }, new List<string> { "Watson", "is" }, tokens, i);
          ReplaceWords(new List<string> { "am", "I" }, new List<string> { "is", "Watson" }, tokens, i);
        }

        ReplaceWords(new List<string> { "your" }, new List<string> { this.character.Name, "'s" }, tokens, i);
        ReplaceWords(new List<string> { "you" }, new List<string> { this.character.Name }, tokens, i);
        ReplaceWords(new List<string> { "I" }, new List<string> { "Watson" }, tokens, i);
        ReplaceWords(new List<string> { "me" }, new List<string> { "Watson" }, tokens, i);
        ReplaceWords(new List<string> { "my" }, new List<string> { "Watson", "'s" }, tokens, i);
        ReplaceWords(new List<string> { "mine" }, new List<string> { "Watson", "'s" }, tokens, i);

        if (replacingIt)
        {
          ReplaceWords(new List<string> { "it" }, new List<string> { "the", entity }, tokens, i);
        }
      }

      //TODO: Else here is a good place to introduce the failstate.
    }

    /// <summary>
    /// Implements a post-process that replaces character names with pronouns in the ouput stream.
    /// </summary>
    /// <param name="tokens">A reference to a list of tokens to act on.</param>
    public void PostProcess(ref List<string> tokens)
    {
      for (int i = 0; i < tokens.Count; i++)
      {
        ReplaceWords(new List<string> { "Watson" }, new List<string> { "you" }, tokens, i);
        ReplaceWords(new List<string> { "the", character.Name }, new List<string> { "me" }, tokens, i);
        ReplaceWords(new List<string> { character.Name }, new List<string> { "me" }, tokens, i);
      }
    }

    private void ReplaceWords(List<string> originals, List<string> replacements, List<string> tokens, int i)
    {
      if (i + originals.Count <= tokens.Count)
      {
        var originalTokens = tokens.GetRange(i, originals.Count);
        if (originals.Zip(originalTokens, (x, y) => x.Equals(y, StringComparison.OrdinalIgnoreCase)).All(x => x))
        {
          tokens.RemoveRange(i, originals.Count);
          tokens.InsertRange(i, replacements);
        }
      }
    }

    private List<Character> FindCharactersInInput(List<string> tokens)
    {
      var storyCharacters = Story.Characters.Values.ToList();
      var inputCharacters = new List<Character>();
      foreach (var token in tokens)
      {
        foreach (var character in storyCharacters)
        {
          if (token.Equals(character.Name))
          {
            inputCharacters.Add(character);
          }
        }
      }
      return inputCharacters;
    }

    private bool FindItWord(List<string> tokens, out string word)
    {
      var sentenceUpToIt = tokens.Take(tokens.FindIndex(x => x == "it"));
      Parse parse;
      var parseExists = parser.Parse(sentenceUpToIt, out parse);

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
