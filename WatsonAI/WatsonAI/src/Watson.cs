using System.Collections.Generic;

namespace WatsonAI
{
  public class Watson : IWatson
  {
    private readonly Parser parser;
    private readonly Thesaurus thesaurus;
    private readonly Dictionary<Character, Memory> characterMemories;
    private const int memoryCapacity = 5;

    /// <summary>
    /// Constructs a new Watson with path to the location of the data files.
    /// </summary>
    /// <param name="stringToPath"></param>
    public Watson(string stringToPath) {
      this.parser = new Parser(stringToPath);
      this.thesaurus = new Thesaurus(stringToPath, Story.Associations);
      this.characterMemories = new Dictionary<Character, Memory>();
      foreach (var character in Story.Characters.Values)
      {
        characterMemories.Add(character, new Memory(character, memoryCapacity));
      }
    }

    /// <summary>
    /// Run the AI on some input speech from a character.
    /// </summary>
    /// <remarks> Currently ignores character. </remarks>
    /// <param name="input">The input given by the player. </param>
    /// <param name="character">The character the player is talking to.</param>
    /// <returns>Response to the player.</returns>
    public string Run(string input, int characterInt)
    {
      var name = (Story.Names)characterInt;
      var character = Story.Characters[name];
      var knowledge = Story.Characters[name].Knowledge;

      var stream = Stream.Tokenise(parser, input);

      var debugs = new DebugProcesses(parser, thesaurus);
      var greetings = new GreetingsProcess(parser, thesaurus);
      var question = new QuestionProcess(parser, knowledge, thesaurus, Story.Associations);
      var fallback = new FallbackProcess();
      var prePronoun = new PronounsProcess(character, characterMemories[character], parser);
      var universeQuestion = new UniverseQuestionProcess(parser, Story.Knowledge, thesaurus, Story.Associations);

      var output = new Processor()
        .AddPreProcesses(prePronoun)
        .AddProcesses(greetings, debugs, question, universeQuestion, fallback)
        .Process(stream);

      var response = string.Join(", ", output.Output);

      characterMemories[character].AppendInput(input);
      characterMemories[character].AppendResponse(response);

      return response;
    }
  }

  /// <summary>
  /// Specifying the interface for how the AI will process input strings.
  /// </summary>
  public interface IWatson
  {
    /// <summary>
    /// Run the AI on some input speech from a character.
    /// </summary>
    /// <param name="input">The input given by the player. </param>
    /// <param name="character">The character the player is talking to.</param>
    /// <returns>Response to the player.</returns>
    string Run(string input,  int character);
  }
}
