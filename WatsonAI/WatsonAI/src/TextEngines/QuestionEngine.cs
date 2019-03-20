using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WatsonAI
{
  public class QuestionEngine : IRule
  {
    private Parser parser;
    private Character character;
    private Knowledge kg;
    private Thesaurus thesaurus;
    private Associations associations;

    /// <summary>
    /// Text engine for debuging the parser.
    /// </summary>
    public QuestionEngine(Character character, Knowledge kg)
    {
      this.parser = new Parser();
      this.character = character;
      this.kg = kg;
    }

    /// <summary>
    /// Text engine for debuging the specified Parser.
    /// </summary>
    /// <param name="parse">The parser to use.</param>
    public QuestionEngine(Parser parse, Character character, Knowledge kg, Thesaurus thesaurus, Associations associations)
    {
      this.parser = parse;
      this.character = character;
      this.kg = kg;
      this.thesaurus = thesaurus;
      this.associations = associations;
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
        var answers = Query(wh.Value, noun.Value, verb.Value);
        if (answers.Count != 0)
        {
          io.output += GenerateResponse(noun.Value, verb.Value, answers);
        }
      }

      return io;
    }

    public List<Entity> Query(string wh, string noun, string verb)
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
