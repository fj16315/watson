using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GameAI;

namespace AITests
{
  using Flags = Relationships.Flags;

  [TestClass]
  public class RelationshipTests
  {
    [TestMethod]
    public void IsNoneTrue()
    {
      var none = new Relationships();
      Assert.IsTrue(none.IsNone());
    }

    [TestMethod]
    public void IsNoneFalse()
    {
      var not_none = new Relationships(Flags.Contains);
      Assert.IsFalse(not_none.IsNone());
    }

    [TestMethod]
    public void ContainsTrue()
    {
      var larger = new Relationships(Flags.Contains | Flags.Owns);
      var smaller = new Relationships(Flags.Owns);
      Assert.IsTrue(larger.Contains(smaller));
    }

    [TestMethod]
    public void ContainsFalse()
    {
      var larger = new Relationships(Flags.Contains | Flags.Owns);
      var smaller = new Relationships(Flags.Wants);
      Assert.IsFalse(larger.Contains(smaller));
    }

    [TestMethod]
    public void EqualNone()
    {
      var lhs = new Relationships();
      var rhs = new Relationships(Flags.None);
      Assert.AreEqual(lhs, rhs);
    }

    [TestMethod]
    public void EqualAll()
    {
      var lhs = new Relationships(Flags.Contains | Flags.Owns | Flags.Wants);
      var rhs = new Relationships(Flags.Contains | Flags.Owns | Flags.Wants);
      Assert.AreEqual(lhs, rhs);
    }

    [TestMethod]
    public void NotEqualSingle()
    {
      var lhs = new Relationships(Flags.Contains);
      var rhs = new Relationships(Flags.Wants);
      Assert.AreNotEqual(lhs, rhs);
    }

    [TestMethod]
    public void NotEqualsOverlap()
    {
      var lhs = new Relationships(Flags.Owns | Flags.Wants);
      var rhs = new Relationships(Flags.Contains | Flags.Wants);
      Assert.AreNotEqual(lhs, rhs);
    }

    [TestMethod]
    public void NotEqualContains()
    {
      var lhs = new Relationships(Flags.Contains | Flags.Owns | Flags.Wants);
      var rhs = new Relationships(Flags.Contains | Flags.Owns);
      Assert.IsTrue(lhs.Contains(rhs));
      Assert.AreNotEqual(lhs, rhs);
    }

    [TestMethod]
    public void NotEqualsAllNone()
    {
      var lhs = new Relationships(Flags.Contains | Flags.Owns | Flags.Wants);
      var rhs = new Relationships();
      Assert.AreNotEqual(lhs, rhs);
    }

    [TestMethod]
    public void AndDisjoint()
    {
      var lhs = new Relationships(Flags.Owns | Flags.Wants);
      var rhs = new Relationships(Flags.Contains);
      var result = new Relationships();
      Assert.AreEqual(lhs & rhs, result);
    }

    [TestMethod]
    public void AndOverlap()
    {
      var lhs = new Relationships(Flags.Owns | Flags.Wants);
      var rhs = new Relationships(Flags.Contains | Flags.Wants);
      var result = new Relationships(Flags.Wants);
      Assert.AreEqual(lhs & rhs, result);
    }

    [TestMethod]
    public void AndNone()
    {
      var rel = new Relationships(Flags.Owns | Flags.Wants);
      var none = new Relationships();
      Assert.AreEqual(rel & none, none);
    }

    [TestMethod]
    public void AndSelf()
    {
      var rel = new Relationships(Flags.Wants);
      Assert.AreEqual(rel & rel, rel);
    }

    [TestMethod]
    public void OrDisjoint()
    {
      var lhs = new Relationships(Flags.Wants);
      var rhs = new Relationships(Flags.Owns);
      var result = new Relationships(Flags.Wants | Flags.Owns);
      Assert.AreEqual(lhs | rhs, result);
    }

    [TestMethod]
    public void OrOverlap()
    {
      var lhs = new Relationships(Flags.Wants | Flags.Owns);
      var rhs = new Relationships(Flags.Wants | Flags.Contains);
      var result = new Relationships(Flags.Contains | Flags.Owns | Flags.Wants);
      Assert.AreEqual(lhs | rhs, result);
    }

    [TestMethod]
    public void OrNone()
    {
      var rel = new Relationships(Flags.Owns);
      var none = new Relationships();
      Assert.AreEqual(rel | none, rel);
    }

    [TestMethod]
    public void OrSelf()
    {
      var rel = new Relationships(Flags.Wants);
      Assert.AreEqual(rel | rel, rel);
    }
  }
}
