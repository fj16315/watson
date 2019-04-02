using Xunit;
using FsCheck;
using FsCheck.Xunit;

using WatsonAI;

namespace WatsonTest
{
  [Properties(Arbitrary = new System.Type[] { typeof(Generators) })]
  public class QuestionProcessTests
  {
    [Fact]
    public void FailResultHasValue()
    {
      var fail = new Result<int>();
      Assert.False(fail.HasValue);
    }

    [Fact]
    public void FailResultsEqual()
    {
      var lhs = new Result<int>();
      var rhs = new Result<int>();
      Assert.True(lhs == rhs);
    }

    [Property]
    public Property ResultEqualsSelf(Result<int> result)
    {
      // Ignore the warning, this should be compared to itself.
      // That's the point of this test.
#pragma warning disable CS1718
      return (result == result)
#pragma warning restore CS1718
             .Classify(result.HasValue, "result has a value")
             .Classify(!result.HasValue, "result is empty");
    }

    [Property]
    public Property LeftOrIdentity(Result<int> rhs)
    {
      var lhs = new Result<int>();
      return ((lhs | rhs) == rhs)
             .Classify(rhs.HasValue, "rhs has a value")
             .Classify(!rhs.HasValue, "rhs is empty");
    }

    [Property]
    public Property RightOrIdentity(Result<int> lhs)
    {
      var rhs = new Result<int>();
      return ((lhs | rhs) == lhs)
             .Classify(lhs.HasValue, "lhs has a value")
             .Classify(!lhs.HasValue, "lhs is empty");
    }

    [Property]
    public bool ResultOrCommutative(Result<int> a, Result<int> b, Result<int> c)
      => ((a | b) | c) == (a | (b | c));

    [Property]
    public Property ValueLeftResultOrLeft(Result<int> lhs, Result<int> rhs)
      => lhs.HasValue.Implies(() => (lhs | rhs) == lhs);

    [Property]
    public Property EmptyLeftResultOrRight(Result<int> lhs, Result<int> rhs)
      => (!lhs.HasValue).Implies(() => (lhs | rhs) == rhs);
  }
}
