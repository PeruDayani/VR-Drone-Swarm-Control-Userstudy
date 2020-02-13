using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK.UnityEventHelper;
using VRTK;

public class Interaction3DVR : MonoBehaviour
{
    private GameObject curDrone;
    private GameObject prevDrone;
    public GameObject controllerRight;

    // Bit shift the index of the layer (8) to get a bit mask
    private int layerMask = 1 << 8;

    // Start is called before the first frame update
    void Start()
    {
        controllerRight = GameObject.FindGameObjectWithTag("controller_right");
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        RaycastHit destinationHit = controllerRight.GetComponent<VRTK_StraightPointerRenderer>().GetDestinationHit();
        if (controllerRight.GetComponent<VRTK_Pointer>().IsStateValid() && destinationHit.collider != null)
        {
            if (destinationHit.collider.gameObject.name.Contains("warningSphere"))
            {
                //Debug.Log("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
                curDrone = destinationHit.collider.gameObject.transform.parent.gameObject;
                Debug.Log(curDrone);
                bool bDown = OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger);
                Debug.Log(bDown);

                if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger))
                {
                    Debug.Log("Clicked!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
                    /**
                    if (curDrone.GetComponent<DroneProperties>().classPointer.safetyStatus == Drone.SafetyStatus.NOT_SAFE) // 0: not safe zone
                    {
                        curDrone.GetComponent<DroneProperties>().classPointer.RaiseDrone();
                    }
                    else if (curDrone.GetComponent<DroneProperties>().classPointer.safetyStatus == Drone.SafetyStatus.SAFE) // 2: safe zone
                    {
                        curDrone.GetComponent<DroneProperties>().classPointer.LowerDrone();
                    }
                    else
                    {
                        Debug.Log("Current status: " + curDrone.GetComponent<DroneProperties>().classPointer.safetyStatus);
                    }
                    **/
                }

                if (prevDrone == null)
                {
                    prevDrone = curDrone;
                }

                if (curDrone != prevDrone)
                {
                    // set previous drone game object to original
                    //prevDrone.GetComponent<DroneProperties>().classPointer.DroneCollideRender(false);
                    prevDrone = curDrone;
                }
                //curDrone.GetComponent<DroneProperties>().classPointer.DroneCollideRender(true);
            }
            
        }
        else
        {
            /**
            if (curDrone != null)
                curDrone.GetComponent<DroneProperties>().classPointer.DroneCollideRender(false);
            **/
        }
    }
}
