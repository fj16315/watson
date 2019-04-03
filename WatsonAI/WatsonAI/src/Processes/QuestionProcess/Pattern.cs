using System.Collections.Generic;

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

    public static Children<a> operator >(Branch branch, Pattern<a> child)
      => new Children<a>(branch, child);

    public static Children<a> operator <(Branch branch, Pattern<a> child)
    {
      throw new System.NotSupportedException("Operator < is not supported for Patterns, it is just required for compilation.");
    }

    public static Descendant<a> operator >=(Branch branch, Pattern<a> child)
      => new Descendant<a>(branch, child);

    public static Descendant<a> operator <=(Branch branch, Pattern<a> child)
    {
      throw new System.NotSupportedException("Operator <= is not supported for Patterns, it is just required for compilation.");
    }
  }

  public static class Patterns
  {
    public static Word Word(Thesaurus t, string word)
      => new Word(t, word);

    public static Branch Branch(string name)
      => new Branch(name);

    public static And<a, b> And<a, b>(Pattern<a> lhs, Pattern<b> rhs)
      => new And<a, b>(lhs, rhs);
  }

  public static class FlattenExtension
  {
    public static Flatten<a> Flatten<a>(this Pattern<IEnumerable<IEnumerable<a>>> pattern)
      => new Flatten<a>(pattern);
  }
}
