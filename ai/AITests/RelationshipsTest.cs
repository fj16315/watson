using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GameAI;

//namespace AITests
//{
//  [TestClass]
//  public class RelationshipTests
//  {
//    [TestMethod]
//    public void IsNoneTrue()
//    {
//      var none = new Relation();
//      Assert.IsTrue(none.IsNone());
//    }

//    [TestMethod]
//    public void IsNoneFalse()
//    {
//      var not_none = new Relation(Flags.Contains);
//      Assert.IsFalse(not_none.IsNone());
//    }

//    [TestMethod]
//    public void ContainsTrue()
//    {
//      var larger = new Relation(Flags.Contains | Flags.Owns);
//      var smaller = new Relation(Flags.Owns);
//      Assert.IsTrue(larger.Contains(smaller));
//    }

//    [TestMethod]
//    public void ContainsFalse()
//    {
//      var larger = new Relation(Flags.Contains | Flags.Owns);
//      var smaller = new Relation(Flags.Wants);
//      Assert.IsFalse(larger.Contains(smaller));
//    }

//    [TestMethod]
//    public void EqualNone()
//    {
//      var lhs = new Relation();
//      var rhs = new Relation(Flags.None);
//      Assert.AreEqual(lhs, rhs);
//    }

//    [TestMethod]
//    public void EqualAll()
//    {
//      var lhs = new Relation(Flags.Contains | Flags.Owns | Flags.Wants);
//      var rhs = new Relation(Flags.Contains | Flags.Owns | Flags.Wants);
//      Assert.AreEqual(lhs, rhs);
//    }

//    [TestMethod]
//    public void NotEqualSingle()
//    {
//      var lhs = new Relation(Flags.Contains);
//      var rhs = new Relation(Flags.Wants);
//      Assert.AreNotEqual(lhs, rhs);
//    }

//    [TestMethod]
//    public void NotEqualsOverlap()
//    {
//      var lhs = new Relation(Flags.Owns | Flags.Wants);
//      var rhs = new Relation(Flags.Contains | Flags.Wants);
//      Assert.AreNotEqual(lhs, rhs);
//    }

//    [TestMethod]
//    public void NotEqualContains()
//    {
//      var lhs = new Relation(Flags.Contains | Flags.Owns | Flags.Wants);
//      var rhs = new Relation(Flags.Contains | Flags.Owns);
//      Assert.IsTrue(lhs.Contains(rhs));
//      Assert.AreNotEqual(lhs, rhs);
//    }

//    [TestMethod]
//    public void NotEqualsAllNone()
//    {
//      var lhs = new Relation(Flags.Contains | Flags.Owns | Flags.Wants);
//      var rhs = new Relation();
//      Assert.AreNotEqual(lhs, rhs);
//    }

//    [TestMethod]
//    public void AndDisjoint()
//    {
//      var lhs = new Relation(Flags.Owns | Flags.Wants);
//      var rhs = new Relation(Flags.Contains);
//      var result = new Relation();
//      Assert.AreEqual(lhs & rhs, result);
//    }

//    [TestMethod]
//    public void AndOverlap()
//    {
//      var lhs = new Relation(Flags.Owns | Flags.Wants);
//      var rhs = new Relation(Flags.Contains | Flags.Wants);
//      var result = new Relation(Flags.Wants);
//      Assert.AreEqual(lhs & rhs, result);
//    }

//    [TestMethod]
//    public void AndNone()
//    {
//      var rel = new Relation(Flags.Owns | Flags.Wants);
//      var none = new Relation();
//      Assert.AreEqual(rel & none, none);
//    }

//    [TestMethod]
//    public void AndSelf()
//    {
//      var rel = new Relation(Flags.Wants);
//      Assert.AreEqual(rel & rel, rel);
//    }

//    [TestMethod]
//    public void OrDisjoint()
//    {
//      var lhs = new Relation(Flags.Wants);
//      var rhs = new Relation(Flags.Owns);
//      var result = new Relation(Flags.Wants | Flags.Owns);
//      Assert.AreEqual(lhs | rhs, result);
//    }

//    [TestMethod]
//    public void OrOverlap()
//    {
//      var lhs = new Relation(Flags.Wants | Flags.Owns);
//      var rhs = new Relation(Flags.Wants | Flags.Contains);
//      var result = new Relation(Flags.Contains | Flags.Owns | Flags.Wants);
//      Assert.AreEqual(lhs | rhs, result);
//    }

//    [TestMethod]
//    public void OrNone()
//    {
//      var rel = new Relation(Flags.Owns);
//      var none = new Relation();
//      Assert.AreEqual(rel | none, rel);
//    }

//    [TestMethod]
//    public void OrSelf()
//    {
//      var rel = new Relation(Flags.Wants);
//      Assert.AreEqual(rel | rel, rel);
//    }
//  }
//}
