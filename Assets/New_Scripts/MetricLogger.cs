using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System;

public class MetricLogger : MonoBehaviour
{
    // Common Logs
    public string name;

    // In userstudyController
    private List<string[]> rowData_UserClick = new List<string[]>();
    private List<string[]> rowData_FlightStats = new List<string[]>();

    // In droneController
    private List<string[]> rowData_DroneCollisions = new List<string[]>();
    private List<string[]> rowData_DroneSaved = new List<string[]>();

    // In each cameraController
    private List<string[]> rowData_CameraTransform = new List<string[]>();
    

    // Use this for initialization
    void Start()
    {
        //Save();
        InitUserClick();
        InitDroneCollisions();
        InitDroneSaved();
        InitFlightStats();
        InitCameraTransform();
    }

    void InitUserClick()
    {
        // Creating First row of titles manually..
        string[] rowDataTemp = new string[2];
        rowDataTemp[0] = "Time";
        rowDataTemp[1] = "Click No";
        rowData_UserClick.Add(rowDataTemp);
    }

    void InitDroneCollisions()
    {
        // Creating First row of titles manually..
        string[] rowDataTemp = new string[4];
        rowDataTemp[0] = "Time";
        rowDataTemp[1] = "Drone 1";
        rowDataTemp[2] = "Drone 2";
        rowDataTemp[3] = "Planned";
        rowData_DroneCollisions.Add(rowDataTemp);
    }

    void InitDroneSaved()
    {
        // Creating First row of titles manually..
        string[] rowDataTemp = new string[4];
        rowDataTemp[0] = "Time";
        rowDataTemp[1] = "Drone 1";
        rowDataTemp[2] = "Drone 2";
        rowDataTemp[3] = "Planned";
        rowData_DroneSaved.Add(rowDataTemp);
    }

    void InitFlightStats()
    {
        // Creating First row of titles manually..
        string[] rowDataTemp = new string[7];
        rowDataTemp[0] = "Time";
        rowDataTemp[1] = "CSV Path";
        rowDataTemp[2] = "Total Planned Collisions";
        rowDataTemp[3] = "Total Planned Collisions Saved";
        rowDataTemp[4] = "Total Planned Collisions Collided";
        rowDataTemp[5] = "Total Collisions";
        rowDataTemp[6] = "User Click Count";
        rowData_FlightStats.Add(rowDataTemp);
    }

    void InitCameraTransform()
    {
        // Creating First row of titles manually..
        string[] rowDataTemp = new string[8];
        rowDataTemp[0] = "Time";
        rowDataTemp[1] = "Pos X";
        rowDataTemp[2] = "Pos Y";
        rowDataTemp[3] = "Pos Z";
        rowDataTemp[4] = "Rot X";
        rowDataTemp[5] = "Rot Y";
        rowDataTemp[6] = "Rot Z";
        rowDataTemp[7] = "Rot W";
        rowData_CameraTransform.Add(rowDataTemp);
    }

    void SaveHelper(List<string[]> rowData, string csvName)
    {
        string[][] output = new string[rowData.Count][];

        for (int i = 0; i < output.Length; i++)
        {
            output[i] = rowData[i];
        }

        int length = output.GetLength(0);
        string delimiter = ",";

        StringBuilder sb = new StringBuilder();

        for (int index = 0; index < length; index++)
            sb.AppendLine(string.Join(delimiter, output[index]));


        string filePath = Application.dataPath + "/CSV/" + csvName + ".csv";

        StreamWriter outStream = System.IO.File.CreateText(filePath);
        outStream.WriteLine(sb);
        outStream.Close();

        Debug.Log(csvName + " Written");
    }

    void Save()
    {
        string currTime = Time.time.ToString();
        SaveHelper(rowData_UserClick, name + "UserData");
        SaveHelper(rowData_DroneCollisions, name + "DroneCollisionData");
        SaveHelper(rowData_DroneSaved, name + "DroneSavedData");
        SaveHelper(rowData_FlightStats, name + "FlightStatsData");
        SaveHelper(rowData_CameraTransform, name + "CameraTransformData");
    }

    public void AddUserClick(string time, string data)
    {
        string[] rowDataTemp = new string[2];
        rowDataTemp[0] = time;
        rowDataTemp[1] = data;
        rowData_UserClick.Add(rowDataTemp);
    }

    public void AddDroneCollision(string time, string droneA, string droneB, string planned)
    {
        string[] rowDataTemp = new string[4];
        rowDataTemp[0] = time;
        rowDataTemp[1] = droneA;
        rowDataTemp[2] = droneB;
        rowDataTemp[3] = planned;
        rowData_DroneCollisions.Add(rowDataTemp);
    }

    public void AddDroneSaved(string time, string droneA, string droneB, string planned)
    {
        string[] rowDataTemp = new string[4];
        rowDataTemp[0] = time;
        rowDataTemp[1] = droneA;
        rowDataTemp[2] = droneB;
        rowDataTemp[3] = planned;
        rowData_DroneSaved.Add(rowDataTemp);
    }

    public void AddFlightStatus(string time, string csvPath, string totalPlannedCollisions, string totalPlannedCollisionsSaved, string totalPlannedCollisionsCollided, string totalCollisions, string userClickCount)
    {
        string[] rowDataTemp = new string[7];
        rowDataTemp[0] = time;
        rowDataTemp[1] = csvPath;
        rowDataTemp[2] = totalPlannedCollisions;
        rowDataTemp[3] = totalPlannedCollisionsSaved;
        rowDataTemp[4] = totalPlannedCollisionsCollided;
        rowDataTemp[5] = totalCollisions;
        rowDataTemp[6] = userClickCount;
        rowData_FlightStats.Add(rowDataTemp);
    }

    public void AddCameraTransform(string time, Transform transform)
    {
        string[] rowDataTemp = new string[8];
        rowDataTemp[0] = time;
        rowDataTemp[1] = transform.position.x.ToString();
        rowDataTemp[2] = transform.position.y.ToString();
        rowDataTemp[3] = transform.position.z.ToString();
        rowDataTemp[4] = transform.rotation.x.ToString();
        rowDataTemp[5] = transform.rotation.y.ToString();
        rowDataTemp[6] = transform.rotation.z.ToString();
        rowDataTemp[7] = transform.rotation.w.ToString();
        rowData_CameraTransform.Add(rowDataTemp);
    }

    private void OnApplicationQuit()
    {
        Save();
    }
}
