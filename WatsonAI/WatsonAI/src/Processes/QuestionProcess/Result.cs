namespace WatsonAI
{
  /// <summary>
  /// This struct is essentially a Nullable or Maybe class.
  /// </summary>
  /// <remarks>
  /// It is necessary because we want to use a feature that requires a Nullable type,
  /// but one of the datatypes we use doesn't work as this. By making our own, we
  /// got around this. 
  /// </remarks>
  /// <typeparam name="T">Any type that Result wraps around.</typeparam>
  public struct Result<T>
  {
    /// <summary>
    /// Whether there is a value: false indicates failure.
    /// </summary>
    public bool HasValue { get; }

    /// <summary>
    /// The value, if it exists.
    /// </summary>
    public T Value { get; }

    /// <summary>
    /// Construct a Result type from a value.
    /// </summary>
    /// <param name="value">The value to be wrapped.</param>
    public Result(T value)
    {
      HasValue = true;
      Value = value;
    }

    /// <summary>
    /// The or operator. Value equal to the lhs if the result is true.
    /// </summary>
    /// <param name="lhs">The left hand side.</param>
    /// <param name="rhs">The right hand side.</param>
    /// <returns>The lhs if the lhs or both are true, else the rhs.</returns>
    public static Result<T> operator |(Result<T> lhs, Result<T> rhs)
      => lhs.HasValue ? lhs : rhs;

    /// <summary>
    /// Returns true if the first result is equal to the second result.
    /// </summary>
    /// <param name="lhs">The first result.</param>
    /// <param name="rhs">The second result.</param>
    /// <returns>True if the first result is equal to the second result.</returns>
    public static bool operator ==(Result<T> lhs, Result<T> rhs)
    {
      if (lhs.HasValue && rhs.HasValue)
      {
        return lhs.Value.Equals(rhs.Value);
      }
      return !lhs.HasValue && !rhs.HasValue;
    }

    /// <summary>
    /// Returns true if the first result is not equal to the second result.
    /// </summary>
    /// <param name="lhs">The first result.</param>
    /// <param name="rhs">The second result.</param>
    /// <returns>True if the first result is not equal to the second result.</returns>
    public static bool operator !=(Result<T> lhs, Result<T> rhs)
      => !(lhs == rhs);

    /// <summary>
    /// Checks whether the result is equal to the obect provided.
    /// </summary>
    /// <remarks>
    /// Results also support the == operator.
    /// </remarks>
    /// <param name="obj">The object to compare against.</param>
    /// <returns>True if the two objects are equal.</returns>
    public override bool Equals(object obj)
    {
      if (obj is Result<T>)
      {
        return this == (Result<T>)obj;
      }
      return false;
    }

    /// <summary>
    /// Returns the hash code of the Result.
    /// </summary>
    /// <returns>The hash code of the result.</returns>
    public override int GetHashCode()
      => HasValue ? Value.GetHashCode() : HasValue.GetHashCode();
  }
}
