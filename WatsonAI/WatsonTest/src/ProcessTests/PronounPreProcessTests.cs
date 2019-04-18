using Xunit;
using WatsonAI;
using System.Collections.Generic;

namespace WatsonTest
{
  public class PronounsRemovalProcessTests
  {
    private readonly Character character;
    private PronounsRemovalProcess pronounsProcess;
    private static readonly Parser parser = new Parser();
    private readonly List<Character> characters;
     
    public PronounsRemovalProcessTests()
    {
      characters = new List<Character>
      {
        new Character("actress", true, Gender.Female),
        new Character("countess", true, Gender.Female),
        new Character("earl", true, Gender.Male),
        new Character("dave", true, Gender.Male),
        new Character("butler", true, Gender.Other)
      };
      character = new Character("actress", true, Gender.Female);
      var genericMemory = new Memory(character, 3);
      pronounsProcess = new PronounsRemovalProcess(this.character, characters, genericMemory, parser);
    }

    [Fact]
    public void You()
    {
      var input = "you are Watson";
      var stream = Stream.Tokenise(parser, input);
      stream = stream = pronounsProcess.Process(stream);
      Assert.Equal(new List<string> { "actress", "is", "Watson" }, stream.Input);
      
      input = "are you ugly?";
      stream = Stream.Tokenise(parser, input);
      stream = pronounsProcess.Process(stream);
      Assert.Equal(new List<string> { "is", "actress", "ugly", "?" }, stream.Input);

      input = "is that you I'm looking for?";
      stream = Stream.Tokenise(parser, input);
      stream = pronounsProcess.Process(stream);
      Assert.Equal(new List<string> { "is", "that", "actress", "Watson", "is", "looking", "for", "?" }, stream.Input);
    }

    [Fact]
    public void Me()
    {
      var input = "do you love me?";
      var stream = Stream.Tokenise(parser, input);
      stream = pronounsProcess.Process(stream);
      Assert.Equal(new List<string> { "does", "actress", "love", "Watson", "?" }, stream.Input);
      
      input = "me ugly?";
      stream = Stream.Tokenise(parser, input);
      stream = pronounsProcess.Process(stream);
      Assert.Equal(new List<string> { "Watson", "ugly", "?" }, stream.Input);
    }

    [Fact]
    public void I()
    {
      var input = "I am ugly.";
      var stream = Stream.Tokenise(parser, input);
      stream = pronounsProcess.Process(stream);
      Assert.Equal(new List<string> { "Watson", "is", "ugly", "." }, stream.Input);
      
      input = "Am I ugly?";
      stream = Stream.Tokenise(parser, input);
      stream = pronounsProcess.Process(stream);
      Assert.Equal(new List<string> { "is", "Watson", "ugly", "?" }, stream.Input);

      input = "I will be the ugliest";
      stream = Stream.Tokenise(parser, input);
      stream = pronounsProcess.Process(stream);
      Assert.Equal(new List<string> { "Watson", "will", "be", "the", "ugliest" }, stream.Input);

      input = "I'm the ugliest";
      stream = Stream.Tokenise(parser, input);
      stream = pronounsProcess.Process(stream);
      Assert.Equal(new List<string> { "Watson", "is", "the", "ugliest" }, stream.Input);
    }

    [Fact]
    public void My()
    {
      var input = "do you love my cat?";
      var stream = Stream.Tokenise(parser, input);
      stream = pronounsProcess.Process(stream);
      Assert.Equal(new List<string> { "does", "actress", "love", "Watson", "'s", "cat", "?" }, stream.Input);
      
      input = "is my hat big?";
      stream = Stream.Tokenise(parser, input);
      stream = pronounsProcess.Process(stream);
      Assert.Equal(new List<string> { "is", "Watson", "'s", "hat", "big", "?" }, stream.Input);
    }

