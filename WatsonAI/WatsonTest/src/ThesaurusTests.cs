using Xunit;
using WatsonAI;
using FsCheck.Xunit;
using FsCheck;
using Syn.WordNet;
using System.Collections.Generic;

namespace WatsonTest
{
  public class ThesaurusTests
  {
    /// <summary>
    /// Having this as static means it is shared amongst all tests.
    /// This is done because the construction is expensive as it must read in data files.
    /// An alternative to static is XUnit fixtures, this may be needed in future.
    /// </summary>
    private static Thesaurus thesaurus = new Thesaurus();

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

    [Fact]
    public void GetSynonyms_Stemming()
    {
      Assert.False(thesaurus.GetSynonyms("cats").GetEnumerator().MoveNext());
      Assert.False(thesaurus.GetSynonyms("cats", PartOfSpeech.Noun).GetEnumerator().MoveNext());

      Assert.True(thesaurus.GetSynonyms("cats", true).GetEnumerator().MoveNext());
      Assert.True(thesaurus.GetSynonyms("cats", PartOfSpeech.Noun, true).GetEnumerator().MoveNext());
    }

    [Fact]
    public void Describes_Stemming()
    {
      Assert.False(thesaurus.Describes("kitty", "cats"));
      Assert.False(thesaurus.Describes("kitty", "cats", PartOfSpeech.Noun));

      Assert.True(thesaurus.Describes("kitty", "cats", true));
      Assert.True(thesaurus.Describes("kitty", "cats", PartOfSpeech.Noun, true));

      Assert.True(thesaurus.Describes("is", "be", true));
      Assert.True(thesaurus.Describes("is", "be", PartOfSpeech.Verb, true));
    }

    [Fact]
    public void Describes_EntityRemoval()
    {
      var associations = new Associations();
      associations.AddEntityName(new Entity(0), "cat");
      associations.AddEntityName(new Entity(1), "man");
      Thesaurus associationThesaurus = new Thesaurus(associations);

      Assert.False(associationThesaurus.Describes("cat", "man"));
      Assert.False(associationThesaurus.Describes("cat", "man", true));
      Assert.False(associationThesaurus.Describes("cat", "man", PartOfSpeech.Noun));
      Assert.False(associationThesaurus.Describes("cat", "man", PartOfSpeech.Noun, true));

      Assert.False(associationThesaurus.Describes("cats", "men", true));
      Assert.False(associationThesaurus.Describes("cats", "men", PartOfSpeech.Noun, true));
      
      Assert.False(associationThesaurus.Describes("cats", "cat"));
      Assert.False(associationThesaurus.Describes("cat", "cat", true));
    }

    [Fact]
    public void Describes_VerbRemoval()
    {
      var associations = new Associations();
      associations.AddVerbName(new Verb(0), "kill");
      associations.AddVerbName(new Verb(1), "consume");
      Thesaurus associationThesaurus = new Thesaurus(associations);

      Assert.False(associationThesaurus.Describes("kill", "consume"));
      Assert.False(associationThesaurus.Describes("kill", "consume", true));
      Assert.False(associationThesaurus.Describes("kill", "consume", PartOfSpeech.Verb));
      Assert.False(associationThesaurus.Describes("kill", "consume", PartOfSpeech.Verb, true));

      Assert.False(associationThesaurus.Describes("killed", "consumes", true));
      Assert.False(associationThesaurus.Describes("killed", "consumes", PartOfSpeech.Verb, true));

      Assert.False(associationThesaurus.Describes("kill", "kill"));
      Assert.False(associationThesaurus.Describes("kill", "killed", true));
    }

    [Fact]
    public void GetSynonyms_NoDuplicates()
    {
      //Plate definitely has duplicates before 
      var words = thesaurus.GetSynonyms("plate");
      var set = new HashSet<string>();
      int count = 0;

      foreach (var word in words)
      {
        set.Add(word);
        count++;
      }
      Assert.True(set.Count == count);


      words = thesaurus.GetSynonyms("plate", PartOfSpeech.Noun);
      set = new HashSet<string>();
      count = 0;

      foreach (var word in words)
      {
        set.Add(word);
        count++;
      }
      Assert.True(set.Count == count);


      SynSetRelation[] relations = { SynSetRelation.SimilarTo };
      words = thesaurus.GetSynonyms("plate", PartOfSpeech.Noun, relations);
      set = new HashSet<string>();
      count = 0;

      foreach (var word in words)
      {
        set.Add(word);
        count++;
      }
      Assert.True(set.Count == count);
    }
  }
}
