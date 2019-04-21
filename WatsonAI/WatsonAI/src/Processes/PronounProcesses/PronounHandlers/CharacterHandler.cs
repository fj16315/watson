using OpenNLP.Tools.Parser;
using System;
using System.Collections.Generic;
using System.Linq;

using static WatsonAI.Patterns;

namespace WatsonAI
{
  public class CharacterPronounHandler : IPronounHandler
  {
    private readonly List<Character> characters;
    private readonly Memory memory;
    private readonly Parser parser;
    private bool awaitingClarification;

    public CharacterPronounHandler(List<Character> characters, Memory memory, Parser parser)
    {
      this.characters = characters;
      this.memory = memory;
      this.parser = parser;
      this.awaitingClarification = false;
    }

    /// <summary>
    /// Returns all the patterns for a replacing character-based pronouns in the tokens.
    /// </summary>
    /// <remarks>
    /// Does not deal with "her", as this is a special case in the English language, so has it's own method.
    /// </remarks>
    /// <param name="tokens">The tokens to check for characters in.</param>
    /// <returns>
    /// List of tuples containing a list of tokens to match against, and list of tokens to replace.
    /// </returns>
    public List<ReplacementRule> GenerateReplacements(Stream stream)
    {
      var replacing = new List<ReplacementRule>();

      var characters = FindCharactersInInputAndMemory(stream.Input);
      replacing.AddRange(SingularCharacterReplacements(characters));
      replacing.AddRange(PluralCharacterReplacements(characters));
      Parse parse;
      var parseExists = parser.Parse(stream.Input, out parse);
      if (parseExists)
      {
        replacing.AddRange(HerReplacements(stream.Input, parse, characters));
      }

      var input = stream.Input;
      if ((input.Contains("he") || input.Contains("she") || input.Contains("her")
         || input.Contains("his") || input.Contains("their") || input.Contains("they")) 
         && replacing.Count == 0)
      {
        this.awaitingClarification = true;
      }

      return replacing;
    }

    public List<ReplacementRule> SingularCharacterReplacements(List<Character> characters)
    {
      var replacing = new List<ReplacementRule>();

      if (characters.Count == 1)
      {
        var inputCharacter = characters.First();
        if (inputCharacter.Gender == Gender.Male)
        {
          replacing.Add(new ReplacementRule(new List<string> { "him" }, new List<string> { inputCharacter.Name }));
          replacing.Add(new ReplacementRule(new List<string> { "his" }, new List<string> { inputCharacter.Name, "'s" }));
          replacing.Add(new ReplacementRule(new List<string> { "he" }, new List<string> { inputCharacter.Name }));
        }
        else if (inputCharacter.Gender == Gender.Female)
        {
          replacing.Add(new ReplacementRule(new List<string> { "hers" }, new List<string> { inputCharacter.Name, "'s" }));
          replacing.Add(new ReplacementRule(new List<string> { "she" }, new List<string> { inputCharacter.Name }));
        }
        if (inputCharacter.Gender == Gender.Other || inputCharacter.Gender == Gender.Male || inputCharacter.Gender == Gender.Female)
        {
          replacing.Add(new ReplacementRule(new List<string> { "they", "are" }, new List<string> { inputCharacter.Name, "is" }));
          replacing.Add(new ReplacementRule(new List<string> { "they", "'re" }, new List<string> { inputCharacter.Name, "is" }));
          replacing.Add(new ReplacementRule(new List<string> { "are", "they" }, new List<string> { "is", inputCharacter.Name }));
        }
      }

      return replacing;
    }

    public List<ReplacementRule> PluralCharacterReplacements(List<Character> characters)
    {
      var replacing = new List<ReplacementRule>();

      if (characters.Any())
      {
        replacing.Add(new ReplacementRule(new List<string> { "they" }, new List<string>(MultiNounSentence(characters.Select(c => c.Name).ToList()))));
        replacing.Add(new ReplacementRule(new List<string> { "them" }, new List<string>(MultiNounSentence(characters.Select(c => c.Name).ToList()))));
        var theirReplacement = new List<string>(MultiNounSentence(characters.Select(c => c.Name).ToList()));
        theirReplacement.Add("'s");
        replacing.Add(new ReplacementRule(new List<string> { "their" }, theirReplacement));
        replacing.Add(new ReplacementRule(new List<string> { "their", "'s" }, theirReplacement));
      }

      return replacing;
    }


