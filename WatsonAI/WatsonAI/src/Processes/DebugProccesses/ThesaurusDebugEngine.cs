using System;
using System.Collections.Generic;
using System.Text;

namespace WatsonAI
{
  /// <summary>
  /// Text engine that allows for printing the parse tree in console.
  /// </summary>
  public class DebugThesaurusProcess : IProcess
  {
    private Thesaurus thesaurus;

    /// <summary>
    /// Text engine for debugging the parser.
    /// </summary>
    public DebugThesaurusProcess()
    {
      this.thesaurus = new Thesaurus();
    }

    /// <summary>
    /// Text engine for debugging the parser.
    /// </summary>
    /// <param name="thesaurus">A thesaurus to use.</param>
    public DebugThesaurusProcess(Thesaurus thesaurus)
    {
      this.thesaurus = thesaurus;
    }

    /// <summary>
    /// Typing 'debugparse x' will print the parse tree for x.
    /// </summary>
    /// <param name="stream">The InputOutput state struct.</param>
    /// <returns>Output with parsetree appended when appropriate.</returns>
    public Stream Process(Stream stream)
    {
      Console.WriteLine(stream.NextToken());
      if (stream.NextToken().Equals("ts", StringComparison.OrdinalIgnoreCase))
      {
        stream.Consume();
        foreach (var word in this.thesaurus.GetSynonyms(stream.NextToken(), true))
        {
          Console.Write($"{word}, ");
        }
      }

      if (stream.NextToken().Equals("t", StringComparison.OrdinalIgnoreCase))
      {
        stream.Consume();
        foreach (var word in this.thesaurus.GetSynonyms(stream.NextToken()))
        {
          Console.Write($"{word}, ");
        }
      }

      if (stream.NextToken().Equals("d", StringComparison.OrdinalIgnoreCase))
      {
        stream.Consume();
        var word1 = stream.NextToken();
        stream.Consume();
        var word2 = stream.NextToken();
        stream.AppendOutput($"{this.thesaurus.Describes(word1, word2)} ");
      }

      if (stream.NextToken().Equals("ds", StringComparison.OrdinalIgnoreCase))
      {
        stream.Consume();
        var word1 = stream.NextToken();
        stream.Consume();
        var word2 = stream.NextToken();
        stream.AppendOutput($"{this.thesaurus.Describes(word1, word2, true)} ");
      }

      if (stream.NextToken().Equals("s", StringComparison.OrdinalIgnoreCase))
      {
        stream.Consume();
        var word1 = stream.NextToken();
        stream.Consume();
        var word2 = stream.NextToken();
        stream.AppendOutput($"{this.thesaurus.Similarity(word1, word2)} ");
      }

      if (stream.NextToken().Equals("steam", StringComparison.OrdinalIgnoreCase))
      {
        stream.Consume();
        var word = stream.NextToken();
        stream.AppendOutput($"{this.thesaurus.Stem(word)}");
      }
      return stream;
    }
  }
}
