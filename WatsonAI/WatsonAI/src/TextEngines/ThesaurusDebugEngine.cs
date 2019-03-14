using System;
using System.Linq;

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
      if (io.remainingInput.Trim().StartsWith("!t ", StringComparison.OrdinalIgnoreCase))
      {
        io.remainingInput = io.remainingInput.Substring("!t ".Length);
        foreach (var word in this.thesaurus.GetSynonyms(io.remainingInput))
        {
          io.output += $"{word}, ";
        }
        io.output += $" ";
      }

      if (io.remainingInput.Trim().StartsWith("!d", StringComparison.OrdinalIgnoreCase))
      {
        io.remainingInput = io.remainingInput.Substring("!d ".Length);
        var foo = io.remainingInput.Split(new char[]{' '});
        if (foo.Length >= 2)
        {
          io.output += $"{this.thesaurus.Describes(foo[0], foo[1])} ";
        }
        io.output += $" ";
      }

      if (io.remainingInput.Trim().StartsWith("!s", StringComparison.OrdinalIgnoreCase))
      {
        io.remainingInput = io.remainingInput.Substring("!s ".Length);
        var foo = io.remainingInput.Split(new char[]{' '});
        if (foo.Length >= 2)
        {
          io.output += $"{this.thesaurus.Similarity(foo[0], foo[1])} ";
        }
        io.output += $" ";
      }

      if (io.remainingInput.Trim().StartsWith("!pos ", StringComparison.OrdinalIgnoreCase))
      {
        io.remainingInput = io.remainingInput.Substring("!pos ".Length);
        var words = io.remainingInput.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        foreach (var word in words) {
          Console.WriteLine($" {word}: {String.Join(", ", thesaurus.GetPartsOfSpeech(word).Distinct())}");
        }
      }
      return io;
    }
  }
}
