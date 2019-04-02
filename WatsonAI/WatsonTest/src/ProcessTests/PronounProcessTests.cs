using Xunit;
using WatsonAI;
using System.Collections.Generic;

namespace WatsonTest
{
  public class PronounsProcessTests
  {
    private Character character;
    private PronounsProcess pronounsProcess;
    private static Parser parser = new Parser();
     
    public PronounsProcessTests()
    {
      character = new Character("actress", true);
      pronounsProcess = new PronounsProcess(this.character);
    }

    [Fact]
    public void You()
    {
      var input = "you are Watson";
      List<string> tokens;
      Stream.Tokenise(parser, input).RemainingInput(out tokens);
      pronounsProcess.PreProcess(ref tokens);
      Assert.Equal(new List<string> { "actress", "is", "Watson" }, tokens);
      
      input = "are you ugly?";
      Stream.Tokenise(parser, input).RemainingInput(out tokens);
      pronounsProcess.PreProcess(ref tokens);
      Assert.Equal(new List<string> { "is", "actress", "ugly", "?" }, tokens);

      input = "is that you I'm looking for?";
      Stream.Tokenise(parser, input).RemainingInput(out tokens);
      pronounsProcess.PreProcess(ref tokens);
      Assert.Equal(new List<string> { "is", "that", "actress", "Watson", "is", "looking", "for", "?" }, tokens);
    }

    [Fact]
    public void Me()
    {
      var input = "do you love me?";
      List<string> tokens;
      Stream.Tokenise(parser, input).RemainingInput(out tokens);
      pronounsProcess.PreProcess(ref tokens);
      Assert.Equal(new List<string> { "do", "actress", "love", "Watson", "?" }, tokens);
      
      input = "me ugly?";
      Stream.Tokenise(parser, input).RemainingInput(out tokens);
      pronounsProcess.PreProcess(ref tokens);
      Assert.Equal(new List<string> { "Watson", "ugly", "?" }, tokens);
    }

    [Fact]
    public void I()
    {
      var input = "I am ugly.";
      List<string> tokens;
      Stream.Tokenise(parser, input).RemainingInput(out tokens);
      pronounsProcess.PreProcess(ref tokens);
      Assert.Equal(new List<string> { "Watson", "is", "ugly", "." }, tokens);
      
      input = "Am I ugly?";
      Stream.Tokenise(parser, input).RemainingInput(out tokens);
      pronounsProcess.PreProcess(ref tokens);
      Assert.Equal(new List<string> { "is", "Watson", "ugly", "?" }, tokens);

      input = "I will be the ugliest";
      Stream.Tokenise(parser, input).RemainingInput(out tokens);
      pronounsProcess.PreProcess(ref tokens);
      Assert.Equal(new List<string> { "Watson", "will", "be", "the", "ugliest" }, tokens);

      input = "I'm the ugliest";
      Stream.Tokenise(parser, input).RemainingInput(out tokens);
      pronounsProcess.PreProcess(ref tokens);
      Assert.Equal(new List<string> { "Watson", "is", "the", "ugliest" }, tokens);
    }

    [Fact]
    public void My()
    {
      var input = "do you love my cat?";
      List<string> tokens;
      Stream.Tokenise(parser, input).RemainingInput(out tokens);
      pronounsProcess.PreProcess(ref tokens);
      Assert.Equal(new List<string> { "do", "actress", "love", "Watson", "'s", "cat", "?" }, tokens);
      
      input = "is my hat big?";
      Stream.Tokenise(parser, input).RemainingInput(out tokens);
      pronounsProcess.PreProcess(ref tokens);
      Assert.Equal(new List<string> { "is", "Watson", "'s", "hat", "big", "?" }, tokens);
    }

    [Fact]
    public void Mine()
    {
      var input = "is this grape mine?";
      List<string> tokens;
      Stream.Tokenise(parser, input).RemainingInput(out tokens);
      pronounsProcess.PreProcess(ref tokens);
      Assert.Equal(new List<string> { "is", "this", "grape", "Watson", "'s", "?" }, tokens);
      
      input = "I enjoy playing minecraft.";
      Stream.Tokenise(parser, input).RemainingInput(out tokens);
      pronounsProcess.PreProcess(ref tokens);
      Assert.Equal(new List<string> { "Watson", "enjoy", "playing", "minecraft", "." }, tokens);
    }

    //[Fact]
    //public void It()
    //{
    //  Assert.True(false);
    //}

    //[Fact]
    //public void He()
    //{
    //  Assert.True(false);
    //}

    //[Fact]
    //public void She()
    //{
    //  Assert.True(false);
    //}

    //[Fact]
    //public void They()
    //{
    //  Assert.True(false);
    //}

    //[Fact]
    //public void Them()
    //{
    //  Assert.True(false);
    //}

    //[Fact]
    //public void Their()
    //{
    //  Assert.True(false);
    //}

    //[Fact]
    //public void Your()
    //{
    //  Assert.True(false);
    //}

    //[Fact]
    //public void We()
    //{
    //  Assert.True(false);
    //}

    //[Fact]
    //public void Us()
    //{
    //  Assert.True(false);
    //}

    //[Fact]
    //public void Ours()
    //{
    //  Assert.True(false);
    //}

    //[Fact]
    //public void His()
    //{
    //  Assert.True(false);
    //}

    //[Fact]
    //public void Her()
    //{
    //  Assert.True(false);
    //}

    //[Fact]
    //public void Him()
    //{
    //  Assert.True(false);
    //}
  }
}
