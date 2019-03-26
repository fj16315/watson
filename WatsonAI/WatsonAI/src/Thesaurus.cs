using Syn.WordNet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using Annytab.Stemmer;

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
    private Stemmer stemmer;
    private Associations associations;
    private IEnumerable<string> entityNames;


    public Thesaurus()
    {
      this.entityNames = associations.EntityNames();
      var directory = Path.Combine(Directory.GetCurrentDirectory(), "res", "WordNet", "dict") + Path.DirectorySeparatorChar;

      wordNet = new WordNetEngine();

      System.Diagnostics.Debug.WriteLine("Loading thesaurus database...");
      wordNet.LoadFromDirectory(directory);
      System.Diagnostics.Debug.WriteLine("Load completed.");

      this.stemmer = new Stemmer();
    }
    /// <summary>
    /// Constructor for a thesaurus.
    /// </summary>
    /// <param name="associations">takes in associations</param>
    /// <remarks>
    /// This loads in the dictionary files, so is slow to construct.
    /// For best results, share the one instance of the thesaurus object.
    /// </remarks>
    public Thesaurus(Associations associations)
    {
      this.associations = associations;
      this.entityNames = associations.EntityNames();
      var directory = Path.Combine(Directory.GetCurrentDirectory(), "res", "WordNet", "dict") + Path.DirectorySeparatorChar;

      wordNet = new WordNetEngine();

      System.Diagnostics.Debug.WriteLine("Loading thesaurus database...");
      wordNet.LoadFromDirectory(directory);
      System.Diagnostics.Debug.WriteLine("Load completed.");

      this.stemmer = new Stemmer();
    }

  

    /// <summary>
    /// Checks if the first word describes the second through the built-in WordNet similarity function.
    /// </summary>
    /// <remarks>Case sensitive.</remarks>
    /// <param name="first">The first word.</param>
    /// <param name="second">The second word.</param>
    /// <param name="stemInput">Whether to also try stemming the input.</param>
    /// <returns>True if the first describes the second.</returns>
    public bool Describes(string first, string second, bool stemInput = false)
    {

      if (entityNames.Contains(first) && entityNames.Contains(second)) return false;
      bool similar = wordNet.GetWordSimilarity(first, second) > 0.25;
      if (stemInput)
      {
        similar = similar 
          || wordNet.GetWordSimilarity(stemmer.GetSteamWord(first), second) > 0.25
          || wordNet.GetWordSimilarity(first, stemmer.GetSteamWord(second)) > 0.25
          || wordNet.GetWordSimilarity(stemmer.GetSteamWord(first), stemmer.GetSteamWord(second)) > 0.25;
      }
      return similar;
    }

    /// <summary>
    /// Checks if the first word describes the second through the built-in WordNet similarity function.
    /// Filters by lexical category.
    /// </summary>
    /// <remarks>Case sensitive.</remarks>
    /// <param name="first">The first word.</param>
    /// <param name="second">The second word.</param>
    /// <param name="lexicalCategory">The lexical category of both words.</param>
    /// <param name="stemInput">Whether to also try stemming the input.</param>
    /// <returns>True if the first describes the second.</returns>
    public bool Describes(string first, string second, PartOfSpeech lexicalCategory, bool stemInput = false)
    {

      if (entityNames.Contains(first) && entityNames.Contains(second) ) return false;
      bool describes = DescribesNoStemming(first, second, lexicalCategory);
      if (stemInput)
      { 
        describes = describes
          || DescribesNoStemming(stemmer.GetSteamWord(first), second, lexicalCategory)
          || DescribesNoStemming(first, stemmer.GetSteamWord(second), lexicalCategory)
          || DescribesNoStemming(stemmer.GetSteamWord(first), stemmer.GetSteamWord(second), lexicalCategory);
      }
      return describes;
    }

    private bool DescribesNoStemming(string first, string second, PartOfSpeech lexicalCategory) 
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
    /// <param name="stemInput">Whether to also try stemming the input.</param>
    /// <returns>True if the first describes the second.</returns>
    public bool Describes(string first, string second, PartOfSpeech lexicalCategory, 
      SynSetRelation[] relations, bool stemInput = false)
    {
      if (entityNames.Contains(first) && entityNames.Contains(second)) return false;
      bool describes = DescribesNoStemming(first, second, lexicalCategory, relations);
      if (stemInput)
      { 
        describes = describes
          || DescribesNoStemming(stemmer.GetSteamWord(first), second, lexicalCategory, relations)
          || DescribesNoStemming(first, stemmer.GetSteamWord(second), lexicalCategory, relations)
          || DescribesNoStemming(stemmer.GetSteamWord(first), stemmer.GetSteamWord(second), lexicalCategory, relations);
      }
      return describes;
    }

    private bool DescribesNoStemming(string first, string second, PartOfSpeech lexicalCategory, SynSetRelation[] relations) 
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
    /// <param name="stemInput">Whether to also try stemming the input.</param>
    /// <returns>The synonyms of the word.</returns>
    public IEnumerable<string> GetSynonyms(string word, bool stemInput = false)
    {
      var synonyms = GetSynonymsNoStemming(word);
      if (stemInput && !synonyms.Any())
      {
        word = stemmer.GetSteamWord(word);
        synonyms = GetSynonymsNoStemming(word);
      }
      return synonyms;
    }

    private IEnumerable<string> GetSynonymsNoStemming(string word)
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
    /// <param name="stemInput">Whether to also try stemming the input.</param>
    /// <returns>The synonyms of the word.</returns>
    public IEnumerable<string> GetSynonyms(string word, PartOfSpeech lexicalCategory, bool stemInput = false)
    {
      var synonyms = GetSynonymsNoStemming(word, lexicalCategory);
      if (stemInput && !synonyms.Any())
      {
        word = stemmer.GetSteamWord(word);
        synonyms = GetSynonymsNoStemming(word, lexicalCategory);
      }
      return synonyms;
    }

    private IEnumerable<string> GetSynonymsNoStemming(string word, PartOfSpeech lexicalCategory)
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
    /// <param name="stemInput">Whether to also try stemming the input.</param>
    /// <returns>The synonyms of the word.</returns>
    public IEnumerable<string> GetSynonyms(string word, PartOfSpeech lexicalCategory, 
      SynSetRelation[] relations, bool stemInput = false)
    {
      var synonyms = GetSynonymsNoStemming(word, lexicalCategory, relations);
      if (stemInput && !synonyms.Any())
      {
        word = stemmer.GetSteamWord(word);
        synonyms = GetSynonymsNoStemming(word, lexicalCategory, relations);
      }
      return synonyms;
    }

    private IEnumerable<string> GetSynonymsNoStemming(string word, PartOfSpeech lexicalCategory, SynSetRelation[] relations)
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
    /// <param name="stemInput">Whether to also try stemming the input.</param>
    /// <returns>The similarity value of two words.</returns>
    public float Similarity(string first, string second, bool stemInput = false)
    {
      if (stemInput)
      {
        first = stemmer.GetSteamWord(first);
        second = stemmer.GetSteamWord(second);
      }
      return wordNet.GetWordSimilarity(first, second);
    }

    /// <summary>
    /// Stems a given word. Eg. cats => cat.
    /// Can sometimes destroy the word if it's already in a stemmed form.
    /// </summary>
    /// <param name="word">The word to stem.</param>
    /// <param name="stemInput">Whether to also try stemming the input.</param>
    /// <returns>The stemmed word.</returns>
    public string Stem(string word) 
      => stemmer.GetSteamWord(word);
  }
}
