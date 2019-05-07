using OpenNLP.Tools.Parser;
using Syn.WordNet;
using System.Collections.Generic;
using System.Linq;

namespace WatsonAI
{
  /// <summary>
  /// A pattern that matches against Verbs, or synonyms of entities, according to a thesaurus.
  /// </summary>
  /// <remarks>Will check stemmed verbs.</remarks>
  public class VerbName : Pattern<IEnumerable<Verb>>
  {
    private readonly Associations associations;
    private readonly Thesaurus thesaurus;

    /// <summary>
    /// Construct a new pattern tha matches against verb, or synonyms of verbs.
    /// </summary>
    /// <param name="associations">Associations containing the entity string mapping.</param>
    /// <param name="thesaurus">Thesaurus to use for comparisons.</param>
    public VerbName(Associations associations, Thesaurus thesaurus)
    {
      this.associations = associations;
      this.thesaurus = thesaurus;
    }

    /// <summary>
    /// Returns an IEnumerable of all matching verbs on the tree.
    /// </summary>
    /// <param name="tree">The tree to match against.</param>
    /// <returns>
    /// A Result of IEnumerable containing all verbs that are matched in the tree.
    /// </returns>
    public override Result<IEnumerable<Verb>> Match(Parse tree)
    {
      var results = associations
           .VerbNames()
           .Where(name => thesaurus.Describes(tree.Value, name, PartOfSpeech.Verb, true))
           .Select(name => associations.UncheckedGetVerb(name));
      if (results.Any()) return new Result<IEnumerable<Verb>>(results);
      else return Result<IEnumerable<Verb>>.Fail;
    }
  }
}
