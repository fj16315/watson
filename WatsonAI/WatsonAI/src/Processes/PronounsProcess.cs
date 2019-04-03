using System;
using System.Collections.Generic;
using System.Linq;

namespace WatsonAI
{
  public class PronounsProcess : IPreProcess, IPostProcess
  {
    Character character;


    public PronounsProcess(Character character)
    {
      this.character = character;
    }

    public void PreProcess(ref List<string> tokens) 
    {
      for (int i = 0; i < tokens.Count; i++)
      {
        if (i < tokens.Count-1)
        {
          ReplaceWords(new List<string> { "you","are"  }, new List<string> { this.character.Name,"is" }, tokens, i);
          ReplaceWords(new List<string> { "are", "you" }, new List<string> { "is", this.character.Name }, tokens, i);

          ReplaceWords(new List<string> { "I", "am" }, new List<string> { "Watson", "is"  }, tokens, i);
          ReplaceWords(new List<string> { "I", "'m" }, new List<string> { "Watson", "is"  }, tokens, i);
          ReplaceWords(new List<string> { "am", "I" }, new List<string> { "is", "Watson" }, tokens, i);
        }

        ReplaceWords(new List<string> { "your" }, new List<string> { this.character.Name, "'s" }, tokens, i);
        ReplaceWords(new List<string> { "you" }, new List<string> { this.character.Name }, tokens, i);
        ReplaceWords(new List<string> { "I" }, new List<string> { "Watson" }, tokens, i);
        ReplaceWords(new List<string> { "me" }, new List<string> { "Watson" }, tokens, i);
        ReplaceWords(new List<string> { "my" }, new List<string> { "Watson", "'s" }, tokens, i);
        ReplaceWords(new List<string> { "mine" }, new List<string> { "Watson", "'s" }, tokens, i);
      }
    }

    public void PostProcess(ref List<string> tokens)
    {
      for (int i = 0; i < tokens.Count; i++)
      {
        ReplaceWords(new List<string> { "Watson" }, new List<string> { "you" }, tokens, i);
        ReplaceWords(new List<string> { "the", character.Name }, new List<string> { "me" }, tokens, i);
        ReplaceWords(new List<string> { character.Name }, new List<string> { "me" }, tokens, i);
      }
    }

    private void ReplaceWords(List<string> originals, List<string> replacements, List<string> tokens, int i) 
    {
      if (i + originals.Count <= tokens.Count) 
      {
        var originalTokens = tokens.GetRange(i, originals.Count);
        if (originals.Zip(originalTokens, (x,y) => x.Equals(y, StringComparison.OrdinalIgnoreCase)).All(x => x))
        {
          tokens.RemoveRange(i, originals.Count);
          tokens.InsertRange(i, replacements);
        }
      }
    }

    private List<Character> FindCharactersInInput(List<string> tokens) 
    {

      var characterList = Story.Characters.Values.ToList();
      List<Character> characters = new List<Character>();
      foreach (var token in tokens)
      {
        foreach( var name in characterList)
        { 
          if (token.Equals(name.Name))
          {
            characters.Add(name);
          }
        }

      }
      return characters;

    }
  }
}
