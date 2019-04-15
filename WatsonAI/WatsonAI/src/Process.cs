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

      foreach (var process in Processes)
      {
        if (!stream.IsSpecialCase || stream.SpecialCaseHandler.Equals(process))
        {
          stream = process.Process(stream);
        }
      }
      PostProcesses.ForEach(stream.PostProcess);
      return stream;
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
}
