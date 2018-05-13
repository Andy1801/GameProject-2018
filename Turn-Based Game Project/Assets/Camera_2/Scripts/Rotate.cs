using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script rotates the grid by 90 degrees to either the left or the right
/// using the "X"(right) and "Z"(left) keys.
/// 
/// FIXES NEEDED:
/// If needed we should modifiy this so that you can rotate the screen by any
/// specific angle.
/// 
/// ERROR:
/// This script doesn't work with our current setup as to be reworked.
/// 
/// </summary>

public class Rotate : MonoBehaviour {

    public float lerpTime;
    public float differenceNeeded;

    // Variables that focus on the amount you rotate
    private const int rotationIncrease = 90;
    private int rotationAngle;
    private int previousAngle;

    private int direction;
    private bool rotating;

    // The new rotational position of the object 
    private Vector3 eulerAngle;

    private Vector3 tempRotation;

	// Use this for initialization
	void Start () {
        rotationAngle = 0;

        direction = 0;
        rotating = false;

        eulerAngle = Vector3.zero;
	}
	
	// Update is called once per frame
	void Update () {
        // Increase the angle you would rotate by. e.g if at 90 rotate to 180
		if (Input.GetKeyDown(KeyCode.X) && !rotating)
        {
            direction = 1;
            previousAngle = rotationAngle;
            rotationAngle += rotationIncrease;
        }
        // Decrease the angle you would rotate by. e.g if at 90 rotate to 0
        else if(Input.GetKeyDown(KeyCode.Z) && !rotating)
        {
            direction = -1;
            previousAngle = rotationAngle;
            rotationAngle -= rotationIncrease;
        }

        
        RotateObject();
	}

    /// <summary>
    /// Rotates the object from the previous rotational angle to the current rotation angle.
    /// </summary>
    private void RotateObject()
    {
        // If not already rotating and the direction is not equal to zero then set new angle
        // and previous angle to start rotating
        if (!rotating && direction != 0)
        {
            eulerAngle = new Vector3(0.0f, rotationAngle, 0.0f);
            tempRotation = new Vector3(0.0f, previousAngle, 0.0f);
            rotating = true;
        }

        // If rotating lerp from the previous rotation to the newest one. Then calculate
        // The difference based on wether it is moving left or right.
        if(rotating)
        {
            tempRotation = Vector3.Lerp(tempRotation, eulerAngle, lerpTime);
            transform.rotation = Quaternion.Euler(tempRotation);
            float difference = 0f;

            if (direction > 0)
            {
                difference = eulerAngle.y - tempRotation.y;
            }
            else
            {
                difference = tempRotation.y - eulerAngle.y;
            }

            // If the current difference is less then the difference needed then
            // set the new rotation and state that we are no longer rotating.
            if (difference <= differenceNeeded)
            {
                transform.rotation = Quaternion.Euler(eulerAngle);
                rotating = false;
                direction = 0;
            }
        }
    }
}
