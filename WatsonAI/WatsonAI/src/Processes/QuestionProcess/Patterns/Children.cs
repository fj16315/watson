using OpenNLP.Tools.Parser;
using System.Collections.Generic;
using System.Linq;

namespace WatsonAI
{
  /// <summary>
  /// A pattern that matches against the children of a branch.
  /// </summary>
  /// <typeparam name="a">The result of matching the child pattern.</typeparam>
  public class Children<a> : Pattern<IEnumerable<a>> 
  {
    private readonly Branch branch;
    private readonly Pattern<a> child;

    /// <summary>
    /// Construct a new pattern that matches against all children of a branch.
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="child"></param>
    public Children(Branch branch, Pattern<a> child)
    {
      this.branch = branch;
      this.child = child;
    }

    /// <summary>
    /// Returns the result of matching the child pattern against all children of the branch.
    /// </summary>
    /// <param name="tree">The tree to match against.</param>
    /// <returns>A Result of IEnumerable conataining all successful matches on child branches.</returns>
    public override Result<IEnumerable<a>> Match(Parse tree)
    {
      var newTree = branch.Match(tree);
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
