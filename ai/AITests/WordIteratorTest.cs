using System;
using GameAI;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AITests
{
  [TestClass]
  public class WordIteratorTests
  {
    [TestMethod]
    public void IterateOneWord()
    {
      var banana = new WordIterator("banana");
    }
  }
}
