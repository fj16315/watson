using System;
using System.Collections.Generic;
using WatsonAI;

// There are commented out lines to allow the rest of the project to build.

public class MainClass
{
  /// <summary>
  ///   Runs the parser as a repl environment.
  /// </summary>
  public static void Main(string[] args)
  {
    //IWatson watson = new Watson();

    while (true)
    {
      Console.Write("> ");
      var line = Console.ReadLine();
      //Console.WriteLine(watson.Run(line));
      if (line == "") { break; }
    }
  }
}

