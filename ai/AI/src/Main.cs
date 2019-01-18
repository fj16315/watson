using GameAI;
using System;

public class MainClass
{
  /// <summary>
  ///   Runs the parser as a repl environment.
  /// </summary>
  public static void Main(string[] args)
  {
    var parser = Parser.FromRecommendedPath();
    KnowledgeGraph _kg = null;

    while (true)
    {
      Console.Write("> ");
      var line = Console.ReadLine();
      if (line == "") { break; }

      var tree = parser.Parse(line);
      Console.WriteLine("{0}\n", tree.pennString());

      foreach (var td in parser.DependenciesFrom(tree))
      {
        Console.WriteLine("{0}", td);
      }
      Console.WriteLine("\n");
      EntityQuery equery = new EntityQuery();
      Console.WriteLine("{0}\n", equery.query(parser.DependenciesFrom(tree), _kg));
    }
  }
}

