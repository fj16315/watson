using OpenNLP.Tools.Parser;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WatsonAI
{
  public class Descendant<a> : Pattern<IEnumerable<a>> 
  {
    private readonly Pattern<Parse> parent;
    private readonly Pattern<a> descendant;

    public Descendant(Pattern<Parse> parent, Pattern<a> descendant)
    {
      this.parent = parent;
      this.descendant = descendant;
    }

    public override Result<IEnumerable<a>> Match(Parse tree)
    {
      var newTree = parent.Match(tree);
      if (newTree.HasValue)
      {
        Func<Parse, IEnumerable<a>> f = null;
        f = (Parse t) => {
          var d = this.descendant.Match(t);
          var ds = t.GetChildren().SelectMany(f);
          if (!d.HasValue) return ds;
          return ds.Append(d.Value);
        };
        return new Result<IEnumerable<a>>(newTree.Value.GetChildren().SelectMany(f));
      } 
      return new Result<IEnumerable<a>>();
    }
  }
}
