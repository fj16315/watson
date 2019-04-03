using System.Collections.Generic;
using System.Linq;

namespace WatsonAI
{
  public class Knowledge
  {
    private readonly List<VerbPhrase> verbPhrases;
    private readonly HashSet<Entity> entities;

    public Knowledge()
    {
      verbPhrases = new List<VerbPhrase>();
      entities = new HashSet<Entity>();
    }

    public void AddVerbPhrase(VerbPhrase verbPhrase)
    {
      verbPhrases.Add(verbPhrase);
      entities.UnionWith(
        verbPhrase.GetValents().Select(v => v.entity)
      );
    }

    public bool RemoveVerbPhrase(VerbPhrase verbPhrase)
    {
      var removed = verbPhrases.Remove(verbPhrase);
      RecheckEntities();
      return removed;
    }

    private void RecheckEntities()
    {
      entities.Clear();
      entities.UnionWith(
        verbPhrases.SelectMany(
          vp => vp.GetValents()
                  .Select( v => v.entity )
        )
      );
    }

    public IEnumerable<VerbPhrase> GetVerbPhrases()
      => verbPhrases;

    public IEnumerable<Entity> GetEntities()
      => entities;
  }
}
