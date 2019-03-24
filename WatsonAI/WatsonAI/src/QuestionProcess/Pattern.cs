using OpenNLP.Tools.Parser;

namespace WatsonAI
{
  /// <summary>
  /// A pattern provides a way to match against a parse tree.
  /// </summary>
  /// <typeparam name="a">The type that the pattern produces after matching.</typeparam>
  public abstract class Pattern<a>
  {
    /// <summary>
    /// Match against a parse tree.
    /// </summary>
    /// <param name="tree">The tree to match against.</param>
    /// <returns>Returns a Result containing the result of matching.</returns>
    public abstract Result<a> Match(Parse tree);

    /// <summary>
    /// The or operator. Uses the Or Pattern to generate the result.
    /// </summary>
    /// <param name="lhs">The first pattern.</param>
    /// <param name="rhs">The second pattern.</param>
    /// <returns>Or of pattern lhs and pattern rhs.</returns>
    public static Pattern<a> operator |(Pattern<a> lhs, Pattern<a> rhs)
      => new Or<a>(lhs, rhs);
  }
}