    [Fact]
    public void Mine()
    {
      var input = "is this grape mine?";
      var stream = Stream.Tokenise(parser, input);
      stream = pronounsProcess.Process(stream);
      Assert.Equal(new List<string> { "is", "this", "grape", "Watson", "'s", "?" }, stream.Input);
      
      input = "I enjoy playing minecraft.";
      stream = Stream.Tokenise(parser, input);
      stream = pronounsProcess.Process(stream);
      Assert.Equal(new List<string> { "Watson", "enjoy", "playing", "minecraft", "." }, stream.Input);
    }

    [Fact]
    public void Your()
    {
      var input = "do you love your cat?";
      var stream = Stream.Tokenise(parser, input);
      stream = pronounsProcess.Process(stream);
      Assert.Equal(new List<string> { "does", "actress", "love", "actress", "'s", "cat", "?" }, stream.Input);
      
      input = "is your hat big?";
      stream = Stream.Tokenise(parser, input);
      stream = pronounsProcess.Process(stream);
      Assert.Equal(new List<string> { "is", "actress", "'s", "hat", "big", "?" }, stream.Input);
    }

    [Fact]
    public void ItSingleEntity()
    {
      var input = "the study is brown, where is it?";
      var stream = Stream.Tokenise(parser, input);
      stream = pronounsProcess.Process(stream);
      Assert.Equal(new List<string> { "the", "study", "is", "brown", ",", "where", "is", "the", "study", "?" }, stream.Input);
    }

    //[Fact]
    //public void ItMultipleEntities()
    //{
    //  var input = "the cat ate the dog, why did it do that?";
    //  Stream.Tokenise(parser, input).RemainingInput(out List<string> tokens);
    //  stream = pronounsProcess.Process(stream);
    //  Assert.Equal(new List<string> { "the", "cat", "ate", "the", "dog", ",", "why", "did", "the", "cat", "do", "that", "?" }, stream.Input);
    
    //  input = "the cat and the mouse ate the dog, why did it do that?";
    //  Stream.Tokenise(parser, input).RemainingInput(out tokens);
    //  stream = pronounsProcess.Process(stream);
    //  //TODO: Need to do something special here.
    //  Assert.True(false);
    //}

    [Fact]
    public void ItSingleEntityWithMemory()
    {
      // Test "it" from last user input
      var inputMemory = new Memory(character, 3);
      var memoryPronounsProcess = new PronounsRemovalProcess(character, characters, inputMemory, parser);

      var memoryInput = "the study is brown.";
      inputMemory.AppendInput(memoryInput);

      var input = "where is it?";
      var stream = Stream.Tokenise(parser, input);
      stream = memoryPronounsProcess.Process(stream);
      Assert.Equal(new List<string> { "where", "is", "the", "study", "?" }, stream.Input);
      

      // Test "it" from last character response
      var responseMemory = new Memory(character, 3);
      memoryPronounsProcess = new PronounsRemovalProcess(character, characters, responseMemory, parser);

      var memoryResponse = "the study is brown.";
      responseMemory.AppendResponse(memoryResponse);

      stream = Stream.Tokenise(parser, input);
      stream = memoryPronounsProcess.Process(stream);
      Assert.Equal(new List<string> { "where", "is", "the", "study", "?" }, stream.Input);
    }

    [Fact]
    public void ItResponseOverridesInput()
    {
      var memory = new Memory(character, 3);
      var memoryPronounsProcess = new PronounsRemovalProcess(character, characters, memory, parser);

      var memoryInput = "the study is brown.";
      memory.AppendInput(memoryInput);
      var memoryResponse = "the music is lame.";
      memory.AppendResponse(memoryResponse);

      var input = "where is it?";
      var stream = Stream.Tokenise(parser, input);
      stream = memoryPronounsProcess.Process(stream);
      Assert.Equal(new List<string> { "where", "is", "the", "music", "?" }, stream.Input);
    }

    //[Fact]
    //public void ItMultipleEntitiesWithMemory()
    //{
    //  // Test "it" from last user input
    //  var inputMemory = new Memory(character, 3);
    //  var memoryPronounsProcess = new PronounsProcess(character, characters, inputMemory, parser);

    //  var memoryInput = "the study is brown.";
    //  inputMemory.AppendInput(memoryInput);

