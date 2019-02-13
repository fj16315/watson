using Xunit;
using WatsonAI;
using FsCheck.Xunit;
using FsCheck;

namespace WatsonTest
{
  public class SanityTests
  {
    [Fact]
    public void SanityTest()
    {
      Assert.NotEqual("CharacterString", "[Char]");
    }

    [Property]
    public Property FsSanityCheck(int x)
    {
      return (2 * x == x + x).Trivial(x == 0);
    }
  }
}
