using Annytab.Stemmer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WatsonAI.Properties;

namespace WatsonAI
{
  public class Stemmer 
  {
    private IStemmer stemmer;
    private Dictionary<string, string> irregularStems;

    public Stemmer()
    {
      this.stemmer = new EnglishStemmer();
      this.irregularStems = ReadInIrregularStems();
    }

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

    private Dictionary<string,string> ReadInIrregularStems()
    {
      var lines = Resources.IrregularStems.Split(
        new[] { "\r\n", "\r", "\n" },
        StringSplitOptions.None);
      var maps = lines.Select(line => line.Split(' '));
      var dict = new Dictionary<string, string>();
      foreach (var map in maps)
      {
        if (map.Length == 2)
        {
          dict.TryAdd(map[0], map[1]);
        }
      }
      return dict;
    }
  }
}
