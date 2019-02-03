using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEditor;
using GameAI;
using System;
using System.Linq;

public class AIController : MonoBehaviour
{
  GameAI.Entity countess;
  GameAI.Entity key;
  GameAI.Entity prize;
  GameAI.Entity box;
  GameAI.Entity gangster;
  GameAI.Entity room;

  GameAI.Relation has;
  GameAI.Relation unlocks;
  GameAI.Relation contains;

  GameAI.KnowledgeGraph graph_all;

  GameAI.Associations assocs;

  GameAI.Query query;

    // Start is called before the first frame update
    void Start()
    {
    this.countess = new GameAI.Entity(0);
    this.key      = new GameAI.Entity(1);
    this.prize    = new GameAI.Entity(2);
    this.box      = new GameAI.Entity(3);
    this.gangster = new GameAI.Entity(4);
    this.room     = new GameAI.Entity(5);

    this.has      = new GameAI.Relation(1);
    this.unlocks  = new GameAI.Relation(2);
    this.contains = new GameAI.Relation(4);

    this.graph_all = new GameAI.KnowledgeGraphBuilder(6)
      .AddEdge(countess, has, box)
      .AddEdge(key, unlocks, box)
      .AddEdge(box, contains, prize)
      .AddEdge(room, contains, countess)
      .AddEdge(room, contains, key)
      .AddEdge(room, contains, prize)
      .AddEdge(room, contains, box)
      .AddEdge(room, contains, gangster)
      .Build();

    this.assocs = new GameAI.Associations(6, 3);

    assocs.SetNameOf(countess, "countess");
    assocs.SetNameOf(key, "key");
    assocs.SetNameOf(prize, "prize");
    assocs.SetNameOf(box, "box");
    assocs.SetNameOf(gangster, "gangster");
    assocs.SetNameOf(room, "room");

    assocs.SetNameOf(new Relation((int?)has.AsSingleRelation() ?? 0), "has");
    assocs.SetNameOf(new Relation((int?)unlocks.AsSingleRelation() ?? 0), "unlocks");
    assocs.SetNameOf(new Relation((int?)contains.AsSingleRelation() ?? 0), "contains");

    assocs.entityWords.Add( "countess",
      new List<Entity>( new Entity[] { countess } ) );
    assocs.entityWords.Add( "key",
      new List<Entity>( new Entity[] { key } ) );
    assocs.entityWords.Add( "prize",
      new List<Entity>( new Entity[] { prize } ) );
    assocs.entityWords.Add( "box",
      new List<Entity>( new Entity[] { box } ) );
    assocs.entityWords.Add( "gangster",
      new List<Entity>( new Entity[] { box } ) );
    assocs.entityWords.Add( "room",
      new List<Entity>( new Entity[] { room } ) );

    assocs.relationWords.Add( "has",
      new List<Relation>( new Relation[] { has } ) );
    assocs.relationWords.Add( "unlocks",
      new List<Relation>( new Relation[] { unlocks } ) );
    assocs.relationWords.Add( "contains",
      new List<Relation>( new Relation[] { contains } ) );

    var parser = new GameAI.Parser("Assets/AI/englishPCFG.ser.gz");
    this.query = new GameAI.Query(parser);

    // query.logger = (string s) => {
    //   Debug.Log(s);
    //   return null;
    // };

    // var question = "What unlocks the box?";
    // var result = query.Run(assocs, question, graph_all);
    //
    // Debug.Log(question);
    // foreach (var entity in result)
    // {
    //   Debug.Log($"Answer: {assocs.NameOf(entity)}");
    // }
    // Debug.Log("----------------");
  }

    // Update is called once per frame
    void Update()
    {

    }

    public string Query(string question)
      => String.Join( ", ",
            query.Run(assocs, question, graph_all)
            .Select(assocs.NameOf)
         );

}
