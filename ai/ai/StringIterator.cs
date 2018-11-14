using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace game_ai
{
    public class StringIterator : IEnumerable
    {
	    private string[] _inputString;

	    public StringIterator(string inputString)
	    {
		    _inputString = inputString.Split(' ');
	    }

	    IEnumerator IEnumerable.GetEnumerator()
	    {
		    return GetEnumerator();
	    }

	    public IEnumerator GetEnumerator( )
	    {
		    foreach (string word in _inputString)
		    {
			    yield return word;
		    }
	    }

        static void Main()
        {

        }
    }
}
