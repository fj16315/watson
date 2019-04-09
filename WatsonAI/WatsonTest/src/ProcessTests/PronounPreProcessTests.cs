using Xunit;
using WatsonAI;
using System.Collections.Generic;

namespace WatsonTest
{
  public class PronounsPreProcessTests
  {
    private readonly Character character;
    private PronounsProcess pronounsProcess;
    private static readonly Parser parser = new Parser();
     
    public PronounsPreProcessTests()
    {
      character = new Character("actress", true);
      pronounsProcess = new PronounsProcess(this.character, parser);
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

    [Fact]
    public void ItSingleEntity()
    {
      var input = "the study is brown, where is it?";
      List<string> tokens;
      Stream.Tokenise(parser, input).RemainingInput(out tokens);
      pronounsProcess.PreProcess(ref tokens);
      Assert.Equal(new List<string> { "the", "study", "is", "brown", ",", "where", "is", "the", "study", "?" }, tokens);
    }

    [Fact]
    public void ItMultipleEntities()
    {
      var input = "the cat ate the dog, why did it do that?";
      List<string> tokens;
      Stream.Tokenise(parser, input).RemainingInput(out tokens);
      pronounsProcess.PreProcess(ref tokens);
      Assert.Equal(new List<string> { "the", "cat", "ate", "the", "dog", ",", "why", "did", "the", "cat", "do", "that", "?" }, tokens);
      
      input = "the cat and the mouse ate the dog, why did it do that?";
      Stream.Tokenise(parser, input).RemainingInput(out tokens);
      pronounsProcess.PreProcess(ref tokens);
      //TODO: Need to do something special here.
      Assert.True(false);
    }

    [Fact]
    public void ItSingleEntityWithMemory()
    {
      // Test "it" from last user input
      var inputMemory = new Memory(character, 3);
      var memoryPronounsProcess = new PronounsProcess(character, inputMemory, parser);

      var memoryInput = "the study is brown.";
      inputMemory.AppendInput(memoryInput);

      var input = "where is it?";
      List<string> tokens;
      Stream.Tokenise(parser, input).RemainingInput(out tokens);
      memoryPronounsProcess.PreProcess(ref tokens);
      Assert.Equal(new List<string> { "where", "is", "the", "study", "?" }, tokens);
      

      // Test "it" from last character response
      var responseMemory = new Memory(character, 3);
      memoryPronounsProcess = new PronounsProcess(character, responseMemory, parser);

      var memoryResponse = "the study is brown.";
      responseMemory.AppendResponse(memoryResponse);

      Stream.Tokenise(parser, input).RemainingInput(out tokens);
      memoryPronounsProcess.PreProcess(ref tokens);
      Assert.Equal(new List<string> { "where", "is", "the", "study", "?" }, tokens);
    }

    [Fact]
    public void ItResponseOverridesInput()
    {
      var memory = new Memory(character, 3);
      var memoryPronounsProcess = new PronounsProcess(character, memory, parser);

      var memoryInput = "the study is brown.";
      memory.AppendInput(memoryInput);
      var memoryResponse = "the music is lame.";
      memory.AppendResponse(memoryResponse);

      var input = "where is it?";
      List<string> tokens;
      Stream.Tokenise(parser, input).RemainingInput(out tokens);
      memoryPronounsProcess.PreProcess(ref tokens);
      Assert.Equal(new List<string> { "where", "is", "the", "music", "?" }, tokens);
    }

    [Fact]
    public void ItMultipleEntitiesWithMemory()
    {
      // Test "it" from last user input
      var inputMemory = new Memory(character, 3);
      var memoryPronounsProcess = new PronounsProcess(character, inputMemory, parser);

      var memoryInput = "the study is brown.";
      inputMemory.AppendInput(memoryInput);

      var input = "where is it?";
      List<string> tokens;
      Stream.Tokenise(parser, input).RemainingInput(out tokens);
      memoryPronounsProcess.PreProcess(ref tokens);
      //TODO: Need to do something special here.
      

      // Test "it" from last character response
      var responseMemory = new Memory(character, 3);
      memoryPronounsProcess = new PronounsProcess(character, responseMemory, parser);

      var memoryResponse = "the study is brown.";
      responseMemory.AppendResponse(memoryResponse);

      Stream.Tokenise(parser, input).RemainingInput(out tokens);
      memoryPronounsProcess.PreProcess(ref tokens);
      //TODO: Need to do something special here.
      Assert.True(false);
    }


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
