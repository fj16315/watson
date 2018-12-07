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
  public class WordIterator: IEnumerable<string>
  {
      private string _inputString;

    /// <summary>
    /// Create a new WordIterator from some input string.
    /// </summary>
    /// <param name="inputString"> The input string. </param>
    public WordIterator(string inputString)
	{
        _inputString = inputString;
	}

    public IEnumerator<string> GetEnumerator()
    {
            string[] words = _inputString.Split(new char[] { ' ' }, System.StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < words.Length; i++)
            {
                yield return words[i];
            }
        }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
	}

  }
}
