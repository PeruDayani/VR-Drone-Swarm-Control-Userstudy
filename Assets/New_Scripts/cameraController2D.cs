using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraController2D : MonoBehaviour
{

    // Camera Movement Variables
    public bool zoomFunc = false;
    public bool transFunc = false;
    public bool clickFunc = false;
    public float moveSpeed = 50.0f;

    // Drone Interaction Variables
    public Camera camera;
    private GameObject prevObj = null;

    void Update()
    {

        if (transFunc)
        {
            moveCamera();
        }

        if (zoomFunc)
        {
            zoomCamera();
        }

        if (clickFunc)
        {
            clickDrone();
        }
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


    void zoomCamera()
    {
        Vector3 p = new Vector3();
        p += new Vector3(0, 0, Input.GetAxis("Mouse ScrollWheel") * 10f);

        p = p * moveSpeed;
        p = p * Time.deltaTime;
        Vector3 newPosition = transform.position;
        transform.Translate(p);
    }
    void moveCamera()
    {
        Vector3 p = GetMoveInput();

        p = p * moveSpeed;
        p = p * Time.deltaTime;
        Vector3 newPosition = transform.position;
        transform.Translate(p);
    }

    private Vector3 GetMoveInput()
    {
        Vector3 p_Velocity = new Vector3();

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
            p_Velocity += new Vector3(0, 1, 0);
        }
        if (Input.GetKey(KeyCode.S))
        {
            p_Velocity += new Vector3(0, -1, 0);
        }
        return p_Velocity;
    }
}
