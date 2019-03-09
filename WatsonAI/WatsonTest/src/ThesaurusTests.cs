using Xunit;
using WatsonAI;
using FsCheck.Xunit;
using FsCheck;
using Syn.WordNet;

namespace WatsonTest
{
  public class ThesaurusTests
  {
    // We initialise this only once for speed improvements.
    private Thesaurus thesaurus = new Thesaurus();

    [Fact]
    public void Describes_Standard()
    {
      Assert.True(thesaurus.Describes("cat", "kitty"));
      Assert.True(thesaurus.Describes("walk", "stroll"));
      Assert.True(thesaurus.Describes("cat", "lion"));
      Assert.True(thesaurus.Describes("lion", "cat"));

      // Synonyms of two strings gives strange results sometimes:
      Assert.True(thesaurus.Describes("cat", "man"));

      Assert.False(thesaurus.Describes("cat", "dog"));
      Assert.False(thesaurus.Describes("catesifjeosifj", "dog"));
      Assert.False(thesaurus.Describes("crockery", "plate"));
    }

    [Fact]
    public void Describes_EdgeCase()
    {
      Assert.True(thesaurus.Describes("cat", "cat"));
      Assert.True(thesaurus.Describes("", ""));

      Assert.False(thesaurus.Describes("", "dog"));
      Assert.False(thesaurus.Describes("cat", ""));

      Assert.False(thesaurus.Describes("cat ", "cat"));
    }

    [Fact]
    public void Describes_Capitalisation()
    {
      Assert.True(thesaurus.Describes("CAT", "cat"));
      Assert.True(thesaurus.Describes("CaT", "cat"));
      Assert.True(thesaurus.Describes("CaT", "cAt"));
      Assert.True(thesaurus.Describes("CAt", "kitty"));
      Assert.True(thesaurus.Describes("kitty", "CAt"));
      Assert.True(thesaurus.Describes("kItty", "cat"));
      Assert.True(thesaurus.Describes("cat", "kItty"));
    }

    [Fact]
    public void Describes_PartOfSpeechFilter_Standard()
    {
      Assert.True(thesaurus.Describes("cat", "kitty", PartOfSpeech.Noun));
      Assert.True(thesaurus.Describes("walk", "stroll", PartOfSpeech.Verb));
      Assert.True(thesaurus.Describes("cat", "lion", PartOfSpeech.Noun));
      Assert.True(thesaurus.Describes("lion", "cat", PartOfSpeech.Noun));

      // Synonyms of two strings gives strange results sometimes:
      Assert.True(thesaurus.Describes("cat", "man", PartOfSpeech.Noun));

      Assert.False(thesaurus.Describes("cat", "dog", PartOfSpeech.Noun));
      Assert.False(thesaurus.Describes("crockery", "plate", PartOfSpeech.Noun));
    }

    [Fact]
    public void Describes_PartOfSpeechFilter_Filtering()
    {
      Assert.False(thesaurus.Describes("key", "crucial", PartOfSpeech.Noun));
      Assert.True(thesaurus.Describes("key", "crucial", PartOfSpeech.Adjective));
    }

    [Fact]
    public void Describes_PartOfSpeechFilter_EdgeCase()
    {
      Assert.True(thesaurus.Describes("cat", "cat", PartOfSpeech.Noun));
      Assert.True(thesaurus.Describes("good-looking", "pretty", PartOfSpeech.Adjective));

      Assert.False(thesaurus.Describes("cat", "cat", PartOfSpeech.Adjective));
      Assert.False(thesaurus.Describes("", "", PartOfSpeech.Noun));
    }

    [Fact]
    public void Describes_PartOfSpeechFilter_NoneExceptionThrowing()
    {
      Assert.Throws<System.Exception>(() => thesaurus.Describes("dog", "", PartOfSpeech.None));
      Assert.Throws<System.Exception>(() => thesaurus.Describes("dog", "catesfliejsf", PartOfSpeech.None));
    }

    [Fact]
    public void Describes_PartOfSpeechFilter_Capitalisation()
    {
      Assert.True(thesaurus.Describes("Cat", "cat", PartOfSpeech.Noun));
    }
  }
}
