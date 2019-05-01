using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace WatsonAI
{
  /// <summary>
  /// Process for querying the universe Knowledge
  /// </summary>
  public class UniverseQuestionProcess : IProcess
  {
    private Parser parser;
    private KnowledgeQuery query;
    private Knowledge knowledge;
    private Thesaurus thesaurus;
    private Associations associations;

    /// <summary>
    /// Constructs a new UniverseQuestionProcess similar to QuestionProcess
    /// </summary>
    /// <param name="parse"></param>
    /// <param name="knowledge"></param>
    /// <param name="thesaurus"></param>
    /// <param name="associations"></param>
    public UniverseQuestionProcess(Parser parse, Knowledge knowledge, Thesaurus thesaurus, Associations associations)
    {
      this.parser = parse;
      this.knowledge = knowledge;
      this.thesaurus = thesaurus;
      this.associations = associations;
      this.query = new KnowledgeQuery(knowledge);
    }

    /// <summary>
    /// Queries the universe Knowledge if the character's Knowledge
    /// returned no response. Appends the output with a tag if the
    /// universe Knowledge returned nothing
    /// </summary>
    /// <param name="stream"></param>
    /// <returns></returns>
    public Stream Process(Stream stream)
    {
      if (!stream.Output.Any())
      {
        var clone = stream.Clone();
        var question = new QuestionProcess(parser, knowledge, thesaurus, Story.Associations);
        question.Process(clone);
        if (!clone.Output.Any())
        {
          stream.AppendOutput("!u");
        }
      }

      return stream;
    }
  }
}
