using OpenNLP.Tools.Parser;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WatsonAI
{

  public struct Result<T>
  {
    public T value { get; }
    public bool hasValue { get; }

    public Result(T value)
    {
      this.hasValue = true;
      this.value = value;
    }

    public static Result<T> operator |(Result<T> lhs, Result<T> rhs)
      => lhs.hasValue ? lhs : rhs;
  }

  public abstract class Pattern<a> 
  {
    public abstract Result<a> Match(Parse tree);

    public static Pattern<a> operator |(Pattern<a> lhs, Pattern<a> rhs)
      => new Or<a>(lhs, rhs);
  }

  public class Or<a> : Pattern<a> 
  {
    private readonly Pattern<a> lhs;
    private readonly Pattern<a> rhs;

    public Or(Pattern<a> lhs, Pattern<a> rhs)
    {
      this.lhs = lhs;
      this.rhs = rhs;
    }

    public override Result<a> Match(Parse tree)
      => lhs.Match(tree) | rhs.Match(tree);
  }

  public class And<a,b> : Pattern<Tuple<a,b>> 
  {
    private readonly Pattern<a> lhs;
    private readonly Pattern<b> rhs;

    public And(Pattern<a> lhs, Pattern<b> rhs)
    {
      this.lhs = lhs;
      this.rhs = rhs;
    }

    public override Result<Tuple<a,b>> Match(Parse tree)
    {
      var lhsResult = lhs.Match(tree);
      var rhsResult = rhs.Match(tree);
      if (lhsResult.hasValue && rhsResult.hasValue)
      {
        return new Result<Tuple<a, b>>(Tuple.Create(lhsResult.value, rhsResult.value));
      }
      return new Result<Tuple<a, b>>();
    }
  }

  public class Word : Pattern<string>
  {
    private readonly string word;

    private static Thesaurus thesaurus;

    public static void SetThesaurus(Thesaurus thesaurus)
    {
      Word.thesaurus = thesaurus;
    }

    public Word(string word)
    {
      this.word = word;
    }

    public override Result<string> Match(Parse tree) 
      => thesaurus.Describes(tree.Value, word) ? new Result<string>(word) : new Result<string>();
  }

  public class Branch : Pattern<Parse>
  {
    private readonly string branch;

    public Branch(string branch)
    {
      this.branch = branch;
    }

    public override Result<Parse> Match(Parse tree)
      => tree.Type == branch ? new Result<Parse>(tree) : new Result<Parse>();
  }

  public class Children<a> : Pattern<IEnumerable<a>> 
  {
    private readonly Pattern<Parse> parent;
    private readonly Pattern<a> child;

    public Children(Pattern<Parse> parent, Pattern<a> child)
    {
      this.parent = parent;
      this.child = child;
    }

    public override Result<IEnumerable<a>> Match(Parse tree)
    {
      var newTree = parent.Match(tree);
      if (newTree.hasValue)
      {
        return new Result<IEnumerable<a>>(newTree.value
          .GetChildren()
          .Select(t => child.Match(t))
          .Where(r => r.hasValue)
          .Select(r => r.value));
      }
      return new Result<IEnumerable<a>>();
    }
  }

  public class Descendant<a> : Pattern<IEnumerable<a>> 
  {
    private readonly Pattern<Parse> parent;
    private readonly Pattern<a> descendant;

    public Descendant(Pattern<Parse> parent, Pattern<a> descendant)
    {
      this.parent = parent;
      this.descendant = descendant;
    }

    public override Result<IEnumerable<a>> Match(Parse tree)
    {
      var newTree = parent.Match(tree);
      if (newTree.hasValue)
      {
        Func<Parse, IEnumerable<a>> f = null;
        f = (Parse t) => {
          var d = this.descendant.Match(t);
          var ds = t.GetChildren().SelectMany(f);
          if (!d.hasValue) return ds;
          return ds.Append(d.value);
        };
        return new Result<IEnumerable<a>>(newTree.value.GetChildren().SelectMany(f));
      } 
      return new Result<IEnumerable<a>>();
    }
  }

  public class EntityName : Pattern<Entity>
  {
    private Associations associations;
    private Thesaurus thesaurus;

    public EntityName(Associations associations, Thesaurus thesaurus)
    {
      this.associations = associations;
      this.thesaurus = thesaurus;
    }

    public override Result<Entity> Match(Parse tree)
      => associations
      .EntityNames()
      .Where(name => thesaurus.Describes(tree.Value, name, true))
      .Select(name => associations.UncheckedGetEntity(name))
      .Select(e => new Result<Entity>(e))
      .FirstOrDefault();
  }

  public class VerbName : Pattern<Verb>
  {
    private Associations associations;
    private Thesaurus thesaurus;

    public VerbName(Associations associations, Thesaurus thesaurus)
    {
      this.associations = associations;
      this.thesaurus = thesaurus;
    }

    public override Result<Verb> Match(Parse tree)
      => associations
      .VerbNames()
      .Where(name => thesaurus.Describes(tree.Value, name, true))
      .Select(name => associations.UncheckedGetVerb(name))
      .Select(v => new Result<Verb>(v))
      .FirstOrDefault();
  }

  public class QuestionProcess : IProcess
  {
    private Parser parser;
    private Character character;
    private Knowledge kg;
    private Thesaurus thesaurus;
    private Associations associations;

    /// <summary>
    /// Text engine for debuging the parser.
    /// </summary>
    public QuestionProcess(Character character, Knowledge kg)
    {
      this.parser = new Parser();
      this.character = character;
      this.kg = kg;
    }

    /// <summary>
    /// Text engine for debuging the specified Parser.
    /// </summary>
    /// <param name="parse">The parser to use.</param>
    public QuestionProcess(Parser parse, Character character, Knowledge kg, Thesaurus thesaurus, Associations associations)
    {
      this.parser = parse;
      this.character = character;
      this.kg = kg;
      this.thesaurus = thesaurus;
      this.associations = associations;
    }

    public Stream Process(Stream stream)
    {
      Word.SetThesaurus(thesaurus);

      var tree = this.parser.Parse(stream.RemainingInput);

      var noun = new EntityName(associations, thesaurus);
      var verb = new VerbName(associations, thesaurus);
      var nounPhrase = new Descendant<Entity>(new Branch("NP"), noun);
      var verbPhrase = new Descendant<Verb>(new Branch("VP"), verb);
      //var nounVerbPhrase = new Descendant<Tuple<IEnumerable<Entity>,IEnumerable<Verb>>>(new Branch("SQ"), new And<IEnumerable<Entity>, IEnumerable<Verb>>(nounPhrase, verbPhrase));
      //var nounVerbPhrase = new And<IEnumerable<Entity>, IEnumerable<Verb>>(nounPhrase, verbPhrase);
      //var question = new Descendant<Tuple<IEnumerable<Entity>,IEnumerable<Verb>>>(new Branch("SBARQ"), nounVerbPhrase);
      //var question = new Descendant<IEnumerable<Tuple<IEnumerable<Entity>,IEnumerable<Verb>>>>(new Branch("SBARQ"), nounVerbPhrase);

      var top = new Branch("TOP");
      //var query = new Descendant<IEnumerable<Tuple<IEnumerable<Entity>,IEnumerable<Verb>>>>(top, question);
      var queryN = new Descendant<IEnumerable<Entity>>(top, nounPhrase);
      var queryV = new Descendant<IEnumerable<Verb>>(top, verbPhrase);
      //var query = new Descendant<IEnumerable<IEnumerable<Tuple<IEnumerable<Entity>,IEnumerable<Verb>>>>>(top, question);
      //var query = new Descendant<IEnumerable<Tuple<IEnumerable<Entity>,IEnumerable<Verb>>>>(top, nounVerbPhrase);


      var pNs = queryN.Match(tree);
      var pVs = queryV.Match(tree);
      if (pNs.hasValue && pVs.hasValue)
      {
        var ns = pNs.value.SelectMany(x => x);
        var vs = pVs.value.SelectMany(x => x);

        var pairs = from n in ns
                    from v in vs
                    select Tuple.Create(n, v);

        foreach (var p in pairs.Distinct())
        {
          var e = p.Item1;
          var v = p.Item2;
          var answers = GetAnswers(v, e);
          string verbName;
          associations.TryNameVerb(v, out verbName);
          string entityName;
          associations.TryNameEntity(e, out entityName);
          stream.AppendOutput(GenerateResponse(entityName, verbName, answers));
        }
      }
      //if (pairses.hasValue)
      //{
      //  var pairs = pairses.value.SelectMany(x => x);
      //  foreach (var p in pairs)
      //  {
      //    var e = p.Item1.First();
      //    var v = p.Item2.First();
      //    var answers = GetAnswers(v, e);
      //    string verbName;
      //    associations.TryNameVerb(v, out verbName);
      //    string entityName;
      //    associations.TryNameEntity(e, out entityName);
      //    stream.AppendOutput(GenerateResponse(entityName, verbName, answers));
      //  }
      //}
      //foreach (var nonu in pairses.value.SelectMany(x => x))
      //{
      //  Console.WriteLine($"{nonu}");
      //}
      //foreach (var pair in pairses.value.SelectMany(x => x))
      //{
      //  Console.WriteLine($"{pair.Item1.First()} , {pair.Item2.First()}");
      //}
      //else
      //{
      //  stream.AppendOutput("Question?!");
      //}
      return stream;
    }



































    public InputOutput Process(InputOutput io)
    {
      if (io.remainingInput.Length == 0) return io;

      var parse = this.parser.Parse(io.remainingInput).GetChildren()[0];
      var nounPhrase = parse;
      var verbPhrase = parse;
      var noun = parse;
      var verb = parse;

      if (parse.Type.Equals("SBARQ"))
      {
        Console.WriteLine("This has a question:");
        var sq = parse.GetChildren()[1];
        if (sq.ChildCount == 1)
        {
          verbPhrase = sq.GetChildren()[0];
          verb = verbPhrase.GetChildren()[0];
          nounPhrase = verbPhrase.GetChildren()[1];
          noun = nounPhrase.GetChildren()[nounPhrase.ChildCount - 1];
        }
        else
        {
          for (int i = 0; i < sq.ChildCount; i++)
          {
            var current = sq.GetChildren()[i];
            if (current.Type.Equals("VP"))
            {
              verbPhrase = current;
              verb = verbPhrase.GetChildren()[0];
            }
            else if (current.Type.Contains("VB"))
            {
              verb = current;
            }
            else
            {
              nounPhrase = current;
              noun = nounPhrase.GetChildren()[nounPhrase.ChildCount - 1];
            }
          }
        }
        var wh = parse.GetChildren()[0].GetChildren()[0];
        //io.output += "Verb = " + verb.Show() + ", Noun = " + noun.Show();
        var answers = Query(noun.Value, verb.Value);
        if (answers.Count != 0)
        {
          io.output += GenerateResponse(noun.Value, verb.Value, answers);
        }
      }

      return io;
    }

    public List<Entity> Query(string noun, string verb)
    {
      Verb graphVerb = new Verb();
      Entity graphEntity = new Entity();
      if (!GetVerb(verb, out graphVerb) || !GetEntity(noun, out graphEntity))
      {
        string verbName;
        string entityName;
        associations.TryNameEntity(graphEntity, out entityName);
        associations.TryNameVerb(graphVerb, out verbName);
        Console.WriteLine("Failed, attempted verb is: " + verbName + " " + graphVerb);
        Console.WriteLine("Failed, attempted entity is: " + entityName + " " + graphEntity);
        return new List<Entity>();
      }
      Console.WriteLine("The verb is: " + verb + " " + graphVerb);
      Console.WriteLine("The entity is: " + noun + " " + graphEntity);

      return GetAnswers(graphVerb, graphEntity);
    }

    private List<Entity> GetAnswers(Verb verb, Entity entity)
    {
      var answers = new List<Entity>();
      foreach (VerbPhrase vp in kg.GetVerbPhrases())
      {
        if (vp.verb.Equals(verb))
        {
          var dobject = Valent.Dobj(entity);
          if (vp.GetValents().Contains(dobject))
          {
            foreach (Valent nextValent in vp.GetValents())
            {
              if (nextValent.tag == Valent.Tag.Subj)
              {
                answers.Add(nextValent.entity);
              }
            }
          }
        } 
      }
      return answers;
    }

    private bool GetVerb(string verb, out Verb graphVerb)
    {
      bool found = false;
      Verb verbAssociation = new Verb();
      foreach (string verbName in associations.VerbNames())
      {
        if (thesaurus.Describes(verb, verbName, true))
        {
          found = found || associations.TryGetVerb(verbName, out verbAssociation);
        }
      }
      graphVerb = verbAssociation;
      return found;
    }

    private bool GetEntity(string noun, out Entity graphEntity)
    {
      bool found = false;
      Entity entityAssociation = new Entity();
      foreach (string entityName in associations.EntityNames())
      {
        if (thesaurus.Describes(noun, entityName, true))
        {
          found = found || associations.TryGetEntity(entityName, out entityAssociation);
        }
      }
      graphEntity = entityAssociation;
      return found;
    }

    private string GenerateResponse(string noun, string verb, List<Entity> answers)
    {
      var response = "The " + noun + " " + verb;
      foreach (Entity entityAnswer in answers)
      {
        if (answers.IndexOf(entityAnswer) == 0)
        {
          string entityName;
          associations.TryNameEntity(entityAnswer, out entityName);
          response += " the " + entityName;
        }
        else
        {
          string entityName;
          associations.TryNameEntity(entityAnswer, out entityName);
          response += " and the " + entityName;
        }
      }
      return response;
    }
  }

}
