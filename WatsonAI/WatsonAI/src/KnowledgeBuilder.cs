using System;
using System.Collections.Generic;
using System.Text;

namespace WatsonAI
{
  public class KnowledgeBuilder
  {
    private readonly Knowledge knowledge;
    private readonly Associations associations;

    public KnowledgeBuilder()
    {
      knowledge = new Knowledge();
      associations = new Associations();
    }

    public void AddEntities(List<string> entities)
    {
      for (int i = 0; i < entities.Count; i++)
      {
        associations.AddEntityName(new Entity((uint)i), entities[i]);
      }
    }

    public void AddVerbs(List<string> verbs)
    {
      for (int i = 0; i < verbs.Count; i++)
      {
        associations.AddVerbName(new Verb((uint)i), verbs[i]);
      }
    }

    public bool AddPhrase(string verb, List<Tuple<Valent.Tag, string>> valents)
    {
      Verb knowledgeVerb;
      if (!associations.TryGetVerb(verb, out knowledgeVerb))
      {
        return false;
      }
      List<Valent> knowledgeValents = new List<Valent>();
      foreach (var v in valents)
      {
        Entity knowledgeEntity;
        if (!associations.TryGetEntity(v.Item2, out knowledgeEntity))
        {
          return false;
        }
        knowledgeValents.Add(Valent.WithTag(v.Item1, knowledgeEntity));
      }
      var verbPhrase = new VerbPhrase(knowledgeVerb, knowledgeValents);
      knowledge.AddVerbPhrase(verbPhrase);
      return true;
    }

    public bool AddSimplePhrase(string subj, string verb, string dobj)
    {
      var valents = new List<Tuple<Valent.Tag, string>>();
      valents.Add(Tuple.Create(Valent.Tag.Subj, subj));
      valents.Add(Tuple.Create(Valent.Tag.Dobj, dobj));
      return AddPhrase(verb, valents);
    }

    public Associations Associations() 
      => associations;

    public Knowledge Knowledge()
      => knowledge;
  }
}
