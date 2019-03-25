using OpenNLP.Tools.Parser;
using System;

namespace WatsonAI
{
  /// <summary>
  /// Pattern that matches if both a and b match.
  /// </summary>
  /// <typeparam name="a">The result of the first match.</typeparam>
  /// <typeparam name="b">The result of the second match.</typeparam>
  public class And<a,b> : Pattern<Tuple<a,b>> 
  {
    private readonly Pattern<a> lhs;
    private readonly Pattern<b> rhs;

    /// <summary>
    /// Construct a new pattern that matches against both patterns.
    /// </summary>
    /// <param name="lhs">The first pattern to match against.</param>
    /// <param name="rhs">The second pattern to match against.</param>
    public And(Pattern<a> lhs, Pattern<b> rhs)
    {
      this.lhs = lhs;
      this.rhs = rhs;
    }

    /// <summary>
    /// Matches both patterns against the tree and returns the result of both.
    /// </summary>
    /// <param name="tree">The tree to match against.</param>
    /// <returns>A Result with value Tuple containing the result of matching both patterns, 
    /// or failure if either fail.</returns>
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
