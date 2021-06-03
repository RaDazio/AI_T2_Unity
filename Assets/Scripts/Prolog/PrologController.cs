using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text.RegularExpressions;


public class PrologController : MonoBehaviour
{
    public PrologProcess process;
    public string pathToApi;
    // Start is called before the first frame update

    Player_Stats PlayerStatus;

    void Start()
    {
    
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void instantiateProlog()
    {        
        process = new PrologProcess(Application.dataPath + pathToApi);
        updateUI();
    }

    public string getStringKnowlodgeUpdate()
    {
        return process.RunCommand(PrologCommands.GetAgentKnowlodge);
    }
    
    public string getStringAgentStateUpdate()
    {
        return process.RunCommand(PrologCommands.GetAgentState);
    }

    public void updateUI()
    {
        string output = getStringAgentStateUpdate();

        string value = Regex.Replace(Regex.Match(output,"\\(.*?\\)").ToString(),@"(\s+|\(|\))","");
        string[] values = Regex.Split(value,",");
        
        Player_Stats statsController = GameObject.FindGameObjectWithTag("Player").GetComponent<Player_Stats>();
        statsController.UpdateLife(int.Parse(values[3]));
        statsController.UpdateAmmo(int.Parse(values[4]));
        statsController.updateScoreHUD(values[5]);
    }

    public Vector2 getPrologPlayerPosition()
    {
        string output = getStringAgentStateUpdate();
        string value = Regex.Replace(Regex.Match(output,"\\(.*?\\)").ToString(),@"(\s+|\(|\))","");
        Debug.Log(value);
        string[] values = Regex.Split(value,",");

        return new Vector2(int.Parse(values[0]),int.Parse(values[1]));
    }

}
