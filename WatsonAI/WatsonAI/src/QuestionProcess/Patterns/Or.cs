using OpenNLP.Tools.Parser;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WatsonAI
{
  /// <summary>
  /// Patterns that matches if the lhs or rhs match.
  /// </summary>
  /// <typeparam name="a">The type of result of lhs or rhs.</typeparam>
  public class Or<a> : Pattern<a> 
  {
    private readonly Pattern<a> lhs;
    private readonly Pattern<a> rhs;

    /// <summary>
    /// Construct a new pattern that matches against either the lhs or the rhs.
    /// </summary>
    /// <param name="lhs">The first pattern to match against.</param>
    /// <param name="rhs">The second pattern to match against.</param>
    public Or(Pattern<a> lhs, Pattern<a> rhs)
    {
      this.lhs = lhs;
      this.rhs = rhs;
    }

    /// <summary>
    /// Matches both patterns against the tree and returns lhs if successful, else rhs.
    /// </summary>
    /// <param name="tree">The tree to match against.</param>
    /// <returns>The result of lhs or rhs, failure if both fail.</returns>
    public override Result<a> Match(Parse tree)
      => lhs.Match(tree) | rhs.Match(tree);
  }
}
