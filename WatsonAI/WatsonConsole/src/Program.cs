using System;
using System.Collections.Generic;
using System.IO;
using WatsonAI;

// There are commented out lines to allow the rest of the project to build.

public class MainClass
{
  /// <summary>
  ///   Runs the parser as a repl environment.
  /// </summary>
  public static void Main(string[] args)
  {
    var path = Directory.GetCurrentDirectory();
    if (Directory.Exists(Path.Combine(path, "bin")))
    {
      path = Path.Combine(path, "bin", "debug", "netcoreapp2.1");
      Directory.SetCurrentDirectory(path);
    }
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

