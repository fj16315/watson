using System;
using OpenNLP.Tools.Parser;

namespace WatsonAI
{
  public class Map<a, b> : Pattern<b>
  {
    private readonly Pattern<a> pattern;
    private readonly Func<a, b> func;

    public Map(Pattern<a> pattern, Func<a, b> func)
    {
      this.pattern = pattern;
      this.func = func;
    }

    public override Result<b> Match(Parse tree)
    {
      var result = pattern.Match(tree);
      if (!result.HasValue)
      {
        return Result<b>.Fail;
      }
      return new Result<b>(func(result.Value));
    }
  }
}
