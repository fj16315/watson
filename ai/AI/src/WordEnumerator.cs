using System;
using System.Collections;
using System.Collections.Generic;

public class WordEnumerator : IEnumerator<string>
{
    private string[] words;
    private int currentIndex;
    private string _current;

	public WordEnumerator(string inputString)
	{
        words = inputString.Split(new char[] { ' ' }, System.StringSplitOptions.RemoveEmptyEntries);
        currentIndex = -1;
	}

    public string Current
    {
        get
        {
            if (words == null || _current == null)
            {
                throw new InvalidOperationException();
            }
            return _current;
        }
    }

    private object Current1
    {
        get
        {
            return this.Current;
        }
    }

    object IEnumerator.Current
    {
        get
        {
            return Current1;
        }
    }

    public bool MoveNext()
    {
        currentIndex++;
        if (currentIndex >= words.Length)
        {
            return false;
        }
        else
        {
            _current = words[currentIndex];
        }
        return true;
    }

    public void Reset()
    {
        currentIndex = -1;
        _current = null;
    }

    void IDisposable.Dispose()
    {

    }
}
