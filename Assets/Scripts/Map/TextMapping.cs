
using UnityEngine;

[System.Serializable]
public class TextMapping
{ 
    public char character;
    public bool useRandom;
    public GameObject[] prefabs;


    public TextMapping(){}

    public TextMapping(TextMapping precomputed)
    {
        character = precomputed.character;
        useRandom = precomputed.useRandom;
        prefabs = precomputed.prefabs;
    }
    
}
