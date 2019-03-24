using OpenNLP.Tools.Parser;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WatsonAI
{
  public class And<a,b> : Pattern<Tuple<a,b>> 
  {
    private readonly Pattern<a> lhs;
    private readonly Pattern<b> rhs;

    public And(Pattern<a> lhs, Pattern<b> rhs)
    {
      this.lhs = lhs;
      this.rhs = rhs;
    }

    public override Result<Tuple<a,b>> Match(Parse tree)
    {
      var lhsResult = lhs.Match(tree);
      var rhsResult = rhs.Match(tree);
      if (lhsResult.HasValue && rhsResult.HasValue)
      {
        return new Result<Tuple<a, b>>(Tuple.Create(lhsResult.Value, rhsResult.Value));
      }
      return new Result<Tuple<a, b>>();
    }
  }
}
