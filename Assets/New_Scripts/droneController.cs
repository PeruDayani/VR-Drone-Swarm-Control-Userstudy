using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using TMPro;

public class droneController : MonoBehaviour
{

    // Drone ID and position display
    public TextMeshPro droneDisplay;
    public TextMeshPro positionX;
    public TextMeshPro positionY;
    public TextMeshPro positionZ;

    // Drone Materials
    public Material redMaterial;
    public Material blueMaterial;
    public Material greenMaterial;
    public Material whiteMaterial;
    public Material prevMaterial;

    // Drone UI Elements
    public GameObject arrow;

    // Flight Variables set though InitTask call
    public Vector3 spawnPosition;
    public Vector3 destPosition;
    public float journeyLength;
    public int droneID;
    public int eventID;
    public int droneCollisionID;
    float startTime;
    bool eventActive = false;
    public GameObject eventObj;

    // Global Variables set through Start via GlobalVariables script
    GlobalVariables globalVariables;

    [Header("Drone Variables")]
    public float speed = 2.0f;
    public float totalSafeTime = 100.0f;

    [Header("UI Features")]
    public bool UI_EnableArrows = true;
    public bool UI_Position = true;
    //public bool UI_EnableLineRender = true;
    //public bool UI_EnableWarning = true;
    //public float UI_WarningBound = 6.0f;

    [Header("Collision Logic")]
    public bool OnCollision_Disappear = true;
    public bool OnCollision_RedBubble = false;

    // Drone State Variables
    public bool safe = false;
    public bool collided = false;
    public bool hover = false;
    public float startSafeTime = Mathf.Infinity;

    public void InitTask(Vector3 initSpawnPosition, Vector3 initDestPosition, float initStartTime, int initDroneID, int initEventID, int initDroneCollisionID, GameObject initEventObj)
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
        this.eventObj = initEventObj;

        if (droneCollisionID == -2)
        {
            this.GetComponent<Renderer>().material = whiteMaterial;
        }
        else
        {
            this.GetComponent<Renderer>().material = whiteMaterial;
        }


        // Start Task
        journeyLength = Vector3.Distance(spawnPosition, destPosition);
        eventActive = true;

        //Debug.LogFormat("Drone {0} activated", droneID);
    }

    public void Click()
    {
        startSafeTime = Time.time;
        safe = true; 
        prevMaterial = this.GetComponent<Renderer>().material;
        this.GetComponent<Renderer>().material = greenMaterial;
        Debug.Log("Drone " + this.name + " safe.");


    }
    public void StartHover()
    {
        if (safe)
        {
            return;
        }
        if (hover == false)
        {
            prevMaterial = this.GetComponent<Renderer>().material;
            this.GetComponent<Renderer>().material = blueMaterial;
            Debug.Log("Start Hover");
        }
        hover = true;
    }
    public void EndHover()
    {
        if (hover)
        {
            this.GetComponent<Renderer>().material = prevMaterial;
            hover = false;
            Debug.Log("End Hover");
        }
    }
    void OnTriggerEnter(Collider other)
    {
        //Collision with a drone
        if (other.gameObject.tag == "drone")
        {
            // Collision prevented
            if (this.safe || other.gameObject.GetComponent<droneController>().safe)
            {
                //Collision prevented
                Debug.LogFormat("SAFE Drone {0} and Drone {1}", this.droneID, other.gameObject.name);
            }
            // Preplanned Collision
            else if (other.gameObject.name == droneCollisionID.ToString())
            {

                Debug.LogFormat("COLLISION Drone {0} with Drone {1}", this.droneID, other.gameObject.name);
                collided = true;

                if (OnCollision_RedBubble)
                {
                    this.GetComponent<Renderer>().material = redMaterial;
                }
                else
                {
                    Destroy(eventObj.gameObject);
                    Destroy(this.gameObject);
                }

                GameObject.Find("UserstudyController").GetComponent<userstudyController>().plannedCollisionCount += 0.5f;
                GameObject.Find("UserstudyController").GetComponent<userstudyController>().totalCollisionCount += 0.5f;

            }
            // Unplanned Collision
            else 
            {
                this.GetComponent<Renderer>().material.color = Color.yellow;
                //Debug.LogFormat("COLLISION Drone {0} with Drone {1}", this.droneID, other.gameObject.name);
                GameObject.Find("UserstudyController").GetComponent<userstudyController>().totalCollisionCount += 0.5f;
            }
        }
    }

    private void Start()
    {
        globalVariables = GameObject.Find("UserstudyController").GetComponent<GlobalVariables>();

        this.speed = globalVariables.droneSpeed;
        this.totalSafeTime = globalVariables.safeTime;

        this.UI_Position = globalVariables.UI_Position;
        if (!UI_Position)
        {
            droneDisplay.gameObject.SetActive(UI_Position);
            positionX.gameObject.SetActive(UI_Position);
            positionY.gameObject.SetActive(UI_Position);
            positionZ.gameObject.SetActive(UI_Position);
        }
        this.UI_EnableArrows = globalVariables.UI_EnableArrows ;
        if (!UI_EnableArrows)
        {
            arrow.SetActive(false);
        }
        //this.UI_WarningBound = globalVariables.UI_WarningBound ;
        //this.UI_EnableWarning = globalVariables.UI_EnableWarning ;
        //this.UI_EnableLineRender = globalVariables.UI_EnableLineRender ;

        this.OnCollision_Disappear = globalVariables.OnCollision_Disappear;
        this.OnCollision_RedBubble = globalVariables.OnCollision_RedBubble;

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

        if (safe)
        {
            if (Time.time > startSafeTime + totalSafeTime)
            {
                safe = false;
                startSafeTime = Mathf.Infinity;
                Debug.Log("Drone " + this.name + " unsafe.");
                Material temp = prevMaterial;
                prevMaterial = this.GetComponent<Renderer>().material;
                this.GetComponent<Renderer>().material = temp;
            }
        }

        if (UI_EnableArrows)
        {
            Vector3 offsetRotation = new Vector3(0, 90, 0);
            Vector3 offsetPosition = new Vector3(0.0f, -0.7f, 0.0f);

            arrow.transform.position = this.transform.position + offsetPosition;
            arrow.transform.LookAt(destPosition);
            arrow.transform.rotation *= Quaternion.Euler(offsetRotation);
        }

    }
}
