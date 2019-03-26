using Xunit;
using FsCheck;
using FsCheck.Xunit;

using WatsonAI;
using System.Collections.Generic;

namespace WatsonTest
{
  public class KnowledgeQueryTests
  {
    private Knowledge knowledge;
    private Associations associations;
    private KnowledgeQuery knowledgeQuery;

    private Entity earl = new Entity(0);
    private Entity actress = new Entity(1);
    private Entity gangster = new Entity(2);
    private Verb kill = new Verb(0);
    private Verb fart = new Verb(1);

    public KnowledgeQueryTests()
    {
      this.associations = new Associations();
      associations.AddEntityName(earl, "earl");
      associations.AddEntityName(actress, "actress");
      associations.AddEntityName(gangster, "gangster");
      associations.AddVerbName(kill, "kill");
      associations.AddVerbName(fart, "fart");

      var truth = new VerbPhrase(kill, new List<Valent>{ Valent.Subj(actress), Valent.Dobj(earl) });
      this.knowledge = new Knowledge();
      knowledge.AddVerbPhrase(truth);
      this.knowledgeQuery = new KnowledgeQuery(knowledge);
    }

    [Fact]
    public void GetSubjAnswers()
    {
      var answer = knowledgeQuery.GetSubjAnswers(kill, earl);
      Assert.Contains(actress, answer);
      Assert.Single(answer);
      Assert.Empty(knowledgeQuery.GetSubjAnswers(kill, actress));

      Assert.Empty(knowledgeQuery.GetSubjAnswers(fart, actress));
      Assert.Empty(knowledgeQuery.GetSubjAnswers(kill, gangster));
    }

    [Fact]
    public void GetDobjAnswers()
    {
      var answer = knowledgeQuery.GetDobjAnswers(kill, actress);
      Assert.Contains(earl, answer);
      Assert.Single(answer);
      Assert.Empty(knowledgeQuery.GetDobjAnswers(kill, earl));

      Assert.Empty(knowledgeQuery.GetDobjAnswers(fart, actress));
      Assert.Empty(knowledgeQuery.GetDobjAnswers(kill, gangster));
    }
  }
}
