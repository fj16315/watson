using System;
using FsCheck;
using WatsonAI;

namespace WatsonTest
{
  public class Generators
  {
    public static Arbitrary<Entity> Entity()
      => Arb.From(
           from n in Arb.Generate<uint>()
           select new Entity(n)
         );

    public static Arbitrary<Knowledge> Knowledge()
      => Arb.From(
           from vps in Arb.Generate<VerbPhrase[]>()
           select ((Func<Knowledge>)(() =>
           {
             var knowledge = new Knowledge();
             foreach (var vp in vps)
             {
               knowledge.AddVerbPhrase(vp);
             }
             return knowledge;
           }))()
         );

    public static Arbitrary<Prep> Prep()
      => Arb.From(Gen.Constant(new Prep()));

    public static Arbitrary<Valent> Valent()
      => Arb.From(
           from n in Gen.Choose(0, 2)
           from e in Arb.Generate<Entity>()
           from p in Arb.Generate<Prep>()
           select ((Func<Valent>)(() =>
           {
             switch (n)
             {
               case 0:
                 return WatsonAI.Valent.Subj(e);
               case 1:
                 return WatsonAI.Valent.Dobj(e);
               default:
                 return WatsonAI.Valent.Iobj(p, e);
             }
           }))()
         );

    public static Arbitrary<Result<T>> Result<T>()
      => Arb.From(
           // This generates empty results with a 25% chance
           from b in Gen.Choose(0, 3).Select(0.Equals)
           from t in Arb.Generate<T>()
           select b ? new Result<T>() : new Result<T>(t)
         );

    public static Arbitrary<Verb> Verb()
      => Arb.From(
           from n in Arb.Generate<uint>()
           select new Verb(n)
         );

    public static Arbitrary<VerbPhrase> VerbPhrase()
      => Arb.From(
           from n in Arb.Generate<Verb>()
           from vs in Arb.Generate<Valent[]>()
           select ((Func<VerbPhrase>)(() =>
           {
             var vp = new VerbPhrase(n);
             foreach (var v in vs)
             {
               vp.AddValent(v);
             }
             return vp;
           }))()
         );
  }
}
