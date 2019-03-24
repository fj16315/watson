using OpenNLP.Tools.Parser;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WatsonAI
{
  public class Children<a> : Pattern<IEnumerable<a>> 
  {
    private readonly Pattern<Parse> parent;
    private readonly Pattern<a> child;

    public Children(Pattern<Parse> parent, Pattern<a> child)
    {
      this.parent = parent;
      this.child = child;
    }

    public override Result<IEnumerable<a>> Match(Parse tree)
    {
      var newTree = parent.Match(tree);
      if (newTree.HasValue)
      {
        return new Result<IEnumerable<a>>(newTree.Value
          .GetChildren()
          .Select(t => child.Match(t))
          .Where(r => r.HasValue)
          .Select(r => r.Value));
      }
      return new Result<IEnumerable<a>>();
    }
  }
}
