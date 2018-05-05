using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The purpose of this script is to pan a angled camera from right to left
/// and forward and back using the "AWSD" keys.
/// 
/// FIXES NEEDED:
/// 1. The speed from pressing two different buttons at the same time seem
/// to be different from just pressing one movement key.
/// 
/// 2. This has to be tested for other angles in case of rotation being added.
/// 
/// </summary>

public class CameraPan : MonoBehaviour {

    // Determines at what speed the camera will pan
    public float panSpeed;

    // The Vectors indicting the direction of the camera in a angle and not global space
    private Vector3 forward;
    private Vector3 right;
    private Vector3 up;

	// Use this for initialization
	void Start () {
        
        // Gets the local forward angle of the camera. Typically the z-axis (blue line in scence view)
        forward = transform.forward;
        forward = Vector3.Normalize(forward);

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

    private void Pan()
    {
        // If "A" or "D" is pressed then horizontal gets changed to 1.
        float horizontal = Input.GetAxisRaw("Horizontal");

        // If "W" or "S" is pressed then Vertical gets changed to 1.
        float vertical = Input.GetAxisRaw("Vertical");

        // Set to vector right if horizontal is 1.
        Vector3 rightMovement = right * horizontal;

        // Set to vector up if vertical is 1.
        Vector3 upMovement = up * vertical;

        // Takes the movement of the two vectors and normilize it to 1.
        // POSSIBLE SOLUTION to problem 1. Needs to be tested more.
        Vector3 direction = Vector3.Normalize(rightMovement + upMovement) * panSpeed * Time.deltaTime;

        // Moves the camera. Look up transform.Translate() on unity API if needed.
        transform.Translate(direction);
    }
}
