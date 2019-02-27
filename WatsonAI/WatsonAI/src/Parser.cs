using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using OpenNLP.Tools.PosTagger;
using OpenNLP.Tools.SentenceDetect;
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
      var modelPathSen = Directory.GetCurrentDirectory() + "\\res\\Models\\EnglishSD.nbin";
      var modelPathTok = Directory.GetCurrentDirectory() + "\\res\\Models\\EnglishTok.nbin";
      var modelPathPos = Directory.GetCurrentDirectory() + "\\res\\Models\\EnglishPOS.nbin";

      var tagDictDir = Directory.GetCurrentDirectory() + "\\res\\Models\\Parser\\tagdict";

      var sentence = "The quick brown fox jumped over the lazy dog. He died.";

      var sentenceDetector = new EnglishMaximumEntropySentenceDetector(modelPathSen);
      var sentences = sentenceDetector.SentenceDetect(sentence);
      var tokenizer = new EnglishRuleBasedTokenizer(true);
      var tokens = tokenizer.Tokenize(sentence);

      var posTagger = new EnglishMaximumEntropyPosTagger(modelPathPos, tagDictDir);
      var pos = posTagger.Tag(tokens);
      
      foreach (var s in pos)
      {
        System.Diagnostics.Debug.WriteLine(s);
      }
    }
  }
}
