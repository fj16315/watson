using Xunit;
using WatsonAI;
using System.Collections.Generic;
using System.Diagnostics;

namespace WatsonTest
{
  public class SpellCheckProcessTests
  {
    private static Parser parser;
    private static SpellCheckProcess spellCheckProcess;

    static SpellCheckProcessTests()
    {
      parser = new Parser();
      spellCheckProcess = new SpellCheckProcess(parser);
    }

    [Fact]
    public void NoSpaceBetweenWordTest()
    {
      string input = "howare you";
      var stream = Stream.Tokenise(parser,input);
      var output = spellCheckProcess.Process(stream);
      Assert.Equal("How are you", output.spellCorrectedInput);

      input = "Didthe actress kill the earl?";
      stream = Stream.Tokenise(parser, input);
      output = spellCheckProcess.Process(stream);
      Assert.Equal("Did the actress kill the earl?", output.spellCorrectedInput);

      input = "hellothere";
      stream = Stream.Tokenise(parser, input);
      output = spellCheckProcess.Process(stream);
      Assert.Equal("Hello there", output.spellCorrectedInput);
    }

    [Fact]
    public void CommonTyposTest()
    {
      string input = "teh colonel";
      var stream = Stream.Tokenise(parser, input);
      var output = spellCheckProcess.Process(stream);
      Assert.Equal("The colonel", output.spellCorrectedInput);

      input = "it's neccessary";
      stream = Stream.Tokenise(parser, input);
      output = spellCheckProcess.Process(stream);
      Assert.Equal("It's necessary", output.spellCorrectedInput);

      input = "that is definately true";
      stream = Stream.Tokenise(parser, input);
      output = spellCheckProcess.Process(stream);
      Assert.Equal("That is definitely true", output.spellCorrectedInput);
    }
    [Fact]
    public void SingleWordCommonTyposTest()
    {
      //Most common singular word typos according to google searches
      string input = "seperate";
      var stream = Stream.Tokenise(parser, input);
      var output = spellCheckProcess.Process(stream);
      Assert.Equal("Separate", output.spellCorrectedInput);

      input = "transexual";
      stream = Stream.Tokenise(parser, input);
      output = spellCheckProcess.Process(stream);
      Assert.Equal("Transsexual", output.spellCorrectedInput);

      input = "calender";
      stream = Stream.Tokenise(parser, input);
      output = spellCheckProcess.Process(stream);
      Assert.Equal("Calendar", output.spellCorrectedInput);

      input = "definately";
      stream = Stream.Tokenise(parser, input);
      output = spellCheckProcess.Process(stream);
      Assert.Equal("Definitely", output.spellCorrectedInput);

      input = "recieve";
      stream = Stream.Tokenise(parser, input);
      output = spellCheckProcess.Process(stream);
      Assert.Equal("Receive", output.spellCorrectedInput);

      input = "offical";
      stream = Stream.Tokenise(parser, input);
      output = spellCheckProcess.Process(stream);
      Assert.Equal("Official", output.spellCorrectedInput);

      input = "managment";
      stream = Stream.Tokenise(parser, input);
      output = spellCheckProcess.Process(stream);
      Assert.Equal("Management", output.spellCorrectedInput);

      input = "goverment";
      stream = Stream.Tokenise(parser, input);
      output = spellCheckProcess.Process(stream);
      Assert.Equal("Government", output.spellCorrectedInput);

      input = "Commerical";
      stream = Stream.Tokenise(parser, input);
      output = spellCheckProcess.Process(stream);
      Assert.Equal("Commercial", output.spellCorrectedInput);

      input = "Febuary";
      stream = Stream.Tokenise(parser, input);
      output = spellCheckProcess.Process(stream);
      Assert.Equal("February", output.spellCorrectedInput);

      input = "Enviroment";
      stream = Stream.Tokenise(parser, input);
      output = spellCheckProcess.Process(stream);
      Assert.Equal("Environment", output.spellCorrectedInput);

      input = "occurence";
      stream = Stream.Tokenise(parser, input);
      output = spellCheckProcess.Process(stream);
      Assert.Equal("Occurrence", output.spellCorrectedInput);

      input = "commision";
      stream = Stream.Tokenise(parser, input);
      output = spellCheckProcess.Process(stream);
      Assert.Equal("Commission", output.spellCorrectedInput);

      input = "calender";
      stream = Stream.Tokenise(parser, input);
      output = spellCheckProcess.Process(stream);
      Assert.Equal("Calendar", output.spellCorrectedInput);

      input = "assocation";
      stream = Stream.Tokenise(parser, input);
      output = spellCheckProcess.Process(stream);
      Assert.Equal("Association", output.spellCorrectedInput);

      input = "milennium";
      stream = Stream.Tokenise(parser, input);
      output = spellCheckProcess.Process(stream);
      Assert.Equal("Millennium", output.spellCorrectedInput);


    }

    [Fact]
    public void CharacterNamesTypoTest()
    {
      string input = "where is the colonell?";
      var stream = Stream.Tokenise(parser, input);
      var output = spellCheckProcess.Process(stream);
      Assert.Equal("Where is the colonel?", output.spellCorrectedInput);

      input = "who is the countesss related to?";
      stream = Stream.Tokenise(parser, input);
      output = spellCheckProcess.Process(stream);
      Assert.Equal("Who is the countess related to?", output.spellCorrectedInput);
        
      input = "where is the bultler?";
      stream = Stream.Tokenise(parser, input);
      output = spellCheckProcess.Process(stream);
      Assert.Equal("Where is the butler?", output.spellCorrectedInput);

      input = "where is the bultler?";
      stream = Stream.Tokenise(parser, input);
      output = spellCheckProcess.Process(stream);
      Assert.Equal("Where is the butler?", output.spellCorrectedInput);
    }

  }
}
