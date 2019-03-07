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
    /// Text engine for debuging the parser.
    /// </summary>
    public ThesaurusDebugEngine()
    {
      this.thesaurus = new Thesaurus();
    }

    /// <summary>
    /// Text engine for debuging the specified Parser.
    /// </summary>
    /// <param name="parse">The parser to use.</param>


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
        this.thesaurus.GetSynonyms(io.remainingInput);
      }

      if (io.remainingInput.Trim().StartsWith("!s", StringComparison.OrdinalIgnoreCase))
      {
        io.remainingInput = io.remainingInput.Substring("!s ".Length);
        var foo = io.remainingInput.Split(' ');
        if (foo.Length >= 2)
        {
          this.thesaurus.Similarity(foo[0], foo[1]);
        }
      }
      return io;
    }
  }
}
