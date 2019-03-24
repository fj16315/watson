using OpenNLP.Tools.Parser;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WatsonAI
{
  public struct Result<T>
  {
    public T Value { get; }
    public bool HasValue { get; }

    public Result(T value)
    {
      HasValue = true;
      Value = value;
    }

    public static Result<T> operator |(Result<T> lhs, Result<T> rhs)
      => lhs.HasValue ? lhs : rhs;

    public static bool operator ==(Result<T> lhs, Result<T> rhs)
    {
      if (lhs.HasValue && rhs.HasValue)
      {
        return lhs.Value.Equals(rhs.Value);
      }
      return !lhs.HasValue && !rhs.HasValue;
    }

    public static bool operator !=(Result<T> lhs, Result<T> rhs)
      => !(lhs == rhs);

    public override bool Equals(object obj)
    {
      if (obj is Result<T>)
      {
        return this == (Result<T>)obj;
      }
      return false;
    }

    public override int GetHashCode()
      => HasValue ? Value.GetHashCode() : HasValue.GetHashCode();
  }
}
