using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class droneWarningHelper : MonoBehaviour
{

    public Material whiteMaterial;
    public Material purpleMaterial;
    public GameObject warningSphereHelper;

    GlobalVariables globalVariables;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "warning")
        {
            if (globalVariables.UI_EnableWarningSphere)
            {
                this.GetComponent<MeshRenderer>().material = purpleMaterial;
            }
            if (globalVariables.UI_EnableWarningDrone)
            {
                warningSphereHelper.GetComponent<MeshRenderer>().material = purpleMaterial;

            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "warning")
        {
            if (globalVariables.UI_EnableWarningSphere)
            {
                this.GetComponent<MeshRenderer>().material = whiteMaterial;
            }
            if (globalVariables.UI_EnableWarningDrone)
            {
                warningSphereHelper.GetComponent<MeshRenderer>().material = null;
            }
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        globalVariables = GameObject.Find("UserstudyController").GetComponent<GlobalVariables>();
        this.gameObject.SetActive(globalVariables.UI_EnableWarningSphere || globalVariables.UI_EnableWarningDrone);
        this.gameObject.transform.localScale = Vector3.one * globalVariables.UI_WarningBound;
    }

}
