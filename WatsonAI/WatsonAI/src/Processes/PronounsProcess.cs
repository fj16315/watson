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
      Parse parse;
      var parseExists = parser.Parse(tokens, out parse);

      var replacing = new List<Tuple<List<string>, List<string>>>();

      replacing.AddRange(SimplePronounReplacements());
      if (parseExists)
      {
        replacing.AddRange(ItPronounReplacements(tokens, parse));
        replacing.AddRange(HerPronounReplacements(tokens, parse));
      }
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
    /// <param name="parse">A parse of the tokens to search for the noun.</param>
    /// <returns>
    /// List of tuples containing a list of tokens to match against, and list of tokens to replace.
    /// </returns>
    public List<Tuple<List<string>, List<string>>> ItPronounReplacements(List<string> tokens, Parse parse)
    {
      var replacing = new List<Tuple<List<string>, List<string>>>();
      
      string entity = "";
      var replacingItWord = CheckForItWord(tokens, parse, out entity);

      if (replacingItWord)
      {
        replacing.Add(Tuple.Create(new List<string> { "it" }, new List<string> { "the", entity }));
      }
      return replacing;
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
    public List<Tuple<List<string>, List<string>>> CharacterPronounReplacements(List<string> tokens)
    {
      var replacing = new List<Tuple<List<string>, List<string>>>();

      var characters = FindCharactersInInputAndMemory(tokens);

      if (characters.Any())
      {
        var inputCharacter = characters.First();
        if (inputCharacter.Gender == Gender.Male)
        {
          replacing.Add(Tuple.Create(new List<string> { "him" }, new List<string> { inputCharacter.Name }));
          replacing.Add(Tuple.Create(new List<string> { "his" }, new List<string> { inputCharacter.Name, "'s" }));
          replacing.Add(Tuple.Create(new List<string> { "he" }, new List<string> { inputCharacter.Name }));
        }
        else if (inputCharacter.Gender == Gender.Female)
        {
          replacing.Add(Tuple.Create(new List<string> { "hers" }, new List<string> { inputCharacter.Name, "'s" }));
          replacing.Add(Tuple.Create(new List<string> { "she" }, new List<string> { inputCharacter.Name }));
        }
        if (inputCharacter.Gender == Gender.Other || inputCharacter.Gender == Gender.Male || inputCharacter.Gender == Gender.Female)
        {
          if (characters.Count == 1)
          {
            replacing.Add(Tuple.Create(new List<string> { "they", "are" }, new List<string> { inputCharacter.Name, "is" }));
            replacing.Add(Tuple.Create(new List<string> { "they", "'re" }, new List<string> { inputCharacter.Name, "is" }));
            replacing.Add(Tuple.Create(new List<string> { "are", "they" }, new List<string> { "is", inputCharacter.Name }));
          }
        }
        replacing.Add(Tuple.Create(new List<string> { "they" }, new List<string>(MultiNounSentence(characters.Select(c => c.Name).ToList()))));
        replacing.Add(Tuple.Create(new List<string> { "them" }, new List<string>(MultiNounSentence(characters.Select(c => c.Name).ToList()))));
        var theirReplacement = new List<string>(MultiNounSentence(characters.Select(c => c.Name).ToList()));
        theirReplacement.Add("'s");
        replacing.Add(Tuple.Create(new List<string> { "their" }, theirReplacement));
        replacing.Add(Tuple.Create(new List<string> { "their", "'s" }, theirReplacement));
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
    public List<Tuple<List<string>, List<string>>> HerPronounReplacements(List<string> tokens, Parse parse)
    {
      var replacing = new List<Tuple<List<string>, List<string>>>();
      var characters = FindCharactersInInputAndMemory(tokens);

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
            replacing.Add(Tuple.Create(new List<string> { "her" }, new List<string> { characters.First().Name, "'s" }));
          }
        }
      }
      if (replacing.Count == 0)
      {
        replacing.Add(Tuple.Create(new List<string> { "her" }, new List<string> { characters.First().Name }));
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
      if (memory != null)
      {
        if (characters.Count == 0 && memory.Responses.Count() != 0)
        {
          characters = FindCharactersInInput(parser.Tokenize(memory.GetLastResponse()));
        }
        else if (characters.Count == 0 && memory.Inputs.Count() != 0)
        {
          characters = FindCharactersInInput(parser.Tokenize(memory.GetLastInput()));
        }
      }
      return characters;
    }


    /// <summary>
    /// Returns true if there exists a "her", followed by a noun (and some potentially adjectives).
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

    /// <summary>
    /// Checks for "it" token and finds the correct replacement in the sentence or in memory.
    /// </summary>
    /// <param name="tokens">The list of tokens to check for "it" word.</param>
    /// <param name="parse">A parse of the tokens to search for the noun.</param>
    /// <param name="word">Out parameter, the noun found to replace "it".</param>
    /// <returns>True if a noun replacement for "it" was found.</returns>
    private bool CheckForItWord(List<string> tokens, Parse parse, out string word)
    {
      Parse itParse = parse;
      if (tokens.Contains("it"))
      {
        var sentenceUpToIt = tokens.Take(tokens.FindIndex(x => x == "it"));
        if (FindItWord(itParse, out word))
        {
          return true;
        }
        else if (this.memory != null)
        {
          var responseTokens = parser.Tokenize(memory.GetLastResponse());
          var parseExists = parser.Parse(responseTokens, out itParse);
          if (parseExists && FindItWord(itParse, out word))
          {
            return true;
          }
          var inputTokens = parser.Tokenize(memory.GetLastInput());
          parseExists = parser.Parse(inputTokens, out itParse);
          if (parseExists && FindItWord(itParse, out word))
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
    /// <param name="parse">A parse of the tokens to search for the noun.</param>
    /// <param name="word">Out paramater, the noun found to replace "it".</param>
    /// <returns>True if a noun was found.</returns>
    private bool FindItWord(Parse parse, out string word)
    {
      var top = Branch("TOP");
      var subject = top >= Branch("NN");
      var entity = subject.Match(parse);

      if (entity.HasValue)
      {
        word = entity.Value.First().Value;
        return true;
      }
      word = "";
      return false;
    }
  }
}
