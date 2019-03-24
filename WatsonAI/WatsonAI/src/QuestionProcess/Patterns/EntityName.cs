using OpenNLP.Tools.Parser;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WatsonAI
{
  public class EntityName : Pattern<IEnumerable<Entity>>
  {
    private readonly Associations associations;
    private readonly Thesaurus thesaurus;

    public EntityName(Associations associations, Thesaurus thesaurus)
    {
      this.associations = associations;
      this.thesaurus = thesaurus;
    }

    public override Result<IEnumerable<Entity>> Match(Parse tree)
      => new Result<IEnumerable<Entity>>(associations
      .EntityNames()
      .Where(name => thesaurus.Describes(tree.Value, name, true))
      .Select(name => associations.UncheckedGetEntity(name)));
  }
}
