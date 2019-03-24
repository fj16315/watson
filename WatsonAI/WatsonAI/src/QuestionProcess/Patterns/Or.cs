using OpenNLP.Tools.Parser;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WatsonAI
{
  public class Or<a> : Pattern<a> 
  {
    private readonly Pattern<a> lhs;
    private readonly Pattern<a> rhs;

    public Or(Pattern<a> lhs, Pattern<a> rhs)
    {
      this.lhs = lhs;
      this.rhs = rhs;
    }

    public override Result<a> Match(Parse tree)
      => lhs.Match(tree) | rhs.Match(tree);
  }
}
