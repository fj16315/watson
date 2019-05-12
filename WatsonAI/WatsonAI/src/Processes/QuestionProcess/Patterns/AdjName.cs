using System;
using System.Collections.Generic;
using System.Text;
using OpenNLP.Tools.Parser;
using Syn.WordNet;
using System.Linq;

namespace WatsonAI
{
  public class AdjName : Pattern<IEnumerable<Entity>>
  {
    private readonly Associations associations;
    private readonly Thesaurus thesaurus;

    /// <summary>
    /// Construct a new pattern tha matches against entities, or synonyms of entities.
    /// </summary>
    /// <param name="associations">Associations containing the entity string mapping.</param>
    /// <param name="thesaurus">Thesaurus to use for comparisons.</param>
    public AdjName(Associations associations, Thesaurus thesaurus)
    {
      this.associations = associations;
      this.thesaurus = thesaurus;
    }

    /// <summary>
    /// Returns an IEnumerable of all matching entities on the tree.
    /// </summary>
    /// <param name="tree">The tree to match against.</param>
    /// <returns>
    /// A Result of IEnumerable containing all entities that are matched in the tree.
    /// </returns>
    public override Result<IEnumerable<Entity>> Match(Parse tree)
      => new Result<IEnumerable<Entity>>(associations
      .EntityNames()
      .Where(name => thesaurus.Describes(tree.Value, name, PartOfSpeech.Adjective, true))
      .Select(name => associations.UncheckedGetEntity(name)));
  }
}
