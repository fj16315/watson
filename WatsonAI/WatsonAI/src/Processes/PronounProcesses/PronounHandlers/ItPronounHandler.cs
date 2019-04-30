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
        if (objects.Count > 1)
        {
          this.awaitingClarification = true;
        }
      }

      return replacing;
    }

    public Stream HandleClarification(Stream stream)
    {
      this.awaitingClarification = false;

      Parse parse;
      var parseExists = parser.Parse(stream.Input, out parse);
      if (parseExists)
      {
        var top = Branch("TOP");
        var subject = top >= Branch("SBARQ");
        var entity = subject.Match(parse);

        var objects = CheckForItWord(stream.Input, parse);
        if (objects.Count == 1)
        {
          if (entity.HasValue)
          {
            return new Stream(ReplaceIt(stream.Input, objects.First()));
          }
          else
          {
            var previousTokens = parser.Tokenize(this.memory.GetLastInput()).ToList();
            return new Stream(ReplaceIt(previousTokens, objects.First()));
          }
        }
      }
      return RequestClarification(stream);
    }

    /// <summary>
    /// Replaces the word "it" in the tokens with the specified replacement.
    /// </summary>
    /// <param name="tokens">The tokens to search for the "it" word.</param>
    /// <param name="replacement">The noun that "it" refers to.</param>
    /// <returns>The tokens with all occurences of "it" replaced.</returns>
    private List<string> ReplaceIt(List<string> tokens, string replacement)
    {
      return tokens.SelectMany(s =>
      {
        if (s == "it")
        {
          return new List<string> { "the", replacement };
        }
        else
        {
          return new List<string> { s };
        }
      }).ToList();
    }

    /// <summary>
    /// Asks for clarification from the user.
    /// </summary>
    /// <param name="stream">The stream where this process is a special case handler.</param>
    public Stream RequestClarification(Stream stream)
    {
      stream.AppendOutput("I'm sorry, what do you mean?");
      return stream;
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
