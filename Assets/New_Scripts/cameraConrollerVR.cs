using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraConrollerVR : MonoBehaviour
{
    private MetricLogger Logger;

    private void Start()
    {
        Logger = GameObject.FindGameObjectWithTag("GameController").GetComponent<MetricLogger>();
    }

    // Update is called once per frame
    void Update()
    {
        Logger.AddCameraTransform(Time.time.ToString(), this.transform);
    }
}
