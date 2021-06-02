using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class CameraScript : MonoBehaviour
{
    // Start is called before the first frame update

    private Transform playerTransform;
    private Camera ZoomCamera;
    public float scrollVelocity;
    public float dragSpeed;

    private bool freeMovement=false;

    public bool fixedCamera;

    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        ZoomCamera = Camera.main;
    } 

    // Update is called once per frame
    void Update()
    {
        if(ZoomCamera == null)
        {
            return;
        }
        if(ZoomCamera.orthographic)
        {
            ZoomCamera.orthographicSize -= Input.GetAxis("Mouse ScrollWheel") * scrollVelocity;
            if(ZoomCamera.orthographicSize <= 0f)
            {
                ZoomCamera.orthographicSize = 0.5f;
            }
        }
        else
        {
            ZoomCamera.fieldOfView -= Input.GetAxis("Mouse ScrollWheel") * scrollVelocity;
            if(ZoomCamera.fieldOfView <= 0f)
            {
                ZoomCamera.fieldOfView = 0.5f;
            }
        }        
    }

    public void ChangeCameraMovement()
    {
        freeMovement = !freeMovement;
    }

    public void ChangeCameraAnchor()
    {
        fixedCamera = !fixedCamera;
    }

    void LateUpdate()
    {
        if(!fixedCamera)
        {
            if(Input.GetKeyUp(KeyCode.F))
            {
                ChangeCameraMovement();
            }

            if(Input.GetKeyUp(KeyCode.G))
            {
                ChangeCameraAnchor();
            }

            if(freeMovement)
            {
                if (Input.GetMouseButton(1))
                {
                    var newPosition = new Vector3();
                    newPosition.x = Input.GetAxis("Mouse X") * dragSpeed * Time.deltaTime;
                    newPosition.y = Input.GetAxis("Mouse Y") * dragSpeed * Time.deltaTime;
                    transform.Translate(-newPosition);
                }
                
            }
            else
            {
                Vector3 temp = transform.position;

                temp.x = playerTransform.position.x;
                temp.y = playerTransform.position.y;

                transform.position = temp;
            } 
        }
        else
        {
            Vector3 temp = transform.position;

            temp.x = GameObject.FindGameObjectWithTag("MapManager").GetComponent<TextToMap>().GetMapWidht()/2-1;
            temp.y = -GameObject.FindGameObjectWithTag("MapManager").GetComponent<TextToMap>().GetMapHeight()/2;

            transform.position = temp;
        }
    }
}
