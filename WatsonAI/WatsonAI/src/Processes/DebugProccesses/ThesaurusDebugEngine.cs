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
    private Associations associations;

    /// <summary>
    /// Text engine for debugging the parser.
    /// </summary>
    public DebugThesaurusProcess()
    {
      this.associations = new Associations();
      associations.AddEntityName(new Entity(0), "cat");
      associations.AddEntityName(new Entity(1), "man");
      this.thesaurus = new Thesaurus(associations);
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
      if (stream.ConsumeIf("ts".Equals))
      {
        string word;
        stream.NextToken(out word);
        foreach (var syn in thesaurus.GetSynonyms(word, true))
        {
          stream.AppendOutput(syn);
        }
      }

      if (stream.ConsumeIf("t".Equals))
      {
        string word;
        stream.NextToken(out word);
        foreach (var syn in thesaurus.GetSynonyms(word))
        {
          stream.AppendOutput(syn);
        }
      }

      if (stream.ConsumeIf("d".Equals))
      {
        string word1, word2;
        stream.NextToken(out word1);
        stream.NextToken(out word2);
        stream.AppendOutput($"{thesaurus.Describes(word1, word2)}");
      }

      if (stream.ConsumeIf("ds".Equals))
      {
        string word1, word2;
        stream.NextToken(out word1);
        stream.NextToken(out word2);
        stream.AppendOutput($"{thesaurus.Describes(word1, word2, true)}");
      }

      if (stream.ConsumeIf("s".Equals))
      {
        string word1, word2;
        stream.NextToken(out word1);
        stream.NextToken(out word2);
        stream.AppendOutput($"{thesaurus.Similarity(word1, word2)}");
      }

      if (stream.ConsumeIf("steam".Equals))
      {
        string word1;
        stream.NextToken(out word1);
        stream.AppendOutput($"{thesaurus.Stem(word1)}");
      }
      return stream;
    }
  }
}
