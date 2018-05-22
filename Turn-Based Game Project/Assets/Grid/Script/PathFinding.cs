using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Finds all the paths that the user can take to get from one position to another based on the users speed.
/// 
/// FIXES NEEDED:
/// 1. Have to make the units to be able to be changed to the clicked unit.
/// 
/// 2. Have to setup a script to translate the position of the tile to that of the world space
/// 
/// 3. Highlighted tiles should be reseted(Function is located in the game manager)
/// 
/// </summary>

public class PathFinding : MonoBehaviour {

    public float lerpTime;

    // Going to hold the next positions to be visited.
    private Stack<Tile_Neighbors> holdNeighbors;

    // Reference to the game manager script in order to access neighbors
    private Game_Manager gameManager;

    //Start position of the neighbors
    private Vector3 startPosition;

    //The tile that the player or the tracker is currently on
    private Tile_Properties tileInfo;
    private GameObject currentTiles;

    // Stores the cost to get to this tile related to the tile neighbor
    private Dictionary<Tile_Neighbors, int> cost;

    private bool findingPath;

    // Change this so it is not public and is the unit you clicked on
    public ClickUnit Unit;

	// Use this for initialization
	void Start () {
        holdNeighbors = new Stack<Tile_Neighbors>();

        gameManager = GetComponent<Game_Manager>();

        startPosition = Unit.transform.position;

        cost = new Dictionary<Tile_Neighbors, int>();

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
                currentTiles = hit.collider.gameObject;
                tileInfo = currentTiles.GetComponent<ClickTile>().property;
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
        // Stores the current tile that we are on.
        Tile_Neighbors sourceTile;
        holdNeighbors.Clear();
        
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

                //If our cost[next] is greater then our new move cost then change it because that is not the shortest path
                //and push it to the top of the stack
                if (cost[next] > (cost[sourceTile] + tileInfo.move_cost) && (cost[sourceTile] + tileInfo.move_cost) <= Unit.property.speed)
                {
                    // Highlights the tiles. This is located in the game manager
                    gameManager.highlight(currentTiles, tileInfo);
                    cost[next] = cost[sourceTile] + tileInfo.move_cost;
                    holdNeighbors.Push(next);
                }
             }

             // Pop the next element from the stack.
             sourceTile = holdNeighbors.Pop();
         }
    }

    /// <summary>
    /// Sets up the shortest path from where the player is to the tile that they clicked.
    /// This is called when the highlighted tile is clicked on.
    /// Remember that stacks FIRST IN LAST OUT.
    /// </summary>
    /// <param xPosition="x position of the tile"></param>
    /// <param yPosition="y"></param>
    public void pathSetup(float x, float y)
    {
        int xTemp = (int)x;
        int yTemp = (int)y;

        // The tile that you want to move to
        Tile_Neighbors startMoveTile = gameManager.graph[xTemp,yTemp];

        holdNeighbors.Push(startMoveTile);

        while(cost[startMoveTile] != 0)
        {
            // The next tile that you move to.
            Tile_Neighbors nextMoveTile = null;

            // Checks all of the tiles neighbors to find the shortest one.
            foreach(Tile_Neighbors neighbor in startMoveTile.neighbors)
            {
                if (nextMoveTile == null)
                    nextMoveTile = neighbor;
                else if (cost[nextMoveTile] > cost[neighbor])
                    nextMoveTile = neighbor;
            }

            Debug.Log(cost[nextMoveTile]);

            // Push the shortest tile that was found in the foreach loop above
            holdNeighbors.Push(nextMoveTile);
            startMoveTile = nextMoveTile;
        }

        StartCoroutine("MoveUnit");
        
    }

    /// <summary>
    /// Move the units from each of the position that are stored in the stack using a CoRoutine 
    /// </summary>
    /// <returns></returns>
    private IEnumerator MoveUnit()
    {
        //Distance between to Vector3
        float distance = 0f;

        // The tile you are moving to
        Tile_Neighbors destinationTile = holdNeighbors.Pop();

        //The position of the tile you are moving to. TODO: Make the new Portion of this into a function
        Vector3 destination = new Vector3(destinationTile.position.x, 1f, destinationTile.position.y);

        // While the stack is not empty keep moving the unit.
        while(holdNeighbors.Count != 0)
        {
            // Get the position in between the to vectors at the rate of the lerpTime
            Vector3 newPosition = Vector3.Lerp(Unit.transform.position, destination, lerpTime);

            //Set the new position to the lerp position
            Unit.transform.position = newPosition;

            distance = Vector3.Distance(Unit.transform.position, destination);

            //If distance is less then a certain number then you have reached the current position
            //set the next one and move to it.
            // Should not be a magic number
            if (distance < 0.1)
            {
                Debug.Log("Distance met");
                Debug.Log("Destination: " + holdNeighbors.Peek().position);
                transform.position = destination;
                destinationTile = holdNeighbors.Pop();
                destination = new Vector3(destinationTile.position.x, 1f, destinationTile.position.y);
            }

            //Wait until the end of frame to run the while loop again.
            yield return new WaitForEndOfFrame();
        }

        // TODO: Everything from this downwards may need some rethinking.
        distance = 1f;

        while(distance > 0.1f)
        {
            Vector3 newPosition = Vector3.Lerp(Unit.transform.position, destination, lerpTime);

            Unit.transform.position = newPosition;

            distance = Vector3.Distance(Unit.transform.position, destination);

            if (distance < 0.1)
            {
                transform.position = destination;
            }
        }

        //findingPath = false;
    }

    /// <summary>
    /// This function is just here for testing.
    /// </summary>
    private void Update()
    {
       if(Input.anyKey && !findingPath)
        {
            findingPath = true;
            pathFinding();
        }
    }
}
