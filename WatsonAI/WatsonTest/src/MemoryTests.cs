using Xunit;
using WatsonAI;
using System.Linq;

namespace WatsonTest
{
  public class MemoryTests
  {
    public static Character testCharacter = new Character("dave", false, Gender.Male);

    [Fact]
    public void EmptyMemoryIsEmpty()
    {
      var memory = new Memory(testCharacter, 3);
      Assert.Equal("", memory.GetLastInput());
      Assert.Equal("", memory.GetLastResponse());

      Assert.Equal("", memory.GetLastInput());
      Assert.Equal("", memory.GetLastResponse());

      Assert.Empty(memory.Inputs);
      Assert.Empty(memory.Responses);
    }

    [Fact]
    public void AddingElements()
    {
      var memory = new Memory(testCharacter, 3);
      memory.AppendInput("cat");
      memory.AppendResponse("dog");

      Assert.Equal("cat", memory.GetLastInput());
      Assert.Equal("dog", memory.GetLastResponse());

      Assert.Single(memory.Inputs);
      Assert.Single(memory.Responses);

      Assert.Equal("cat", memory.Inputs.First());
      Assert.Equal("dog", memory.Responses.First());


      memory.AppendInput("mouse");
      memory.AppendResponse("chicken curry");

      Assert.Equal("mouse", memory.GetLastInput());
      Assert.Equal("chicken curry", memory.GetLastResponse());

      Assert.Equal(2, memory.Inputs.Count());
      Assert.Equal(2, memory.Responses.Count());


      Assert.Equal("mouse", memory.Inputs.First());
      Assert.Equal("chicken curry", memory.Responses.First());
      Assert.Equal("cat", memory.Inputs.Skip(1).First());
      Assert.Equal("dog", memory.Responses.Skip(1).First());
    }

    [Fact]
    public void MemoryCapacity()
    {
      var memory = new Memory(testCharacter, 1);
      memory.AppendInput("cat");
      memory.AppendResponse("dog");

      Assert.Equal("cat", memory.GetLastInput());
      Assert.Equal("dog", memory.GetLastResponse());

      Assert.Single(memory.Inputs);
      Assert.Single(memory.Responses);

      Assert.Equal("cat", memory.Inputs.First());
      Assert.Equal("dog", memory.Responses.First());

      memory.AppendInput("mouse");
      memory.AppendResponse("chicken curry");

      Assert.Single(memory.Inputs);
      Assert.Single(memory.Responses);

      Assert.Equal("mouse", memory.GetLastInput());
      Assert.Equal("chicken curry", memory.GetLastResponse());
    }
  }
}
