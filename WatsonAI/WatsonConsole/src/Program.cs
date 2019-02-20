using System;
using System.Collections.Generic;
using WatsonAI;

public class MainClass
{
  /// <summary>
  ///   Runs the parser as a repl environment.
  /// </summary>
  public static void Main(string[] args)
  {
    var parser = new Parser();
    var associations = new Associations(1,0);
    associations.SetNameOf(new Entity(0), "walk");

    while (true)
    {
      Console.Write("> ");
      var line = Console.ReadLine();
      var b = associations.Describes(line, new Entity(0));
      Console.WriteLine(b);
      if (line == "") { break; }
    }
  }
}