    //  var input = "where is it?";
    //  List<string> tokens;
    //  var stream = Stream.Tokenise(parser, input);
    //  stream = memoryPronounsProcess.Process(stream);
    //  //TODO: Need to do something special here.
    

    //  // Test "it" from last character response
    //  var responseMemory = new Memory(character, 3);
    //  memoryPronounsProcess = new PronounsProcess(character, characters, responseMemory, parser);

    //  var memoryResponse = "the study is brown.";
    //  responseMemory.AppendResponse(memoryResponse);

    //  stream = Stream.Tokenise(parser, input);
    //  stream = memoryPronounsProcess.Process(stream);
    //  //TODO: Need to do something special here.
    //  Assert.True(false);
    //}

    [Fact]
    public void HeSinglePerson()
    {
      var input = "the earl is stupid, where is he?";
      var stream = Stream.Tokenise(parser, input);
      stream = pronounsProcess.Process(stream);
      Assert.Equal(new List<string> { "the", "earl", "is", "stupid", ",", "where", "is", "earl", "?" }, stream.Input);

      input = "the earl is stupid, isn't he?";
      stream = Stream.Tokenise(parser, input);
      stream = pronounsProcess.Process(stream);
      Assert.Equal(new List<string> { "the", "earl", "is", "stupid", ",", "isn't", "earl", "?" }, stream.Input);
    }

    //[Fact]
    //public void HeMultiplePeople()
    //{
    //  var input = "the cat ate the dog, why did it do that?";
    //  var stream = Stream.Tokenise(parser, input).RemainingInput(out List<string> tokens);
    //  stream = pronounsProcess.Process(stream);
    //  Assert.Equal(new List<string> { "the", "cat", "ate", "the", "dog", ",", "why", "did", "the", "cat", "do", "that", "?" }, stream.Input);
    
    //  input = "the cat and the mouse ate the dog, why did it do that?";
    //  stream = Stream.Tokenise(parser, input).RemainingInput(out tokens);
    //  stream = pronounsProcess.Process(stream);
    //  //TODO: Need to do something special here.
    //  Assert.True(false);
    //}

    [Fact]
    public void HeMemory()
    {
      var memory = new Memory(character, 3);
      var memoryPronounsProcess = new PronounsRemovalProcess(character, characters, memory, parser);

      var memoryInput = "the earl is stupid.";
      memory.AppendInput(memoryInput);

      var input = "where is he?";
      var stream = Stream.Tokenise(parser, input);
      stream = memoryPronounsProcess.Process(stream);
      Assert.Equal(new List<string> { "where", "is", "earl", "?" }, stream.Input);
      
      var memoryResponse = "dave is lame.";
      memory.AppendResponse(memoryResponse);

      stream = Stream.Tokenise(parser, input);
      stream = memoryPronounsProcess.Process(stream);
      Assert.Equal(new List<string> { "where", "is", "dave", "?" }, stream.Input);
    }

    [Fact]
    public void SheSinglePerson()
    {
      var input = "the actress is stupid, where is she?";
      var stream = Stream.Tokenise(parser, input);
      stream = pronounsProcess.Process(stream);
      Assert.Equal(new List<string> { "the", "actress", "is", "stupid", ",", "where", "is", "actress", "?" }, stream.Input);
    }

    //[Fact]
    //public void SheMultiplePeople()
    //{
    //  Assert.True(false);
    //}

    [Fact]
    public void SheMemory()
    {
      var memory = new Memory(character, 3);
      var memoryPronounsProcess = new PronounsRemovalProcess(character, characters, memory, parser);

      var memoryInput = "the actress is stupid.";
      memory.AppendInput(memoryInput);

      var input = "where is she?";
      var stream = Stream.Tokenise(parser, input);
      stream = memoryPronounsProcess.Process(stream);
      Assert.Equal(new List<string> { "where", "is", "actress", "?" }, stream.Input);
      
      var memoryResponse = "the countess is lame.";
      memory.AppendResponse(memoryResponse);

      stream = Stream.Tokenise(parser, input);
      stream = memoryPronounsProcess.Process(stream);
      Assert.Equal(new List<string> { "where", "is", "countess", "?" }, stream.Input);
    }

