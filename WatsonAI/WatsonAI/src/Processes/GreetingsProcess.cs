using OpenNLP.Tools.Parser;
using System;
using System.Collections.Generic;

namespace WatsonAI
{
  /// <summary>
  /// Engine for processing greetings at the start of the input.
  /// </summary>
  public class GreetingsProcess : IProcess
  {
    private Parser parser;
    private Character character;
    private Thesaurus thesaurus;


    public GreetingsProcess(Parser parse, Thesaurus thesaurus, Character character) 
    {
      this.parser = parse;
      this.thesaurus = thesaurus;
      this.character = character;
    }

    /// <summary>
    /// Checks for a greeting at the start of the string. 
    /// and adds a greeting to the output.
    /// </summary>
    /// <param name="stream">The input stream.</param>
    /// <returns>Mutated stream.</returns>
    public Stream Process(Stream stream)
    {
      var streamNew = stream.Clone();
      string word;
      streamNew.NextToken(out word);

      Parse tree;
      if (!parser.Parse(word, out tree))
      {
        return stream;
      }

      var top = new Branch("TOP");
      var hello = new Descendant<string>(top, new Word(thesaurus, "hello"));
      var result = hello.Match(tree);

      if (result.HasValue)
      {
        streamNew.AppendOutput(character.GetGreeting());
        return streamNew;
      }
      return stream;
    }
  }
}
