using System;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Threading;
using UnityEngine;

public class AIClient : MonoBehaviour
{
    private MemoryMappedFile mmfi;
    private MemoryMappedFile mmfo;

    Mutex input;
    Mutex output;

    // Start is called before the first frame update
    void Start()
    {
        try
        {
            mmfi = MemoryMappedFile.OpenExisting("watsoninput");
            mmfo = MemoryMappedFile.OpenExisting("watsonoutput");
            input = Mutex.OpenExisting("watsoninputmutex");
            output = Mutex.OpenExisting("watsonoutputmutex");
        }
        catch (FileNotFoundException)
        {
            Debug.LogError("Memory-mapped file does not exist. Run Watson Server first.");
        }
    }

    public Tuple<string, string> GetResponse(string inputMessage, int character)
    {
        string corrected;
        string response;

        input.WaitOne();
        using (MemoryMappedViewStream stream = mmfi.CreateViewStream(0,4))
        {
            BinaryWriter writer = new BinaryWriter(stream);
            writer.Write((Int32)character);
        }
        using (MemoryMappedViewStream stream = mmfi.CreateViewStream(4,0))
        {
            StreamWriter writer = new StreamWriter(stream);
            writer.WriteLine(inputMessage);
            writer.Flush();
        }
        input.ReleaseMutex();

        output.WaitOne();
        using (MemoryMappedViewStream stream = mmfo.CreateViewStream())
        {
            StreamReader reader = new StreamReader(stream);
            corrected = reader.ReadLine();
            response = reader.ReadLine();
        }
        output.ReleaseMutex();
        return Tuple.Create(corrected, response);
    }

    private void OnApplicationQuit()
    {
        mmfi.Dispose();
        mmfo.Dispose();
        input.Dispose();
        output.Dispose();
    }
}
