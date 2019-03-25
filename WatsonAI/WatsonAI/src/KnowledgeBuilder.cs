using System;
using System.Collections.Generic;
using System.Text;

namespace WatsonAI
{
  /// <summary>
  /// Builds a new Knowledge object and its Associations
  /// </summary>
  public class KnowledgeBuilder
  {
    private readonly Knowledge knowledge;
    private readonly Associations associations;

    /// <summary>
    /// Constructs empty Knowledge and Associations
    /// </summary>
    public KnowledgeBuilder()
    {
      knowledge = new Knowledge();
      associations = new Associations();
    }

    /// <summary>
    /// Takes a list of entity names, and generates new Entities for each
    /// in the Associations
    /// </summary>
    /// <param name="entities"></param>
    public void AddEntities(List<string> entities)
    {
      for (int i = 0; i < entities.Count; i++)
      {
        associations.AddEntityName(new Entity((uint)i), entities[i]);
      }
    }

    /// <summary>
    /// Takes a list of verb names, and generates new Verbs for each
    /// in the Associations
    /// </summary>
    /// <param name="verbs"></param>
    public void AddVerbs(List<string> verbs)
    {
      for (int i = 0; i < verbs.Count; i++)
      {
        associations.AddVerbName(new Verb((uint)i), verbs[i]);
      }
    }

    /// <summary>
    /// Adds a new VerbPhrase to Knowledge given a verb and the entities associated with it
    /// </summary>
    /// <param name="verb"></param>
    /// <param name="valents"></param>
    /// <returns></returns>
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

    /// <summary>
    /// Adds a new VerbPhrase given a simple 3 word statement:
    /// "subj" -> "verb" -> "dobj"
    /// e.g. "cat kill earl"
    /// </summary>
    /// <param name="subj"></param>
    /// <param name="verb"></param>
    /// <param name="dobj"></param>
    /// <returns></returns>
    public bool AddSimplePhrase(string subj, string verb, string dobj)
    {
      var valents = new List<Tuple<Valent.Tag, string>>();
      valents.Add(Tuple.Create(Valent.Tag.Subj, subj));
      valents.Add(Tuple.Create(Valent.Tag.Dobj, dobj));
      return AddPhrase(verb, valents);
    }

    /// <summary>
    /// Gets Associations
    /// </summary>
    /// <returns></returns>
    public Associations Associations() 
      => associations;

    /// <summary>
    /// Gets Knowledge
    /// </summary>
    /// <returns></returns>
    public Knowledge Knowledge()
      => knowledge;
  }
}
