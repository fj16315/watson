using OpenNLP.Tools.Parser;
using Syn.WordNet;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using static WatsonAI.Patterns;

namespace WatsonAI
{
  public class PassiveBoolean : IBoolMatcher
  {
    private readonly CommonPatterns cp;
    private readonly KnowledgeQuery query;
    private readonly Associations associations;
    private readonly Thesaurus thesaurus;

    private bool answer = false;
    private string response = null;

    public PassiveBoolean(CommonPatterns cp, KnowledgeQuery query, Associations associations, Thesaurus thesaurus) {
      this.cp = cp;
      this.query = query;
      this.associations = associations;
      this.thesaurus = thesaurus;
    }

    public bool MatchOn(Parse tree)
    {
      var booleanQuestion = cp.Top >= (Branch("SQ") > cp.SimpleVerb);
      var passive = cp.Top >= (And(Branch("SQ") > Branch("NP"), Branch("SQ") > (Branch("VP") > Branch("NP"))));
      var passiveBoolean = And(booleanQuestion, passive);

      var isPassiveBoolean = passiveBoolean.Match(tree).HasValue;
      Debug.WriteLineIf(isPassiveBoolean, "Passive Boolean");

      if (isPassiveBoolean)
      {
        var subjPattern = (cp.Top >= (Branch("SQ") > cp.NounPhrase)).Flatten().Flatten();
        var subj = subjPattern.Match(tree).Value;

        var dobjPattern = (cp.Top >= (Branch("SQ") > (Branch("VP") > cp.NounPhrase))).Flatten().Flatten().Flatten();
        var dobj = dobjPattern.Match(tree).Value;

        var verbPattern = (cp.Top >= (Branch("SQ") > cp.VerbPhrase)).Flatten().Flatten();
        var verb = verbPattern.Match(tree).Value;

        answer = query.GetBoolAnswer(verb.First(), subj.First(), dobj.First());
        string[] responseParts;
        if (answer)
        {
          responseParts = new string[] { "yes" };
        }
        else
        {
          responseParts = new string[] { "no" };
        }
        response = string.Join(" ", responseParts);
        Debug.WriteLine("Response: " + response);
        return answer;
      }

      return isPassiveBoolean;
    }

    public string GenerateResponse()
    {
      return response;
    }

    public bool GetAnswer()
    {
      return answer;
    }
  }
}
