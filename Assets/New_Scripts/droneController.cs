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
    public Material collisionMaterial;
    public Material hoverMaterial;
    public Material safeMaterial;
    public Material flashMaterial;
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
    public float droneCollisionTime;
    float startTime;
    bool eventActive = false;
    public GameObject eventObj;

    // Global Variables set through Start via GlobalVariables script
    GlobalVariables globalVariables;

    [Header("Drone Variables")]
    public float speed = 2.0f;
    public float totalSafeTime = 100.0f;
    public float flashTime = 2.0f;

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

    //Logger
    private MetricLogger Logger;
    private userstudyController userstudyController;

    public void InitTask(Vector3 initSpawnPosition, Vector3 initDestPosition, float initStartTime, int initDroneID, int initEventID, int initDroneCollisionID, float initDroneCollilisionTime ,GameObject initEventObj, MetricLogger initLogger)
    {
        //Logger
        Logger = initLogger;

        // Display Name
        droneDisplay.text = initDroneID.ToString();

        // Init variables
        spawnPosition = initSpawnPosition;
        destPosition = initDestPosition;
        startTime = initStartTime;
        droneID = initDroneID;
        eventID = initEventID;
        droneCollisionID = initDroneCollisionID;
        droneCollisionTime = initDroneCollilisionTime;
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

        if (collided)
        {
            return;
        }

        startSafeTime = Time.time;
        safe = true; 
        prevMaterial = this.GetComponent<Renderer>().material;
        this.GetComponent<Renderer>().material = safeMaterial;
        Debug.Log("Drone " + this.name + " safe.");
    }
    public void StartHover()
    {
        if (safe || collided)
        {
            return;
        }
        if (hover == false)
        {
            prevMaterial = this.GetComponent<Renderer>().material;
            this.GetComponent<Renderer>().material = hoverMaterial;
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
        // Upon ANY collision
        userstudyController.totalCollisions += 0.5f;

        if (collided)
        {
            return;
        }


        //Collision with a drone
        if (other.gameObject.tag == "drone")
        {
            // Collision prevented: not happening for now
            if (this.safe || other.gameObject.GetComponent<droneController>().safe)
            {
                //GameObject.Find("UserstudyController").GetComponent<userstudyController>().totalCollisionPrevented += 0.5f;

                // Preplanned collision saved
                if (other.gameObject.name == droneCollisionID.ToString())
                {

                    Debug.LogFormat("SAFE Drone {0} and Drone {1}", this.droneID, other.gameObject.name);

                    // Successful saved a planned collision
                    Logger.AddDroneSaved(Time.time.ToString(), this.name, other.gameObject.name, "1");
                    userstudyController.plannedCollisions_Saved += 0.5f;
                    userstudyController.plannedCollisions += 0.5f;
                    this.collided = true;

                    prevMaterial = flashMaterial;
                    this.GetComponent<Renderer>().material = flashMaterial;
                    Destroy(this.gameObject, flashTime);
                }
                else
                {
                    Logger.AddDroneSaved(Time.time.ToString(), this.name, other.gameObject.name, "0");
                    //GameObject.Find("UserstudyController").GetComponent<userstudyController>().unplannedCollisionCount += 0.5f;
                }

            }
            // Preplanned Collision
            else if (other.gameObject.name == droneCollisionID.ToString())
            {

                Debug.LogFormat("PREPLANNED COLLISION Drone {0} with Drone {1}", this.droneID, other.gameObject.name);
                this.collided = true;

                if (OnCollision_RedBubble)
                {
                    this.GetComponent<Renderer>().material = collisionMaterial;
                    Destroy(this.gameObject, flashTime);
                }
                else
                {
                    Destroy(eventObj.gameObject);
                    Destroy(this.gameObject);
                }

                // A planned collision happening
                userstudyController.plannedCollisions_Collided += 0.5f;
                userstudyController.plannedCollisions += 0.5f;

                Logger.AddDroneCollision(Time.time.ToString(), this.name, other.gameObject.name, "1");
            }
            // Unplanned Collision
            else 
            {
                //this.GetComponent<Renderer>().material.color = Color.magenta;
                Debug.LogFormat("UNPLANNED COLLISION Drone {0} with Drone {1}", this.droneID, other.gameObject.name);

                //GameObject.Find("UserstudyController").GetComponent<userstudyController>().unplannedCollisionCount += 0.5f;

                Logger.AddDroneCollision(Time.time.ToString(), this.name, other.gameObject.name, Vector3.Distance(this.transform.position, other.gameObject.transform.position).ToString() );

            }
        }
    }

    private void Start()
    {
        globalVariables = GameObject.Find("UserstudyController").GetComponent<GlobalVariables>();
        userstudyController = GameObject.Find("UserstudyController").GetComponent<userstudyController>();


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
                if (this.droneCollisionID != -2)
                {
                    this.GetComponent<Renderer>().material.color = Color.magenta;
                    Destroy(this.gameObject, flashTime);
                }
                else
                {
                    Destroy(gameObject);
                }
            }

            // Update Debug Location
            positionX.text = "X: " + this.transform.position.x.ToString();

            string helperY = this.transform.position.y.ToString();

            if (helperY.Length < 4)
            {
                positionY.text = "Y: " + helperY;
            }
            else
            {
                positionY.text = "Y: " + helperY.Substring(0, 4);
            }

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
            Vector3 offsetPosition = new Vector3(0.0f, -0.7f, 1.0f);

            arrow.transform.position = this.transform.position + offsetPosition;
            arrow.transform.LookAt(destPosition);
            arrow.transform.rotation *= Quaternion.Euler(offsetRotation);
        }


        /**
        if(Time.time > droneCollisionTime + 1.0f && collided == false && droneCollisionID !=-2)
        {
            GameObject.Find("UserstudyController").GetComponent<userstudyController>().missedCollisionCount += 0.5f;
            this.GetComponent<Renderer>().material.color = Color.black;
            Logger.AddDroneCollision(Time.time.ToString(), this.name, "-", "-1");
            collided = true;
        }
        **/  
    }
}
