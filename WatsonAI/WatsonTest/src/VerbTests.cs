using FsCheck.Xunit;
using FsCheck;

using WatsonAI;

namespace WatsonTest
{
  [Properties(Arbitrary = new System.Type[] { typeof(Generators) })]
  public class VerbTests
  {

    [Property]
    public bool VerbConversion(uint n)
      => n == (uint)new Verb(n);

    // This checks the transitive property of equality:
    //   - if a equals b and b equals c, then a equals c
    //   - (a == b) && (b == c) => (a == c)
    //
    // C# does not have an implies operator so is replaced with the following.
    //  ( p => q ) := ( !p || q )
    [Property]
    public Property VerbEqualityTransitivity(Verb a, Verb b, Verb c)
      => (!(a == b && b == c) || (a == c))
         .ToProperty()
         .Classify(a == b && a == c && b == c, "All Equal")
         .Classify(a != b && a != c && b != c, "All Different");

    [Property]
    public Property VerbEqualityuint(Verb entity, uint n)
      => ((entity == n) == (entity == new Verb(n)))
         .ToProperty()
         .Classify(entity == n, "Equal")
         .Classify(entity != n, "Different");
  }
}
