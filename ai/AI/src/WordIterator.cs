using System.Collections;
using System.Collections.Generic;

namespace GameAI
{
  /// <summary>
  /// For iterating over words in the input.
  /// </summary>
  /// <remarks>
  /// Splits the incoming string into words which can then be iterated over.
  /// </remarks>
  public class WordIterator : IEnumerable<string>
  {
  //private readonly string[] words;
      private string _inputString;

    /// <summary>
    /// Create a new WordIterator from some input string.
    /// </summary>
    /// <param name="inputString"> The input string. </param>
    public WordIterator(string inputString)
	{
      //words = inputString.Split(new char[] { ' ' }, System.StringSplitOptions.RemoveEmptyEntries);
        _inputString = inputString;
	}

    public IEnumerator<string> GetEnumerator()
    {
        return new WordEnumerator(_inputString);
    }

    private IEnumerator GetEnumerator1()
    {
        return this.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator1();
	}

  }
}
