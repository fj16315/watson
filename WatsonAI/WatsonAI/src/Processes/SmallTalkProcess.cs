using System;
using System.Collections.Generic;
using System.Text;

namespace WatsonAI
{
  class SmallTalkProcess : IProcess
  {
    private Character character;

    public SmallTalkProcess(Character character)
    {
      this.character = character;
    }

    public Stream Process(Stream stream)
    {
      var clone = stream.Clone();
      var remainingInput = new List<string>();
      clone.RemainingInput(out remainingInput);
      var input = "";
      foreach (var v in remainingInput)
      {
        input = input + " " + v;
      }
      input = input.ToLower();
      Console.WriteLine(input);
      if (input.Contains(character.Name + " is the murderer"))
      {
        stream.AssignSpecialCaseHandler(this);
        stream.AppendOutput("How dare you, of course I'm not!");
      }
      else if (input.Contains("?"))
      {
        if (input.Contains("what is going on"))
        {
          stream.AssignSpecialCaseHandler(this);
          stream.AppendOutput("The earl has been posioned and you need to find out who did it.");
        }
        if (input.Contains("how is " + character.Name))
        {
          stream.AssignSpecialCaseHandler(this);
          stream.AppendOutput(character.Mood);
        }
        if (input.Contains("where are we"))
        {
          stream.AssignSpecialCaseHandler(this);
          stream.AppendOutput("We are in the " + character.Location);
        }
        if (input.Contains("what did you see"))
        {
          stream.AssignSpecialCaseHandler(this);
          stream.AppendOutput(character.Seen);
        }
      }
      return stream;
    }
  }
}