    [Fact]
    public void HimSinglePerson()
    {
      var input = "the earl is stupid, where can I find him?";
      var stream = Stream.Tokenise(parser, input);
      stream = pronounsProcess.Process(stream);
      Assert.Equal(new List<string> { "the", "earl", "is", "stupid", ",", "where", "can", "Watson", "find", "earl", "?" }, stream.Input);
    }

    //[Fact]
    //public void HimMultiplePeople()
    //{
    //  Assert.True(false);
    //}

    [Fact]
    public void HimMemory()
    {
      var memory = new Memory(character, 3);
      var memoryPronounsProcess = new PronounsRemovalProcess(character, characters, memory, parser);

      var memoryInput = "the earl is stupid.";
      memory.AppendInput(memoryInput);

      var input = "where can I find him?";
      var stream = Stream.Tokenise(parser, input);
      stream = memoryPronounsProcess.Process(stream);
      Assert.Equal(new List<string> { "where", "can", "Watson", "find", "earl", "?" }, stream.Input);
      
      var memoryResponse = "dave is lame.";
      memory.AppendResponse(memoryResponse);

      stream = Stream.Tokenise(parser, input);
      stream = memoryPronounsProcess.Process(stream);
      Assert.Equal(new List<string> { "where", "can", "Watson", "find", "dave", "?" }, stream.Input);
    }

    [Fact]
    public void HerSinglePerson()
    {
      var input = "the actress is stupid, where can I find her?";
      var stream = Stream.Tokenise(parser, input);
      stream = pronounsProcess.Process(stream);
      Assert.Equal(new List<string> { "the", "actress", "is", "stupid", ",", "where", "can", "Watson", "find", "actress", "?" }, stream.Input);

      input = "the actress is ugly, but do you love her cat?";
      stream = Stream.Tokenise(parser, input);
      stream = pronounsProcess.Process(stream);
      Assert.Equal(new List<string> { "the", "actress", "is", "ugly", ",", "but", "does", "actress", "love", "actress", "'s", "cat", "?" }, stream.Input);
    }

    //[Fact]
    //public void HerMultiplePeople()
    //{
    //  Assert.True(false);
    //}

    [Fact]
    public void HerMemory()
    {
      var memory = new Memory(character, 3);
      var memoryPronounsProcess = new PronounsRemovalProcess(character, characters, memory, parser);

      var memoryInput = "the actress is stupid.";
      memory.AppendInput(memoryInput);

      var input = "where can I find her?";
      var stream = Stream.Tokenise(parser, input);
      stream = memoryPronounsProcess.Process(stream);
      Assert.Equal(new List<string> { "where", "can", "Watson", "find", "actress", "?" }, stream.Input);
      
      var memoryResponse = "the countess is lame.";
      memory.AppendResponse(memoryResponse);

      stream = Stream.Tokenise(parser, input);
      stream = memoryPronounsProcess.Process(stream);
      Assert.Equal(new List<string> { "where", "can", "Watson", "find", "countess", "?" }, stream.Input);
    }

    [Fact]
    public void TheySinglePerson()
    {
      var input = "the butler is stupid, where are they?";
      var stream = Stream.Tokenise(parser, input);
      stream = pronounsProcess.Process(stream);
      Assert.Equal(new List<string> { "the", "butler", "is", "stupid", ",", "where", "is", "butler", "?" }, stream.Input);

      input = "the butler is stupid, I hope they're not ugly too";
      stream = Stream.Tokenise(parser, input);
      stream = pronounsProcess.Process(stream);
      Assert.Equal(new List<string> { "the", "butler", "is", "stupid", ",", "Watson", "hope", "butler", "is", "not", "ugly", "too" }, stream.Input);
    }

