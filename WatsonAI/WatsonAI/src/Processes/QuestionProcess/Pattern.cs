using System;
using System.Linq;
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
    /// Returns the <see cref="Result{T}"/> of the first pattern that successfully matches
    /// with a preference for the left hand side pattern.
    /// </summary>
    /// <param name="lhs">The first pattern.</param>
    /// <param name="rhs">The second pattern.</param>
    /// <returns>The first successful <see cref="Result{T}"/>.</returns>
    public static Pattern<a> operator |(Pattern<a> lhs, Pattern<a> rhs)
      => new Or<a>(lhs, rhs);

    /// <summary>
    /// Runs <paramref name="child"/> on the children of
    /// <paramref name="branch"/>.
    /// </summary>
    /// <param name="branch">The parent <see cref="WatsonAI.Branch"/>.</param>
    /// <param name="child">The child <see cref="WatsonAI.Pattern{T}"/>.</param>
    /// <returns>The <see cref="Result{T}"/> from <paramref name="child"/>
    /// if both succeed.</returns>
    public static Children<a> operator >(Branch branch, Pattern<a> child)
      => new Children<a>(branch, child);

    /// <summary>
    /// Runs <paramref name="descendant"/> on the descendants of
    /// <paramref name="branch"/>.
    /// </summary>
    /// <param name="branch">The ancestor <see cref="WatsonAI.Branch"/>.</param>
    /// <param name="descendant">The descendant <see cref="WatsonAI.Pattern{T}"/>.</param>
    /// <returns>The <see cref="Result{T}"/> from <paramref name="descendant"/>
    /// if both succeed.</returns>
    public static Descendant<a> operator >=(Branch branch, Pattern<a> descendant)
      => new Descendant<a>(branch, descendant);

    // This operator is not used, it is only defined as comparison operators
    // must be defined in pairs. Using this throws a compilation error.
    [Obsolete("Use operator > for declaring a child relation", true)]
    public static Children<a> operator <(Branch branch, Pattern<a> child)
    {
      throw new NotSupportedException("Operator < is not supported for Patterns, it is just required for compilation.");
    }

    // This operator is not used, it is only defined as comparison operators
    // must be defined in pairs. Using this throws a compilation error.
    [Obsolete("Use operator >= for declaring a descendant relation", true)]
    public static Descendant<a> operator <=(Branch branch, Pattern<a> child)
    {
      throw new NotSupportedException("Operator <= is not supported for Patterns, it is just required for compilation.");
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

  public static class PatternExtension
  {
    /// <summary>
    /// Flattens one level of the structure inside <paramref name="pattern"/>.
    /// </summary>
    /// <returns>A pattern containing the flattened structure.</returns>
    /// <param name="pattern">Pattern.</param>
    /// <remarks>Equivalent to <c>fmap join</c>.</remarks>
    public static Pattern<IEnumerable<a>> Flatten<a>(this Pattern<IEnumerable<IEnumerable<a>>> pattern)
      => pattern.Map(e => e.SelectMany(x => x));

    /// <summary>
    /// Apply <paramref name="func"/> to the <see cref="Result{T}"/> of
    /// <paramref name="pattern"/>.
    /// </summary>
    /// <returns>A <see cref="Pattern{b}"/> with the result of
    /// <paramref name="func"/>.</returns>
    /// <param name="pattern">The pattern to match.</param>
    /// <param name="func">The function to apply.</param>
    /// <typeparam name="a">The input type.</typeparam>
    /// <typeparam name="b">The return type.</typeparam>
    public static Pattern<b> Map<a, b>(this Pattern<a> pattern, Func<a, b> func)
      => new Map<a, b>(pattern, func);
  }
}
