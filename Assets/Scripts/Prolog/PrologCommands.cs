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

    // Setters --- This part is ONLY for test purposes 
    public static string Shoot { get { return (new PrologCommands("shoot.")).Value; }}
    public static string TakeDamage { get { return (new PrologCommands("take_damage(30).")).Value; }}

    public static string Move { get { return (new PrologCommands("move.")).Value; }}
    
    

}
