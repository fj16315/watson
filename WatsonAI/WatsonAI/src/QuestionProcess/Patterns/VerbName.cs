using OpenNLP.Tools.Parser;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WatsonAI
{
  public class VerbName : Pattern<IEnumerable<Verb>>
  {
    private Associations associations;
    private Thesaurus thesaurus;

    public VerbName(Associations associations, Thesaurus thesaurus)
    {
      this.associations = associations;
      this.thesaurus = thesaurus;
    }

    public override Result<IEnumerable<Verb>> Match(Parse tree)
      => new Result<IEnumerable<Verb>>(associations
      .VerbNames()
      .Where(name => thesaurus.Describes(tree.Value, name, true))
      .Select(name => associations.UncheckedGetVerb(name)));
  }
}
