using System;
using System.Collections.Generic;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.IO.Pipes;
using System.Security.Principal;
using System.Threading;
using WatsonAI;

public class MainClass
{
  /// <summary>
  ///   Runs the parser as a repl environment.
  /// </summary>
  public static void Main(string[] args)
  {
    //Initialise Watson
    var path = Directory.GetCurrentDirectory();
    if (Directory.Exists(Path.Combine(path, "bin")))
    {
      path = Path.Combine(path, "bin", "debug", "netcoreapp2.1");
    }
    IWatson watson = new Watson(path);

    using (MemoryMappedFile mmfi = MemoryMappedFile.CreateNew("watsoninput", 10000))
    {
      using (MemoryMappedFile mmfo = MemoryMappedFile.CreateNew("watsonoutput", 10000))
      {
        Mutex input = new Mutex(true, "watsoninputmutex");
        input.ReleaseMutex();
        Mutex output = new Mutex(true, "watsonoutputmutex");

        Console.WriteLine("Press enter when the game has launched.");
        Console.ReadLine();

        while (true)
        {
          Console.WriteLine("Waiting for input mutex.");

          int characterNumber;

          input.WaitOne();
          using (MemoryMappedViewStream stream = mmfi.CreateViewStream(0, 4))
          {
            BinaryReader reader = new BinaryReader(stream);
            characterNumber = reader.ReadInt32();
            Console.WriteLine($"Character: {characterNumber}");
          }

          string inputMessage;
          using (MemoryMappedViewStream stream = mmfi.CreateViewStream(4, 0))
          {
            StreamReader reader = new StreamReader(stream);
            inputMessage = reader.ReadLine();
            Console.WriteLine($"Input: {inputMessage}");
          }
          input.ReleaseMutex();

          Console.WriteLine("Waiting for output mutex.");

          if (inputMessage[0] != '\0')
          {
            using (MemoryMappedViewStream stream = mmfo.CreateViewStream())
            {
              StreamWriter writer = new StreamWriter(stream);
              var response = watson.Run(inputMessage, characterNumber);
              writer.WriteLine(response.Item1);
              writer.WriteLine(response.Item2);
              writer.Flush();
              Console.WriteLine("New input: ", response.Item1);
              Console.WriteLine("Response: ", response.Item2);
            }
          }
          output.ReleaseMutex();
          output.WaitOne();
        }
      }
    }
  }
}

