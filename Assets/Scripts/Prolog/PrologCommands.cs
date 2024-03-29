using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrologCommands 
{
    private PrologCommands(string value) { Value = value; }

    public string Value { get; private set; }

    // Getters
    public static string GetAgentKnowlodge { get { return (new PrologCommands("get_agent_knowlodge.")).Value; }}
    public static string GetAgentState { get { return (new PrologCommands("get_agent_state.")).Value; }}

    public static string Loop { get { return (new PrologCommands("loop.")).Value; }} 

    public static string GetAgentMode { get { return (new PrologCommands("get_agent_mode.")).Value; }} 
    

}
