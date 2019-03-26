using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WatsonAI
{
  public class KnowledgeQuery
  {
    private Knowledge kg;

    public KnowledgeQuery(Knowledge kg)
    {
      this.kg = kg;
    }

    public List<Entity> GetDobjAnswers(Verb verb, Entity entity)
    {
      var answers = new List<Entity>();
      foreach (VerbPhrase vp in kg.GetVerbPhrases())
      {
        if (vp.verb.Equals(verb))
        {
          var dobject = Valent.Subj(entity);
          if (vp.GetValents().Contains(dobject))
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
      return answers;
    }

    public List<Entity> GetIobjAnswers(Verb verb, Entity entity)
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

    public List<Entity> GetSubjAnswers(Verb verb, Entity entity)
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

    public bool GetBoolAnswer(Verb verb, Entity subjEntity, Entity dobjEntity)
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
  }
}
