using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Navigate3DVR : MonoBehaviour
{
    //pivot is the center of the scene
    public GameObject pivot;
    //originalScale is the original localScale of the world
    public Vector3 originalScale;
    //This is the 1/10th of the originalScale of the world
    public Vector3 minScale;
    //This is the 10 times the originalScale of the world
    public Vector3 maxScale;
    //currentScale is the current localScale
    public Vector3 currentScale;

    // current world
    public GameObject rotatingTable;

    // Rotation
    public LinkedList<float> angles;
    public bool handleHeldTrigger = false;
    public OVRInput.Controller currentController;
    private Vector3 oldVec;
    private float movementAngle;
    public float movementAngleDecay = .95f;
    private float rotSpeed = 0.05f;  // Rotation speed(in rev/s)
    private float scalingSpeed = 0.05f;
    //This is the speed at which the map can be moved at
    public float speed = 3;

    // Start is called before the first frame update
    void Start()
    {
        //Pivot assignment
        pivot = GameObject.FindWithTag("pivot");
        originalScale = transform.localScale;
        minScale = Vector3.Scale(originalScale, new Vector3(0.1F, 0.1F, 0.1F));
        maxScale = Vector3.Scale(originalScale, new Vector3(10F, 10F, 10F));
    }

    // Update is called once per frame
    void Update()
    {
        MoveCamera();
        ControllerRotateWorld();
    }

    private void MoveCamera()
    {
        // Control lateral movement
        float moveX = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick).x;
        float moveZ = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick).y;

        // Control height
        float moveY = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick).y;

        Vector3 position = transform.position;
        if (moveX != 0 || moveZ != 0)
        {
            // update map position based on input
            position.x += moveX * speed * Time.deltaTime;
            position.z += moveZ * speed * Time.deltaTime;
            transform.position = position;
        }

        if (moveY != 0)
        {
            position.y += moveY * speed * Time.deltaTime;
            transform.position = position;
        }
    }


    private void ControllerRotateWorld()
    {
        float deltaX = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick).x;

        // We only consider inputs above a certain threshold.
        if (Mathf.Abs(deltaX) > 0.2f)
        {

            float angle = deltaX * rotSpeed * 360 * Time.fixedDeltaTime;

            transform.RotateAround(pivot.transform.position, Vector3.up, angle);

            if (rotatingTable)
            {
                rotatingTable.transform.RotateAround(pivot.transform.position, Vector3.up, angle);
            }

        }
    }
}
