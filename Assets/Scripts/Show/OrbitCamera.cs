using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitCamera: MonoBehaviour
{
    public Transform target; //What to rotate around
    public float distance = 5.0f; //How far away to orbit
    public float xSpeed = 125.0f; //X sensitivity
    public float ySpeed = 50.0f; //Y sensitivity
    public float zoom = 0.25f; // чувствительность при увеличении, колесиком мышки
    public float zoomMax = 10; // макс. увеличение
    public float zoomMin = 3; // мин. увеличение

    private float x = 0.0f; //Angle of the y rotation?
    private float y = 0.0f; //Angle of the x rotation?

    private Vector3 p_Velocity;
    public float mainSpeed = 0.1f; //regular speed

    void Start()
    {
        //Initialize the angles
        var angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;
        //Btn3D.onBtnDown += onBtnDown;
    }

    void onBtnDown(string btn)
    {
        Debug.Log("Camera btn down" + btn);
    }

    void LateUpdate()
    {
        //Every frame, do this as late as you can
        if (target)
        {
            if (Input.GetAxis("Mouse ScrollWheel") > 0) distance += zoom;
            else if (Input.GetAxis("Mouse ScrollWheel") < 0) distance -= zoom;
            distance = Mathf.Clamp(distance, -Mathf.Abs(zoomMax), -Mathf.Abs(zoomMin));

            if (Input.GetMouseButton(1) || Input.GetKey(KeyCode.LeftShift))
            { 
                //There's a target
                //Change the angles by the mouse movement
                x += Input.GetAxis("Mouse X") * xSpeed * 0.02f;
                y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;

            }

            //Rotate the camera to those angles 
            var rotation = Quaternion.Euler(y, x, 0);
            transform.rotation = rotation;

            //Keyboard commands
            /*GetBaseInput();*/

            var position = rotation * new Vector3(0.0f, 0.0f, distance) + target.position;
            transform.position = position + p_Velocity * mainSpeed;
        }
    }

    public void init(float camDistanse, Vector3 cameraPos, Vector3 cameraRot)
    {
        distance = camDistanse;
        x = cameraRot.y;
        y = cameraRot.x;
    }

    private void GetBaseInput()
    {

        float vert = Input.GetAxis("Vertical");
        float hor = Input.GetAxis("Horizontal");
        //p_Velocity += new Vector3(-vert, 0, hor);

        p_Velocity += vert * Camera.main.transform.forward + hor * Camera.main.transform.right;

        return;
        if (Input.GetKey(KeyCode.D))
        {
            p_Velocity += new Vector3(0, 0, 1);
        }
        if (Input.GetKey(KeyCode.A))
        {
            p_Velocity += new Vector3(0, 0, -1);
        }
        if (Input.GetKey(KeyCode.W))
        {
            p_Velocity += new Vector3(-1, 0, 0);
        }
        if (Input.GetKey(KeyCode.S))
        {
            p_Velocity += new Vector3(1, 0, 0);
        }
    }
}
