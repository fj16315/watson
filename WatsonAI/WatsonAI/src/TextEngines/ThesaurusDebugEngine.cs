using System;
using System.Collections.Generic;
using System.Text;

namespace WatsonAI
{
  /// <summary>
  /// Text engine that allows for printing the parse tree in console.
  /// </summary>
  public class ThesaurusDebugEngine : IRule
  {
    private Thesaurus thesaurus;

    /// <summary>
    /// Text engine for debugging the parser.
    /// </summary>
    public ThesaurusDebugEngine()
    {
      this.thesaurus = new Thesaurus();
    }

    /// <summary>
    /// Text engine for debugging the parser.
    /// </summary>
    /// <param name="thesaurus">A thesaurus to use.</param>
    public ThesaurusDebugEngine(Thesaurus thesaurus)
    {
      this.thesaurus = thesaurus;
    }

    /// <summary>
    /// Typing 'debugparse x' will print the parse tree for x.
    /// </summary>
    /// <param name="io">The InputOutput state struct.</param>
    /// <returns>Output with parsetree appended when appropriate.</returns>
    public InputOutput Process(InputOutput io)
    {
      if (io.remainingInput.Trim().StartsWith("!ts ", StringComparison.OrdinalIgnoreCase))
      {
        io.remainingInput = io.remainingInput.Substring("!ts ".Length);
        foreach (var word in this.thesaurus.GetSynonyms(io.remainingInput, true))
        {
          Console.Write($"{word}, ");
        }
      }

      if (io.remainingInput.Trim().StartsWith("!t ", StringComparison.OrdinalIgnoreCase))
      {
        io.remainingInput = io.remainingInput.Substring("!t ".Length);
        foreach (var word in this.thesaurus.GetSynonyms(io.remainingInput))
        {
          Console.Write($"{word}, ");
        }
      }

      if (io.remainingInput.Trim().StartsWith("!d ", StringComparison.OrdinalIgnoreCase))
      {
        io.remainingInput = io.remainingInput.Substring("!d ".Length);
        var foo = io.remainingInput.Split(' ');
        if (foo.Length >= 2)
        {
          Console.WriteLine($"{this.thesaurus.Describes(foo[0], foo[1])} ");
        }
      }

      if (io.remainingInput.Trim().StartsWith("!ds ", StringComparison.OrdinalIgnoreCase))
      {
        io.remainingInput = io.remainingInput.Substring("!ds ".Length);
        var foo = io.remainingInput.Split(' ');
        if (foo.Length >= 2)
        {
          Console.WriteLine($"{this.thesaurus.Describes(foo[0], foo[1], true)} ");
        }
      }

      if (io.remainingInput.Trim().StartsWith("!s ", StringComparison.OrdinalIgnoreCase))
      {
        io.remainingInput = io.remainingInput.Substring("!s ".Length);
        var foo = io.remainingInput.Split(' ');
        if (foo.Length >= 2)
        {
          Console.WriteLine($"{this.thesaurus.Similarity(foo[0], foo[1])}");
        }
      }

      if (io.remainingInput.Trim().StartsWith("!stem ", StringComparison.OrdinalIgnoreCase))
      {
        io.remainingInput = io.remainingInput.Substring("!stem ".Length);
        var foo = io.remainingInput.Split(' ');
        if (foo.Length == 1)
        {
          Console.WriteLine($"{this.thesaurus.Stem(foo[0])}");
        }
      }
      return io;
    }
  }
}
