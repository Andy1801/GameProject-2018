using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Finds all the paths that the user can take to get from one position to another based on the users speed.
/// 
/// FIXES NEEDED:
/// 1. Have to make the units to be able to be changed to the clicked unit.
/// </summary>

public class PathFinding : MonoBehaviour {

    // Going to hold the next positions to be visited.
    private Stack<Tile_Neighbors> holdNeighbors;

    // Reference to the game manager script in order to access neighbors
    private Game_Manager gameManager;

    //Start position of the neighbors
    private Vector3 startPosition;

    //The tile that the player or the tracker is currently on
    private Tile_Properties currentTiles;

    // Change this so it is not public and is the unit you clicked on
    public GameObject Unit;

	// Use this for initialization
	void Start () {
        holdNeighbors = new Stack<Tile_Neighbors>();

        gameManager = GetComponent<Game_Manager>();

        startPosition = Unit.transform.position;
	}

    /// <summary>
    /// Gets the tile that the player or the track is standing by casting a 
    /// ray cast into the floor.
    /// </summary>
	private void TileOn()
    {
        RaycastHit hit;
        Vector3 currentPosition = startPosition;

        //if ray cast hit something then store it into hit
        if(Physics.Raycast(currentPosition, Vector3.down, out hit))
        {
            //if hit is not equal to null then set the current tile
            if (hit.collider != null)
            {
                currentTiles = hit.collider.GetComponent<ClickTile>().property;
            }
            else
                Debug.LogError("The player is not standing on a platform");
        }
    }

    /// <summary>
    /// This function is just here for testing.
    /// </summary>
    private void Update()
    {
       if(Input.anyKey)
        {
            TileOn();
            Debug.Log(currentTiles.tileName);
        }
    }
}
