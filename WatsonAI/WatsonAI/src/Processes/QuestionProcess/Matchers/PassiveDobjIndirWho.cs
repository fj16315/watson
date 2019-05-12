using OpenNLP.Tools.Parser;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using static WatsonAI.Patterns;

namespace WatsonAI
{
  public class PassiveDobjIndirWho : IEntityMatcher
  {
    private readonly CommonPatterns cp;
    private readonly KnowledgeQuery query;
    private readonly Associations associations;
    private readonly Thesaurus thesaurus;

    private IEnumerable<Entity> answers = null;
    private string response = null;

    public PassiveDobjIndirWho(CommonPatterns cp, KnowledgeQuery query, Associations associations, Thesaurus thesaurus)
    {
      this.cp = cp;
      this.query = query;
      this.associations = associations;
      this.thesaurus = thesaurus;

    }

    public bool MatchOn(Parse tree)
    {
      var whoQuestion = (cp.Top >= (Branch("SBAR") > Branch("WHNP"))).Flatten();

      var containsWho = cp.Top >= Word(thesaurus, "who");
      var containsWhat = cp.Top >= Word(thesaurus, "what");
      var patternWhoQuestion = And(containsWho, whoQuestion);
      var patternWhatQuestion = And(containsWhat, whoQuestion);
      var isWhoQuestion = patternWhoQuestion.Match(tree).HasValue;
      var isWhatQuestion = patternWhatQuestion.Match(tree).HasValue;

      var passiveDobjQuestion = cp.Top >= (Branch("S") > (Branch("VP") > (Branch("VP") > (Branch("PP") > Branch("NP"))))).Flatten().Flatten().Flatten();
      var isPassiveDobjQuestion = passiveDobjQuestion.Match(tree).HasValue;
      Debug.WriteLineIf(isPassiveDobjQuestion, "Passive Dobj Question");

      var passiveDobjWho = And(whoQuestion, passiveDobjQuestion);
      var isPassiveDobjWho = passiveDobjWho.Match(tree).HasValue;

      Debug.WriteLineIf(isPassiveDobjWho, "Passive Dobj WhoWhat Question");

      if (isPassiveDobjWho)
      {
        var entityPattern = (cp.Top >= (Branch("S") > (Branch("VP") > (Branch("VP") > (Branch("PP") > cp.NounPhrase))))).Flatten().Flatten().Flatten().Flatten().Flatten();
        var entities = entityPattern.Match(tree).Value;

        var verbPattern = (cp.Top >= (Branch("S") > (Branch("VP") > cp.VerbPhrase))).Flatten().Flatten().Flatten();

        var verbs = verbPattern.Match(tree).Value;
        foreach (var v in verbs)
        {
          string answer;
          associations.TryNameVerb(v, out answer);
          Console.WriteLine("Verbs: " + answer);
        }
        answers = GenerateAnswers(entities.Distinct(), verbs.Distinct());
        if (isWhoQuestion) { entities = Story.WhoEntityFilter(answers); }
        if (isWhatQuestion) { entities = Story.WhatEntityFilter(answers); }
        if (answers.Any())
        {
          var verbWordPattern = (cp.Top >= (Branch("S") > (Branch("VP") > Branch("VP")))).Flatten().Flatten();
          var restOfQuestion = verbWordPattern.Match(tree).Value.First().Value;

          var answer = associations.UncheckedNameEntity(answers.First());
          var responseParts = new string[] { "The", answer, restOfQuestion };
          response = string.Join(" ", responseParts);
          Debug.WriteLine("Response: " + response);
        }
      }

      return isPassiveDobjWho && answers.Any();
    }

    public string GenerateResponse()
    {
      return response;
    }

    public IEnumerable<Entity> GetAnswers()
    {
      return answers;
    }

    private List<Entity> GenerateAnswers(IEnumerable<Entity> entities, IEnumerable<Verb> verbs)
    {
      var pairs = from e in entities
                  from v in verbs
                  select Tuple.Create(e, v);

      var answers = new List<Entity>();
      foreach (var p in pairs.Distinct())
      {
        var e = p.Item1;
        var v = p.Item2;
        answers.AddRange(query.GetDobjAnswers(v, e));
      }
      return answers;
    }
  }
}