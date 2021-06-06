using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Threading;

public class PlayerMovement : MonoBehaviour
{

    public Rigidbody2D rb;
    public Animator animator_controler;
    public bool lockPlayer = true;

    public float WaitTime;
    
    private bool isMoving;
    private Vector3 origPos,targetPos;
    private float defaultTimeMovement = 0.2f;


    // Start is called before the first frame update 
    void Start()
    {

        rb = GetComponent<Rigidbody2D>();
        
        Vector2 spawn = GameObject.FindGameObjectWithTag("MapManager").GetComponent<TextToMap>().getSpawnPoint();
        transform.position=spawn;

        GameObject.FindGameObjectWithTag("MapManager").GetComponent<TextToMap>().revealTile(spawn);

        GameObject.FindGameObjectWithTag("HUD_LOG").GetComponent<TextSetter>().setText("Teste");
        PrologController prologController = GameObject.FindGameObjectWithTag("PrologController").GetComponent<PrologController>();
        // prologController.runNextTurn();
        prologController.updateUI();

    }

    // Update is called once per frame
    void Update()
    {
        if(!lockPlayer) {
            if(Input.GetKey(KeyCode.W) )
            {
                TriggerMovement(Vector3.up);
            }
            if(Input.GetKey(KeyCode.A))
            {
                TriggerMovement(Vector3.left);
            }
            if(Input.GetKey(KeyCode.S))
            {
                TriggerMovement(Vector3.down);
            }
            if(Input.GetKey(KeyCode.D))
            {
                TriggerMovement(Vector3.right);
            }
            if(Input.GetKeyUp(KeyCode.Space))
            {
                PrologController prologController = GameObject.FindGameObjectWithTag("PrologController").GetComponent<PrologController>();
                prologController.runNextTurn();

                StartCoroutine(TurnPlayer(prologController.getPrologPlayerDirection()));
            
                Vector2 newPlayerPosition =  prologController.getWorldPlayerNewPosition();
                Vector2 direction = newPlayerPosition - new Vector2(transform.position.x, transform.position.y);

                if(direction.magnitude > 1)
                {
                    teleport(newPlayerPosition);
                }
                else if (direction.magnitude > 0)
                {
                    TriggerMovement(direction);
                }
                
                prologController.updateUI();
            }
        }

        if(Input.GetKeyUp(KeyCode.KeypadPlus) || Input.GetKeyUp(KeyCode.Equals))
        {
            MinusTimeOnAnimation();
        }
        if(Input.GetKeyUp(KeyCode.KeypadMinus) ||Input.GetKeyUp(KeyCode.Minus))
        {
            PlusTimeOnAnimation();
        }
    }

    public IEnumerator TurnPlayer(string prolog_direction)
    {
        if(prolog_direction == "north")
        {
            animator_controler.SetBool("Up",true);
            yield return new WaitForSecondsRealtime(0.1f);
            animator_controler.SetBool("Up",false);
        }
        else if(prolog_direction == "east")
        {
            animator_controler.SetBool("Right",true);
            yield return new WaitForSecondsRealtime(0.1f);
            animator_controler.SetBool("Right",false);
        }
        else if(prolog_direction == "west")
        {
            animator_controler.SetBool("Left",true);
            yield return new WaitForSecondsRealtime(0.1f);
            animator_controler.SetBool("Left",false);
        }
        else if(prolog_direction == "south")
        {
            animator_controler.SetBool("Down",true);
            yield return new WaitForSecondsRealtime(0.1f);
            animator_controler.SetBool("Down",false);
        }
    }


    public void MinusTimeOnAnimation()
    {
        WaitTime-=0.1f;
        if(WaitTime<0)
        {
            WaitTime=0;
        }
    }
    
    public void PlusTimeOnAnimation()
    {
        WaitTime+=0.1f;
    }


    private void TriggerMovement(Vector3 movementVector ) {
        if(!isMoving) {
            StartCoroutine(MovePlayer(movementVector));
        }
    }

    private string getAnimationName(Vector3 movementVector) {
        bool areEquals(Vector3 vA, Vector3 vB)
        {
            return vA.x == vB.x && vA.y == vB.y && vA.z == vB.z;
        }
        if(areEquals(movementVector, Vector3.up)) {
            return "Up";
        }
        if(areEquals(movementVector, Vector3.left)) {
            return "Left";
        }
        if(areEquals(movementVector, Vector3.down)) {
            return "Down";
        }
        if(areEquals(movementVector, Vector3.right)) {
            return "Right";
        }
        return "";
    }

    private void teleport(Vector2 position)
    {
        transform.position = new Vector3(position.x, position.y);
    }

    private IEnumerator MovePlayer(Vector3 direction)
    {
        isMoving = true;
        string animationDirection = getAnimationName(direction);
        animator_controler.SetBool(animationDirection,true);
        animator_controler.SetBool("Moving", true);

        float elapsedTime = 0;

        origPos = transform.position;
        targetPos = origPos + direction;

        var MapManager = GameObject.FindGameObjectWithTag("MapManager").GetComponent<TextToMap>();

        float timeMovement = defaultTimeMovement ;       
        
        while(elapsedTime < timeMovement )
        {
            transform.position = Vector3.Lerp(origPos, targetPos, (elapsedTime/timeMovement));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        //MapManager.changeTile(targetPos);

        transform.position = targetPos;
        animator_controler.SetBool(animationDirection,false);
        animator_controler.SetBool("Moving", false);
        isMoving=false;
        GameObject.FindGameObjectWithTag("MapManager").GetComponent<TextToMap>().revealTile(targetPos);
    }
}
