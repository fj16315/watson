using System;

namespace GameAI
{
  public struct Relationships
  {
    [Flags]
    public enum RelFlag : int
    {
      None = 0, Contains = 1,
      Owns = 2, Wants = 4
    };

    public RelFlag rel_flag {
      get;
      set;
    };
  }
}
