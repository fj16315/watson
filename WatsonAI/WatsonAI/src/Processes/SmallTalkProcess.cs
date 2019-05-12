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
      if (input.Contains(character.Name + " is the murderer") || input.Contains("is " +character.Name + " the murderer") || input.Contains("did " + character.Name + " kill"))
      {
        stream.AssignSpecialCaseHandler(this);
        stream.AppendOutput("How dare you, of course not!");
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
          stream.AppendOutput(character.GetMood());
        }
        if (input.Contains("where are we") || input.Contains("where is " + character.Name))
        {
          stream.AssignSpecialCaseHandler(this);
          stream.AppendOutput("We are in the " + character.Location);
        }
        if (input.Contains("what did " + character.Name + " see"))
        {
          stream.AssignSpecialCaseHandler(this);
          stream.AppendOutput(character.GetSeen());
        }
        if(input.Contains("what does " + character.Name + " know") || input.Contains("does " + character.Name + " know anything"))
        {
          stream.AssignSpecialCaseHandler(this);
          stream.AppendOutput(character.GetKnowledgeResponse());
        }
      }
      return stream;
    }
  }
}
