using OpenNLP.Tools.Parser;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WatsonAI
{
  public class Word : Pattern<string>
  {
    private readonly string word;

    private static Thesaurus thesaurus;

    public static void SetThesaurus(Thesaurus thesaurus)
    {
      Word.thesaurus = thesaurus;
    }

    public Word(string word)
    {
      this.word = word;
    }

    public override Result<string> Match(Parse tree) 
      => thesaurus.Describes(tree.Value, word) ? new Result<string>(word) : new Result<string>();
  }
}
