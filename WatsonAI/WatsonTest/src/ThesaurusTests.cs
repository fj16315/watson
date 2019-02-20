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
  }
}
