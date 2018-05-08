using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The purpose of this script is to pan a angled camera from right to left
/// and forward and back using the "AWSD" keys. It also limits the panning
/// distance that we allow the player to do.
/// 
/// FIXES NEEDED:
/// 1. The speed from pressing two different buttons at the same time seem
/// to be different from just pressing one movement key.
/// 
/// 2. This script has to be tested for other angles in case of rotation being added.
/// 
/// 6. Calculation function should be moved to another script
/// 
/// 7. As the zoom changes we should move the actual clamp to represent the newly
/// calculated clamp.
/// 
/// PROBLEMS FIXED:
/// 3. When we get the stage out of view it gets replaced with a blue soild color
/// due to the fact that the camera isnt viewing it anymore. Desired effect is that
/// the stage just moves into the corner smoothing not like it is being overtaken.
/// 
/// 4. There has to be a limit to how far the player can pan away from the stage.
/// 
/// 5. If you are on the right side of the screen then pressing 2 buttons
/// at the same time to pan will make you move out of range.
/// 
/// SOLUTIONS:
/// 3. Make sure that when translating the camera you don't change the z-axis position.
/// When you do that there is a chance that the camera and the stage will be overlapping
/// on the same plane so the border becomes what ever the camera is meant to view by default.
/// 
/// 4. By testing both the x and y directions with the horizontal and vertical float numbers
/// we can contiously track to see if the position of the camera has passed the x clamped position
/// and the y clamp position. We then change the direction vector's variables x and y to 0 respectable.
/// This make it so the translate doesn't happen on them.
/// 
/// 5. By changing the if statement condition to also account for when you horizontal and vertical 
/// numbers are equal to zero it stops the situation in the corner.
/// 
/// </summary>

public class CameraPan : MonoBehaviour {

    // Determines at what speed the camera will pan
    public float panSpeed = 50f;

    // How far the player can pan in the x-axis and y-axis
    public float xClamp = 35f;
    public float yClamp = 20f;

    // To accessed the deltaZoom inside the Camera Zoom.
    private CameraZoom cameraZoom;

    private float horizontal;
    private float vertical;

    private float originalPanSpeed;
    private float originalXClamp;
    private float originalYClamp;

    // The Vectors indicting the direction of the camera in a angle and not global space
    private Vector3 forward;
    private Vector3 right;
    private Vector3 up;

	// Use this for initialization
	void Start () {

        cameraZoom = GetComponent<CameraZoom>();

        originalPanSpeed = panSpeed;
        originalXClamp = xClamp;
        originalYClamp = yClamp;

        // Gets the local forward angle of the camera. Typically the z-axis (blue line in scence view)
        forward = transform.forward;
        forward = Vector3.Normalize(forward);

        //Locks down the y-axis so it doesn't move as much
        forward.y = 0.0f;

        // Creates a vector with the forward angle now rotated 90 degress on the x-axis(to the right)
        right = Quaternion.Euler(new Vector3(0, 90, 0)) * forward;

        // Creates a vector with the forward angle now rotated 45 degress on the y-axis(upward)
        up = Quaternion.Euler(new Vector3(0, 0, 45)) * forward;
	}
	
	// Update is called once per frame
	void Update ()
    {
        // Calls this function when a key is pressed
        if (Input.anyKey)
            Pan();
    }

    // LateUpdate is called after both Update and FixedUpdate has ran
    private void LateUpdate()
    {
        if(cameraZoom.Zoomed != 0)
        {
            xClamp = calculation(originalXClamp);
            yClamp = calculation(originalYClamp);
            panSpeed = calculation(originalPanSpeed);
        }
        else
        {
            xClamp = originalXClamp;
            yClamp = originalYClamp;
            panSpeed = originalPanSpeed;
        }
    }

    // Calculates new values as the player zooms in and out of the screen:  
    private float calculation(float value)
    {
        //Turns the change in zoom into a ratio
        float zoomRatio = (cameraZoom.Zoomed / 100) * 5;

        //Gets the ratio based of the original value and the zoomRatio previously
        float valueRatio = value * zoomRatio;

        //Subtracts the ratio of the original value above with the original value
        value -= valueRatio;

        return value;
    }

    private void Pan()
    {
        // If "A" or "D" is pressed then horizontal gets changed to 1.
        horizontal = Input.GetAxisRaw("Horizontal");

        // If "W" or "S" is pressed then Vertical gets changed to 1.
        vertical = Input.GetAxisRaw("Vertical");

        // Set to vector right if horizontal is 1.
        Vector3 rightMovement = right * horizontal;

        // Set to vector up if vertical is 1.
        Vector3 upMovement = up * vertical;

        // Takes the movement of the two vectors and normilize it to 1.
        // POSSIBLE SOLUTION to problem 1. Needs to be tested more.
        Vector3 direction = Vector3.Normalize(rightMovement + upMovement) * panSpeed * Time.deltaTime;
        
        // Clamps the direction vector.
        direction = panClamp(direction);

        // Moves the camera. Look up transform.Translate() on unity API if needed.
        transform.Translate(new Vector3(direction.x, direction.y, 0.0f));
    }

    private Vector3 panClamp(Vector3 direction)
    {
        // Calculates if we should be moving in a certain direction with 1's and 0's
        float tempX = horizontal + vertical;
        float tempY = vertical - horizontal;

        // To determine if the player has moved to far left and right.
        if (transform.position.x + tempX < -xClamp && tempX <= 0)
            direction.x = 0;
        
        if (transform.position.x + tempX > xClamp && tempX >= 0) 
            direction.x = 0;

        // To determine if the player has moved to far up or down.
        if (transform.position.y + tempY < -yClamp  && tempY <= 0)
            direction.y = 0;

        if (transform.position.y + tempY > yClamp && tempY >= 0)
            direction.y = 0;

        return direction;
    }
}
