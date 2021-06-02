using UnityEngine;
using System.Text.RegularExpressions;
using System;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SbsSW.SwiPlCs;



public class TextToMap : MonoBehaviour
{
    // Character to Images and Costs
    public TextMapping[] mappingData;

    public Dictionary<Vector2, List<GameObject>> AllMapTiles;
    public Dictionary<Vector2, List<GameObject>> KnowTiles;
    public Dictionary<Vector2, GameObject> FogTiles;
    public Dictionary<Vector2, GameObject> Doubts;

    public string charectersToIgnore;

    public bool useDistribuitionOnRandom;

    public int qtdSmallEnemy;
    public int qtdBigEnemy;
    public int qtdHoles;
    public int qtdTeleporter;
    public int qtdPowerUp;
    public int qtdGold;
   
    public int XSize;
    public int YSize;

    public char smallEnemyChar;
    public char bigEnemyChar;
    public char holeChar;
    public char teleporterChar;
    public char powerUpChar;
    public char goldChar;
    public char groundChar;

    public bool GenerateRandomMap;

    public GameObject[] outsideTiles;

    public DoubtTilesRep[] DoubtTiles;
    
    // Text file
    public TextAsset mapText;
    
    // Characters Configuration
    public char spawnChar;

    public GameObject fogTile;

    // Path tiles
    //public GameObject pathTile;
    //public GameObject openTile;
    //public GameObject closedTile;
   
    // String to map representation
    string[] rows;

    // Variables to turn off and on path tiles

    System.Random rnd = new RandomProportional();
 
    void OnEnable()
    {
        if(MapConfigurations.Map!= null)
        {
            Debug.Log("New Map!!");
            rows = Regex.Split(MapConfigurations.Map,"\r\n|\r|\n");
        }
        else
        {
            rows = Regex.Split(mapText.text, "\r\n|\r|\n");
        }
        
       // Gyms = LoadGyms();
        AllMapTiles = new Dictionary<Vector2, List<GameObject>>();
        KnowTiles = new Dictionary<Vector2, List<GameObject>>();
        FogTiles = new Dictionary<Vector2, GameObject>();
        Doubts = new Dictionary<Vector2, GameObject>();
        GenerateMap();

        GameObject.FindGameObjectWithTag("PrologController").GetComponent<PrologController>().instantiateProlog();
        Debug.Log(GameObject.FindGameObjectWithTag("PrologController").GetComponent<PrologController>().getStringUpdate());
    }

    public void clearTile(Vector2 position)
    {
        List<GameObject> aux;
        if(AllMapTiles.TryGetValue(position, out aux))
        {
            GameObject groundTile = aux[0];
            for (int i = 1; i < aux.Count(); i++)
            {
                Destroy(aux[i]);
            }
            aux.Clear();
            aux.Add(groundTile);
        }
    }

    public void setDoubt(char doubtChar, Vector2 position)
    {
        foreach(var doubt in DoubtTiles)
        {
            if(doubtChar==doubt.tilechar)
            {
                GameObject aux;
                if(!Doubts.TryGetValue(position,out aux))
                {
                    Doubts.Add(position,  Instantiate(doubt.TileObject, position, Quaternion.identity, transform));
                }
                
            }
        }
    }

    public void SetAllActive(bool active)
    {
        foreach(KeyValuePair<Vector2, List<GameObject>> kvp in AllMapTiles)
        {
            foreach(GameObject obj in kvp.Value)
            {
                obj.SetActive(active);
            }
        }
    }

    public TextMapping getTile(Vector2 position)
    {
        int indx = Math.Abs((int)position.x);
        int idxy = Math.Abs((int)position.y);
        if(idxy  >= rows.Length ||idxy < 0 || indx>= rows[0].Length || indx <0 )
        {
            return new TextMapping(mappingData[0]);
        }
        
        char c = rows[idxy][indx];

        foreach (TextMapping tiletype in mappingData)
        {
            if (tiletype.character == c)
            {
                return tiletype;
            }
        }
        return mappingData[0];
    }

