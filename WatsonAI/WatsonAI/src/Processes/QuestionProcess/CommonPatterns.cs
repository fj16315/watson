using OpenNLP.Tools.Parser;
using System;
using System.Collections.Generic;

using static WatsonAI.Patterns;

namespace WatsonAI
{
  public class CommonPatterns
  {
    private Thesaurus thesaurus;
    private Associations associations;

    public CommonPatterns(Thesaurus thesaurus, Associations associations)
    {
      this.thesaurus = thesaurus;
      this.associations = associations;
    }

    public Branch Top {
      get {
        return Branch("TOP");
      }
    }
    public EntityName Noun {
      get {
        return new EntityName(associations, thesaurus);
      }
    }
    public VerbName Verb {
      get {
        return new VerbName(associations, thesaurus);
      }
    }
    public AdjName Adj
    {
      get
      {
        return new AdjName(associations, thesaurus);
      }
    }
    public Pattern<IEnumerable<Entity>> NounPhrase {
      get {
        return (Branch("NP") > Noun).Flatten();
      }
    }
    public Pattern<IEnumerable<Verb>> VerbPhrase {
      get {
        return (Branch("VP") > Verb).Flatten();
      }
    }
    public Pattern<IEnumerable<Entity>> AdjPhrase
    {
      get
      {
        return (Branch("ADJP") > Adj).Flatten();
      }
    }
    public Pattern<Parse> SimpleVerb {
      get {
        return Branch("VB")
             | Branch("VBD")
             | Branch("VBG")
             | Branch("VBN")
             | Branch("VBP")
             | Branch("VBZ");
      }
    }
  }
}