    /// <summary>
    /// Returns the patterns for "her" in the tokens.
    /// </summary>
    /// <param name="tokens">The tokens to check for "her" in.</param>
    /// <param name="parse">A parse of the tokens to check for "her" in.</param>
    /// <returns>
    /// List of tuples containing a list of tokens to match against, and list of tokens to replace.
    /// </returns>
    public List<ReplacementRule> HerReplacements(List<string> tokens, Parse parse, List<Character> characters)
    {
      var replacing = new List<ReplacementRule>();

      if (!tokens.Contains("her") || characters.Count != 1)
      {
        return replacing;
      }

      var top = Branch("TOP");
      var nounPhrase = top >= Branch("NP");
      var nounPhrases = nounPhrase.Match(parse);

      if (nounPhrases.HasValue)
      {
        foreach (var np in nounPhrases.Value)
        {
          if (HerIsInPosessiveForm(np))
          {
            replacing.Add(new ReplacementRule(new List<string> { "her" }, new List<string> { characters.First().Name, "'s" }));
          }
        }
      }
      if (replacing.Count == 0)
      {
        replacing.Add(new ReplacementRule(new List<string> { "her" }, new List<string> { characters.First().Name }));
      }

      return replacing;
    }

    /// <summary>
    /// Returns a list of characters who's names are present in either the input, last response, or last input.
    /// </summary>
    /// <param name="tokens">The tokens of the input.</param>
    /// <returns>The characters in either the input, last response, or last input. In that preference.</returns>
    private List<Character> FindCharactersInInputAndMemory(IEnumerable<string> tokens)
    {
      var characters = FindCharactersInInput(tokens);

      if (characters.Count == 0 && memory.Responses.Count() != 0)
      {
        characters = FindCharactersInInput(parser.Tokenize(memory.GetLastResponse()));
      }
      else if (characters.Count == 0 && memory.Inputs.Count() != 0)
      {
        characters = FindCharactersInInput(parser.Tokenize(memory.GetLastInput()));
      }

      return characters;
    }

    /// <summary>
    /// Returns true if there exists a "her", followed by a noun (and potentially some adjectives).
    /// </summary>
    /// <param name="np">The noun phrase to check.</param>
    /// <returns>True if there exists a "her" indirectly followed by a noun, false otherwise.</returns>
    private bool HerIsInPosessiveForm(Parse np)
    {
      var children = np.GetChildren();
      return children.Any()
                 && children.First().Value.Equals("her", StringComparison.OrdinalIgnoreCase)
                 && children.Last().Type == "NN"
                 && (children.Length <= 2 || children.Length > 2 && children.ToList().GetRange(1, children.Count()).All(c => c.Type == "JJ"));
    }

    /// <summary>
    /// Converts a list of nouns into multiple noun sentence form: eg. [eggs, bacon, ham] => "eggs, bacon and ham".
    /// </summary>
    /// <param name="enumerable"></param>
    /// <returns></returns>
    private List<string> MultiNounSentence(List<string> tokens)
    {
      var sentence = new List<string>(tokens);

      for (int i = 0; i < tokens.Count - 2; i++)
      {
        sentence.Insert(2 * i + 1, ",");
      }

      if (tokens.Count > 1)
      {
        sentence.Insert(sentence.Count - 1, "and");
      }
      return sentence;
    }

    /// <summary>
    /// Finds any character names in the list of tokens.
    /// </summary>
    /// <param name="tokens">The list of tokens to search.</param>
    /// <returns>A list of character names in the tokens.</returns>
    private List<Character> FindCharactersInInput(IEnumerable<string> tokens)
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

    public bool RequiresClarification()
    {
      return this.awaitingClarification;
    }

    public Stream RequestClarification(Stream stream)
    {
      stream.AppendOutput("I'm sorry, who do you mean?");
      return stream;
    }

    public Stream HandleClarification(Stream stream)
    {
      if (JustClarificationProvided(stream.Input))
      {
        var charactersInStream = FindCharactersInInputAndMemory(stream.Input);
        stream = Stream.Tokenise(parser, memory.GetLastInput());
      }
      this.awaitingClarification = false;
      return stream;
    }

    /// <summary>
    /// Checks if the whole question is repeated or just a clarification is provided.
    /// </summary>
    /// <remarks>
    /// Makes a crude assumption about the size of the messages, because I couldn't 
    /// think of a better way of doing this.
    /// </remarks>
    /// <param name="input">The new input.</param>
    /// <returns>True if just a clarification was provided.</returns>
    private bool JustClarificationProvided(List<string> input) 
      => input.Count <= 3 * FindCharactersInInput(input).Count
        || input.Count < memory.GetLastInput().Length / 2;
  }
}
