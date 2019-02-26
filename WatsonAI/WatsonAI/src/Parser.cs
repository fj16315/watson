using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using OpenNLP.Tools.PosTagger;
using OpenNLP.Tools.Tokenize;

namespace WatsonAI
{
  /// <summary>
  /// Parses sentences into grammatical trees.
  /// </summary>
  public class Parser
  {
    /// <summary>
    /// Initialises a new instance of the <see cref="GameAI.Parser"/> class.
    /// </summary>
    public Parser()
    {
      var modelPath = Directory.GetCurrentDirectory() + "\\res\\Models\\EnglishPOS.nbin";
      var tagDictDir = Directory.GetCurrentDirectory() + "\\res\\Models\\Parser\\tagdict";

      var sentence = "The quick brown fox jumped over the lazy dog.";

      var tokenizer = new EnglishRuleBasedTokenizer(false);
      var tokens = tokenizer.Tokenize(sentence);

      var posTagger = new EnglishMaximumEntropyPosTagger(modelPath, tagDictDir);
      var pos = posTagger.Tag(tokens);
      
      System.Diagnostics.Debug.WriteLine(pos);
    }
  }
}
