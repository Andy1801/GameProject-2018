using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// The purpose of this script to give the player the ability to zoom
/// in and out using the mouse scrollwhell
/// 
/// FIXES NEEDED:
/// 1. Modifier both panning speed and panning clamps based on the how zoomed in
/// and out we are.
/// 
/// SUGGESTION:
/// 1. The modifier of problem 1 can happen in a brand new script.
/// </summary>

public class CameraZoom : MonoBehaviour {

    // Determines the min and max zoom the player can get to.
    public float minZoom;
    public float maxZoom;

    // The speed at which the zoom happens.
    public float zoomSpeed;

    // Holds whether the player wants to scroll using the axis for scroll wheel
    private float zooming;

    //Stores the main camera from the inspector
    private Camera mainCamera;

	// Use this for initialization
	void Start () {
        //Sets the main camera to be orthographic
        mainCamera = Camera.main;
        mainCamera.orthographic = true;
	}
	
	// Update is called once per frame
	void Update ()
    {
        // 1: if scrolling inwards.
        // -1: if scrolling outwards.
        // 0: if not moving the scrollwheel.
        zooming = Input.GetAxisRaw("Mouse ScrollWheel");

		if (zooming != 0)
        {
            Zoom();
        }
	}

    private void Zoom()
    {
        // Stores the new zoom size that we will get.
        float newZoom = mainCamera.orthographicSize + zooming * zoomSpeed * Time.deltaTime;

        // Clamps the zoom size based on the min and max that the player can go to
        newZoom = Mathf.Clamp(newZoom, minZoom, maxZoom);

        // Sets the new zoom size
        mainCamera.orthographicSize = newZoom;
    }
}
