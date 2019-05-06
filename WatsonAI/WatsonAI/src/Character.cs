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

    public string MoodResponse { get; set; }

    public string Location { get; }

    public string SeenResponse { get; set; }

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

      /*switch (name)
      {
        case "actress":
          Mood = "I'm an actress";
          Location = "study";
          Seen = "I saw the earl die";
          break;
        case "butler":
          Mood = "I'm a butler";
          Location = "dining room";
          Seen = "I saw the earl die";
          break;
        case "colonel":
          Mood = "I'm a colonel";
          Location = "study";
          Seen = "I saw the earl die";
          break;
        case "countess":
          Mood = "I'm a countess";
          Location = "study";
          Seen = "I saw the earl die";
          break;
        case "gangster":
          Mood = "I'm a gangster";
          Location = "foyer";
          Seen = "I saw the earl die";
          break;
        case "police":
          Mood = "I'm a policeman";
          Location = "foyer";
          Seen = "I saw the earl die";
          break;
      }*/
    }
  }

  public enum Gender { Male, Female, Other }
}
