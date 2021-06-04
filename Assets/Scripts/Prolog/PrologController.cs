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
        process.RunCommand("loop.");
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

 
        TextToMap mapController = GameObject.FindGameObjectWithTag("MapManager").GetComponent<TextToMap>();
    
        output = getStringKnowlodgeUpdate();
        var matches = Regex.Matches(output,"\\[(\\(\\d+,\\d+\\),*)*\\]");
        values = new string[matches.Count];
        for (int i =0 ; i<matches.Count; i++)
        {
            values[i] = Regex.Replace(matches[i].ToString(), @"(\s+|\[|\])","");
        }

        Vector2 spawn = mapController.getSpawnPoint();
        
        mapController.ClearAllDoubts();

        for (int i = 0; i < matches.Count; i++)
        {
            var newmatches = Regex.Matches(values[i],"\\d+,\\d+");
            foreach (var match in newmatches)
            {
                
                value = match.ToString();
                if(value == null || value == "" || value == ",")
                    continue;
  
                string[] positions = value.Split(',');
                Vector2 prologPoint = new Vector2(int.Parse(positions[0]), int.Parse(positions[1]));
                Vector2 worldPoint = new Vector2(prologPoint.x-1 + spawn.x, spawn.y-1 + prologPoint.y);
                switch (i)
                {
                    case 0:
                        mapController.setDoubt('D', worldPoint);
                        break;
                    case 1:
                        mapController.setDoubt('T', worldPoint);
                        break;
                    case 2:
                        mapController.setDoubt('P', worldPoint);
                        break;
                    case 3:                        
                        mapController.setDoubt('.', worldPoint);
                        break;
                    case 4:
                        mapController.setDoubt('d', worldPoint);
                        break;
                    case 5:
                        mapController.setDoubt('t', worldPoint);
                        break;
                    case 6: 
                        mapController.setDoubt('p', worldPoint);
                        break;
                    case 7:
                        mapController.clearDoubt(worldPoint);
                        break;
                    default:
                        continue;
                }
            }
        }
        
        

        

    }

    public Vector2 getPrologPlayerPosition()
    {
        string output = getStringAgentStateUpdate();
        string value = Regex.Replace(Regex.Match(output,"\\(.*?\\)").ToString(),@"(\s+|\(|\))","");
        string[] values = Regex.Split(value,",");

        return new Vector2(int.Parse(values[0]),int.Parse(values[1]));
    }

}
