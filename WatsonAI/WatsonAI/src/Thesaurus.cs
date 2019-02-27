using System;
using System.Collections.Generic;
using System.Text;
using wordLib = Microsoft.Office.Interop.Word;

namespace WatsonAI
{
  /// <summary>
  /// Class providing synonym checking.
  /// Uses microsoft word library for checking synonyms. 
  /// For best results, provide DictionaryWords rather than strings to methods.
  /// </summary>
  public class Thesaurus
  {
    private wordLib.Application app;

    /// <summary>
    /// Constructor for a thesaurus.
    /// Generates new word application so is slow to construct.
    /// </summary>
    public Thesaurus()
    {
      this.app = new wordLib.Application();
    }

    /// <summary>
    /// Constructor for a thesaurus.
    /// Passing in an existing word application is faster for mutliple thesaurus construction.
    /// </summary>
    /// <param name="app"></param>
    public Thesaurus(wordLib.Application app)
    {
      this.app = app;
    }

    /// <summary>
    /// Checks if string a is a synonym of string b.
    /// Sometimes gives weird results e.g. man is a synonym for cat.
    /// </summary>
    /// <param name="a">The first word.</param>
    /// <param name="b">The second word.</param>
    /// <returns>A boolean value, whether they are synonyms.</returns>
    public bool IsSynonymOf(string a, string b)
    {
      if (a.Equals(b, StringComparison.OrdinalIgnoreCase)) return true;

      if (CheckSynonymRelation(a, b)) return true;

      // We do this both ways because the Word synonyms aren't a two way relation.
      // Apparantly.
      return CheckSynonymRelation(b, a);
    }

    /// <summary>
    /// Checks if string a is a synonym of string b.
    /// Providing the Lexical Category of the first word helps filter invalid synonyms.
    /// Sometimes gives weird results e.g. man is a synonym for cat.
    /// </summary>
    /// <param name="a">The first word.</param>
    /// <param name="b">The second word.</param>
    /// <returns>A boolean value, whether they are synonyms.</returns>
    public bool IsSynonymOf(DictionaryWord a, string b)
    {
      if (a.word.Equals(b, StringComparison.OrdinalIgnoreCase)) return true;

      if (CheckSynonymRelation(a, b)) return true;

      // We do this both ways because the Word synonyms aren't a two way relation.
      // Apparantly.
      return CheckSynonymRelation(new DictionaryWord(b, a.category), a.word);
    }

    /// <summary>
    /// Checks if string a is a synonym of string b.
    /// Providing the Lexical Category of both words helps filter invalid synonyms.
    /// Sometimes gives weird results e.g. man is a synonym for cat.
    /// </summary>
    /// <param name="a">The first word.</param>
    /// <param name="b">The second word.</param>
    /// <returns>A boolean value, whether they are synonyms.</returns>
    public bool IsSynonymOf(DictionaryWord a, DictionaryWord b)
    {
      if (a.word.Equals(b.word, StringComparison.OrdinalIgnoreCase)) return true;
      if (a.category != b.category) return false;

      if (CheckSynonymRelation(a, b.word)) return true;

      // We do this both ways because the Word synonyms aren't a two way relation.
      // Apparantly.
      return CheckSynonymRelation(b, a.word);
    }

    /// <summary>
    /// Checks the one way synonym relation for a to b.
    /// To get all synonyms, this should be called with parameters swapped.
    /// </summary>
    /// <param name="a">The first word.</param>
    /// <param name="b">The second word.</param>
    /// <returns>Whether b is a synonym for a.</returns>
    private bool CheckSynonymRelation(string a, string b)
    {
      var infosyn = app.SynonymInfo[a, wordLib.WdLanguageID.wdEnglishUK];

      foreach (var item in infosyn.MeaningList as Array)
      {
        foreach (var word in infosyn.SynonymList[item] as Array)
        {
          if ((word as string).Equals(b,StringComparison.OrdinalIgnoreCase)) return true;
        }
      }

      return false;
    }

    /// <summary>
    /// Checks the one way synonym relation for a to b.
    /// To get all synonyms, this should be called with parameters swapped.
    /// Uses the lexical category of a to filter synonyms.
    /// </summary>
    /// <remarks> The lists are one indexed! </remarks>
    /// <param name="a">The first word.</param>
    /// <param name="b">The second word.</param>
    /// <returns>Whether b is a synonym for a.</returns>
    private bool CheckSynonymRelation(DictionaryWord a, string b)
    {
      var asyn = app.SynonymInfo[a.word, wordLib.WdLanguageID.wdEnglishUK];
      var meaningList = asyn.MeaningList as Array;
      // These lists are 1 indexed for some reason, caution!
      for (int meaning = 1; meaning <= meaningList.Length; meaning++)
      {
        foreach (var word in asyn.SynonymList[meaningList.GetValue(meaning)] as Array)
        {
          var partOfSpeechList = asyn.PartOfSpeechList as Array;
          if ((int)partOfSpeechList.GetValue(meaning) == (int)a.category
            && word.ToString().Equals(b, StringComparison.OrdinalIgnoreCase))
          {
              return true;
          }
        }
      }
      return false;
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
