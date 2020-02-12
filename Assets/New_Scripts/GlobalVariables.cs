using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalVariables : MonoBehaviour
{

    [Header("Drone Variables")]
    public float droneSpeed = 2.0f;
    public float safeTime = 100.0f;

    [Header("UI Features")]
    public bool UI_Position = true;
    public bool UI_EnableArrows = true;
    //public bool UI_EnaleLineRender = true;
    public bool UI_EnableWarningSphere = true;
    public bool UI_EnableWarningDrone = true;
    public float UI_WarningBound = 2.0f;

    [Header("Interaction Features")]
    //public bool Interact_Pause = false;
    //public bool Interact_GreenBubble = false;
    //public bool Interact_FlyUp = false;
    //public bool Interact_FlyUpDiagonal = true;


    [Header("Collision Logic")]
    public bool OnCollision_Disappear = true;
    public bool OnCollision_RedBubble = false;


    [Header("Debug Helpers")]
    public bool FlightDebugTrip = false;
    public bool FlightDebugCol = false;
    public bool FlightPlanDebug = false;
    public bool TextDebug = false;

}
