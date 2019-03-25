using OpenNLP.Tools.Parser;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WatsonAI
{
  /// <summary>
  /// A pattern that flattens previous matches. 
  /// </summary>
  /// <typeparam name="a">The type of the containing IEnumerables.</typeparam>
  public class Flatten<a> : Pattern<IEnumerable<a>>
  {
    private readonly Pattern<IEnumerable<IEnumerable<a>>> pattern;

    /// <summary>
    /// Construct a new pattern that flattens previous matches.
    /// </summary>
    /// <param name="pattern">An IEnumerable of IEnumerable to be flattened.</param>
    public Flatten(Pattern<IEnumerable<IEnumerable<a>>> pattern)
    {
      this.pattern = pattern;
    }

    /// <summary>
    /// Returns the flattened result of matching against the parse tree.
    /// </summary>
    /// <param name="tree">The tree to match against.</param>
    /// <returns>A result of the flattened IEnumerable.</returns>
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
