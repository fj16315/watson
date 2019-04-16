using System;
using System.Collections.Generic;
using System.Text;

namespace WatsonAI
{
  class SmallTalkProcess : IProcess
  {
    public SmallTalkProcess()
    {

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
      if (input.Contains("What is going on"))
      {
        stream.AssignSpecialCaseHandler(this);
        stream.AppendOutput("The earl has been posioned and you need to find out who did it.");
      }
      return stream;
    }
  }
}
