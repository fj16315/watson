using Xunit;
using WatsonAI;
using FsCheck.Xunit;
using FsCheck;
using Syn.WordNet;
using System.Collections.Generic;

namespace WatsonTest
{
  public class MultipleWordsProcessTests
  {
    /// <summary>
    /// Having this as static means it is shared amongst all tests.
    /// This is done because the construction is expensive as it must read in data files.
    /// An alternative to static is XUnit fixtures, this may be needed in future.
    /// </summary>
    private static MultipleWordProcess multipleWordProcess = new MultipleWordProcess();
    private static MultipleWordRemovalProcess multipleWordRemovalProcess = new MultipleWordRemovalProcess();
    private static readonly Parser parser = new Parser();

    [Fact]
    public void AddingUnderscoreTests()
    {
      var input = "rat poison";
      var stream = Stream.Tokenise(parser, input);
      stream = multipleWordProcess.Process(stream);
      Assert.True(stream.Input.Count == 1);
      Assert.Equal("rat_poison", stream.Input[0]);


      input = "master bedroom";
      stream = Stream.Tokenise(parser, input);
      stream = multipleWordProcess.Process(stream);
      Assert.True(stream.Input.Count == 1);
      Assert.Equal("master_bedroom", stream.Input[0]);

      input = "dining room";
      stream = Stream.Tokenise(parser, input);
      stream = multipleWordProcess.Process(stream);
      Assert.True(stream.Input.Count == 1);
      Assert.Equal("dining_room", stream.Input[0]);


      input = "barbital tolerance";
      stream = Stream.Tokenise(parser, input);
      stream = multipleWordProcess.Process(stream);
      Assert.True(stream.Input.Count == 1);
      Assert.Equal("barbital_tolerance", stream.Input[0]);


      input = "sleeping aid";
      stream = Stream.Tokenise(parser, input);
      stream = multipleWordProcess.Process(stream);
      Assert.True(stream.Input.Count == 1);
      Assert.Equal("sleeping_aid", stream.Input[0]);

    }

    [Fact]
    public void UnderscoresAddedInQuestions()
    {
      var input = "where is the rat poison?";
      var stream = Stream.Tokenise(parser, input);
      stream = multipleWordProcess.Process(stream);
      Assert.True(stream.Input.Count == 5);
      Assert.Equal(new List<string> { "where", "is", "the", "rat_poison", "?", },stream.Input);


      input = "where is the master bedroom?";
      stream = Stream.Tokenise(parser, input);
      stream = multipleWordProcess.Process(stream);
      Assert.True(stream.Input.Count == 5);
      Assert.Equal(new List<string> { "where", "is", "the", "master_bedroom", "?", },stream.Input);

    }

    [Fact]
    public void HyphenWordTests() 
    {
      var input = "what is fast-acting?";
      var stream = Stream.Tokenise(parser, input);
      stream = multipleWordProcess.Process(stream);
      Assert.True(stream.Input.Count == 4);
      Assert.Equal(new List<string> { "what", "is", "fast_acting", "?" }, stream.Input);


      input = "what is slow-acting?";
      stream = Stream.Tokenise(parser, input);
      stream = multipleWordProcess.Process(stream);
      Assert.True(stream.Input.Count == 4);
      Assert.Equal(new List<string> { "what", "is", "slow_acting", "?"}, stream.Input);


    }

    [Fact]
    public void UnderscoreRemovalTests() 
    {

      var input = "rat poison";
      var stream = Stream.Tokenise(parser, input);
      stream.SetOutput(new List<string> { "rat_poison" });
      stream = multipleWordRemovalProcess.Process(stream);
      Assert.True(stream.Output.Count == 1);
      Assert.Equal("rat poison", stream.Output[0]);
      

      input = "master bedroom";
      stream = Stream.Tokenise(parser, input);
      stream.SetOutput(new List<string> { "master_bedroom" });
      stream = multipleWordRemovalProcess.Process(stream);
      Assert.True(stream.Output.Count == 1);
      Assert.Equal("master bedroom", stream.Output[0]);


      input = "hi ";
      stream = Stream.Tokenise(parser, input);
      stream.SetOutput(new List<string> { "the rat_poison is in the kitchen" });
      stream = multipleWordRemovalProcess.Process(stream);
      Assert.True(stream.Output.Count == 1);
      Assert.Equal("the rat poison is in the kitchen", stream.Output[0]);


      input = "hi ";
      stream = Stream.Tokenise(parser, input);
      stream.SetOutput(new List<string> { "the master_bedroom is in the house" });
      stream = multipleWordRemovalProcess.Process(stream);
      Assert.True(stream.Output.Count == 1);
      Assert.Equal("the master bedroom is in the house", stream.Output[0]);

    }

    [Fact]
    public void RemoveUnderscoresHyphenWordsTest() 
    {
      var input = "hi ";
      var stream = Stream.Tokenise(parser, input);
      stream.SetOutput(new List<string> { "the arsenic is fast_acting" });
      stream = multipleWordRemovalProcess.Process(stream);
      Assert.True(stream.Output.Count == 1);
      Assert.Equal("the arsenic is fast acting", stream.Output[0]);
    }
  }  
}
