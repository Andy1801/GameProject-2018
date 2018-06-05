using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotate : MonoBehaviour {

    private Game_Manager gameManager;

    private Vector3 axisPoint;

    private float gridCenter;
    private float direction;
    private float rotation;
    private bool rotating;

    private void Start()
    {
        gameManager = GameObject.FindWithTag("GameManager").GetComponent<Game_Manager>();

        axisPoint = Vector3.zero;

        gridCenter = gameManager.dimensions / 2;
        direction = 0;
        rotation = transform.eulerAngles.y;
        rotating = false;
    }

    private void Update()
    {

        if(Input.GetKeyDown(KeyCode.A) && !rotating)
        {
            //Create the rotate function here.
            direction = 1;
            if (rotation == 270f)
                rotation = 0f;
            else
                rotation += 90f;
        }

        if (Input.GetKeyDown(KeyCode.D) && !rotating)
        {
            direction = -1;
            if (rotation == 0f)
                rotation = 270f;
            else
                rotation -= 90f;
        }

        RotateObject();
    }

    private void RotateObject()
    {
        // If not already rotating and the direction is not equal to zero then set new angle
        // and previous angle to start rotating
        if (!rotating && direction != 0)
        {
            axisPoint = new Vector3(gridCenter, 0.0f, gridCenter);   
            rotating = true;
        }

        // If rotating lerp from the previous rotation to the newest one. Then calculate
        // The difference based on wether it is moving left or right.
        if (rotating)
        {
            transform.RotateAround(axisPoint, Vector3.up, 90f * direction * Time.deltaTime);

            float difference = Vector3.Distance(transform.eulerAngles, new Vector3(0.0f, rotation, 0.0f));

            Debug.Log("Euler Angle: " + transform.eulerAngles.y);
            Debug.Log("Rotation: " + rotation);
            Debug.Log("Difference: " + difference);

            // If the current difference is less then the difference needed then
            // set the new rotation and state that we are no longer rotating.
            if (difference > 1)
            {
                rotating = false;
                direction = 0;
            }
        }
    }

}
