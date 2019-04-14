using System.Collections.Generic;
using System.Linq;

namespace WatsonAI
{
  /// <summary>
  /// Represents the memory of one character.
  /// </summary>
  /// <remarks>
  /// Stores the lists of inputs and responses backwards for constant 
  /// time complexity adding, up to the capacity, then linear.
  /// Accessing a memory element has constant time complexity.
  /// </remarks>
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
    /// <remarks>This returns a lazy view of the list.</remarks>
    public IEnumerable<string> Inputs {
      get {
        // This uses as enumerable to allow for LINQ to reverse lazily.
        foreach (var input in inputs.AsEnumerable().Reverse())
        {
          yield return input;
        }
      }
    }

    /// <summary>
    /// The responses that the character has given to the player that 
    /// are stored in the memory.
    /// </summary>
    /// <remarks>This returns a lazy view of the list.</remarks>
    public IEnumerable<string> Responses {
      get {
        // This uses as enumerable to allow for LINQ to reverse lazily.
        foreach (var response in responses.AsEnumerable().Reverse())
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
      inputs.Add(input);
      if (inputs.Count > capacity)
      {
        inputs.RemoveAt(0);
      }
    }

    /// <summary>
    /// Store a new character response in the memory.
    /// </summary>
    /// <param name="response">The response to add.</param>
    public void AppendResponse(string response)
    {
      responses.Add(response);
      if (responses.Count > capacity)
      {
        responses.RemoveAt(0);
      }
    }

    /// <summary>
    /// Gets the last input given by the player.
    /// </summary>
    /// <returns>The last input given by the player, or empty if there is none.</returns>
    public string GetLastInput()
    {
      if (inputs.Count > 0)
      {
        return inputs[inputs.Count - 1];
      }
      return "";
    }

    /// <summary>
    /// Gets the last response given by the character.
    /// </summary>
    /// <returns>The last response given by the character, or empty if there is none.</returns>
    public string GetLastResponse()
    {
      if (responses.Count > 0)
      {
        return responses[responses.Count - 1];
      }
      return "";
    }
  }
}
