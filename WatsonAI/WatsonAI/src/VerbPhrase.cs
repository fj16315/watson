using System.Collections.Generic;

namespace WatsonAI
{
  public class VerbPhrase
  {
    // TODO: Make a wrapper class
    public uint verb { get; }
    private readonly HashSet<ITag> tags;
    private readonly List<Valent> valents;

    public IEnumerable<Valent> GetValents()
      => valents;
  }

  public struct Valent
  {
    public enum Tag { Subj, Dobj, Iobj };

    public Tag tag { get; }

    public Prep? prep { get; }
    public Entity entity { get; }
  }

  public struct Prep
  {
    // TODO: Decide on representation for this.
    //       The options currently are:
    //        - A hard-coded enum
    //        - An int/uint
    //        - A string
  }

  public interface ITag
  {
    // TODO: Decide on the contents of the interface.
    //       The common functionality of these tags is not yet clear.
  }
}

