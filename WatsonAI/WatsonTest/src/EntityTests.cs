using FsCheck.Xunit;
using FsCheck;

using WatsonAI;

namespace WatsonTest
{
  [Properties(Arbitrary = new System.Type[] { typeof(Generators) })]
  public class EntityTests
  {
    
    [Property]
    public Property EntityConversion(uint n)
    {
      var entity = new Entity(n);
      return (n == (uint)entity).ToProperty();
    }

    // This checks the transitive property of equality:
    //   - if a equals b and b equals c, then a equals c
    //   - (a == b) && (b == c) => (a == c)
    //
    // C# does not have an implies operator which is the logic in this.
    //  ( a => b ) := ( !a || b )
    [Property]
    public Property EqualityTransitivity(Entity a, Entity b, Entity c)
      => (!(a == b && b == c) || (a == c)).ToProperty();

    [Property]
    public Property Equalityuint(Entity entity, uint n)
      => ((entity == n) == (entity == new Entity(n))).ToProperty();
  }
}
