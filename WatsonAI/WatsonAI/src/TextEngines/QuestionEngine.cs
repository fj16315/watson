using System;
using System.Collections.Generic;
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
      var parse = this.parser.Parse(io.remainingInput).GetChildren()[0];
      var nounPhrase = parse;
      var verbPhrase = parse;
      var noun = parse;
      var verb = parse;

      if (parse.Type.Equals("SBARQ"))
      {
        io.output = "This has a question: ";
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
        io.output = io.output + "Verb = " + verb.Show() + ", Noun = " + noun.Show();
        io.output = io.output + Query(wh.Show(), noun.Show(), verb.Show());
      }

      return io;
    }

    public string Query(string wh, string noun, string verb)
    {
      Verb graphVerb;
      Entity graphEntity;
      if (!GetVerb(verb, out graphVerb) || !GetEntity(noun, out graphEntity))
      {
        return "";
      }

      var answers = new List<Entity>();
      foreach (VerbPhrase vp in kg.GetVerbPhrases())
      {
        if (vp.verb.Equals(graphVerb))
        {
          foreach (Valent valent in vp.GetValents())
          {
            if (valent.entity.Equals(graphEntity) && valent.tag == Valent.Tag.Subj)
            {
              foreach (Valent nextValent in vp.GetValents())
              {
                if (nextValent.tag == Valent.Tag.Dobj)
                {
                  answers.Add(nextValent.entity);
                }
              }
            }
          }
        } 
      }
      return GenerateResponse(noun, verb, answers);
    }

    private bool GetVerb(string verb, out Verb graphVerb)
    {
      bool found = false;
      Verb verbAssociation = new Verb();
      foreach (string verbKey in associations.VerbNames())
      {
        if (thesaurus.Describes(verb, verbKey))
        {
          found = found || associations.TryGetVerb(verbKey, out verbAssociation);
        }
      }
      graphVerb = verbAssociation;
      return found;
    }

    private bool GetEntity(string noun, out Entity graphEntity)
    {
      bool found = false;
      Entity entityAssociation = new Entity();
      foreach (string nounKey in associations.EntityNames())
      {
        if (thesaurus.Describes(noun, nounKey))
        {
          found = found || associations.TryGetEntity(nounKey, out entityAssociation);
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
        if (entityAnswer.Equals(answers[0]))
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
