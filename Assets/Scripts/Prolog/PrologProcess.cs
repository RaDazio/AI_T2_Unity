
using SbsSW.SwiPlCs;
using System;
using System.Collections;
using System.Diagnostics;
using System.Threading;
using System.IO;
using UnityEngine;

public class PrologProcess 
{
    Process prologProcess;
    StreamWriter writer;
    StreamReader reader;

    public PrologProcess(string apiPath)
    {
        prologProcess = new Process();
        
        prologProcess.StartInfo.FileName = "swipl";
        prologProcess.StartInfo.Arguments = apiPath;
        prologProcess.StartInfo.RedirectStandardInput = true; 
        prologProcess.StartInfo.RedirectStandardOutput = true;
        prologProcess.StartInfo.UseShellExecute = false;

        
        prologProcess.Start();
        writer = prologProcess.StandardInput;
        reader = prologProcess.StandardOutput;

    }

    private void WriteInput(string text)
    {
        writer.WriteLine(text);
    }

    private string GetOutput()
    {
        string text = String.Empty;
        string line= "";
        do
        {
            line = reader.ReadLine();
            text+=line;
        }while(line!= null && line!="");
        
        return text;
    }

    public string RunCommand(string text)
    {
        UnityEngine.Debug.Log(text);
        WriteInput(text);
        return GetOutput();
    }

}
