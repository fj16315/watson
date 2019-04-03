using OpenNLP.Tools.Parser;
using System.Collections.Generic;
using WatsonAI;
using Xunit;

namespace WatsonTest
{
  public class ParserTests
  {
    public static Parser parser = new Parser();

    [Fact]
    public void ParseOnStringBoolIndicatesNull()
    {
      Parse parse;
      var success = parser.Parse("hi", out parse);
      Assert.True(success);
      Assert.NotNull(parse);

      success = parser.Parse("", out parse);
      Assert.False(success);
      Assert.Null(parse);
    }

    [Fact]
    public void ParseOnTokensBoolIndicatesNull()
    {
      Parse parse;

      var tokens = new List<string> { "" };
      var success = parser.Parse("", out parse);
      Assert.False(success);
      Assert.Null(parse);

      tokens.Add("hi");
      success = parser.Parse(tokens, out parse);
      Assert.True(success);
      Assert.NotNull(parse);
    }

    [Fact]
    public void ParseOnTokensWithEmptyList()
    {
      Parse parse;
      var tokens = new List<string> { };
      var success = parser.Parse("", out parse);
      Assert.False(success);
      Assert.Null(parse);
    }
  }
}
