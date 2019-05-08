using System;
using OpenNLP.Tools.Parser;

namespace WatsonAI
{
  public class Then<a,b> : Pattern<Tuple<a,b>>
  {
    private readonly Pattern<a> first;
    private readonly Pattern<b> second;

    public Then(Pattern<a> first, Pattern<b> second)
    {
      this.first = first;
      this.second = second;
    }

    public override Result<Tuple<a, b>> Match(Parse tree)
    {
      var children = tree.GetChildren();
      var ix = 0;
      var value_f = default(a);
      for (; ix < children.Length; ++ix)
      {
        var result = first.Match(children[ix]);
        if (result.HasValue)
        {
          value_f = result.Value;
          ++ix;
          break;
        }
      }
      for (; ix < children.Length; ++ix)
      {
        var result = second.Match(children[ix]);
        if (result.HasValue)
        {
          // By this point, value_f will have been assigned to.
          return new Result<Tuple<a, b>>(
            Tuple.Create(value_f, result.Value)
          );
        }
      }
      return Result<Tuple<a, b>>.Fail;
    }
  }
}
