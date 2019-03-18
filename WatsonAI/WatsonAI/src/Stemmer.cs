using Annytab.Stemmer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WatsonAI.Properties;

namespace WatsonAI
{
  /// <summary>
  /// Stems words: eg. cats -> cat, is -> be, has -> have.
  /// </summary>
  public class Stemmer 
  {
    private IStemmer stemmer;

    private Dictionary<string, string> irregularStems;

    /// <summary>
    /// Constructurs a new stemmer.
    /// </summary>
    public Stemmer()
    {
      this.stemmer = new EnglishStemmer();
      this.irregularStems = ReadInIrregularStems();
    }

    /// <summary>
    /// Gets the stemmed version of a word.
    /// </summary>
    /// <remarks>Can sometimes break words if stemming is not necessary.</remarks>
    /// <param name="word">The word to stem.</param>
    /// <returns>The stemmed word.</returns>
    public string GetSteamWord(string word)
    {
      if (irregularStems.ContainsKey(word))
      {
        return irregularStems[word];
      }
      else
      {
        return this.stemmer.GetSteamWord(word);
      }
    }

    /// <summary>
    /// Read in the irregular stems from the resources file.
    /// </summary>
    /// <returns>A dictionary containing mapping from irregular to stemmed word.</returns>
    private Dictionary<string,string> ReadInIrregularStems()
    {
      var lines = Resources.IrregularStems.Split(
        new[] { "\r\n", "\r", "\n" },
        StringSplitOptions.None);
      var maps = lines.Select(line => line.Split(' '));
      var dict = new Dictionary<string, string>();
      foreach (var map in maps)
      {
        if (map.Length >= 2)
        {
          dict.TryAdd(map[0], map[1]);
        }
      }
      return dict;
    }
  }
}
