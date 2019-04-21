using System;
using System.Collections.Generic;
using System.Text;

namespace WatsonAI
{
  public struct ReplacementRule
  {
    public List<string> OriginalTokens { get; }

    public List<string> ReplacementTokens { get; }

    public ReplacementRule(List<string> originalTokens, List<string> replacementTokens)
    {
      OriginalTokens = originalTokens;
      ReplacementTokens = replacementTokens;
    }
  }
}
