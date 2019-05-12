using System;
using System.Collections.Generic;

namespace WatsonAI
{
  /// <summary>
  /// Basic overview of character containing information for game mechanics.
  /// </summary>
  public class Character
  {
    public string Name { get; }

    public bool Murderer { get; }

    public Knowledge Knowledge { get; set; }

    public Gender Gender { get; }

    public List<string> KnowledgeResponses { get; set; }
    public List<string> MoodResponses;

    public string Location { get; }

    public List<string> SeenResponses;

    public List<string> Greetings;

    /// <summary>
    /// Constructor for a character.
    /// </summary>
    /// <param name="name">Thier name.</param>
    /// <param name="murderer">Whether they are the murderer or not.</param>
    public Character(string name, bool murderer, Gender gender, string location)
    {
      this.Name = name;
      this.Murderer = murderer;
      this.Knowledge = new Knowledge();
      this.Gender = gender;
      this.Location = location;
      MoodResponses = new List<string>();
      SeenResponses = new List<string>();
      Greetings = new List<string>();
      KnowledgeResponses = new List<string>();
    }

    public void AddKnowledgeResponse(string knowledge)
    => KnowledgeResponses.Add(knowledge);

    public string GetKnowledgeResponse()
    {
      var rnd = new Random();
      var index = rnd.Next(KnowledgeResponses.Count);
      return KnowledgeResponses[index];
    }

    public void AddMood(string mood)
    {
      MoodResponses.Add(mood);
    }

    public string GetMood()
    {
      var rnd = new Random();
      var index = rnd.Next(MoodResponses.Count);
      return MoodResponses[index];
    }

    public void AddSeen(string seen)
    {
      SeenResponses.Add(seen);
    }

    public string GetSeen()
    {
      var rnd = new Random();
      var index = rnd.Next(SeenResponses.Count);
      return SeenResponses[index];
    }

    public void AddGreeting(string greeting)
    {
      Greetings.Add(greeting);
    }

    public string GetGreeting()
    {
      var rnd = new Random();
      var index = rnd.Next(Greetings.Count);
      return Greetings[index];
    }
  }

  public enum Gender { Male, Female, Other }
}
