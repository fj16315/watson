using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace WatsonAI
{
  /// <summary>
  /// Interface for processing Streams.
  /// </summary>
  public interface IProcess
  {
    Stream Process(Stream stream);
  }

  public struct Stream
  {
    private readonly List<string> input;
    private readonly List<string> output;
    private int position;

    public IEnumerable<string> RemainingInput {
      get {
        return input.Skip(position);
      }
    }

    public Stream(List<string> tokens)
    {
      this.input = tokens;
      this.output = new List<string>();
      this.position = 0;
    }

    public static Stream Tokenise(Parser parser, string sentence) 
      => new Stream(new List<string>(parser.Tokenize(sentence)));

    public void AppendOutput(string token)
    {
      this.output.Add(token);
    }

    public string Output()
      => string.Join(", ", output);

    /// <summary>
    /// Increments the position, indicating that a token has been dealt with.
    /// </summary>
    public void Consume()
    {
      this.position++;
    }

    /// <summary>
    /// Calls the process on itself, for nice syntax.
    /// </summary>
    /// <param name="process">The process to use.</param>
    /// <returns>The processed stream.</returns>
    public Stream ProcessWith(IProcess process)
      => process.Process(this);
  }
}
