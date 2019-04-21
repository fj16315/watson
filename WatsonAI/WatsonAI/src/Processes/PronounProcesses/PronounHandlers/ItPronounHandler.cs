using OpenNLP.Tools.Parser;
using System.Collections.Generic;
using System.Linq;

using static WatsonAI.Patterns;

namespace WatsonAI
{
  public class ItPronounHandler : IPronounHandler
  {
    private readonly Parser parser;
    private Memory memory;
    private bool awaitingClarification;


    public ItPronounHandler(Parser parser, Memory memory)
    {
      this.parser = parser;
      this.memory = memory;
      this.awaitingClarification = false;
    }

    /// <summary>
    /// Returns all the patterns for a replacing it-based pronouns in the tokens.
    /// </summary>
    /// <param name="tokens">The tokens to check for nouns in.</param>
    /// <param name="parse">A parse of the tokens to search for the noun.</param>
    /// <returns>
    /// List of tuples containing a list of tokens to match against, and list of tokens to replace.
    /// </returns>
    public List<ReplacementRule> GenerateReplacements(Stream stream)
    {
      var replacing = new List<ReplacementRule>();

      Parse parse;
      var parseExists = parser.Parse(stream.Input, out parse);
      if (parseExists)
      {
        var objects = CheckForItWord(stream.Input, parse);
        if (objects.Count == 1)
        {
          replacing.Add(new ReplacementRule(new List<string> { "it" }, new List<string> { "the", objects.First() }));
        }
      }

      return replacing;
    }

    /// <summary>
    /// Asks for clarification from the user.
    /// </summary>
    /// <param name="stream">The stream where this process is a special case handler.</param>
    /// <param name="remainingPronounType">The type of the remaining pronoun.</param>
    public Stream HandleClarification(Stream stream)
    {
      stream.AppendOutput("I'm sorry, what do you mean?");
      return stream;
    }

    public Stream RequestClarification(Stream stream)
    {
      throw new System.NotImplementedException();
    }

    public bool RequiresClarification()
    {
      return this.awaitingClarification;
    }

    /// <summary>
    /// Checks for "it" token and finds the correct replacement in the sentence or in memory.
    /// </summary>
    /// <param name="tokens">The list of tokens to check for "it" word.</param>
    /// <param name="parse">A parse of the tokens to search for the noun.</param>
    /// <returns>A list of possible nouns.</returns>
    private List<string> CheckForItWord(List<string> tokens, Parse parse)
    {
      Parse itParse = parse;
      if (tokens.Contains("it"))
      {
        var sentenceUpToIt = tokens.Take(tokens.FindIndex(x => x == "it"));
        var objects = FindItWord(itParse);
        if (objects.Count != 0) return objects;

        var responseTokens = parser.Tokenize(memory.GetLastResponse());
        var parseExists = parser.Parse(responseTokens, out itParse);
        if (parseExists)
        {
          objects = FindItWord(itParse);
          if (objects.Count != 0) return objects;
        }

        var inputTokens = parser.Tokenize(memory.GetLastInput());
        parseExists = parser.Parse(inputTokens, out itParse);
        if (parseExists)
        {
          objects = FindItWord(itParse);
          if (objects.Count != 0) return objects;
        }
      }
      return new List<string>();
    }

    /// <summary>
    /// Finds the noun that an it references.
    /// </summary>
    /// <param name="parse">A parse of the tokens to search for the noun.</param>
    /// <returns>A list of possible nouns.</returns>
    private List<string> FindItWord(Parse parse)
    {
      var top = Branch("TOP");
      var subject = top >= Branch("NN");
      var entity = subject.Match(parse);

      if (entity.HasValue)
      {
        return entity.Value.Select(x => x.Value).ToList();
      }
      return new List<string>();
    }
  }
}
