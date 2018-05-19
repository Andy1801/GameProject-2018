using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Finds all the paths that the user can take to get from one position to another based on the users speed.
/// 
/// FIXES NEEDED:
/// 1. Have to make the units to be able to be changed to the clicked unit.
/// 
/// 2. Need to make so you do not calculate for the same tiles that you already came from.
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

    private bool findingPath;

    // Change this so it is not public and is the unit you clicked on
    public ClickUnit Unit;

	// Use this for initialization
	void Start () {
        holdNeighbors = new Stack<Tile_Neighbors>();

        gameManager = GetComponent<Game_Manager>();

        startPosition = Unit.transform.position;

        findingPath = false;
	}

    /// <summary>
    /// Gets the tile that the player or the track is standing by casting a 
    /// ray cast into the floor.
    /// </summary>
	private void TileOn(Vector3 currentPosition)
    {
        RaycastHit hit;
        currentPosition = new Vector3(currentPosition.x, 1f, currentPosition.y);

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
    /// The purpose is to find the shortest path well taking into account the speed
    /// of the player. This uses a dictionary to key-pair with the speed and adds 
    /// the movecost of the tile to remove from speed.
    /// </summary>
    public void pathFinding()
    {
        // Stores the cost to get to this tile related to the tile neighbor
        Dictionary<Tile_Neighbors, int> cost = new Dictionary<Tile_Neighbors, int>();

        // Stores the current tile that we are on.
        Tile_Neighbors sourceTile;
        
        int x = (int)startPosition.x;
        int z = (int)startPosition.z;

        // Sets all the dictionary pairs to zero based on the graph in game manager
        foreach (Tile_Neighbors current in gameManager.graph)
        {
            cost[current] = 1000;
        }

        // Sets the players position as the first location.
        sourceTile = gameManager.graph[x,z];

        cost[sourceTile] = 0;

        //Pushes the sourceTile into the stack
        holdNeighbors.Push(sourceTile);

        //Debug.Log(cost[sourceTile]);

        // As long as the stack as some elements then go through their
        // neighbors and calculate the cost of the speed.
         while(holdNeighbors.Count != 0)
         {
             Tile_Neighbors[] neighbors = sourceTile.neighbors;

             foreach (Tile_Neighbors next in neighbors)
             {
                 TileOn(next.position);

                // For checking on the console
                Debug.Log("Before speed: " + cost[next]);
                Debug.Log("Condition: " + (cost[next] > (cost[sourceTile] + currentTiles.move_cost)));
                Debug.Log("Source Tile" + sourceTile.position);
                Debug.Log("Current Tile" + next.position);


                //If our cost[next] is greater then our new move cost then change it because that is not the shortest path
                //and push it to the top of the stack
                if (cost[next] > (cost[sourceTile] + currentTiles.move_cost) && (cost[sourceTile] + currentTiles.move_cost) <= Unit.property.speed)
                {
                    cost[next] = cost[sourceTile] + currentTiles.move_cost;
                    holdNeighbors.Push(next);
                }

                Debug.Log("Speed: " + cost[next]);
             }

             // Pop the next element from the stack.
             sourceTile = holdNeighbors.Pop();
         }

        findingPath = false;
    }

    /// <summary>
    /// This function is just here for testing.
    /// </summary>
    private void Update()
    {
       if(Input.anyKey && !findingPath)
        {
            Debug.Log("Update");
            findingPath = true;
            pathFinding();
        }
    }
}
