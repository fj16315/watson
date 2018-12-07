using System;
using GameAI;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AITests
{
  [TestClass]
  public class WordIteratorTests
  {
    [TestMethod]
    public void IterateOnEmpty()
    {
            var empty = new WordIterator("").GetEnumerator();
      Assert.IsFalse(empty.MoveNext());
    }

    [TestMethod]
    public void IterateOneWord()
    {
      var banana = new WordIterator("banana").GetEnumerator();
      Assert.IsTrue(banana.MoveNext());
      Assert.AreEqual(banana.Current, "banana");
      Assert.IsFalse(banana.MoveNext());
    }
  }
}
