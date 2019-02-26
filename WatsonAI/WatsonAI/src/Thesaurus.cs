using System;
using System.Collections.Generic;
using System.Text;
using wordLib = Microsoft.Office.Interop.Word;

namespace WatsonAI
{
  public class Thesaurus
  {
    private wordLib.Application app;

    public Thesaurus()
    {
      this.app = new wordLib.Application();
    }

    public Thesaurus(wordLib.Application app)
    {
      this.app = app;
    }

    public bool IsSynonymOf(string a, string b)
    {
      if (a.Equals(b, StringComparison.OrdinalIgnoreCase)) return true;

      if (CheckSynonymRelation(a, b)) return true;

      // We do this both ways because the Word synonyms aren't a two way relation.
      // Apparantly.
      return CheckSynonymRelation(b, a);
    }

    public bool IsSynonymOf(DictionaryWord a, string b)
    {
      if (a.word.Equals(b, StringComparison.OrdinalIgnoreCase)) return true;

      if (CheckSynonymRelation(a, b)) return true;

      // We do this both ways because the Word synonyms aren't a two way relation.
      // Apparantly.
      return CheckSynonymRelation(new DictionaryWord(b, a.category), a.word);
    }

    public bool IsSynonymOf(DictionaryWord a, DictionaryWord b)
    {
      if (a.word.Equals(b.word, StringComparison.OrdinalIgnoreCase)) return true;
      if (a.category != b.category) return false;

      if (CheckSynonymRelation(a, b.word)) return true;

      // We do this both ways because the Word synonyms aren't a two way relation.
      // Apparantly.
      return CheckSynonymRelation(b, a.word);
    }

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

  public struct DictionaryWord
  {
    public string word { get; }
    public LexicalCategory category { get; }

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
