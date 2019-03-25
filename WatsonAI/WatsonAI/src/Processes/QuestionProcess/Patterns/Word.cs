using OpenNLP.Tools.Parser;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WatsonAI
{
  /// <summary>
  /// Pattern that matches against a specified word.
  /// </summary>
  public class Word : Pattern<string>
  {
    private readonly string word;

    private readonly Thesaurus thesaurus;

    /// <summary>
    /// Construct a new pattern that matches against a specified word, or synonyms for that word.
    /// </summary>
    /// <param name="thesaurus">The thesaurus to use for synonym checking.</param>
    /// <param name="word">The word to match against.</param>
    public Word(Thesaurus thesaurus, string word)
    {
      this.thesaurus = thesaurus;
      this.word = word;
    }

    /// <summary>
    /// Matches the word and synonyms for it against the parse tree.
    /// </summary>
    /// <param name="tree">The tree to match against.</param>
    /// <returns>The result containing the matching string, or failure.</returns>
    public override Result<string> Match(Parse tree) 
      => thesaurus.Describes(tree.Value, word) ? new Result<string>(word) : new Result<string>();
  }
}
