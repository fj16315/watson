using Xunit;
using FsCheck;
using FsCheck.Xunit;

using WatsonAI;
using System.Collections.Generic;

namespace WatsonTest
{


  public class SpellCheckProcessTests
  {
    private SpellCheckProcess spellCheckProcess = new SpellCheckProcess();
    private readonly Parser parser = new Parser();


    [Fact]
    public void NoSpaceBetweenWordTest()
    {
      var input = "howare";
      var stream = Stream.Tokenise(parser,input);
      stream = spellCheckProcess.Process(stream);

      Assert.Equal(new List<string> { "how", "are" }, stream.Output);

    }
  }
}