    public Vector2 getSpawnPoint()
    {
        Vector2 curr = new Vector2(0, 0);
        foreach (string row in rows)
        {
            foreach (char c in row)
            {
                foreach (TextMapping tm in mappingData)
                {
                    if (c == spawnChar)
                    {
                        return new Vector2(curr.x, curr.y);
                    }
                }
                curr = new Vector2(++curr.x, curr.y);
            }
            curr = new Vector2(0, --curr.y);
        }
        return new Vector2(0, 0);
    }

    private void generateType(int qtd, char tile, Dictionary<Vector2, char> mappedDict, List<Vector2> generalLimitations = null, List<Tuple<Vector2, char>> relativeLimitations=null)
    {
        System.Random r = new System.Random();
        Dictionary<Vector2, char> sortedTiles = new Dictionary<Vector2, char>();
        int randomQtd = 0;
        char aux;
        while(randomQtd < qtd)
        {   
            Vector2 position = new Vector2(r.Next(XSize), r.Next(YSize));
            if(generalLimitations!=null && generalLimitations.Contains(position))
                continue;
            
            if(relativeLimitations!= null)
            {
                int qtdInfrations=0;
                bool infration = false;

                for (int i = 0; i < relativeLimitations.Count; i++)
                {
                    Tuple<Vector2, char> lim = relativeLimitations[i];
                    Vector2 checkPosition = position+lim.Item1;
                    if(mappedDict.TryGetValue(checkPosition, out aux))
                    {
                        if(aux == lim.Item2)
                        {
                            qtdInfrations++;
                        }
                    }

                    if(checkPosition.x <0 || checkPosition.x >XSize-1 || checkPosition.y<0 || checkPosition.y>YSize-1)
                    {
                        qtdInfrations++;
                    }
                    
                    if(qtdInfrations == 4)
                    {
                        infration=true;
                    }
                }
                if(infration)
                {
                    continue;
                }
            }

            if(!mappedDict.TryGetValue(position, out aux))
            {
                mappedDict.Add(position,tile);
                randomQtd++;
            }
        }

    }

