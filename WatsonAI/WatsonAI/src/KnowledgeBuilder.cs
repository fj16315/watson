using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace WatsonAI
{
  /// <summary>
  /// Builds a new Knowledge object and its Associations
  /// </summary>
  public class KnowledgeBuilder : IEnumerable<VerbPhrase>
  {
    public Knowledge Knowledge { get; }
    public Associations Associations { get; }

    /// <summary>
    /// Begins construction of a new instance of
    /// <see cref="WatsonAI.Knowledge"/> with <paramref name="entities"/> and
    /// <paramref name="verbs"/>.
    /// </summary>
    public KnowledgeBuilder(EntityBuilder entities, VerbBuilder verbs)
    {
      Knowledge = new Knowledge();
      Associations = new Associations();

      Debug.WriteLineIf(!AddEntities(entities), "Failed to add entities!");
      Debug.WriteLineIf(!AddVerbs(verbs), "Failed to add verbs!");
    }

    /// <summary>
    /// Takes an <see cref="IEnumerable"/> of entity names, and generates a new
    /// <see cref="WatsonAI.Entity"/> in <see cref="Associations"/> for each.
    /// </summary>
    public bool AddEntities<Names>(Names names) where Names : IEnumerable<string>
    {
      return names
        .Select((e, i) => Associations.AddEntityName(new Entity((uint)i), e))
        .All(b => b);
    }

    /// <summary>
    /// Takes an <see cref="IEnumerable"/> of verb names, and generates a new
    /// <see cref="WatsonAI.Verb"/> in <see cref="Associations"/> for each.
    /// </summary>
    public bool AddVerbs<Names>(Names names) where Names : IEnumerable<string>
    {
      return names
        .Select((v, i) => Associations.AddVerbName(new Verb((uint)i), v))
        .All(b => b);
    }

    /// <summary>
    /// Adds a new <see cref="WatsonAI.Valent"/> to
    /// <see cref="Knowledge"/> constructed from the parameters.
    /// </summary>
    public void Add(string subj_s, string verb_s, params Object[] objs_s)
    {
      var subj = Valent.Subj(Associations.UncheckedGetEntity(subj_s));

      var valents = objs_s.Select(obj =>
      {
        var entity = Associations.UncheckedGetEntity(obj.Name);
        if (obj.IsDirect)
        {
          return Valent.Dobj(entity);
        }
        return Valent.Iobj(new Prep(), entity);
      })
      .Prepend(subj);

      var verb = Associations.UncheckedGetVerb(verb_s);

      var vp = new VerbPhrase(verb, valents.ToList());
      Knowledge.AddVerbPhrase(vp);
    }

    public void Add(string subj_s, string verb_s, string dobj_s)
    {
      var verb = Associations.UncheckedGetVerb(verb_s);
      var valents = new List<Valent>
      {
        Valent.Subj(Associations.UncheckedGetEntity(subj_s)),
        Valent.Dobj(Associations.UncheckedGetEntity(dobj_s))
      };

      Knowledge.AddVerbPhrase(
        new VerbPhrase(verb, valents)
      );
    }

    /// <remarks>
    /// Here to allow collection initialisation
    /// </remarks>
    IEnumerator<VerbPhrase> IEnumerable<VerbPhrase>.GetEnumerator()
      => Knowledge.GetVerbPhrases().GetEnumerator();

    /// <remarks>
    /// Here to allow collection initialisation
    /// </remarks>
    IEnumerator IEnumerable.GetEnumerator()
      => Knowledge.GetVerbPhrases().GetEnumerator();
  }

  public class EntityBuilder : IEnumerable<string>
  {
    public HashSet<string> Names { get; }

    public EntityBuilder()
    {
      Names = new HashSet<string>();
    }

    public bool Add(string entityName)
    {
      return Names.Add(entityName);
    }

    IEnumerator<string> IEnumerable<string>.GetEnumerator()
      => Names.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
      => Names.GetEnumerator();
  }

  public class VerbBuilder : IEnumerable<string>
  {
    public HashSet<string> Names { get; }

    public VerbBuilder()
    {
      Names = new HashSet<string>();
    }

    public bool Add(string verbName)
    {
      return Names.Add(verbName);
    }

    IEnumerator<string> IEnumerable<string>.GetEnumerator()
      => Names.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
      => Names.GetEnumerator();
  }

  public struct Object
  {
    public string Prep { get; }
    public string Name { get; }

    public bool HasPrep
    {
      get
      {
        return Prep != null;
      }
    }

    public bool IsDirect
    {
      get
      {
        return !HasPrep;
      }
    }

    public bool IsIndirect
    {
      get
      {
        return HasPrep;
      }
    }

    private Object(string prep, string name)
    {
      Name = name;
      Prep = prep;
    }

    public static Object Direct(string name)
      => new Object(null, name);

    public static Object Indirect(string prep, string name)
      => new Object(prep, name);
  }
}
