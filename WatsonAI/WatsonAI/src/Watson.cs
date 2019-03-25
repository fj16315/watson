using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace WatsonAI
{
  public class Watson : IWatson
  {
    private Parser parser = new Parser();
    private Thesaurus thesaurus = new Thesaurus();
    private Dictionary<Character, Knowledge> characters = new Dictionary<Character, Knowledge>();
    private Associations associations = new Associations();

    public Watson(Character character)
    {
      // Constructor to initialise the dictionary.

      /*                Places                 */
      characters.Add(character, new Knowledge());
      associations.AddEntityName(new Entity(0), "lounge");
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


      /*               Items                     */
      associations.AddEntityName(new Entity(11), "arsenic"); //Bottle of arsenic
      associations.AddEntityName(new Entity(12), "barbital"); //Bottle of barbital
      associations.AddEntityName(new Entity(13), "berry"); //nightshade berry
      associations.AddEntityName(new Entity(14), "will"); //the earls will
      associations.AddEntityName(new Entity(15), "money"); //the earls will

      /*                   Characters                  */
      associations.AddEntityName(new Entity(16), "detective"); 
      associations.AddEntityName(new Entity(17), "countess"); 
      associations.AddEntityName(new Entity(18), "actress");
      associations.AddEntityName(new Entity(19), "gangster");
      associations.AddEntityName(new Entity(20), "colonel");
      associations.AddEntityName(new Entity(21), "butler");
      associations.AddEntityName(new Entity(22), "police");
      associations.AddEntityName(new Entity(23), "Earl");



      /*                Verbs                    */
      associations.AddVerbName(new Verb(0), "be");
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


    }

    /// <summary>
    /// Run the AI on some input speech.
    /// </summary>
    /// <param name="input">The input given by the player. </param>
    /// <returns>Response to the player.</returns>
    public string Run(string input, Character character)
    {
      var kg = characters[character];
      //Actress kill Earl
      kg.AddVerbPhrase(new VerbPhrase(new Verb(1), new List<Valent> { Valent.Subj(new Entity(18)), Valent.Dobj(new Entity(23)) }));
      //Earl owe money
      kg.AddVerbPhrase(new VerbPhrase(new Verb(3), new List<Valent> { Valent.Subj(new Entity(23)), Valent.Dobj(new Entity(15)) }));
      //Actress inherit will
      kg.AddVerbPhrase(new VerbPhrase(new Verb(13), new List<Valent> { Valent.Subj(new Entity(18)), Valent.Dobj(new Entity(14)) }));
      //Butler launder money
      kg.AddVerbPhrase(new VerbPhrase(new Verb(10), new List<Valent> { Valent.Subj(new Entity(21)), Valent.Dobj(new Entity(15)) }));
      //Earl meet gangster
      kg.AddVerbPhrase(new VerbPhrase(new Verb(15), new List<Valent> { Valent.Subj(new Entity(23)), Valent.Dobj(new Entity(19)) }));
      //Chamber contains money
      kg.AddVerbPhrase(new VerbPhrase(new Verb(19), new List<Valent> { Valent.Subj(new Entity(7)), Valent.Dobj(new Entity(15)) }));
      //Suite contains money
      kg.AddVerbPhrase(new VerbPhrase(new Verb(19), new List<Valent> { Valent.Subj(new Entity(8)), Valent.Dobj(new Entity(15)) }));
      //Library contains arsenic 
      kg.AddVerbPhrase(new VerbPhrase(new Verb(19), new List<Valent> { Valent.Subj(new Entity(6)), Valent.Dobj(new Entity(11)) }));



      var stream = Stream.Tokenise(parser, input);

      var greetings = new GreetingsEngine();
      var debugParse = new DebugParseEngine(parser);
      var fallback = new FallbackEngine();
      var thesaurusDebug = new ThesaurusDebugEngine(thesaurus);
      var question = new QuestionProcess(parser, character, kg, thesaurus, associations);

      //var output = io
      //  .Process(greetings)
      //  .Process(debugParse)
      //  .Process(thesaurusDebug)
      //  .Process(question)
      //  .Process(fallback);
      var output = stream
        .ProcessWith(debugParse)
        .ProcessWith(question);
      return output.Output();
    }

    /// <summary>
    /// Run the AI on some input speech from a character.
    /// </summary>
    /// <remarks> Currently ignores character. </remarks>
    /// <param name="input">The input given by the player. </param>
    /// <param name="character">The character the player is talking to.</param>
    /// <returns>Response to the player.</returns>
    /*public string Run(string input, Character character)
    {
      return Run(input);
    }*/
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
    //string Run(string input);

    /// <summary>
    /// Run the AI on some input speech from a character.
    /// </summary>
    /// <param name="input">The input given by the player. </param>
    /// <param name="character">The character the player is talking to.</param>
    /// <returns>Response to the player.</returns>
    string Run(string input, Character character);
  }
}