    private void GenerateMap()
    {
        if(GenerateRandomMap)
        {
            
            Dictionary<Vector2, char> sortedTiles = new Dictionary<Vector2, char>();

            string newmap = string.Empty;

            string possiblevalues = ""+smallEnemyChar+bigEnemyChar+holeChar+teleporterChar+powerUpChar+goldChar;
            
            string header = string.Empty;
            header += '=' + new string('3',XSize+4)+"A\n";
            header += "4-" + new string('2',XSize+2) + "BL\n";
            header += "45+" + new string('1',XSize) + "CKL";

            string footer = string.Empty;
            footer+="45)"+new string('7',XSize) + ":KL\n";
            footer+="\\("+new string('8',XSize+2) + ";\\\n";

            string leftSide = "456";
            string rightSide = "JKL\n";
            
            Vector2[] spawnProtection ={new Vector2(0,0), new Vector2(0,1), new Vector2(1,0)};
            List<Tuple<Vector2, char>> goldProtection = new List<Tuple<Vector2, char>>()
            {
                Tuple.Create(new Vector2(-1,0), holeChar),
                Tuple.Create(new Vector2(1,0),  holeChar),
                Tuple.Create(new Vector2(0,-1), holeChar),
                Tuple.Create(new Vector2(0,-1), holeChar),

                Tuple.Create(new Vector2(-1,0), smallEnemyChar),
                Tuple.Create(new Vector2(1,0),  smallEnemyChar),
                Tuple.Create(new Vector2(0,-1), smallEnemyChar),
                Tuple.Create(new Vector2(0,-1), smallEnemyChar),

                Tuple.Create(new Vector2(-1,0), bigEnemyChar),
                Tuple.Create(new Vector2(1,0),  bigEnemyChar),
                Tuple.Create(new Vector2(0,-1), bigEnemyChar),
                Tuple.Create(new Vector2(0,-1), bigEnemyChar)
            };

            generateType(qtdSmallEnemy, smallEnemyChar, sortedTiles, new List<Vector2>(spawnProtection));
            generateType(qtdBigEnemy, bigEnemyChar, sortedTiles, new List<Vector2>(spawnProtection));
            generateType(qtdHoles,holeChar, sortedTiles, new List<Vector2>(spawnProtection));
            generateType(qtdTeleporter, teleporterChar, sortedTiles, new List<Vector2>(spawnProtection));
            generateType(qtdPowerUp, powerUpChar, sortedTiles, new List<Vector2>(spawnProtection));
            
            generateType(qtdGold, goldChar, sortedTiles, new List<Vector2>(spawnProtection), goldProtection);

            for(int j = YSize-1; j>=0; j--)
            {
                newmap+=leftSide;
                for(int i = 0; i < XSize ; i++)
                {

                    Vector2 position = new Vector2(i,j);
                    char aux;
                    if(!sortedTiles.TryGetValue(position,out aux))
                    {
                        aux = groundChar;
                        if(i==0&&j==0)
                        {
                            aux = spawnChar;
                        }
                        sortedTiles.Add(position, aux);
                    }
                    newmap+=aux;
                }
                newmap+=rightSide;
            }      
            newmap = header+'\n'+newmap+footer;
            rows = Regex.Split(newmap,"\r\n|\r|\n");
        }
        Vector2 currentPosition = new Vector2(0, 0);

        foreach (string row in rows)
        {
            foreach (char c in row)
            {
                foreach (TextMapping tm in mappingData)
                {
                    if (c == tm.character)
                    {
                        List<GameObject> instantiatedGameObjects = new List<GameObject>();
                        
                        GameObject o;
                        if (tm.useRandom)
                        {
                            int tileindex = rnd.Next(0, tm.prefabs.Length); 
                            o = Instantiate(tm.prefabs[tileindex], currentPosition, Quaternion.identity, transform);
                            o.GetComponent<SpriteRenderer>().sortingOrder=0;
                            instantiatedGameObjects.Add(o);
                        }
                        else
                        {
                            int i =0;
                            foreach (GameObject prefab in tm.prefabs)
                            {
                                o = Instantiate(prefab, currentPosition, Quaternion.identity, transform);
                                o.GetComponent<SpriteRenderer>().sortingOrder=i++ ;
                                instantiatedGameObjects.Add(o);
                                
                            }
                        }
                        if(!charectersToIgnore.Contains(c))
                        {
                            FogTiles.Add(currentPosition, Instantiate(fogTile, currentPosition, Quaternion.identity, transform));
                            AllMapTiles.Add(currentPosition, instantiatedGameObjects);
                        }
                       
                    }                    
                }

                currentPosition = new Vector2(++currentPosition.x, currentPosition.y);
            }
            currentPosition = new Vector2(0, --currentPosition.y);
        }
        SetAllActive(false);

        for (int i = -60; i < GetMapWidht()+60; i++)
        {
            for (int j = -60-GetMapHeight(); j<60;j++)
            {
                if(!((i>-1 && i<GetMapWidht()) && (j<1 && j>-GetMapHeight()+1)))
                {
                    int tileindex = rnd.Next(0, outsideTiles.Length); 
                    Instantiate(outsideTiles[tileindex], new Vector3(i,j,0), Quaternion.identity, transform);
                }
            }
        }
    }
    
    public void revealTile(Vector2 position)
    {
        List<GameObject> aux_know;
        List<GameObject> aux_all;
        GameObject aux_fog;

        if(AllMapTiles.TryGetValue(position, out aux_all))
        {
            foreach (var item in aux_all)
            {
                item.SetActive(true);
            } 
            if(!AllMapTiles.TryGetValue(position, out aux_know))
            {
                KnowTiles.Add(position, aux_all);
            } 
        }
        if(FogTiles.TryGetValue(position, out aux_fog))
        {
            aux_fog.SetActive(false);
        }
    }


    public int GetMapWidht()
    {
        return rows[0].Length;
    }

    public int GetMapHeight()
    {
        return rows.Length;
    }

    private bool mapIsValid(string map)
    {
        return true;
    }
}



public class RandomProportional : System.Random
{
    // The Sample method generates a distribution proportional to the value
    // of the random numbers, in the range [0.0, 1.0].
    protected override double Sample()
    {
        return Math.Sqrt(base.Sample());
    }

    public override int Next()
    {
       return (int) (Sample() * int.MaxValue);
    }
}
