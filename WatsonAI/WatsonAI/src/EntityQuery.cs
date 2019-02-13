using System.Collections.Generic;
using System.Linq;

namespace WatsonAI
{
  public class Query
  {
    private readonly Parser parser;

    public Query(Parser parser)
    {
      this.parser = parser;
    }

    public IEnumerable<Entity> Run(Associations assocs, KnowledgeGraph kg, string sentence)
    {
      return Enumerable.Empty<Entity>();
    }
  }
}
