using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Text;
using TMPro;


public class userstudyController : MonoBehaviour
{

    public bool DebugEvent = true;
    public string csv_path = "Assets/Flightplans/flightplan_new.csv";
    //public bool DebugCollision = true;

    public GameObject dronePrefab;
    public GameObject eventPrefab;

    public List<CsvRow> flightPlan = new List<CsvRow>();
    public int flightPlanIndex = 0;

    public float displayTime = 0;
    public float userClicks = 0;

    public float totalCollisionPrevented = 0;

    // total collisions
    public float totalCollisionCount = 0;

    // black
    public float missedCollisionCount = 0;

    // red
    public float flightplanCollisionCount = 0;

    // purple
    public float unplannedCollisionCount = 0;

    
    private float userstudyStartTime;
    private bool userstudyRunning = false;

    private MetricLogger Logger;

    public class CsvRow
    {
        public int eventID;
        public int droneID;
        public float startTime;

        public float startingPositionsX;
        public float startingPositionsY;
        public float startingPositionsZ;

        public float endingPositionX;
        public float endingPositionY;
        public float endingPositionZ;

        public int collidesWithNum;
        public float[] collidesAtPos;
        public float collidesAtTime;
        public int collidesWithEvent;

        public CsvRow(string[] values)
        {
            eventID = strToInt(values[0]);
            droneID = strToInt(values[1]);
            startTime = strToFloat(values[2]);

            startingPositionsX = strToFloat(values[3]);
            startingPositionsY = strToFloat(values[4]);
            startingPositionsZ = strToFloat(values[5]);

            endingPositionX = strToFloat(values[6]);
            endingPositionY = strToFloat(values[7]);
            endingPositionZ = strToFloat(values[8]);

            collidesWithNum = strToInt(values[9]);
            //collidesAtPos = ;
            collidesAtTime = strToFloat(values[11]);
            collidesWithEvent = strToInt(values[12]);
        }

        public static int strToInt(string str)
        {
            int numVal = -1;
            try
            {
                if (str == "none")
                {
                    return -1;
                }
                numVal = Int32.Parse(str);
            }
            catch (FormatException e)
            {
                Debug.Log("Invalid Seed file." + e);
            }
            return numVal;
        }

        public static float strToFloat(string str)
        {
            float numVal = -1;
            try
            {
                if (str == "none")
                {
                    return -1;
                }
                numVal = float.Parse(str);
            }
            catch (FormatException e)
            {
                Debug.Log("Invalid Seed file." + e);
            }
            return numVal;
        }
    }

    public void ReadCsv()
    {
        StreamReader csv_reader = new StreamReader(csv_path);

        string header = csv_reader.ReadLine();

        while (!csv_reader.EndOfStream)
        {
            string line = csv_reader.ReadLine();
            string[] values = line.Split(',');

            if (!string.IsNullOrEmpty(line))
            {
                CsvRow csvRow = new CsvRow(values);
                flightPlan.Add(csvRow);
            }
        }

    }

    public void CreateEvent(CsvRow eventData, float time)
    {
        Vector3 startingPosition = new Vector3(eventData.startingPositionsX, eventData.startingPositionsY, eventData.startingPositionsZ);
        Vector3 endingPosition = new Vector3(eventData.endingPositionX, eventData.endingPositionY, eventData.endingPositionZ);

        GameObject eventVar = Instantiate(eventPrefab, endingPosition, Quaternion.identity);
        eventVar.name = eventData.eventID.ToString();
        eventVar.GetComponent<eventController>().InitTask(eventData.droneID);

        GameObject drone = Instantiate(dronePrefab, startingPosition, Quaternion.identity);
        drone.name = eventData.droneID.ToString();
        drone.GetComponent<droneController>().InitTask(startingPosition, endingPosition, eventData.startTime, eventData.droneID, eventData.eventID, eventData.collidesWithNum, eventData.collidesAtTime, eventVar, Logger );

        if (DebugEvent)
        {
            Debug.LogFormat("Event {0} : Drone {1} : Time {2} :Start at {3} End at {4} ", eventData.eventID, eventData.droneID, time, startingPosition, endingPosition);
        }

    }

    public void AllSafe()
    {
        foreach(GameObject drone in GameObject.FindGameObjectsWithTag("drone"))
        {
            drone.GetComponent<droneController>().Click();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        ReadCsv();
        Debug.Log("CSV completely read");


        Logger = this.GetComponent<MetricLogger>();

        userstudyStartTime = Time.time;
        userstudyRunning = true;
        Debug.LogFormat("Starting study at {0}", userstudyStartTime);
    }

    // Update is called once per frame
    void Update()
    {
        if (userstudyRunning)
        {
            float currentTime = Time.time - userstudyStartTime;
            displayTime = currentTime;

            if (flightPlan[flightPlanIndex].startTime < currentTime)
            {
                CreateEvent(flightPlan[flightPlanIndex],currentTime);
                flightPlanIndex += 1;

                if (flightPlanIndex >= flightPlan.Count)
                {
                    userstudyRunning = false;
                    Debug.Log("All events created");
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            AllSafe();
            userClicks += 1;
            Logger.AddUserClick(Time.time.ToString(), userClicks.ToString());
        }

        Logger.AddFlightStatus(Time.time.ToString(), csv_path, totalCollisionCount.ToString(), totalCollisionPrevented.ToString(), flightplanCollisionCount.ToString(), unplannedCollisionCount.ToString(), userClicks.ToString());
    }
}
