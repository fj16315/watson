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
      Assert.Equal("how are you", output.spellCorrectedInput);

      input = "Didthe actress kill the earl?";
      stream = Stream.Tokenise(parser, input);
      output = spellCheckProcess.Process(stream);
      Assert.Equal("did the actress kill the earl?", output.spellCorrectedInput);

      input = "hellothere";
      stream = Stream.Tokenise(parser, input);
      output = spellCheckProcess.Process(stream);
      Assert.Equal("hello there", output.spellCorrectedInput);
    }

    [Fact]
    public void CommonTyposTest()
    {
      string input = "teh colonel";
      var stream = Stream.Tokenise(parser, input);
      var output = spellCheckProcess.Process(stream);
      Assert.Equal("the colonel", output.spellCorrectedInput);

      input = "it's neccessary";
      stream = Stream.Tokenise(parser, input);
      output = spellCheckProcess.Process(stream);
      Assert.Equal("it's necessary", output.spellCorrectedInput);

      input = "that is definately true";
      stream = Stream.Tokenise(parser, input);
      output = spellCheckProcess.Process(stream);
      Assert.Equal("that is definitely true", output.spellCorrectedInput);
    }
    [Fact]
    public void SingleWordCommonTyposTest()
    {
      //Most common singular word typos according to google searches
      string input = "seperate";
      var stream = Stream.Tokenise(parser, input);
      var output = spellCheckProcess.Process(stream);
      Assert.Equal("separate", output.spellCorrectedInput);

      input = "transexual";
      stream = Stream.Tokenise(parser, input);
      output = spellCheckProcess.Process(stream);
      Assert.Equal("transsexual", output.spellCorrectedInput);

      input = "calender";
      stream = Stream.Tokenise(parser, input);
      output = spellCheckProcess.Process(stream);
      Assert.Equal("calendar", output.spellCorrectedInput);

      input = "definately";
      stream = Stream.Tokenise(parser, input);
      output = spellCheckProcess.Process(stream);
      Assert.Equal("definitely", output.spellCorrectedInput);

      input = "recieve";
      stream = Stream.Tokenise(parser, input);
      output = spellCheckProcess.Process(stream);
      Assert.Equal("receive", output.spellCorrectedInput);

      input = "offical";
      stream = Stream.Tokenise(parser, input);
      output = spellCheckProcess.Process(stream);
      Assert.Equal("official", output.spellCorrectedInput);

      input = "managment";
      stream = Stream.Tokenise(parser, input);
      output = spellCheckProcess.Process(stream);
      Assert.Equal("management", output.spellCorrectedInput);

      input = "goverment";
      stream = Stream.Tokenise(parser, input);
      output = spellCheckProcess.Process(stream);
      Assert.Equal("government", output.spellCorrectedInput);

      input = "commerical";
      stream = Stream.Tokenise(parser, input);
      output = spellCheckProcess.Process(stream);
      Assert.Equal("commercial", output.spellCorrectedInput);

      input = "Febuary";
      stream = Stream.Tokenise(parser, input);
      output = spellCheckProcess.Process(stream);
      Assert.Equal("february", output.spellCorrectedInput);

      input = "enviroment";
      stream = Stream.Tokenise(parser, input);
      output = spellCheckProcess.Process(stream);
      Assert.Equal("environment", output.spellCorrectedInput);

      input = "occurence";
      stream = Stream.Tokenise(parser, input);
      output = spellCheckProcess.Process(stream);
      Assert.Equal("occurrence", output.spellCorrectedInput);

      input = "commision";
      stream = Stream.Tokenise(parser, input);
      output = spellCheckProcess.Process(stream);
      Assert.Equal("commission", output.spellCorrectedInput);

      input = "calender";
      stream = Stream.Tokenise(parser, input);
      output = spellCheckProcess.Process(stream);
      Assert.Equal("calendar", output.spellCorrectedInput);

      input = "assocation";
      stream = Stream.Tokenise(parser, input);
      output = spellCheckProcess.Process(stream);
      Assert.Equal("association", output.spellCorrectedInput);

      input = "milennium";
      stream = Stream.Tokenise(parser, input);
      output = spellCheckProcess.Process(stream);
      Assert.Equal("millennium", output.spellCorrectedInput);


    }

    [Fact]
    public void CharacterNamesTypoTest()
    {
      string input = "where is the colonell?";
      var stream = Stream.Tokenise(parser, input);
      var output = spellCheckProcess.Process(stream);
      Assert.Equal("where is the colonel?", output.spellCorrectedInput);

      input = "who is the countesss related to?";
      stream = Stream.Tokenise(parser, input);
      output = spellCheckProcess.Process(stream);
      Assert.Equal("who is the countess related to?", output.spellCorrectedInput);
        
      input = "where is the bultler?";
      stream = Stream.Tokenise(parser, input);
      output = spellCheckProcess.Process(stream);
      Assert.Equal("where is the butler?", output.spellCorrectedInput);

      input = "where is the bultler?";
      stream = Stream.Tokenise(parser, input);
      output = spellCheckProcess.Process(stream);
      Assert.Equal("where is the butler?", output.spellCorrectedInput);
    }

  }
}
