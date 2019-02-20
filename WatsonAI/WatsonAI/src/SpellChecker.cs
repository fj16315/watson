using System;
using System.Collections.Generic;
using System.Text;
using wordLib = Microsoft.Office.Interop.Word;

namespace WatsonAI
{
  class SpellChecker
  {
    private wordLib.Application app;

    public SpellChecker()
    {
      this.app = new wordLib.Application();
    }

    public SpellChecker(wordLib.Application app)
    {
      this.app = app;
    }

    public List<string> Suggestions(string word)
    {
      var suggestions = new List<string>();
      var wordSuggestions = app.GetSpellingSuggestions(word);
      foreach (var item in wordSuggestions)
      {
        suggestions.Add(item.ToString());
      }
      return suggestions;
    }
  }
}
