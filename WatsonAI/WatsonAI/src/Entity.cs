namespace WatsonAI
{
  /// <summary>
  /// Wrapper type around <see cref="int"/> for safety.
  /// </summary>
  public struct Entity
  {
    private readonly uint n;

    /// <summary>
    /// Initializes a new instance of the <see cref="WatsonAI.Entity"/> struct.
    /// </summary>
    /// <param name="n">The <see cref="uint"/> to wrap.</param>
    public Entity(uint n) 
    {
      this.n = n;
    }

    /// <summary>
    /// Returns a copy of the wrapped <see cref="int"/>.
    /// </summary>
    /// <returns>The wrapped <see cref="int"/>.</returns>
    /// <param name="e">The wrapping <see cref="GameAI.Entity"/>.</param>
    public static explicit operator uint(Entity e)
      => e.n;

    public static bool operator ==(Entity l, Entity r)
      => l.n == r.n;

    public static bool operator !=(Entity l, Entity r)
      => !(l == r);

    public static bool operator ==(Entity e, uint i)
      => e.n == i;

    public static bool operator !=(Entity e, uint i)
      => !(e == i);

    public static bool operator ==(uint i, Entity e)
      => e == i;

    public static bool operator !=(uint i, Entity e)
      => e != i;

    public override bool Equals(object obj)
    {
      if (obj == null)
      {
        throw new System.ArgumentNullException();
      }

      if (obj is Entity)
      {
        return this == (Entity)obj;
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
      => $"Entity{{{n}}}";
  }
}
