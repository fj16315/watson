using Syn.WordNet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;

namespace WatsonAI
{
  /// <summary>
  /// Class providing synonym checking.
  /// Uses microsoft word library for checking synonyms. 
  /// For best results, provide DictionaryWords rather than strings to methods.
  /// </summary>
  public class Thesaurus
  {

    private WordNetEngine wordNet;

    /// <summary>
    /// Constructor for a thesaurus.
    /// Generates new word application so is slow to construct.
    /// </summary>
    public Thesaurus()
    {
      var directory = Path.Combine(Directory.GetCurrentDirectory(), "res", "WordNet", "dict") + Path.DirectorySeparatorChar;


      wordNet = new WordNetEngine();

      Console.WriteLine("Loading database...");
      wordNet.LoadFromDirectory(directory);
      Console.WriteLine("Load completed.");
    }

    public void GetSynonyms(string word)
    {
      var synSetList = wordNet.GetSynSets(word);

      foreach (var synSet in synSetList)
      {
        Console.WriteLine("SynSet Words:");
        foreach (var relatedWord in synSet.Words)
        {
          Console.Write(relatedWord + " ");
        }
        Console.Write("\n");
        Console.Write("\n");

        foreach (var relation in synSet.SemanticRelations)
        {
          Console.WriteLine("Related SynSet Words:");
          //if (wordNet.GetSynSets( > 0)
          //{
            Console.WriteLine("Sense Greater Than Zero: Related SynSet Words:");
            foreach (var relatedSynSet in synSet.GetRelatedSynSets(relation, true))
            {
              Console.Write("     ");
              foreach (var relatedWord in relatedSynSet.Words)
              {
                Console.Write(relatedWord + " ");
              }
              Console.Write("\n");
            }
          //}
          Console.Write("\n");
        }
      }
    }

    public void Similarity(string first, string second)
    {
      var relations = new SynSetRelation[]
      {
        SynSetRelation.SimilarTo,
        SynSetRelation.Hypernym
        //SynSetRelation.Hyponym
      };

      //var foo = from synSetA in wordNet.GetSynSets(first)
                //from synSetB in wordNet.GetSynSets(second)
                //from aaa in synSetA.GetShortestPathTo(synSetB, relations)
                //select aaa;

      foreach (var synSetA in wordNet.GetSynSets(first))
      {
        foreach (var synSetB in wordNet.GetSynSets(second))
        {
          foreach (var aaa in synSetA.GetShortestPathTo(synSetB, relations) ?? Enumerable.Empty<SynSet>())
          {
            Console.WriteLine($"{aaa}");
          }
        }
      }
    }
  }

  /// <summary>
  /// A struct containing the string for the word and it's lexical category.
  /// </summary>
  public struct DictionaryWord
  {
    public string word { get; }
    public LexicalCategory category { get; }

    /// <summary>
    /// A constructor for a DictionaryWord.
    /// </summary>
    /// <param name="word">The word.</param>
    /// <param name="category">The lexical category of the word. (See enum.)</param>
    public DictionaryWord(string word, LexicalCategory category)
    {
      this.word = word;
      this.category = category;
    }
  }

  public enum LexicalCategory
  {
    Adjective, Noun, Adverb, Verb, Pronoun, Conjunction, Preposition, Interjection, Idiom, Other
  }
}
