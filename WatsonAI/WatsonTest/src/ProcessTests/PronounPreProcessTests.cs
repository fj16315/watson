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
    private readonly List<Character> characters;
     
    public PronounsPreProcessTests()
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
      pronounsProcess = new PronounsProcess(this.character, characters, parser);
    }

    [Fact]
    public void You()
    {
      var input = "you are Watson";
      Stream.Tokenise(parser, input).RemainingInput(out List<string> tokens);
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
      Stream.Tokenise(parser, input).RemainingInput(out List<string> tokens);
      pronounsProcess.PreProcess(ref tokens);
      Assert.Equal(new List<string> { "does", "actress", "love", "Watson", "?" }, tokens);
      
      input = "me ugly?";
      Stream.Tokenise(parser, input).RemainingInput(out tokens);
      pronounsProcess.PreProcess(ref tokens);
      Assert.Equal(new List<string> { "Watson", "ugly", "?" }, tokens);
    }

    [Fact]
    public void I()
    {
      var input = "I am ugly.";
      Stream.Tokenise(parser, input).RemainingInput(out List<string> tokens);
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
      Stream.Tokenise(parser, input).RemainingInput(out List<string> tokens);
      pronounsProcess.PreProcess(ref tokens);
      Assert.Equal(new List<string> { "does", "actress", "love", "Watson", "'s", "cat", "?" }, tokens);
      
      input = "is my hat big?";
      Stream.Tokenise(parser, input).RemainingInput(out tokens);
      pronounsProcess.PreProcess(ref tokens);
      Assert.Equal(new List<string> { "is", "Watson", "'s", "hat", "big", "?" }, tokens);
    }

    [Fact]
    public void Mine()
    {
      var input = "is this grape mine?";
      Stream.Tokenise(parser, input).RemainingInput(out List<string> tokens);
      pronounsProcess.PreProcess(ref tokens);
      Assert.Equal(new List<string> { "is", "this", "grape", "Watson", "'s", "?" }, tokens);
      
      input = "I enjoy playing minecraft.";
      Stream.Tokenise(parser, input).RemainingInput(out tokens);
      pronounsProcess.PreProcess(ref tokens);
      Assert.Equal(new List<string> { "Watson", "enjoy", "playing", "minecraft", "." }, tokens);
    }

    [Fact]
    public void Your()
    {
      var input = "do you love your cat?";
      Stream.Tokenise(parser, input).RemainingInput(out List<string> tokens);
      pronounsProcess.PreProcess(ref tokens);
      Assert.Equal(new List<string> { "does", "actress", "love", "actress", "'s", "cat", "?" }, tokens);
      
      input = "is your hat big?";
      Stream.Tokenise(parser, input).RemainingInput(out tokens);
      pronounsProcess.PreProcess(ref tokens);
      Assert.Equal(new List<string> { "is", "actress", "'s", "hat", "big", "?" }, tokens);
    }

    [Fact]
    public void ItSingleEntity()
    {
      var input = "the study is brown, where is it?";
      Stream.Tokenise(parser, input).RemainingInput(out List<string> tokens);
      pronounsProcess.PreProcess(ref tokens);
      Assert.Equal(new List<string> { "the", "study", "is", "brown", ",", "where", "is", "the", "study", "?" }, tokens);
    }

    //[Fact]
    //public void ItMultipleEntities()
    //{
    //  var input = "the cat ate the dog, why did it do that?";
    //  Stream.Tokenise(parser, input).RemainingInput(out List<string> tokens);
    //  pronounsProcess.PreProcess(ref tokens);
    //  Assert.Equal(new List<string> { "the", "cat", "ate", "the", "dog", ",", "why", "did", "the", "cat", "do", "that", "?" }, tokens);
    
    //  input = "the cat and the mouse ate the dog, why did it do that?";
    //  Stream.Tokenise(parser, input).RemainingInput(out tokens);
    //  pronounsProcess.PreProcess(ref tokens);
    //  //TODO: Need to do something special here.
    //  Assert.True(false);
    //}

    [Fact]
    public void ItSingleEntityWithMemory()
    {
      // Test "it" from last user input
      var inputMemory = new Memory(character, 3);
      var memoryPronounsProcess = new PronounsProcess(character, characters, inputMemory, parser);

      var memoryInput = "the study is brown.";
      inputMemory.AppendInput(memoryInput);

      var input = "where is it?";
      Stream.Tokenise(parser, input).RemainingInput(out List<string> tokens);
      memoryPronounsProcess.PreProcess(ref tokens);
      Assert.Equal(new List<string> { "where", "is", "the", "study", "?" }, tokens);
      

      // Test "it" from last character response
      var responseMemory = new Memory(character, 3);
      memoryPronounsProcess = new PronounsProcess(character, characters, responseMemory, parser);

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
      var memoryPronounsProcess = new PronounsProcess(character, characters, memory, parser);

      var memoryInput = "the study is brown.";
      memory.AppendInput(memoryInput);
      var memoryResponse = "the music is lame.";
      memory.AppendResponse(memoryResponse);

      var input = "where is it?";
      Stream.Tokenise(parser, input).RemainingInput(out List<string> tokens);
      memoryPronounsProcess.PreProcess(ref tokens);
      Assert.Equal(new List<string> { "where", "is", "the", "music", "?" }, tokens);
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
    //  Stream.Tokenise(parser, input).RemainingInput(out tokens);
    //  memoryPronounsProcess.PreProcess(ref tokens);
    //  //TODO: Need to do something special here.
    

    //  // Test "it" from last character response
    //  var responseMemory = new Memory(character, 3);
    //  memoryPronounsProcess = new PronounsProcess(character, characters, responseMemory, parser);

    //  var memoryResponse = "the study is brown.";
    //  responseMemory.AppendResponse(memoryResponse);

    //  Stream.Tokenise(parser, input).RemainingInput(out tokens);
    //  memoryPronounsProcess.PreProcess(ref tokens);
    //  //TODO: Need to do something special here.
    //  Assert.True(false);
    //}

    [Fact]
    public void HeSinglePerson()
    {
      var input = "the earl is stupid, where is he?";
      Stream.Tokenise(parser, input).RemainingInput(out List<string> tokens);
      pronounsProcess.PreProcess(ref tokens);
      Assert.Equal(new List<string> { "the", "earl", "is", "stupid", ",", "where", "is", "earl", "?" }, tokens);
    }

    //[Fact]
    //public void HeMultiplePeople()
    //{
    //  var input = "the cat ate the dog, why did it do that?";
    //  Stream.Tokenise(parser, input).RemainingInput(out List<string> tokens);
    //  pronounsProcess.PreProcess(ref tokens);
    //  Assert.Equal(new List<string> { "the", "cat", "ate", "the", "dog", ",", "why", "did", "the", "cat", "do", "that", "?" }, tokens);
    
    //  input = "the cat and the mouse ate the dog, why did it do that?";
    //  Stream.Tokenise(parser, input).RemainingInput(out tokens);
    //  pronounsProcess.PreProcess(ref tokens);
    //  //TODO: Need to do something special here.
    //  Assert.True(false);
    //}

    [Fact]
    public void HeMemory()
    {
      var memory = new Memory(character, 3);
      var memoryPronounsProcess = new PronounsProcess(character, characters, memory, parser);

      var memoryInput = "the earl is stupid.";
      memory.AppendInput(memoryInput);

      var input = "where is he?";
      Stream.Tokenise(parser, input).RemainingInput(out List<string> tokens);
      memoryPronounsProcess.PreProcess(ref tokens);
      Assert.Equal(new List<string> { "where", "is", "earl", "?" }, tokens);
      
      var memoryResponse = "dave is lame.";
      memory.AppendResponse(memoryResponse);

      Stream.Tokenise(parser, input).RemainingInput(out tokens);
      memoryPronounsProcess.PreProcess(ref tokens);
      Assert.Equal(new List<string> { "where", "is", "dave", "?" }, tokens);
    }

    [Fact]
    public void SheSinglePerson()
    {
      var input = "the actress is stupid, where is she?";
      Stream.Tokenise(parser, input).RemainingInput(out List<string> tokens);
      pronounsProcess.PreProcess(ref tokens);
      Assert.Equal(new List<string> { "the", "actress", "is", "stupid", ",", "where", "is", "actress", "?" }, tokens);
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
      var memoryPronounsProcess = new PronounsProcess(character, characters, memory, parser);

      var memoryInput = "the actress is stupid.";
      memory.AppendInput(memoryInput);

      var input = "where is she?";
      Stream.Tokenise(parser, input).RemainingInput(out List<string> tokens);
      memoryPronounsProcess.PreProcess(ref tokens);
      Assert.Equal(new List<string> { "where", "is", "actress", "?" }, tokens);
      
      var memoryResponse = "the countess is lame.";
      memory.AppendResponse(memoryResponse);

      Stream.Tokenise(parser, input).RemainingInput(out tokens);
      memoryPronounsProcess.PreProcess(ref tokens);
      Assert.Equal(new List<string> { "where", "is", "countess", "?" }, tokens);
    }

    [Fact]
    public void HimSinglePerson()
    {
      var input = "the earl is stupid, where can I find him?";
      Stream.Tokenise(parser, input).RemainingInput(out List<string> tokens);
      pronounsProcess.PreProcess(ref tokens);
      Assert.Equal(new List<string> { "the", "earl", "is", "stupid", ",", "where", "can", "Watson", "find", "earl", "?" }, tokens);
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
      var memoryPronounsProcess = new PronounsProcess(character, characters, memory, parser);

      var memoryInput = "the earl is stupid.";
      memory.AppendInput(memoryInput);

      var input = "where can I find him?";
      Stream.Tokenise(parser, input).RemainingInput(out List<string> tokens);
      memoryPronounsProcess.PreProcess(ref tokens);
      Assert.Equal(new List<string> { "where", "can", "Watson", "find", "earl", "?" }, tokens);
      
      var memoryResponse = "dave is lame.";
      memory.AppendResponse(memoryResponse);

      Stream.Tokenise(parser, input).RemainingInput(out tokens);
      memoryPronounsProcess.PreProcess(ref tokens);
      Assert.Equal(new List<string> { "where", "can", "Watson", "find", "dave", "?" }, tokens);
    }

    [Fact]
    public void HerSinglePerson()
    {
      var input = "the actress is stupid, where can I find her?";
      Stream.Tokenise(parser, input).RemainingInput(out List<string> tokens);
      pronounsProcess.PreProcess(ref tokens);
      Assert.Equal(new List<string> { "the", "actress", "is", "stupid", ",", "where", "can", "Watson", "find", "actress", "?" }, tokens);

      input = "the actress is ugly, but do you love her cat?";
      Stream.Tokenise(parser, input).RemainingInput(out tokens);
      pronounsProcess.PreProcess(ref tokens);
      Assert.Equal(new List<string> { "the", "actress", "is", "ugly", ",", "but", "does", "actress", "love", "actress", "'s", "cat", "?" }, tokens);
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
      var memoryPronounsProcess = new PronounsProcess(character, characters, memory, parser);

      var memoryInput = "the actress is stupid.";
      memory.AppendInput(memoryInput);

      var input = "where can I find her?";
      Stream.Tokenise(parser, input).RemainingInput(out List<string> tokens);
      memoryPronounsProcess.PreProcess(ref tokens);
      Assert.Equal(new List<string> { "where", "can", "Watson", "find", "actress", "?" }, tokens);
      
      var memoryResponse = "the countess is lame.";
      memory.AppendResponse(memoryResponse);

      Stream.Tokenise(parser, input).RemainingInput(out tokens);
      memoryPronounsProcess.PreProcess(ref tokens);
      Assert.Equal(new List<string> { "where", "can", "Watson", "find", "countess", "?" }, tokens);
    }

    [Fact]
    public void TheySinglePerson()
    {
      var input = "the butler is stupid, where are they?";
      Stream.Tokenise(parser, input).RemainingInput(out List<string> tokens);
      pronounsProcess.PreProcess(ref tokens);
      Assert.Equal(new List<string> { "the", "butler", "is", "stupid", ",", "where", "is", "butler", "?" }, tokens);

      input = "the butler is stupid, I hope they're not ugly too";
      Stream.Tokenise(parser, input).RemainingInput(out tokens);
      pronounsProcess.PreProcess(ref tokens);
      Assert.Equal(new List<string> { "the", "butler", "is", "stupid", ",", "Watson", "hope", "butler", "is", "not", "ugly", "too" }, tokens);
    }

    [Fact]
    public void TheyMultiplePeople()
    {
      var input = "the butler and actress are stupid, where are they?";
      Stream.Tokenise(parser, input).RemainingInput(out List<string> tokens);
      pronounsProcess.PreProcess(ref tokens);
      Assert.Equal(new List<string> { "the", "butler", "and", "actress", "are", "stupid", ",", "where", "are", "butler", "and", "actress", "?" }, tokens);

      input = "butler, actress, dave and earl are ugly, where are they?";
      Stream.Tokenise(parser, input).RemainingInput(out tokens);
      pronounsProcess.PreProcess(ref tokens);
      Assert.Equal(new List<string> { "butler", ",", "actress", ",", "dave", "and", "earl", "are", "ugly", ",",
        "where", "are", "butler", ",", "actress", ",", "dave", "and", "earl", "?" }, tokens);
    }

    [Fact] public void TheyMemory()
    {
      var memory = new Memory(character, 3);
      var memoryPronounsProcess = new PronounsProcess(character, characters, memory, parser);

      var memoryInput = "the butler and actress are stupid";
      memory.AppendInput(memoryInput);

      var input = "where are they?";
      Stream.Tokenise(parser, input).RemainingInput(out List<string> tokens);
      memoryPronounsProcess.PreProcess(ref tokens);
      Assert.Equal(new List<string> { "where", "are", "butler", "and", "actress", "?" }, tokens);
      
      var memoryResponse = "butler, actress, dave and earl are ugly";
      memory.AppendResponse(memoryResponse);

      Stream.Tokenise(parser, input).RemainingInput(out tokens);
      memoryPronounsProcess.PreProcess(ref tokens);
      Assert.Equal(new List<string> { "where", "are", "butler", ",", "actress", ",", "dave", "and", "earl", "?" }, tokens);
    }

    [Fact]
    public void TheirSinglePerson()
    {
      var input = "butler is ugly, but do you love their cat?";
      Stream.Tokenise(parser, input).RemainingInput(out List<string> tokens);
      pronounsProcess.PreProcess(ref tokens);
      Assert.Equal(new List<string> { "butler", "is", "ugly", ",", "but", "does", "actress", "love", "butler", "'s", "cat", "?" }, tokens);
    }

    [Fact]
    public void TheirMultiplePeople()
    {
      var input = "butler is cool and actress is ugly, but do you love their cat?";
      Stream.Tokenise(parser, input).RemainingInput(out List<string> tokens);
      pronounsProcess.PreProcess(ref tokens);
      Assert.Equal(new List<string> { "butler", "is", "cool", "and", "actress", "is", "ugly", ",",
        "but", "does", "actress", "love", "butler", "and", "actress", "'s", "cat", "?" }, tokens);
    }

    //[Fact]
    //public void TheirSinglePersonWithMemory()
    //{
    //  Assert.True(false);
    //}

    [Fact]
    public void HisSinglePerson()
    {
      var input = "earl is ugly, but do you love his cat?";
      Stream.Tokenise(parser, input).RemainingInput(out List<string> tokens);
      pronounsProcess.PreProcess(ref tokens);
      Assert.Equal(new List<string> { "earl", "is", "ugly", ",", "but", "does", "actress", "love", "earl", "'s", "cat", "?" }, tokens);
    }

    //[Fact]
    //public void HisMultiplePeople()
    //{
    //  Assert.True(false);
    //}

    //[Fact]
    //public void HisSinglePersonWithMemory()
    //{
    //  Assert.True(false);
    //}

    //[Fact]
    //public void HisResponseOverridesInput()
    //{
    //  Assert.True(false);
    //}

    //[Fact]
    //public void HisMultiplePeopleWithMemory()
    //{
    //  Assert.True(false);
    //}

    //[Fact]
    //public void HisGendersCorrectly()
    //{
    //  Assert.True(false);
    //}

    [Fact]
    public void HersSinglePerson()
    {
      var input = "actress is ugly, but is this cat hers?";
      Stream.Tokenise(parser, input).RemainingInput(out List<string> tokens);
      pronounsProcess.PreProcess(ref tokens);
      Assert.Equal(new List<string> { "actress", "is", "ugly", ",", "but", "is", "this", "cat", "actress", "'s", "?" }, tokens);
    }

    //[Fact]
    //public void HersMultiplePeople()
    //{
    //  Assert.True(false);
    //}

    //[Fact]
    //public void HersSinglePersonWithMemory()
    //{
    //  Assert.True(false);
    //}

    [Fact]
    public void ThemSinglePerson()
    {
      var input = "butler is cool, but does this cat belong to them?";
      Stream.Tokenise(parser, input).RemainingInput(out List<string> tokens);
      pronounsProcess.PreProcess(ref tokens);
      Assert.Equal(new List<string> { "butler", "is", "cool", ",", "but", "does", "this", "cat", "belong", "to", "butler", "?" }, tokens);
    }

    [Fact]
    public void ThemMultiplePeople()
    {
      var input = "butler is cool and actress is ugly, but does this cat belong to them?";
      Stream.Tokenise(parser, input).RemainingInput(out List<string> tokens);
      pronounsProcess.PreProcess(ref tokens);
      Assert.Equal(new List<string> { "butler", "is", "cool", "and", "actress", "is", "ugly", ",",
        "but", "does", "this", "cat", "belong", "to", "butler", "and", "actress", "?" }, tokens);

      input = "butler, actress, dave and earl are ugly, but does this cat belong to them?";
      Stream.Tokenise(parser, input).RemainingInput(out tokens);
      pronounsProcess.PreProcess(ref tokens);
      Assert.Equal(new List<string> { "butler", ",", "actress", ",", "dave", "and", "earl", "are", "ugly", ",",
        "but", "does", "this", "cat", "belong", "to", "butler", ",", "actress", ",", "dave", "and", "earl", "?" }, tokens);
    }

    //[Fact]
    //public void ThemSinglePersonWithMemory()
    //{
    //  Assert.True(false);
    //}

    //[Fact]
    //public void ThemMultiplePeopleWithMemory()
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

  }
}
