using GameAI;
using System;
using System.Collections.Generic;

public class MainClass
{
  /// <summary>
  ///   Runs the parser as a repl environment.
  /// </summary>
  public static void Main(string[] args)
  {
    var parser = Parser.FromRecommendedPath();

    Entity countess = new Entity(0);
    Entity key = new Entity(1);
    Entity prize = new Entity(2);
    Entity box = new Entity(3);
    Entity gangster = new Entity(4);
    Entity room = new Entity(5);

    Relation has = new Relation(1);
    Relation unlocks = new Relation(2);
    Relation contains = new Relation(4);

    KnowledgeGraph kg = new KnowledgeGraphBuilder(6)
      .AddEdge(countess, has, box)
      .AddEdge(key, unlocks, box)
      .AddEdge(box, contains, prize)
      .AddEdge(room, contains, countess)
      .AddEdge(room, contains, key)
      .AddEdge(room, contains, prize)
      .AddEdge(room, contains, box)
      .AddEdge(room, contains, gangster)
      .Build();

    Associations assocs = new Associations(6, 3);
    assocs.SetNameOf(countess, "countess");
    assocs.SetNameOf(key, "key");
    assocs.SetNameOf(prize, "prize");
    assocs.SetNameOf(box, "box");
    assocs.SetNameOf(gangster, "gangster");
    assocs.SetNameOf(room, "room");

    assocs.SetNameOf(new Relation((int?)has.AsSingleRelation() ?? 0), "has");
    assocs.SetNameOf(new Relation((int?)unlocks.AsSingleRelation() ?? 0), "unlocks");
    assocs.SetNameOf(new Relation((int?)contains.AsSingleRelation() ?? 0), "contains");

    assocs.entityWords.Add("countess",
      new List<Entity>(new Entity[] { countess }));
    assocs.entityWords.Add("key",
      new List<Entity>(new Entity[] { key }));
    assocs.entityWords.Add("prize",
      new List<Entity>(new Entity[] { prize }));
    assocs.entityWords.Add("box",
      new List<Entity>(new Entity[] { box }));
    assocs.entityWords.Add("gangster",
      new List<Entity>(new Entity[] { box }));
    assocs.entityWords.Add("room",
      new List<Entity>(new Entity[] { room }));

    assocs.relationWords.Add("has",
      new List<Relation>(new Relation[] { has }));
    assocs.relationWords.Add("unlocks",
      new List<Relation>(new Relation[] { unlocks }));
    assocs.relationWords.Add("contains",
      new List<Relation>(new Relation[] { contains }));

    Query query = new Query(parser);

    while (true)
    {
      Console.Write("> ");
      var line = Console.ReadLine();
      if (line == "") { break; }

      var tree = parser.Parse(line);
      //Console.WriteLine("{0}\n", tree.pennString());
      var tdList = parser.DependenciesFrom(tree);

      /*foreach (var td in parser.DependenciesFrom(tree))
      {
        Console.WriteLine("{0}", td);
      }*/
      IEnumerable<Entity> results = query.Run(assocs, line, kg);
      //Console.WriteLine("Results found:");
      //Console.WriteLine(assocs.NameOf(results.GetEnumerator().Current));
      string response = SpeechSynthesis.Synthesise(tdList, results, assocs);
      //Console.WriteLine("Response generated");

      Console.WriteLine("{0}", response);
    }
  }
}

