using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class eventController : MonoBehaviour
{
    public int droneID = 0;

    public void InitTask(int drone)
    {
        droneID = drone;
        this.transform.Rotate(0.0f, 180.0f, 0.0f, Space.Self);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == droneID.ToString())
        {
            Destroy(this.gameObject);
        }
        else
        {
            // Event spawning on each other, we can ignore this.
            // Previous drone hitting this event, we can ignore this.
        }
    }

}
