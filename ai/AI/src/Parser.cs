using edu.stanford.nlp.process;
//using edu.stanford.nlp.ling;
using edu.stanford.nlp.trees;
using edu.stanford.nlp.parser.lexparser;

using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace GameAI
{
  /// <summary>
  /// Parses sentences into grammatical trees.
  /// </summary>
  public class Parser
  {
    /// <summary>
    /// The internal parser object that this class wraps.
    /// </summary>
    private readonly LexicalizedParser _lp;

    /// <summary>
    /// Initialises a new instance of the <see cref="GameAI.Parser"/> class.
    /// </summary>
    /// <param name="path">The path to the data for the parser.</param>
    public Parser(string path)
    {
      _lp = LexicalizedParser.loadModel(path);
    }

    /// <summary>
    /// Initialises a new instance of <see cref="GameAI.Parser"/> from data
    /// at a recommended path.
    /// </summary>
    /// <returns>A new instance of <see cref="GameAI.Parser"/>.</returns>
    public static Parser FromRecommendedPath()
      => new Parser("../../res/englishPCFG.ser.gz");

    /// <summary>
    /// Tokenises the specified sentence.
    /// </summary>
    /// <returns>The tokenised sentence as a <see cref="java.util.List"/>.</returns>
    /// <param name="sentence">The sentence to be tokenised.</param>
    /// <remarks>
    /// The return value is a Java object and not compatible with many C#
    /// concepts.
    /// </remarks>
    private java.util.List Tokenise(string sentence)
    {
      var tokFac = PTBTokenizer.factory(new CoreLabelTokenFactory(), "");
      var reader = new java.io.StringReader(sentence);
      var words = tokFac.getTokenizer(reader).tokenize();
      reader.close();

      return words;
    }

    /// <summary>
    /// Parses the specified sentence.
    /// </summary>
    /// <returns>The grammatical tree.</returns>
    /// <param name="sentence">The sentence to parse.</param>
    public Tree Parse(string sentence)
      => _lp.apply(Tokenise(sentence));

    /// <summary>
    /// Gets the typed dependencies from a grammatical tree.
    /// </summary>
    /// <returns>An opaque <see cref="IEnumerable{TypedDependency}"/>.</returns>
    /// <param name="tree">The grammatical tree containing dependencies.</param>
    public TypedDependenciesList DependenciesFrom(Tree tree)
    {
      var tdList = new PennTreebankLanguagePack()
                    .grammaticalStructureFactory()
                    .newGrammaticalStructure(tree)
                    .typedDependenciesCCprocessed();
      return new TypedDependenciesList(tdList);
    }
  }

  /// <summary>
  /// A wrapper around <see cref="java.util.List"/>.
  /// </summary>
  public class TypedDependenciesList : IEnumerable<TypedDependency>
  {
    private java.util.List _tdList;

    public TypedDependenciesList(java.util.List tdList)
    {
      _tdList = tdList;
    }

    public IEnumerator<TypedDependency> GetEnumerator()
    {
      var tdIterator = _tdList.iterator();
      while (tdIterator.hasNext())
      {
        yield return tdIterator.next() as TypedDependency;
      }
    }

    IEnumerator IEnumerable.GetEnumerator()
      => GetEnumerator();

    public TypedDependency GetRoot()
      => this.First(
           (td) => td.reln().getShortName() == "root"
         );
  }
}
