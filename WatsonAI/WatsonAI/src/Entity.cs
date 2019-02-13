using System;

namespace WatsonAI
{
  /// <summary>
  /// Wrapper type around <see cref="int"/> for safety.
  /// </summary>
  public struct Entity
  {
    private int _n;

    /// <summary>
    /// Initializes a new instance of the <see cref="GameAI.Entity"/> struct.
    /// </summary>
    /// <param name="n">The <see cref="int"/> to wrap.</param>
    public Entity(int n) 
    {
      _n = n;
    }

    /// <summary>
    /// Returns a copy of the wrapped <see cref="int"/>.
    /// </summary>
    /// <returns>The wrapped <see cref="int"/>.</returns>
    /// <param name="e">The wrapping <see cref="GameAI.Entity"/>.</param>
    public static explicit operator int(Entity e)
      => e._n;
  }
}
