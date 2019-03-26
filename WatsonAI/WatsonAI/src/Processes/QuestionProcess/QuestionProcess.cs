using System;
using System.Collections.Generic;
using System.Linq;

namespace WatsonAI
{
  public class QuestionProcess : IProcess
  {
    private Parser parser;
    private Knowledge kg;
    private Thesaurus thesaurus;
    private Associations associations;

    /// <summary>
    /// Text engine for debuging the specified Parser.
    /// </summary>
    /// <param name="parse">The parser to use.</param>
    public QuestionProcess(Parser parse, Knowledge kg, Thesaurus thesaurus, Associations associations)
    {
      this.parser = parse;
      this.kg = kg;
      this.thesaurus = thesaurus;
      this.associations = associations;
    }

    public Stream Process(Stream stream)
    {
      PrintVerbs(stream);
      PrintEntities(stream);

      var tree = parser.Parse(stream.RemainingInput);

      var noun = new EntityName(associations, thesaurus);
      var verb = new VerbName(associations, thesaurus);
      var nounPhrase = new Flatten<Entity>(new Descendant<IEnumerable<Entity>>(new Branch("NP"), noun));
      var verbPhrase = new Flatten<Verb>(new Descendant<IEnumerable<Verb>>(new Branch("VP"), verb));

      var top = new Branch("TOP");
      var queryN = new Flatten<Entity>(new Descendant<IEnumerable<Entity>>(top, nounPhrase));
      var queryV = new Flatten<Verb>(new Descendant<IEnumerable<Verb>>(top, verbPhrase));


      var pNs = queryN.Match(tree);
      var pVs = queryV.Match(tree);
      if (pNs.HasValue && pVs.HasValue)
      {
        var ns = pNs.Value;
        var vs = pVs.Value;

        var pairs = from n in ns
                    from v in vs
                    select Tuple.Create(n, v);

        foreach (var p in pairs.Distinct())
        {
          var e = p.Item1;
          var v = p.Item2;
          var answers = GetSubjAnswers(v, e);
          string verbName;
          associations.TryNameVerb(v, out verbName);
          string entityName;
          associations.TryNameEntity(e, out entityName);
          if (answers.Count != 0)
          {
            stream.AppendOutput(GenerateResponse(entityName, verbName, answers));
          }
        }
      }
      return stream;
    }

    private void PrintVerbs(Stream stream)
    {
      var tree = this.parser.Parse(stream.RemainingInput);

      var verb = new VerbName(associations, thesaurus);
      var top = new Branch("TOP");

      var query = new Descendant<IEnumerable<Verb>>(top, verb);

      var verbs = query.Match(tree);

      Console.WriteLine("Verbs: ");
      if (verbs.HasValue)
      {
        foreach (var v in verbs.Value.SelectMany(x => x).Distinct())
        {
          string verbName;
          associations.TryNameVerb(v, out verbName);
          Console.WriteLine($"{v} {verbName}");
        }
      }
    }
    
    private void PrintEntities(Stream stream)
    {
      var tree = this.parser.Parse(stream.RemainingInput);

      var entity = new EntityName(associations, thesaurus);
      var top = new Branch("TOP");

      var query = new Flatten<Entity>(new Descendant<IEnumerable<Entity>>(top, entity));

      var entities = query.Match(tree);

      Console.WriteLine("Entities: ");
      if (entities.HasValue)
      {
        foreach (var e in entities.Value.Distinct())
        {
          string entityName;
          associations.TryNameEntity(e, out entityName);
          Console.WriteLine($"{e} {entityName}");
        }
      }
    }

    private List<Entity> GetSubjAnswers(Verb verb, Entity entity)
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

    private List<Entity> GetIobjAnswers(Verb verb, Entity entity)
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
              if (nextValent.tag == Valent.Tag.Iobj)
              {
                answers.Add(nextValent.entity);
              }
            }
          }
        }
      }
      return answers;
    }

    private bool GetBoolAnswer(Verb verb, Entity subjEntity, Entity dobjEntity)
    {
      foreach (VerbPhrase vp in kg.GetVerbPhrases())
      {
        if (vp.verb.Equals(verb))
        {
          var dobject = Valent.Dobj(dobjEntity);
          if (vp.GetValents().Contains(dobject))
          {
            foreach (Valent nextValent in vp.GetValents())
            {
              if (nextValent.tag == Valent.Tag.Subj && nextValent.entity.Equals(subjEntity))
              {
                return true;
              }
            }
          }
        }
      }
      return false;
    }

    private string GenerateResponse(string noun, string verb, List<Entity> answers)
    {
      var response = "The " + noun + " " + verb;
      foreach (Entity entityAnswer in answers.Distinct())
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
