using System;
using System.Collections.Generic;
using System.Text;

namespace WatsonAI
{
  public class QuestionEngine : IRule
  {
    private Parser parser;

    /// <summary>
    /// Text engine for debuging the parser.
    /// </summary>
    public QuestionEngine()
    {
      this.parser = new Parser();
    }

    /// <summary>
    /// Text engine for debuging the specified Parser.
    /// </summary>
    /// <param name="parse">The parser to use.</param>
    public QuestionEngine(Parser parse)
    {
      this.parser = parse;
    }

    public InputOutput Process(InputOutput io)
    {
      var parse = this.parser.Parse(io.remainingInput).GetChildren()[0];
      var nounPhrase = parse;
      var verbPhrase = parse;
      var noun = parse;
      var verb = parse;

      if (parse.Type.Equals("SBARQ"))
      {
        io.output = "This has a question: ";
        var sq = parse.GetChildren()[1];
        if (sq.ChildCount == 1)
        {
          verbPhrase = sq.GetChildren()[0];
          verb = verbPhrase.GetChildren()[0];
          nounPhrase = verbPhrase.GetChildren()[1];
          noun = nounPhrase.GetChildren()[nounPhrase.ChildCount - 1];
        }
        else
        {
          for (int i = 0; i < sq.ChildCount; i++)
          {
            var current = sq.GetChildren()[i];
            if (current.Type.Equals("VP"))
            {
              verbPhrase = current;
              verb = verbPhrase.GetChildren()[0];
            }
            else if (current.Type.Contains("VB"))
            {
              verb = current;
            }
            else
            {
              nounPhrase = current;
              noun = nounPhrase.GetChildren()[nounPhrase.ChildCount - 1];
            }
          }
        }
        var wh = parse.GetChildren()[0].GetChildren()[0];
        io.output = io.output + "Verb = " + verb.Show() + ", Noun = " + noun.Show();
        io.output = io.output + Query(wh.Show(), noun.Show(), verb.Show());
      }

      return io;
    }

    public string Query(string wh, string noun, string verb)
    {
      return " ........ mep";
    }

  }
}
