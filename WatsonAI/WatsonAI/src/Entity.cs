using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace WatsonAI
{
  /// <summary>
  /// Wrapper type around <see cref="int"/> for safety.
  /// </summary>
  public struct Entity
  {
    // This is an int because it is often used as an index and C# uses
    // ints, rather than uints, for indexing.
    private readonly int n;

    /// <summary>
    /// Initializes a new instance of the <see cref="WatsonAI.Entity"/> struct.
    /// </summary>
    /// <param name="n">The <see cref="int"/> to wrap.</param>
    public Entity(int n) 
    {
      this.n = n;
    }

    /// <summary>
    /// Returns a copy of the wrapped <see cref="int"/>.
    /// </summary>
    /// <returns>The wrapped <see cref="int"/>.</returns>
    /// <param name="e">The wrapping <see cref="GameAI.Entity"/>.</param>
    public static explicit operator int(Entity e)
      => e.n;

    public static bool operator ==(Entity l, Entity r)
      => l.n == r.n;

    public static bool operator !=(Entity l, Entity r)
      => !(l == r);

    public static bool operator ==(Entity e, int i)
      => e.n == i;

    public static bool operator !=(Entity e, int i)
      => !(e == i);

    public static bool operator ==(int i, Entity e)
      => e == i;

    public static bool operator !=(int i, Entity e)
      => e != i;

    public override bool Equals(object obj)
    {
      Contract.Requires(obj != null);

      if (obj is Entity)
      {
        return this == (Entity)obj;
      }
      if (obj is int)
      {
        return this == (int)obj;
      }
      return false;
    }

    public override int GetHashCode()
      => n.GetHashCode();
  }
}
