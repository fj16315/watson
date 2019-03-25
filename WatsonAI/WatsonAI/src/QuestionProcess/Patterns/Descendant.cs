using OpenNLP.Tools.Parser;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WatsonAI
{
  /// <summary>
  /// A pattern that matches against the decendants of a branch.
  /// A decendant is a child, child of child, etc.
  /// </summary>
  /// <typeparam name="a">The result of matching the child pattern.</typeparam>
  public class Descendant<a> : Pattern<IEnumerable<a>> 
  {
    private readonly Branch branch;
    private readonly Pattern<a> descendant;

    /// <summary>
    /// Construct a new pattern that matches against all decendants of a branch.
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="child"></param>
    public Descendant(Branch branch, Pattern<a> descendant)
    {
      this.branch = branch;
      this.descendant = descendant;
    }

    /// <summary>
    /// Returns the result of matching the child pattern against all decendants of the branch.
    /// </summary>
    /// <param name="tree">The tree to match against.</param>
    /// <returns>A Result of IEnumerable conataining all successful matches on decendant branches.</returns>
    public override Result<IEnumerable<a>> Match(Parse tree)
    {
      var newTree = branch.Match(tree);
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
