using System.Collections.Generic;

namespace WatsonAI
{
  public interface IPronounHandler
  {
    List<ReplacementRule> GenerateReplacements(Stream stream);

    bool RequiresClarification();

    Stream RequestClarification(Stream stream);

    Stream HandleClarification(Stream stream);
  }
}
