using System.Collections.Generic;
using System.Linq;
using System.IO;
using System;

namespace WatsonAI
{
  public class Watson : IWatson
  {
    private readonly Parser parser;
    private readonly Thesaurus thesaurus;
    private readonly SymSpell symSpell;
    private readonly Dictionary<Character, Memory> characterMemories;
    private const int memoryCapacity = 5;
    private const int initialCapacity = 549313;
    private const int maxEditDistanceDictionary = 2;
    private const int termIndex = 0;
    private const int countIndex = 1;
    /// <summary>
    /// Constructs a new Watson with path to the location of the data files.
    /// </summary>
    /// <param name="stringToPath"></param>
    public Watson(string stringToPath) {
      if (symSpell == null)
      {
        symSpell = new SymSpell(initialCapacity, maxEditDistanceDictionary);
        string directory = Path.Combine(stringToPath, "res", "dictionary", "frequency_dictionary.txt");
        if (!symSpell.LoadDictionary(directory, termIndex, countIndex))
        {
          System.Diagnostics.Debug.WriteLine("File not found");
        }
      }
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
    public Tuple<string, string> Run(string input, int characterInt)
    {
      var name = (Story.Names)characterInt;
      var character = Story.Characters[name];
      var characters = Story.Characters.Values.ToList();
      var knowledge = Story.Characters[name].Knowledge;
      var memory = characterMemories[character];

      var stream = Stream.Tokenise(parser, input);
      stream = memory.MaybeHandleStream(stream);

      var debugs = new DebugProcesses(characters, parser, thesaurus);
      var greetings = new GreetingsProcess(parser, thesaurus, character);
      var question = new QuestionProcess(parser, Story.Knowledge, thesaurus, Story.Associations);
      var fallback = new FallbackProcess();
      var prePronoun = new PronounsRemovalProcess(character, characters, memory, parser);
      var postPronoun = new PronounsAddingProcess(character, parser);
      var universeQuestion = new UniverseQuestionProcess(parser, Story.Knowledge, thesaurus, Story.Associations);
      var multipleWord = new MultipleWordProcess();
      var multipleWordRemoval = new MultipleWordRemovalProcess();
      var smallTalk = new SmallTalkProcess(character);
      var spellCheck = new SpellCheckProcess(symSpell, parser);

      var output = new Processor()
        .AddProcesses(debugs,
                      spellCheck, 
                      multipleWord,
                      prePronoun, 
                      greetings, 
                      smallTalk,
                      question, 
                      postPronoun, 
                      universeQuestion,
                      multipleWordRemoval, 
                      fallback)
        .Process(stream);

      var response = string.Join(", ", output.Output);

      memory.AppendInput(input);
      memory.AppendResponse(response);

      return Tuple.Create(output.spellCorrectedInput, response);
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
    Tuple<string, string> Run(string input,  int character);
  }
}