    [Fact]
    public void TheyMultiplePeople()
    {
      var input = "the butler and actress are stupid, where are they?";
      var stream = Stream.Tokenise(parser, input);
      stream = pronounsProcess.Process(stream);
      Assert.Equal(new List<string> { "the", "butler", "and", "actress", "are", "stupid", ",", "where", "are", "butler", "and", "actress", "?" }, stream.Input);

      input = "butler, actress, dave and earl are ugly, where are they?";
      stream = Stream.Tokenise(parser, input);
      stream = pronounsProcess.Process(stream);
      Assert.Equal(new List<string> { "butler", ",", "actress", ",", "dave", "and", "earl", "are", "ugly", ",",
        "where", "are", "butler", ",", "actress", ",", "dave", "and", "earl", "?" }, stream.Input);
    }

    [Fact] public void TheyMemory()
    {
      var memory = new Memory(character, 3);
      var memoryPronounsProcess = new PronounsRemovalProcess(character, characters, memory, parser);

      var memoryInput = "the butler and actress are stupid";
      memory.AppendInput(memoryInput);

      var input = "where are they?";
      var stream = Stream.Tokenise(parser, input);
      stream = memoryPronounsProcess.Process(stream);
      Assert.Equal(new List<string> { "where", "are", "butler", "and", "actress", "?" }, stream.Input);
      
      var memoryResponse = "butler, actress, dave and earl are ugly";
      memory.AppendResponse(memoryResponse);

      stream = Stream.Tokenise(parser, input);
      stream = memoryPronounsProcess.Process(stream);
      Assert.Equal(new List<string> { "where", "are", "butler", ",", "actress", ",", "dave", "and", "earl", "?" }, stream.Input);
    }

    [Fact]
    public void TheirSinglePerson()
    {
      var input = "butler is ugly, but do you love their cat?";
      var stream = Stream.Tokenise(parser, input);
      stream = pronounsProcess.Process(stream);
      Assert.Equal(new List<string> { "butler", "is", "ugly", ",", "but", "does", "actress", "love", "butler", "'s", "cat", "?" }, stream.Input);
    }

    [Fact]
    public void TheirMultiplePeople()
    {
      var input = "butler is cool and actress is ugly, but do you love their cat?";
      var stream = Stream.Tokenise(parser, input);
      stream = pronounsProcess.Process(stream);
      Assert.Equal(new List<string> { "butler", "is", "cool", "and", "actress", "is", "ugly", ",",
        "but", "does", "actress", "love", "butler", "and", "actress", "'s", "cat", "?" }, stream.Input);
    }

    [Fact]
    public void TheirMemory()
    {
      var memory = new Memory(character, 3);
      var memoryPronounsProcess = new PronounsRemovalProcess(character, characters, memory, parser);

      var memoryInput = "the butler and actress are stupid";
      memory.AppendInput(memoryInput);

      var input = "do you love their cat?";
      var stream = Stream.Tokenise(parser, input);
      stream = memoryPronounsProcess.Process(stream);
      Assert.Equal(new List<string> { "does", "actress", "love", "butler", "and", "actress", "'s", "cat", "?" }, stream.Input);
      
      var memoryResponse = "butler, actress, dave and earl are ugly";
      memory.AppendResponse(memoryResponse);

      stream = Stream.Tokenise(parser, input);
      stream = memoryPronounsProcess.Process(stream);
      Assert.Equal(new List<string> { "does", "actress", "love", "butler", ",", "actress", ",", "dave", "and", "earl", "'s", "cat", "?" }, stream.Input);
    }

    [Fact]
    public void HisSinglePerson()
    {
      var input = "earl is ugly, but do you love his cat?";
      var stream = Stream.Tokenise(parser, input);
      stream = pronounsProcess.Process(stream);
      Assert.Equal(new List<string> { "earl", "is", "ugly", ",", "but", "does", "actress", "love", "earl", "'s", "cat", "?" }, stream.Input);
    }

    //[Fact]
    //public void HisMultiplePeople()
    //{
    //  Assert.True(false);
    //}

