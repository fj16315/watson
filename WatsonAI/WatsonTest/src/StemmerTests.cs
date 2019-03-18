using Xunit;
using WatsonAI;
using FsCheck.Xunit;
using FsCheck;
using Syn.WordNet;
using System.Collections.Generic;

namespace WatsonTest
{
  public class StemmerTests
  {
    /// <summary>
    /// Having this as static means it is shared amongst all tests.
    /// This is done because the construction is expensive as it must read in data files.
    /// An alternative to static is XUnit fixtures, this may be needed in future.
    /// </summary>
    private static Stemmer stemmer = new Stemmer();

    [Fact]
    public void SpecialCaseStemming()
    {
      Assert.True(stemmer.GetSteamWord("is") == "be");
    }
  }
}
