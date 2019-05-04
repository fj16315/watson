using System.Collections.Generic;

namespace WatsonAI
{
  interface IEntityMatcher : IMatcher
  {
    IEnumerable<Entity> GetAnswers();
  }
}
