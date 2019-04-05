using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace WatsonAI
{
  public class UniverseQuestionProcess : IProcess
  {
    private Parser parser;
    private KnowledgeQuery query;
    private Knowledge knowledge;
    private Thesaurus thesaurus;
    private Associations associations;

    public UniverseQuestionProcess(Parser parse, Knowledge knowledge, Thesaurus thesaurus, Associations associations)
    {
      this.parser = parse;
      this.knowledge = knowledge;
      this.thesaurus = thesaurus;
      this.associations = associations;
      this.query = new KnowledgeQuery(knowledge);
    }

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