    [Fact]
    public void HisMemory()
    {
      var memory = new Memory(character, 3);
      var memoryPronounsProcess = new PronounsRemovalProcess(character, characters, memory, parser);

      var memoryInput = "the earl is stupid.";
      memory.AppendInput(memoryInput);

      var input = "where can I find him?";
      var stream = Stream.Tokenise(parser, input);
      stream = memoryPronounsProcess.Process(stream);
      Assert.Equal(new List<string> { "where", "can", "Watson", "find", "earl", "?" }, stream.Input);
      
      var memoryResponse = "dave is lame.";
      memory.AppendResponse(memoryResponse);

      stream = Stream.Tokenise(parser, input);
      stream = memoryPronounsProcess.Process(stream);
      Assert.Equal(new List<string> { "where", "can", "Watson", "find", "dave", "?" }, stream.Input);
    }

    [Fact]
    public void HersSinglePerson()
    {
      var input = "actress is ugly, but is this cat hers?";
      var stream = Stream.Tokenise(parser, input);
      stream = pronounsProcess.Process(stream);
      Assert.Equal(new List<string> { "actress", "is", "ugly", ",", "but", "is", "this", "cat", "actress", "'s", "?" }, stream.Input);
    }

    //[Fact]
    //public void HersMultiplePeople()
    //{
    //  Assert.True(false);
    //}

    [Fact]
    public void HersMemory()
    {
      var memory = new Memory(character, 3);
      var memoryPronounsProcess = new PronounsRemovalProcess(character, characters, memory, parser);

      var memoryInput = "actress is ugly";
      memory.AppendInput(memoryInput);

      var input = "is this cat hers?";
      var stream = Stream.Tokenise(parser, input);
      stream = memoryPronounsProcess.Process(stream);
      Assert.Equal(new List<string> { "is", "this", "cat", "actress", "'s", "?" }, stream.Input);
      
      var memoryResponse = "countess is wicked";
      memory.AppendResponse(memoryResponse);

      stream = Stream.Tokenise(parser, input);
      stream = memoryPronounsProcess.Process(stream);
      Assert.Equal(new List<string> { "is", "this", "cat", "countess", "'s", "?" }, stream.Input);
    }

    [Fact]
    public void ThemSinglePerson()
    {
      var input = "butler is cool, but does this cat belong to them?";
      var stream = Stream.Tokenise(parser, input);
      stream = pronounsProcess.Process(stream);
      Assert.Equal(new List<string> { "butler", "is", "cool", ",", "but", "does", "this", "cat", "belong", "to", "butler", "?" }, stream.Input);
    }

    [Fact]
    public void ThemMultiplePeople()
    {
      var input = "butler is cool and actress is ugly, but does this cat belong to them?";
      var stream = Stream.Tokenise(parser, input);
      stream = pronounsProcess.Process(stream);
      Assert.Equal(new List<string> { "butler", "is", "cool", "and", "actress", "is", "ugly", ",",
        "but", "does", "this", "cat", "belong", "to", "butler", "and", "actress", "?" }, stream.Input);

      input = "butler, actress, dave and earl are ugly, but does this cat belong to them?";
      stream = Stream.Tokenise(parser, input);
      stream = pronounsProcess.Process(stream);
      Assert.Equal(new List<string> { "butler", ",", "actress", ",", "dave", "and", "earl", "are", "ugly", ",",
        "but", "does", "this", "cat", "belong", "to", "butler", ",", "actress", ",", "dave", "and", "earl", "?" }, stream.Input);
    }

    [Fact]
    public void ThemMemory()
    {
      var memory = new Memory(character, 3);
      var memoryPronounsProcess = new PronounsRemovalProcess(character, characters, memory, parser);

      var memoryInput = "the butler and actress are stupid";
      memory.AppendInput(memoryInput);

      var input = "where can I find them?";
      var stream = Stream.Tokenise(parser, input);
      stream = memoryPronounsProcess.Process(stream);
      Assert.Equal(new List<string> { "where", "can", "Watson", "find", "butler", "and", "actress", "?" }, stream.Input);
      
      var memoryResponse = "butler, actress, dave and earl are ugly";
      memory.AppendResponse(memoryResponse);

      stream = Stream.Tokenise(parser, input);
      stream = memoryPronounsProcess.Process(stream);
      Assert.Equal(new List<string> { "where", "can", "Watson", "find", "butler", ",", "actress", ",", "dave", "and", "earl", "?" }, stream.Input);
    }

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

  }
}
