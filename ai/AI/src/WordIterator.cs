using System.Collections;

namespace GameAI
{
  /// <summary>
  /// For iterating over words in the input.
  /// </summary>
  /// <remarks>
  /// Splits the incoming string into words which can then be iterated over.
  /// </remarks>
  public class WordIterator : IEnumerable
  {
  private readonly string[] words;

    /// <summary>
    /// Create a new WordIterator from some input string.
    /// </summary>
    /// <param name="inputString"> The input string. </param>
    public WordIterator(string inputString)
	{
      words = inputString.Split(new char[] { ' ' }, System.StringSplitOptions.RemoveEmptyEntries);
	}

    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
	}

	public IEnumerator GetEnumerator( )
	{
	  foreach (var word in words)
	  {
	    yield return word;
	  }
	}
  }
}
