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
      return stream;
    }
  }
}
