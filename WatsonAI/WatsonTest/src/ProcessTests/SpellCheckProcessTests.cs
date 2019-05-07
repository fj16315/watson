using Xunit;
using WatsonAI;
using System.Collections.Generic;

namespace WatsonTest
{
  public class SpellCheckProcessTests
  {
    private static Parser parser = new Parser();
    private static SpellCheckProcess spellCheckProcess = new SpellCheckProcess();

    [Fact]
    public void NoSpaceBetweenWordTest()
    {
      string input = "howare you";
      var stream = Stream.Tokenise(parser,input);
      var output = spellCheckProcess.Process(stream);
      Assert.Equal(new List<string> { "how are you" }, output.Output);

      input = "Didthe actress kill the earl?";
      stream = Stream.Tokenise(parser, input);
      output = spellCheckProcess.Process(stream);
      Assert.Equal(new List<string> { "did the actress kill the earl?" }, output.Output);

      input = "hellothere";
      stream = Stream.Tokenise(parser, input);
      output = spellCheckProcess.Process(stream);
      Assert.Equal(new List<string> { "hello there" }, output.Output);
    }

    [Fact]
    public void CommonTyposTest()
    {
      string input = "teh colonel";
      var stream = Stream.Tokenise(parser, input);
      var output = spellCheckProcess.Process(stream);
      Assert.Equal(new List<string> { "the colonel" }, output.Output);

      input = "it's neccessary";
      stream = Stream.Tokenise(parser, input);
      output = spellCheckProcess.Process(stream);
      Assert.Equal(new List<string> { "it's necessary" }, output.Output);

      input = "that is definately true";
      stream = Stream.Tokenise(parser, input);
      output = spellCheckProcess.Process(stream);
      Assert.Equal(new List<string> { "that is definitely true" }, output.Output);
    }
    [Fact]
    public void SingleWordCommonTyposTest()
    {
      //Most common singular word typos according to google searches
      string input = "seperate";
      var stream = Stream.Tokenise(parser, input);
      var output = spellCheckProcess.Process(stream);
      Assert.Equal(new List<string> { "separate" }, output.Output);

      input = "transexual";
      stream = Stream.Tokenise(parser, input);
      output = spellCheckProcess.Process(stream);
      Assert.Equal(new List<string> { "transsexual" }, output.Output);

      input = "calender";
      stream = Stream.Tokenise(parser, input);
      output = spellCheckProcess.Process(stream);
      Assert.Equal(new List<string> { "calendar" }, output.Output);

      input = "definately";
      stream = Stream.Tokenise(parser, input);
      output = spellCheckProcess.Process(stream);
      Assert.Equal(new List<string> { "definitely" }, output.Output);

      input = "recieve";
      stream = Stream.Tokenise(parser, input);
      output = spellCheckProcess.Process(stream);
      Assert.Equal(new List<string> { "receive" }, output.Output);

      input = "offical";
      stream = Stream.Tokenise(parser, input);
      output = spellCheckProcess.Process(stream);
      Assert.Equal(new List<string> { "official" }, output.Output);

      input = "managment";
      stream = Stream.Tokenise(parser, input);
      output = spellCheckProcess.Process(stream);
      Assert.Equal(new List<string> { "management" }, output.Output);

      input = "goverment";
      stream = Stream.Tokenise(parser, input);
      output = spellCheckProcess.Process(stream);
      Assert.Equal(new List<string> { "government" }, output.Output);

      input = "commerical";
      stream = Stream.Tokenise(parser, input);
      output = spellCheckProcess.Process(stream);
      Assert.Equal(new List<string> { "commercial" }, output.Output);

      input = "Febuary";
      stream = Stream.Tokenise(parser, input);
      output = spellCheckProcess.Process(stream);
      Assert.Equal(new List<string> { "february" }, output.Output);

      input = "enviroment";
      stream = Stream.Tokenise(parser, input);
      output = spellCheckProcess.Process(stream);
      Assert.Equal(new List<string> { "environment" }, output.Output);

      input = "occurence";
      stream = Stream.Tokenise(parser, input);
      output = spellCheckProcess.Process(stream);
      Assert.Equal(new List<string> { "occurrence" }, output.Output);

      input = "commision";
      stream = Stream.Tokenise(parser, input);
      output = spellCheckProcess.Process(stream);
      Assert.Equal(new List<string> { "commission" }, output.Output);

      input = "calender";
      stream = Stream.Tokenise(parser, input);
      output = spellCheckProcess.Process(stream);
      Assert.Equal(new List<string> { "calendar" }, output.Output);

      input = "assocation";
      stream = Stream.Tokenise(parser, input);
      output = spellCheckProcess.Process(stream);
      Assert.Equal(new List<string> { "association" }, output.Output);

      input = "milennium";
      stream = Stream.Tokenise(parser, input);
      output = spellCheckProcess.Process(stream);
      Assert.Equal(new List<string> { "millennium" }, output.Output);


    }

    [Fact]
    public void CharacterNamesTypoTest()
    {
      // it should be noted that not all common typos would be corrected, e.g bulter gets corrected to butter due to edit distance
      // so the tests below are just ones that highlight which typos work

      string input = "where is the colonell?";
      var stream = Stream.Tokenise(parser, input);
      var output = spellCheckProcess.Process(stream);
      Assert.Equal(new List<string> { "where is the colonel?" }, output.Output);

      input = "who is the countesss related to?";
      stream = Stream.Tokenise(parser, input);
      output = spellCheckProcess.Process(stream);
      Assert.Equal(new List<string> { "who is the countess related to?" }, output.Output);
        
      input = "where is the bultler?";
      stream = Stream.Tokenise(parser, input);
      output = spellCheckProcess.Process(stream);
      Assert.Equal(new List<string> { "where is the butler?" }, output.Output);

      input = "where is the bultler?";
      stream = Stream.Tokenise(parser, input);
      output = spellCheckProcess.Process(stream);
      Assert.Equal(new List<string> { "where is the butler?" }, output.Output);
    }

  }
}
