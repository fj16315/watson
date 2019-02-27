using System;
using WatsonAI;

public class MainClass
{
  /// <summary>
  ///   Runs the parser as a repl environment.
  /// </summary>
  public static void Main(string[] args)
  {
    IWatson watson = new Watson();

    while (true)
    {
      Console.Write("> ");
      var line = Console.ReadLine();
      Console.WriteLine(watson.Run(line));
      if (line == "") { break; }
    }
  }
}

