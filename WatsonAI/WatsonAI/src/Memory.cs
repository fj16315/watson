using System.Collections.Generic;

namespace WatsonAI
{
  public class Memory
  {
    private readonly List<string> inputs;
    private readonly List<string> responses;
    private readonly int capacity;
    private readonly Character character;

    /// <summary>
    /// The inputs that the player gave to the character that are
    /// stored in the memory.
    /// </summary>
    public IEnumerable<string> Inputs {
      get {
        foreach (var input in this.inputs)
        {
          yield return input;
        }
      }
    }

    /// <summary>
    /// The responses that the character has given to the player that 
    /// are stored in the memory.
    /// </summary>
    public IEnumerable<string> Responses {
      get {
        foreach (var response in this.responses)
        {
          yield return response;
        }
      }
    }

    /// <summary>
    /// Construct a new character memory.
    /// </summary>
    /// <param name="character">The character the memory belongs to.</param>
    /// <param name="capacity">How many input & response pairs back to store.</param>
    public Memory(Character character, int capacity)
    {
      this.capacity = capacity;
      this.character = character;
      this.inputs = new List<string>();
      this.responses = new List<string>();
    }

    /// <summary>
    /// Store a new character input in the memory.
    /// </summary>
    /// <param name="input">The input to add.</param>
    public void AppendInput(string input)
    {
      this.inputs.Insert(0, input);
      if (this.inputs.Count > capacity)
      {
        this.inputs.RemoveAt(0);
      }
    }

    /// <summary>
    /// Store a new character response in the memory.
    /// </summary>
    /// <param name="response"></param>
    public void AppendResponse(string response)
    {
      this.responses.Insert(0, response);
      if (this.responses.Count > capacity)
      {
        this.responses.RemoveAt(0);
      }
    }

    /// <summary>
    /// Gets the last input given by the player.
    /// </summary>
    /// <returns>The last input given by the player.</returns>
    public string GetLastInput()
    {
      if (this.inputs.Count > 0)
      {
        return this.inputs[0];
      }
      return "";
    }

    /// <summary>
    /// Gets the last response given by the character.
    /// </summary>
    /// <returns>The last response given by the character.</returns>
    public string GetLastResponse()
    {
      if (this.responses.Count > 0)
      {
        return this.responses[0];
      }
      return "";
    }
  }
}
