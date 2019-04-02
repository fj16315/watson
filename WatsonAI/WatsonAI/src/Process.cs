using System;
using System.Linq;
using System.Collections.Generic;

namespace WatsonAI
{
  public class Processor
  {
    public List<IPreProcess> PreProcesses { get; }
    public List<IProcess> Processes { get; }
    public List<IPostProcess> PostProcesses { get; }

    public Processor()
    {
      PreProcesses = new List<IPreProcess>();
      Processes = new List<IProcess>();
      PostProcesses = new List<IPostProcess>();
    }

    public Processor AddPreProcesses(params IPreProcess[] preProcesses)
    {
      PreProcesses.AddRange(preProcesses);
      return this;
    }

    public Processor AddProcesses(params IProcess[] processes)
    {
      Processes.AddRange(processes);
      return this;
    }

    public Processor AddPostProcesses(params IPostProcess[] postProcesses)
    {
      PostProcesses.AddRange(postProcesses);
      return this;
    }

    public Stream Process(Stream stream)
    {
      PreProcesses.ForEach(stream.PreProcess);
      var new_stream = Processes.Aggregate(stream, (s, p) => p.Process(s));
      PostProcesses.ForEach(new_stream.PostProcess);
      return new_stream;
    }
  }

  public interface IPreProcess
  {
    void PreProcess(ref List<string> tokens);
  }

  /// <summary>
  /// Interface for processing Streams.
  /// </summary>
  public interface IProcess
  {
    Stream Process(Stream stream);
  }

  public interface IPostProcess
  {
    void PostProcess(ref List<string> tokens);
  }

  public struct Stream
  {
    private List<string> input;
    private List<string> output;
    private int position;

    // Maybe not readonly?
    public IEnumerable<string> Output
    {
      get
      {
        return output.AsReadOnly();
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
    }

    public Stream Clone()
      => new Stream(input, output.ToList(), position);

    public static Stream Tokenise(Parser parser, string sentence) 
      => new Stream(new List<string>(parser.Tokenize(sentence)));

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
  }

  public enum Read
  {
    Consume, Peek
  }
}
