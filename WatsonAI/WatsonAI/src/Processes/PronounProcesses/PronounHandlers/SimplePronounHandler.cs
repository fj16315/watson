using System.Collections.Generic;

namespace WatsonAI
{
  public class SimplePronounHandler : IPronounHandler
  {
    private readonly Character character;
    private readonly string detectiveName;

    public SimplePronounHandler(Character character, string detectiveName)
    {
      this.character = character;
      this.detectiveName = detectiveName;
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
    public List<ReplacementRule> GenerateReplacements(Stream stream)
    {
      var replacing = new List<ReplacementRule>
      {
        new ReplacementRule(new List<string> { "do", "you" }, new List<string> { "does", this.character.Name }),
        new ReplacementRule(new List<string> { "you", "are" }, new List<string> { this.character.Name, "is" }),
        new ReplacementRule(new List<string> { "are", "you" }, new List<string> { "is", this.character.Name }),
        new ReplacementRule(new List<string> { "do", "I" }, new List<string> { "does", this.detectiveName }),
        new ReplacementRule(new List<string> { "I", "am" }, new List<string> { "Watson", "is" }),
        new ReplacementRule(new List<string> { "I", "'m" }, new List<string> { "Watson", "is" }),
        new ReplacementRule(new List<string> { "am", "I" }, new List<string> { "is", this.detectiveName }),
        new ReplacementRule(new List<string> { "your" }, new List<string> { this.character.Name, "'s" }),
        new ReplacementRule(new List<string> { "you" }, new List<string> { this.character.Name }),
        new ReplacementRule(new List<string> { "I" }, new List<string> { this.detectiveName }),
        new ReplacementRule(new List<string> { "me" }, new List<string> { this.detectiveName }),
        new ReplacementRule(new List<string> { "my" }, new List<string> { this.detectiveName, "'s" }),
        new ReplacementRule(new List<string> { "mine" }, new List<string> { this.detectiveName, "'s" })
      };
      return replacing;
    }

    /// <summary>
    /// Stream will never need clarification so just returns the same stream.
    /// </summary>
    /// <param name="stream">The stream.</param>
    /// <returns>The stream unmodified.</returns>
    public Stream HandleClarification(Stream stream) => stream;

    /// <summary>
    /// Stream will never need clarification so just returns the same stream.
    /// </summary>
    /// <param name="stream">The stream.</param>
    /// <returns>The stream unmodified.</returns>
    public Stream RequestClarification(Stream stream) => stream;

    /// <summary>
    /// Always returns false as never requires clarification.
    /// </summary>
    /// <returns>False.</returns>
    public bool RequiresClarification() => false;
  }
}
