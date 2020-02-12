using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraController3D : MonoBehaviour
{

    protected Transform xFormCamera;
    protected Transform xFormParent;

    protected Vector3 localRot;
    protected float cameraDistance = 10f;

    public float mouseSensitivity = 4f;
    //public float scrollSensitivity = 2f;
    public float orbitSpeed = 10f;
    //public float scrollSpeed = 6f;
    //public bool cameraDisabled = false;
    float moveSpeed = 50.0f; //for move



    // Drone Interaction Variables
    public Camera camera;
    private GameObject prevObj = null;
    // Start is called before the first frame update
    void Start()
    {
        this.xFormCamera = this.transform;
        this.xFormParent = this.transform.parent;
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButton(1))
        {
            orbitSpeed = 10f;
            transform.RotateAround(xFormParent.position, Vector3.up, Input.GetAxis("Mouse X") * orbitSpeed);
            transform.RotateAround(xFormParent.position, Vector3.up, Input.GetAxis("Mouse Y") * orbitSpeed);
        }

        orbitSpeed = 0;
        moveCamera();
        clickDrone();
    }


    void clickDrone()
    {
        RaycastHit[] hits;
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        hits = Physics.RaycastAll(ray.origin, ray.direction, 100.0F);

        foreach (RaycastHit hit in hits)
        {
            GameObject obj = hit.collider.gameObject;

            // Set prevObj
            if (prevObj == null)
            {
                prevObj = obj;
            }
            // Update object hovering
            else if (prevObj != obj)
            {

                if (prevObj.tag == "drone")
                {
                    prevObj.GetComponent<droneController>().EndHover();

                }

                if (obj.tag == "drone")
                {
                    obj.GetComponent<droneController>().StartHover();
                }
                prevObj = obj;
            }

            // On click
            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("Clicked: " + obj.name);
                if (obj.tag == "drone")
                {
                    obj.GetComponent<droneController>().EndHover();
                    obj.GetComponent<droneController>().Click();
                }
            }
        }
    }

    // Move parameters
    void moveCamera()
    {
        Vector3 p = GetMoveInput();
        // Debug.Log("Camera" + p);
        p = p * moveSpeed;
        p = p * Time.deltaTime;
        Vector3 newPosition = transform.position;
        transform.Translate(p);
    }




    private Vector3 GetMoveInput()
    {
        Vector3 p_Velocity = new Vector3();
        //if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.W))
        //{
        //    p_Velocity += new Vector3(0, 0, 1);
        //}
        //if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.S))
        //{
        //    p_Velocity += new Vector3(0, 0, -1);
        //}
        p_Velocity += new Vector3(0, 0, Input.GetAxis("Mouse ScrollWheel") * 5f);

        if (Input.GetKey(KeyCode.A))
        {
            p_Velocity += new Vector3(-1, 0, 0);
        }
        if (Input.GetKey(KeyCode.D))
        {
            p_Velocity += new Vector3(1, 0, 0);
        }
        if (Input.GetKey(KeyCode.W))
        {
            p_Velocity += new Vector3(0, 0, 1);
        }
        if (Input.GetKey(KeyCode.S))
        {
            p_Velocity += new Vector3(0, 0, -1);
        }
        return p_Velocity;
    }
}

