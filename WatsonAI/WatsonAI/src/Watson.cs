namespace WatsonAI
{
  public class Watson : IWatson
  {
      // Constructor to initialise the dictionary.

      /*                Places                 */
      /*associations.AddEntityName(new Entity(0), "lounge");
      associations.AddEntityName(new Entity(1), "bar"); // billiards room
      associations.AddEntityName(new Entity(2), "kitchen");
      associations.AddEntityName(new Entity(3), "diner"); //dining room
      associations.AddEntityName(new Entity(4), "entry-room"); 
      associations.AddEntityName(new Entity(5), "hall");
      associations.AddEntityName(new Entity(6), "library");
      associations.AddEntityName(new Entity(7), "chamber"); //master bedroom
      associations.AddEntityName(new Entity(8), "suite"); //butler's room
      associations.AddEntityName(new Entity(9), "room"); //other room
      associations.AddEntityName(new Entity(10), "bathroom"); 
      */

      /*               Items                     */
      /*associations.AddEntityName(new Entity(11), "arsenic"); //Bottle of arsenic
      associations.AddEntityName(new Entity(12), "barbital"); //Bottle of barbital
      associations.AddEntityName(new Entity(13), "berry"); //nightshade berry
      associations.AddEntityName(new Entity(14), "will"); //the earls will
      associations.AddEntityName(new Entity(15), "money"); //the earls will
      */
      /*                   Characters                  */
      /*associations.AddEntityName(new Entity(16), "detective"); 
      associations.AddEntityName(new Entity(17), "countess"); 
      associations.AddEntityName(new Entity(18), "actress");
      associations.AddEntityName(new Entity(19), "gangster");
      associations.AddEntityName(new Entity(20), "colonel");
      associations.AddEntityName(new Entity(21), "butler");
      associations.AddEntityName(new Entity(22), "police");
      associations.AddEntityName(new Entity(23), "Earl");
      */


      /*                Verbs                    */
      /*associations.AddVerbName(new Verb(0), "be");
      associations.AddVerbName(new Verb(1), "kill");
      associations.AddVerbName(new Verb(2), "consume");
      associations.AddVerbName(new Verb(3), "owe"); //Earl owed the gangster money
      associations.AddVerbName(new Verb(4), "poison");
      associations.AddVerbName(new Verb(6), "marry");
      associations.AddVerbName(new Verb(7), "fight");
      associations.AddVerbName(new Verb(8), "employ");
      associations.AddVerbName(new Verb(9), "give");
      associations.AddVerbName(new Verb(10), "launder");
      associations.AddVerbName(new Verb(11), "worry");
      associations.AddVerbName(new Verb(12), "happy");
      associations.AddVerbName(new Verb(13), "inherit");
      associations.AddVerbName(new Verb(14), "admit");
      associations.AddVerbName(new Verb(15), "meet");
      associations.AddVerbName(new Verb(16), "admit");
      associations.AddVerbName(new Verb(17), "study");
      associations.AddVerbName(new Verb(18), "marry");
      associations.AddVerbName(new Verb(19), "contains");
      */
    private readonly Parser parser;
    private readonly Thesaurus thesaurus; 

    /// <summary>
    /// Constructs a new Watson with path to the location of the data files.
    /// </summary>
    /// <param name="stringToPath"></param>
    public Watson(string stringToPath) {
      this.parser = new Parser(stringToPath);
      this.thesaurus = new Thesaurus(stringToPath, Story.Associations);
    }

    /// <summary>
    /// Run the AI on some input speech.
    /// </summary>
    /// <param name="input">The input given by the player. </param>
    /// <returns>Response to the player.</returns>
    public string Run(string input)
    {
      var knowledge = Story.Knowledge;

      var stream = Stream.Tokenise(parser, input);
      //TODO: Add in Character Knowledge dictionary in story class  

      var debugs = new DebugProcesses(parser, thesaurus);
      var greetings = new GreetingsProcess(parser,thesaurus);
      var fallback = new FallbackProcess();
      var question = new QuestionProcess(parser, knowledge, thesaurus, Story.Associations);

      var output = new Processor()
        .AddProcesses(greetings, debugs, question, fallback)
        .Process(stream);
      return string.Join(", ", output.Output);
    }
  
    /// <summary>
    /// Run the AI on some input speech from a character.
    /// </summary>
    /// <remarks> Currently ignores character. </remarks>
    /// <param name="input">The input given by the player. </param>
    /// <param name="character">The character the player is talking to.</param>
    /// <returns>Response to the player.</returns>
    public string Run(string input, Character character)
    {
      return Run(input);
    }
  }

  /// <summary>
  /// Specifying the interface for how the AI will process input strings.
  /// </summary>
  public interface IWatson
  {
    /// <summary>
    /// Run the AI on some input speech.
    /// </summary>
    /// <param name="input">The input given by the player. </param>
    /// <returns>Response to the player.</returns>
    string Run(string input);

    /// <summary>
    /// Run the AI on some input speech from a character.
    /// </summary>
    /// <param name="input">The input given by the player. </param>
    /// <param name="character">The character the player is talking to.</param>
    /// <returns>Response to the player.</returns>
    string Run(string input, Character character);
  }
}
