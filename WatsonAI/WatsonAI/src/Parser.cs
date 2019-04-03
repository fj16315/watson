using System.IO;
using System.Collections.Generic;

using OpenNLP.Tools.Parser;
using OpenNLP.Tools.Tokenize;

namespace WatsonAI
{
  /// <summary>
  /// Parses sentences into grammatical trees.
  /// </summary>
  public class Parser
  {

    private EnglishTreebankParser parser;

    private EnglishRuleBasedTokenizer tokenizer;

    /// <summary>
    /// Initialises a new instance of the <see cref="GameAI.Parser"/> class.
    /// </summary>
    public Parser()
    {
      var modelPathParse = Directory.GetCurrentDirectory();
      modelPathParse = Path.Combine(modelPathParse, "res", "Models") + Path.DirectorySeparatorChar;

      this.tokenizer = new EnglishRuleBasedTokenizer(false);

      System.Diagnostics.Debug.WriteLine("Loading parser database...");
      this.parser = new EnglishTreebankParser(modelPathParse);
      System.Diagnostics.Debug.WriteLine("Load completed.");
    }

    public Parser(string stringToPath) {
      var modelPathParse = stringToPath;
      modelPathParse = Path.Combine(modelPathParse, "res", "Models") + Path.DirectorySeparatorChar;

      this.tokenizer = new EnglishRuleBasedTokenizer(false);

      System.Diagnostics.Debug.WriteLine("Loading parser database...");
      this.parser = new EnglishTreebankParser(modelPathParse);
      System.Diagnostics.Debug.WriteLine("Load completed.");
    }

    /// <summary>
    /// Generates a parse tree of the gramaitical structure of the sentence.
    /// </summary>
    /// <remarks>
    /// To avoid buggy results, tokenize before parsing, even though parse accepts a string.
    /// </remarks>
    /// <param name="sentence">The sentence.</param>
    /// <param name="parse">A parse tree of the sentence or null.</param>
    /// <returns>True if there is a parse, false if the parse is null.</returns>
    public bool Parse(string sentence, out Parse parse)
    {
      var tokens = Tokenize(sentence);
      parse = this.parser.DoParse(tokens);
      return parse != null;
    }

    /// <summary>
    /// Generates a parse tree of the gramaitical structure of the sentence.
    /// </summary>
    /// <param name="tokens">An IEnumerable of tokens to parse.</param>
    /// <param name="parse">A parse tree of the sentence or null.</param>
    /// <returns>True if there is a parse, false if the parse is null.</returns>
    public bool Parse(IEnumerable<string> tokens, out Parse parse)
    {
      parse = this.parser.DoParse(tokens);
      return parse != null;
    }

    /// <summary>
    /// Splits the sentence into tokens (words).
    /// </summary>
    /// <param name="sentence">The sentence to split.</param>
    /// <returns>A list of tokens.</returns>
    public string[] Tokenize(string sentence)
    {
      return this.tokenizer.Tokenize(sentence);
    }
  }
}
