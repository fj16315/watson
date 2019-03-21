using FsCheck.Xunit;
using FsCheck;

using WatsonAI;

namespace WatsonTest
{
  [Properties(Arbitrary = new System.Type[] { typeof(Generators) })]
  public class EntityTests
  {

    [Property]
    public bool EntityConversion(uint n)
      => n == (uint)new Entity(n);

    // This checks the transitive property of equality:
    //   - if a equals b and b equals c, then a equals c
    //   - (a == b) && (b == c) => (a == c)
    //
    // C# does not have an implies operator so is replaced with the following.
    //  ( p => q ) := ( !p || q )
    [Property]
    public Property EntityEqualityTransitivity(Entity a, Entity b, Entity c)
      => (!(a == b && b == c) || (a == c))
         .ToProperty()
         .Classify(a == b && a == c && b == c, "All Equal")
         .Classify(a != b && a != c && b != c, "All Different");

    [Property]
    public Property EntityEqualityuint(Entity entity, uint n)
      => ((entity == n) == (entity == new Entity(n)))
         .ToProperty()
         .Classify(entity == n, "Equal")
         .Classify(entity != n, "Different");
  }
}
