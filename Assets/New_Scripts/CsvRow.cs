using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Text;
using TMPro;


public class CsvRow : MonoBehaviour
{
    public int eventID;
    public int droneID;

    public int startingLaunchpadIndexX;
    public int startingLaunchpadIndexY;
    public int startingLaunchpadPos;

    public int shelfIndexX;
    public int shelfIndexY;
    public int sheldIndexPos;

    public int followedBy;

    public int teleportToLaunchpadIndexX;
    public int teleportToLaunchpadIndexY;
    public int teleportToLaunchpadIndexPos;

    public int collidesWithNum;
    public float[] collidesAtPos;
    public float collidesAtTime;
    public int collidesWithEvent;

    public CsvRow(string[] values)
    {
        eventID = strToInt(values[0]);
        droneID = strToInt(values[1]);

        startingLaunchpadIndexX = strToInt(values[2]);
        startingLaunchpadIndexY = strToInt(values[3]);
        startingLaunchpadPos = startingLaunchpadIndexX + startingLaunchpadIndexY * 10;

        shelfIndexX = strToInt(values[4]);
        shelfIndexY = strToInt(values[5]);
        sheldIndexPos = shelfIndexX + shelfIndexY * 10;

        followedBy = strToInt(values[6]);

        teleportToLaunchpadIndexX = strToInt(values[7]);
        teleportToLaunchpadIndexY = strToInt(values[8]);
        teleportToLaunchpadIndexPos = teleportToLaunchpadIndexX + teleportToLaunchpadIndexY * 10;

        collidesWithNum = strToInt(values[9]);
        //collidesAtPos = 10;
        collidesAtTime = strToFloat(values[11]);
        collidesWithEvent = strToInt(values[12]);

    }

    public List<int> ToEvent()
    {
        List<int> event_values = new List<int> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

        event_values[0] = eventID;
        event_values[1] = droneID; 
        event_values[2] = startingLaunchpadIndexX + startingLaunchpadIndexY * 10;
        event_values[3] = shelfIndexX + shelfIndexY * 10;
        event_values[4] = followedBy;
        event_values[5] = teleportToLaunchpadIndexX + teleportToLaunchpadIndexY * 10;
        event_values[6] = collidesWithNum;

        return event_values;
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
