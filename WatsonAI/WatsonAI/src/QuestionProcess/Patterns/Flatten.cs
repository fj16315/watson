using OpenNLP.Tools.Parser;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WatsonAI
{
  public class Flatten<a> : Pattern<IEnumerable<a>>
  {
    private readonly Pattern<IEnumerable<IEnumerable<a>>> pattern;

    public Flatten(Pattern<IEnumerable<IEnumerable<a>>> pattern)
    {
      this.pattern = pattern;
    }

    public override Result<IEnumerable<a>> Match(Parse tree)
    {
      var result = pattern.Match(tree);
      if (!result.HasValue)
      {
        return new Result<IEnumerable<a>>();
      }
      return new Result<IEnumerable<a>>(
        result.Value.SelectMany(x => x)
      );
    }
  }
}
