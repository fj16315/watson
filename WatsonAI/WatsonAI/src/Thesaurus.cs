using Syn.WordNet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;

namespace WatsonAI
{
  /// <summary>
  /// Class providing synonym checking and other related features.
  /// Uses wornet internally, requires dict files in res/WordNet/dict directory.
  /// All methods are case sensitive.
  /// </summary>
  public class Thesaurus
  {

    private WordNetEngine wordNet;

    /// <summary>
    /// Constructor for a thesaurus.
    /// </summary>
    /// <remarks>
    /// This loads in the dictionary files, so is slow to construct.
    /// For best results, share the one instance of the thesaurus object.
    /// </remarks>
    public Thesaurus()
    {
      var directory = Path.Combine(Directory.GetCurrentDirectory(), "res", "WordNet", "dict") + Path.DirectorySeparatorChar;

      wordNet = new WordNetEngine();

      System.Diagnostics.Debug.WriteLine("Loading thesaurus database...");
      wordNet.LoadFromDirectory(directory);
      System.Diagnostics.Debug.WriteLine("Load completed.");
    }

    public Thesaurus(string stringToPath)
    {
      var directory = Path.Combine(stringToPath, "res", "WordNet", "dict") + Path.DirectorySeparatorChar;

      wordNet = new WordNetEngine();

      System.Diagnostics.Debug.WriteLine("Loading thesaurus database...");
      wordNet.LoadFromDirectory(directory);
      System.Diagnostics.Debug.WriteLine("Load completed.");
    }

    /// <summary>
    /// Checks if the first word describes the second through the built-in WordNet similarity function.
    /// </summary>
    /// <remarks>Case sensitive.</remarks>
    /// <param name="first">The first word.</param>
    /// <param name="second">The second word.</param>
    /// <returns>True if the first describes the second.</returns>
    public bool Describes(string first, string second)
      => wordNet.GetWordSimilarity(first, second) > 0.25;

    /// <summary>
    /// Checks if the first word describes the second through the built-in WordNet similarity function.
    /// Filters by lexical category.
    /// </summary>
    /// <remarks>Case sensitive.</remarks>
    /// <param name="first">The first word.</param>
    /// <param name="second">The second word.</param>
    /// <param name="lexicalCategory">The lexical category of both words.</param>
    /// <returns>True if the first describes the second.</returns>
    public bool Describes(string first, string second, PartOfSpeech lexicalCategory)
      => (from synSet in wordNet.GetSynSets(first, lexicalCategory)
          from relation in synSet.SemanticRelations
          from relatedSynSet in synSet.GetRelatedSynSets(relation, true)
          where relatedSynSet.PartOfSpeech == lexicalCategory
          from synonym in synSet.Words.Union(relatedSynSet.Words)
          select synonym.Contains(second))
        .Any(x => x);

    /// <summary>
    /// Checks if the first word describes the second through the built-in WordNet similarity function.
    /// Filters by lexical category and the specified relations.
    /// </summary>
    /// <remarks>Case sensitive.</remarks>
    /// <param name="first">The first word.</param>
    /// <param name="second">The second word.</param>
    /// <param name="lexicalCategory">The lexical category of both words.</param>
    /// <param name="relations">The possible relations between the road.</param>
    /// <returns>True if the first describes the second.</returns>
    public bool Describes(string first, string second, PartOfSpeech lexicalCategory, SynSetRelation[] relations)
      => (from synSet in wordNet.GetSynSets(first, lexicalCategory)
          from relation in relations
          from relatedSynSet in synSet.GetRelatedSynSets(relation, true)
          where relatedSynSet.PartOfSpeech == lexicalCategory
          from synonym in synSet.Words.Union(relatedSynSet.Words)
          select synonym.Contains(second))
        .Any(x => x);

    /// <summary>
    /// Returns the synonyms of the first word.
    /// </summary>
    /// <remarks>Case sensitive.</remarks>
    /// <param name="word">The word.</param>
    /// <returns>The synonyms of the word.</returns>
    public IEnumerable<string> GetSynonyms(string word)
      => (from synSet in wordNet.GetSynSets(word)
         from relation in synSet.SemanticRelations
         from relatedSynSet in synSet.GetRelatedSynSets(relation, true)
         from synonym in relatedSynSet.Words.Union(synSet.Words)
         select synonym).Distinct();

    /// <summary>
    /// Returns the synonyms of the first word.
    /// Filters by lexical category.
    /// </summary>
    /// <remarks>Case sensitive.</remarks>
    /// <param name="word">The word.</param>
    /// <param name="lexicalCategory">The lexical category of the word.</param>
    /// <returns>The synonyms of the word.</returns>
    public IEnumerable<string> GetSynonyms(string word, PartOfSpeech lexicalCategory)
      => (from synSet in wordNet.GetSynSets(word, lexicalCategory)
         from relation in synSet.SemanticRelations
         from relatedSynSet in synSet.GetRelatedSynSets(relation, true)
         where relatedSynSet.PartOfSpeech == lexicalCategory
         from synonym in relatedSynSet.Words.Union(synSet.Words)
         select synonym).Distinct();

    /// <summary>
    /// Returns the synonyms of the first word.
    /// Filters by lexical category and a list of possible synSet relations.
    /// </summary>
    /// <remarks>Case sensitive.</remarks>
    /// <param name="word">The word.</param>
    /// <param name="lexicalCategory">The lexical category of the word.</param>
    /// <param name="relations">The possible synSetReltions between synsets accepted.</param>
    /// <returns>The synonyms of the word.</returns>
    public IEnumerable<string> GetSynonyms(string word, PartOfSpeech lexicalCategory, SynSetRelation[] relations)
      => (from synSet in wordNet.GetSynSets(word, lexicalCategory)
         from relation in relations
         from relatedSynSet in synSet.GetRelatedSynSets(relation, true)
         where relatedSynSet.PartOfSpeech == lexicalCategory
         from synonym in relatedSynSet.Words.Union(synSet.Words)
         select synonym).Distinct();

    /// <summary>
    /// Returns the similarity value of two words according to WordNet similarity.
    /// </summary>
    /// <remarks>Case sensitive.</remarks>
    /// <param name="first">The first word.</param>
    /// <param name="second">The second word.</param>
    /// <returns>The similarity value of two words.</returns>
    public float Similarity(string first, string second)
      => wordNet.GetWordSimilarity(first, second);

    public IEnumerable<PartOfSpeech> GetPartsOfSpeech(string word)
      => wordNet.GetSynSets(word).Select(ss => ss.PartOfSpeech);
  }
}
