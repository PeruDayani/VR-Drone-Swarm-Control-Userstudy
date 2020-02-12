using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using TMPro;

public class droneController : MonoBehaviour
{

    public TextMeshPro droneDisplay;
    public TextMeshPro positionX;
    public TextMeshPro positionY;
    public TextMeshPro positionZ;

    // Flight Variables
    public Vector3 spawnPosition;
    public Vector3 destPosition;
    public float journeyLength;
    public int droneID;
    public int eventID;
    public int droneCollisionID;
    float startTime;
    bool eventActive = false;

    // Movement speed in units per second.
    // Same speed as used in Python script
    public float speed = 2.0F;

    public void InitTask(Vector3 initSpawnPosition, Vector3 initDestPosition, float initStartTime, int initDroneID, int initEventID, int initDroneCollisionID)
    {
        // Display Name
        droneDisplay.text = initDroneID.ToString();

        // Init variables
        spawnPosition = initSpawnPosition;
        destPosition = initDestPosition;
        startTime = initStartTime;
        droneID = initDroneID;
        eventID = initEventID;
        droneCollisionID = initDroneCollisionID;

        if (droneCollisionID == -2)
        {
            this.GetComponent<Renderer>().material.color = Color.green;
        }
        else
        {
            this.GetComponent<Renderer>().material.color = Color.blue;
        }


        // Start Task
        journeyLength = Vector3.Distance(spawnPosition, destPosition);
        eventActive = true;

        //Debug.LogFormat("Drone {0} activated", droneID);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == droneCollisionID.ToString())
        {
            this.GetComponent<Renderer>().material.color = Color.red;
            Debug.LogFormat("COLLISION Drone {0} with Drone {1}", this.droneID, other.gameObject.name);
            GameObject.Find("UserstudyController").GetComponent<userstudyController>().plannedCollisionCount += 0.5f;
            GameObject.Find("UserstudyController").GetComponent<userstudyController>().totalCollisionCount += 0.5f;
        }
        else
        {
            this.GetComponent<Renderer>().material.color = Color.yellow;
            Debug.LogFormat("COLLISION Drone {0} with Drone {1}", this.droneID, other.gameObject.name);
            GameObject.Find("UserstudyController").GetComponent<userstudyController>().totalCollisionCount += 0.5f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (eventActive)
        {
            // Distance moved equals elapsed time times speed..
            float distCovered = (Time.time - startTime) * speed;

            // Fraction of journey completed equals current distance divided by total distance.
            float fractionOfJourney = distCovered / journeyLength;

            // Set our position as a fraction of the distance between the markers.
            transform.position = Vector3.Lerp(spawnPosition, destPosition, fractionOfJourney);

            if (fractionOfJourney > 0.999)
            {
                //Debug.LogFormat("Drone {0} completed Event {1}", droneID, eventID);
                Destroy(gameObject);
            }

            // Update Debug Location
            positionX.text = "X: " + this.transform.position.x.ToString();
            positionY.text = "Y: " + this.transform.position.y.ToString();
            positionZ.text = "Z: " + this.transform.position.z.ToString();

        }
    }
}
