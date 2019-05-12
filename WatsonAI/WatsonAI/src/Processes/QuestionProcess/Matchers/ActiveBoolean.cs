using OpenNLP.Tools.Parser;
using Syn.WordNet;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using static WatsonAI.Patterns;

namespace WatsonAI
{
  public class ActiveBoolean : IBoolMatcher
  {
    private readonly CommonPatterns cp;
    private readonly KnowledgeQuery query;
    private readonly Associations associations;
    private readonly Thesaurus thesaurus;

    private bool answer = false;
    private string response = null;

    public ActiveBoolean(CommonPatterns cp, KnowledgeQuery query, Associations associations, Thesaurus thesaurus) {
      this.cp = cp;
      this.query = query;
      this.associations = associations;
      this.thesaurus = thesaurus;
    }

    public bool MatchOn(Parse tree)
    {
      var booleanQuestion = cp.Top >= (Branch("SQ") > cp.SimpleVerb);
      var active = cp.Top >= (And(Branch("SQ") > Branch("NP"), Branch("SQ") > Branch("NP")));
      var activeBoolean = And(booleanQuestion, active);

      var isActiveBoolean = activeBoolean.Match(tree).HasValue;
      Debug.WriteLineIf(isActiveBoolean, "Active Boolean");

      if (isActiveBoolean)
      {
        var entityPattern = (cp.Top >= (Branch("SQ") > cp.NounPhrase)).Flatten().Flatten();
        var entities = entityPattern.Match(tree).Value.ToList();
       
        var verbPattern = (cp.Top >= (Branch("SQ") > cp.SimpleVerb)).Flatten();
        var verbString = verbPattern.Match(tree).Value.FirstOrDefault().Value;

        var results = associations
             .VerbNames()
             .Where(name => thesaurus.Describes(verbString, name, PartOfSpeech.Verb, true))
             .Select(name => associations.UncheckedGetVerb(name));

        if (entities.Count == 2 && results.Any())
        {
          answer = query.GetBoolAnswer(results.First(), entities[0], entities[1]);
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
        return false;
      }

      return isActiveBoolean;
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
