using edu.stanford.nlp.process;
//using edu.stanford.nlp.ling;
using edu.stanford.nlp.trees;
using edu.stanford.nlp.parser.lexparser;

using System.Collections;
using System.Collections.Generic;

namespace GameAI
{
  public class Parser
  {
    private readonly LexicalizedParser _lp;
    public Parser(string path)
    {
      _lp = LexicalizedParser.loadModel(path);
    }

    public static Parser FromRecommendedPath()
      => new Parser("../../res/englishPCFG.ser.gz");

    private java.util.List Tokenise(string sentence)
    {
      var tokFac = PTBTokenizer.factory(new CoreLabelTokenFactory(), "");
      var reader = new java.io.StringReader(sentence);
      var words = tokFac.getTokenizer(reader).tokenize();
      reader.close();

      return words;
    }

    public Tree Parse(string sentence)
      => _lp.apply(Tokenise(sentence));

    public IEnumerable<TypedDependency> DependenciesFrom(Tree tree)
    {
      var tdList = new PennTreebankLanguagePack()
                    .grammaticalStructureFactory()
                    .newGrammaticalStructure(tree)
                    .typedDependenciesCCprocessed();
      return new TypedDependeciesList(tdList);
    }
  }

  public class TypedDependeciesList : IEnumerable<TypedDependency>
  {
    private java.util.List _tdList;

    public TypedDependeciesList(java.util.List tdList)
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
  }
}
