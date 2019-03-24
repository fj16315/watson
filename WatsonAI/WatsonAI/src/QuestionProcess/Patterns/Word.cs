using OpenNLP.Tools.Parser;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WatsonAI
{
  public class Word : Pattern<string>
  {
    private readonly string word;

    private readonly Thesaurus thesaurus;

    public Word(Thesaurus thesaurus, string word)
    {
      this.thesaurus = thesaurus;
      this.word = word;
    }

    public override Result<string> Match(Parse tree) 
      => thesaurus.Describes(tree.Value, word) ? new Result<string>(word) : new Result<string>();
  }
}
