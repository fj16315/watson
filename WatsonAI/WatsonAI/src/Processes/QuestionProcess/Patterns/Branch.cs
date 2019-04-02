using OpenNLP.Tools.Parser;

namespace WatsonAI
{
  /// <summary>
  /// A pattern that matches against a particular branch in the parse tree.
  /// </summary>
  public class Branch : Pattern<Parse>
  {
    private readonly string branch;

    /// <summary>
    /// Construct a new pattern that matches against a particular branch.
    /// </summary>
    /// <param name="branch">The branch to match, (in caps).</param>
    public Branch(string branch)
    {
      this.branch = branch;
    }

    /// <summary>
    /// Returns the branch of the tree if it matches.
    /// </summary>
    /// <param name="tree">The tree to match against.</param>
    /// <returns>The matching branch of the parse tree.</returns>
    public override Result<Parse> Match(Parse tree)
      => tree.Type == branch ? new Result<Parse>(tree) : new Result<Parse>();
  }
}
