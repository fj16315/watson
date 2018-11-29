// ﻿using System;
using System.Collections;
// using System.Collections.Generic;
// using System.Linq;
// using System.Text;
// using System.Threading.Tasks;

namespace GameAI
{
    public class StringIterator : IEnumerable
    {
	    private string[] words;

	    public StringIterator(string inputString)
	    {
		    words = inputString.Split(' ');
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
