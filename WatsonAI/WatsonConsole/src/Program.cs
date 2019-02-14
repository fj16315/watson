using System;
using WatsonAI;

public class MainClass
{
  /// <summary>
  ///   Runs the parser as a repl environment.
  /// </summary>
  public static void Main(string[] args)
  {
    var parser = new Parser();

    while (true)
    {
      Console.Write("> ");
      var line = Console.ReadLine();
      if (line == "") { break; }
    }
  }
}

