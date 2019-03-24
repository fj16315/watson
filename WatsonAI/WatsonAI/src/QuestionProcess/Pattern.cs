using OpenNLP.Tools.Parser;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WatsonAI
{
  public abstract class Pattern<a>
  {
    public abstract Result<a> Match(Parse tree);

    public static Pattern<a> operator |(Pattern<a> lhs, Pattern<a> rhs)
      => new Or<a>(lhs, rhs);
  }
}
