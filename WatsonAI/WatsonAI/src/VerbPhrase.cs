using System.Collections.Generic;

namespace WatsonAI
{
  /// <summary>
  /// Represents a verb and its subjects and objects.
  /// </summary>
  /// <remarks>
  /// A piece of knowledge is represented by stating a fact using this type.
  /// </remarks>
  public class VerbPhrase
  {
    public Verb verb { get; }
    private readonly HashSet<ITag> tags;
    private readonly List<Valent> valents;

    public VerbPhrase(Verb verb)
    {
      this.verb = verb;
      this.tags = new HashSet<ITag>();
      this.valents = new List<Valent>();
    }

    public VerbPhrase(Verb verb, List<Valent> valents)
    {
      this.verb = verb;
      this.tags = new HashSet<ITag>();
      this.valents = valents;
    }

    public IEnumerable<Valent> GetValents()
      => valents;

    public void AddValent(Valent valent)
    {
      valents.Add(valent);
    }

    public bool RemoveValent(Valent valent)
    {
      return valents.Remove(valent);
    }

    public override string ToString()
    {
      var tagString = string.Join(", ", tags);
      var valentString = string.Join(", ", valents);
      return $"VerbPhrase{{{verb}, [{tagString}], [{valentString}]}}";
    }

    public override bool Equals(object obj)
    {
      var phrase = obj as VerbPhrase;
      return phrase != null &&
             verb.Equals(phrase.verb) &&
             tags.Equals(phrase.tags) &&
             valents.Equals(phrase.valents);
    }
  }

  /// <summary>
  /// Represents a subject or object in a clause.
  /// </summary>
  /// <remarks>
  /// All valents contain an <see cref="WatsonAI.Entity"/> and indirect objects
  /// also contain a preposition (<see cref="WatsonAI.Prep"/>).
  /// 
  /// The name <see cref="WatsonAI.Valent"/> comes from the valency of a verb
  /// but is not an actual word.
  /// </remarks>
  public struct Valent
  {
    public enum Tag { Subj, Dobj, Iobj };

    public Tag tag { get; }

    public Prep? prep { get; }
    public Entity entity { get; }

    private Valent(Tag tag, Prep? prep, Entity entity)
    {
      this.tag = tag;
      this.prep = prep;
      this.entity = entity;
    }

    public static Valent WithTag(Tag tag, Entity entity)
      => new Valent(tag, null, entity);

    public static Valent Subj(Entity entity)
      => new Valent(Tag.Subj, null, entity);

    public static Valent Dobj(Entity entity)
      => new Valent(Tag.Dobj, null, entity);

    public static Valent Iobj(Prep prep, Entity entity)
      => new Valent(Tag.Iobj, prep, entity);

    public override string ToString()
    {
      switch (tag)
      {
        case Tag.Subj:
          return $"Subj({entity})";
        case Tag.Dobj:
          return $"Dobj({entity})";
        default:
          return $"Iobj({prep}, {entity})";
      }
    }

    public override bool Equals(object obj)
    {
      if (!(obj is Valent))
      {
        return false;
      }

      var valent = (Valent)obj;
      return tag == valent.tag &&
             prep.Equals(valent.prep) &&
             entity.Equals(valent.entity);
    }
  }

  public struct Prep
  {
    // TODO: Decide on representation for this.
    //       The options currently are:
    //        - A hard-coded enum
    //        - An int/uint
    //        - A string
    public override bool Equals(object obj)
    {
      return true;
    }
  }

  public interface ITag
  {
    // TODO: Decide on the contents of the interface.
    //       The common functionality of these tags is not yet clear.
  }

  public struct Verb
  {
    private readonly uint n;

    public Verb(uint n)
    {
      this.n = n;
    }

    public static explicit operator uint(Verb v)
      => v.n;

    public static bool operator ==(Verb l, Verb r)
      => l.n == r.n;

    public static bool operator !=(Verb l, Verb r)
      => !(l == r);

    public static bool operator ==(Verb v, uint i)
      => v.n == i;

    public static bool operator !=(Verb v, uint i)
      => !(v == i);

    public static bool operator ==(uint i, Verb v)
      => v == i;

    public static bool operator !=(uint i, Verb v)
      => v != i;

    public override bool Equals(object obj)
    {
      if (obj == null)
      {
        throw new System.ArgumentNullException();
      }

      if (obj is Verb)
      {
        return this == (Verb)obj;
      }

      if (obj is int)
      {
        return this == (uint)obj;
      }
      return false;
    }

    public override int GetHashCode()
      => n.GetHashCode();

    public override string ToString()
      => $"Verb{{{n}}}";
  }
}

