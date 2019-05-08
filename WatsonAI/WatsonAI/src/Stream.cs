using System;
using System.Collections.Generic;
using System.Linq;

namespace WatsonAI
{
  public struct Stream
  {
    public string spellCorrectedInput;
    public string nonTokenisedInput;
    private List<string> input;
    private List<string> output;
    private int position;

    public List<string> Input { get { return input; } }
    public List<string> Output { get { return output; } }

    /// <summary>
    /// If this is defined, only this process will run.
    /// </summary>
    public IProcess SpecialCaseHandler { get; private set; }

    /// <summary>
    /// If this stream is a special case: one process wants to handle it.
    /// </summary>
    /// <returns>True if the stream is a special case.</returns>
    public bool IsSpecialCase {
      get 
      {
        return this.SpecialCaseHandler != null;
      }
    }

    private Stream(List<string> tokens)
      : this(tokens, new List<string>(), 0)
    {
      // Purposefully empty
    }

    private Stream(List<string> input, List<string> output, int position)
    {
      this.input = input;
      this.output = output;
      this.position = position;
      this.SpecialCaseHandler = null;
      this.nonTokenisedInput = null;
      this.spellCorrectedInput = null;
    }

    public Stream Clone()
      => new Stream(new List<string>(input), new List<string>(output), position);

    public static Stream Tokenise(Parser parser, string sentence)
    {
      Stream stream = new Stream(new List<string>(parser.Tokenize(sentence)));
      stream.nonTokenisedInput = sentence;
      stream.spellCorrectedInput = sentence;
      return stream;
    }

    public bool NextToken(out string token, Read read = Read.Consume)
    {
      if (position >= input.Count)
      {
        token = null;
        return false;
      }
      token = input[position];
      if (read == Read.Consume)
      {
        position += 1;
      }
      return true;
    }

    public bool RemainingInput(out List<string> tokens, Read read = Read.Consume)
    {
      tokens = input.Skip(position).ToList();
      if (read == Read.Consume)
      {
        position = input.Count;
      }
      return tokens.Any();
    }

    public void ForEach(Read read, Func<string,bool> func)
    {
      foreach (var token in input.Skip(position))
      {
        func(token);
      }
      if (read == Read.Consume)
      {
        position = input.Count;
      }
    }

    public bool ConsumeIf(Func<string,bool> pred, out string token)
    {
      var succ = NextToken(out token, Read.Peek);
      if (!pred(token))
      {
        return false;
      }
      Consume();
      return true;
    }

    public bool ConsumeIf(Func<string,bool> pred)
    {
      string _ignore;
      return ConsumeIf(pred, out _ignore);
    }

    public bool ConsumeWhile(Func<string,bool> pred, out List<string> tokens)
    {
      tokens = new List<string>();
      string token;
      while (ConsumeIf(pred, out token))
      {
        tokens.Append(token);
      }
      return tokens.Any();
    }

    public bool Consume()
    {
      if (position < input.Count)
      {
        position += 1;
        return true;
      }
      return false;
    }

    public void AppendOutput(string token)
    {
      output.Add(token);
    }

    public void PreProcess(IPreProcess process)
    {
      process.PreProcess(ref input);
    }

    public void PostProcess(IPostProcess process)
    {
      process.PostProcess(ref output);
    }

    /// <summary>
    /// Resets output to be empty
    /// </summary>
    public void ClearOutput()
    {
      output = new List<string>();
    }

    /// <summary>
    /// Assigns this process to be a special case handler for the stream.
    /// </summary>
    /// <param name="process">The process to handle the special case.</param>
    public void AssignSpecialCaseHandler(IProcess process)
    {
      this.SpecialCaseHandler = process;
    }

    /// <summary>
    /// Removes the special case handler for the stream.
    /// </summary>
    public void RemoveSpecialCaseHandler()
    {
      this.SpecialCaseHandler = null;
    }

    public void SetInput(List<string> newInput)
    {
      input = newInput;
    }
    public void SetOutput(List<string> newOutput)
    {
      output = newOutput;
    }
  }

  public enum Read
  {
    Consume, Peek
  }
}
