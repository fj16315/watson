using System;
using System.Collections.Generic;
using WatsonAI;
using NHunspell;
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

      Hunspell hunspell = new Hunspell("en_us.aff", "en_us.dic");
      List<string> suggestions = hunspell.Suggest("helo");
      foreach (string suggestion in suggestions)
      {
        Console.WriteLine("Suggestion is : " + suggestion);
      }


      Console.Write("> ");
      var line = Console.ReadLine();
      Console.WriteLine(watson.Run(line));
      if (line == "") { break; }
    }
  }
}

