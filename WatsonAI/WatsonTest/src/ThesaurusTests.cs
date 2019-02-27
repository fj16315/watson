using Xunit;
using WatsonAI;
using FsCheck.Xunit;
using FsCheck;

namespace WatsonTest
{
  public class ThesaurusTests
  {
    // We initialise this only once for speed improvements.
    private Thesaurus thesaurus = new Thesaurus();

    [Fact]
    public void IsSynonymOf_TwoString_Standard()
    {
      Assert.True(thesaurus.IsSynonymOf("cat", "kitty"));
      Assert.True(thesaurus.IsSynonymOf("walk", "stroll"));

      // Synonyms of two strings gives strange results sometimes:
      Assert.True(thesaurus.IsSynonymOf("cat", "man"));

      Assert.False(thesaurus.IsSynonymOf("cat", "dog"));
      Assert.False(thesaurus.IsSynonymOf("cat", "lion"));
      Assert.False(thesaurus.IsSynonymOf("crockery", "plate"));
    }

    [Fact]
    public void IsSynonymOf_TwoString_EdgeCase()
    {
      Assert.True(thesaurus.IsSynonymOf("cat", "cat"));
      Assert.True(thesaurus.IsSynonymOf("", ""));
      Assert.True(thesaurus.IsSynonymOf("good-looking", "pretty"));
      Assert.True(thesaurus.IsSynonymOf("pretty", "good-looking"));

      Assert.False(thesaurus.IsSynonymOf("", "dog"));
      Assert.False(thesaurus.IsSynonymOf("cat", ""));

      Assert.False(thesaurus.IsSynonymOf("cat ", "cat"));
    }

    [Fact]
    public void IsSynonymOf_TwoString_Capitalisation()
    {
      Assert.True(thesaurus.IsSynonymOf("CAT", "cat"));
      Assert.True(thesaurus.IsSynonymOf("CaT", "cat"));
      Assert.True(thesaurus.IsSynonymOf("CaT", "cAt"));
      Assert.True(thesaurus.IsSynonymOf("CAt", "kitty"));
      Assert.True(thesaurus.IsSynonymOf("kitty", "CAt"));
      Assert.True(thesaurus.IsSynonymOf("kItty", "cat"));
      Assert.True(thesaurus.IsSynonymOf("cat", "kItty"));
    }

    [Fact]
    public void IsSynonymOf_OneStringOneDictWord_Standard()
    {
      var word = new DictionaryWord("cat", LexicalCategory.Noun);
      Assert.True(thesaurus.IsSynonymOf(word, "kitty"));
      word = new DictionaryWord("walk", LexicalCategory.Noun);
      Assert.True(thesaurus.IsSynonymOf(word, "stroll"));

      // Synonyms of two strings gives strange results sometimes:
      word = new DictionaryWord("cat", LexicalCategory.Noun);
      Assert.True(thesaurus.IsSynonymOf(word, "man"));

      word = new DictionaryWord("cat", LexicalCategory.Noun);
      Assert.False(thesaurus.IsSynonymOf(word, "dog"));
      word = new DictionaryWord("cat", LexicalCategory.Noun);
      Assert.False(thesaurus.IsSynonymOf(word, "lion"));
      word = new DictionaryWord("crockery", LexicalCategory.Noun);
      Assert.False(thesaurus.IsSynonymOf(word, "plate"));
    }

    [Fact]
    public void IsSynonymOf_OneStringOneDictWord_Filtering()
    {
      var word = new DictionaryWord("key", LexicalCategory.Noun);
      Assert.False(thesaurus.IsSynonymOf(word, "crucial"));
      word = new DictionaryWord("key", LexicalCategory.Adjective);
      Assert.True(thesaurus.IsSynonymOf(word, "crucial"));
    }

    [Fact]
    public void IsSynonymOf_OneStringOneDictWord_EdgeCase()
    {
      var word = new DictionaryWord("cat", LexicalCategory.Noun);
      Assert.True(thesaurus.IsSynonymOf(word, "cat"));
      word = new DictionaryWord("", LexicalCategory.Other);
      Assert.True(thesaurus.IsSynonymOf(word, ""));
      word = new DictionaryWord("good-looking", LexicalCategory.Adjective);
      Assert.True(thesaurus.IsSynonymOf(word, "pretty"));
      word = new DictionaryWord("pretty", LexicalCategory.Adjective);
      Assert.True(thesaurus.IsSynonymOf(word, "good-looking"));

      word = new DictionaryWord("", LexicalCategory.Other);
      Assert.False(thesaurus.IsSynonymOf(word, "dog"));
      word = new DictionaryWord("dog", LexicalCategory.Noun);
      Assert.False(thesaurus.IsSynonymOf(word, ""));

      word = new DictionaryWord("cat ", LexicalCategory.Noun);
      Assert.False(thesaurus.IsSynonymOf(word, "cat"));
    }

    [Fact]
    public void IsSynonymOf_OneStringOneDictWord_Capitalisation()
    {
      var word = new DictionaryWord("Cat", LexicalCategory.Noun);
      Assert.True(thesaurus.IsSynonymOf(word, "cat"));
    }

    [Fact]
    public void IsSynonymOf_TwoDictWords_Standard()
    {
      var word1 = new DictionaryWord("cat", LexicalCategory.Noun);
      var word2 = new DictionaryWord("kitty", LexicalCategory.Noun);
      Assert.True(thesaurus.IsSynonymOf(word1, word2));
      word1 = new DictionaryWord("walk", LexicalCategory.Noun);
      word2 = new DictionaryWord("stroll", LexicalCategory.Noun);
      Assert.True(thesaurus.IsSynonymOf(word1, word2));

      // Synonyms of two strings gives strange results sometimes:
      word1 = new DictionaryWord("cat", LexicalCategory.Noun);
      word2 = new DictionaryWord("man", LexicalCategory.Noun);
      Assert.True(thesaurus.IsSynonymOf(word1, word2));

      word1 = new DictionaryWord("cat", LexicalCategory.Noun);
      word2 = new DictionaryWord("dog", LexicalCategory.Noun);
      Assert.False(thesaurus.IsSynonymOf(word1, word2));
      word1 = new DictionaryWord("cat", LexicalCategory.Noun);
      word2 = new DictionaryWord("lion", LexicalCategory.Noun);
      Assert.False(thesaurus.IsSynonymOf(word1, word2));
      word1 = new DictionaryWord("crockery", LexicalCategory.Noun);
      word2 = new DictionaryWord("plate", LexicalCategory.Noun);
      Assert.False(thesaurus.IsSynonymOf(word1, word2));
    }

    [Fact]
    public void IsSynonymOf_TwoDictWord_Filtering()
    {
      var word1 = new DictionaryWord("key", LexicalCategory.Noun);
      var word2 = new DictionaryWord("crucial", LexicalCategory.Adjective);
      Assert.False(thesaurus.IsSynonymOf(word1, word2));
      word1 = new DictionaryWord("key", LexicalCategory.Adjective);
      word2 = new DictionaryWord("crucial", LexicalCategory.Adjective);
      Assert.True(thesaurus.IsSynonymOf(word1, word2));
    }

    [Fact]
    public void IsSynonymOf_TwoDictWords_Capitalisation()
    {
      var word1 = new DictionaryWord("cAt", LexicalCategory.Noun);
      var word2 = new DictionaryWord("cat", LexicalCategory.Noun);
      Assert.True(thesaurus.IsSynonymOf(word1, word2));
    }
  }
}
