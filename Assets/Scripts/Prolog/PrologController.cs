using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class PrologController : MonoBehaviour
{
    private PrologProcess process;
    public string pathToApi;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void instantiateProlog()
    {        
        Debug.Log(Application.dataPath + pathToApi);
        process = new PrologProcess(Application.dataPath + pathToApi);
    }

    public string getStringUpdate()
    {
        process.WriteInput(PrologCommands.GetAgentKnowlodge);
        return process.GetOutput();
    }
}
