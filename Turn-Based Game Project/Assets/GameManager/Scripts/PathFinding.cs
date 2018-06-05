using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Finds all the paths that the user can take to get from one position to another based on the users speed.
/// 
/// FIXES NEEDED: 
/// 2. Have to setup a script to translate the position of the tile to that of the world space
/// 
/// 3. Create a enumerated type for the translation of tiles to world coordinates and vice versa
/// 
/// </summary>

public class PathFinding : MonoBehaviour {

    public float lerpTime;
    public float distanceDifference;

    // Going to hold the next positions to be visited.
    private Stack<Tile_Neighbors> holdNeighbors;

    // Reference to the game manager script in order to access neighbors
    private Game_Manager gameManager;

    //The tile that the player or the tracker is currently on
    private Tile_Properties tileInfo;
    private Tiles currentTiles;

    // Stores the cost to get to this tile related to the tile neighbor
    private Dictionary<Tile_Neighbors, int> cost;

    private bool findingPath;

    public bool FindingPath
    {
        get { return findingPath; }
    }

    // Change this so it is not public and is the unit you clicked on
    [System.NonSerialized]
    public Allies Unit;

	// Use this for initialization
	void Start () {
        holdNeighbors = new Stack<Tile_Neighbors>();

        gameManager = GetComponent<Game_Manager>();

        cost = new Dictionary<Tile_Neighbors, int>();

        findingPath = false;
	}

    public void RemovePath()
    {
        foreach (Tile_Neighbors current in gameManager.graph)
        {
            // If the cost of the tile had been modified then find the tile and take away the highlight.
            if (cost[current] != 1000)
            {
                currentTiles = gameManager.TileOn(gameManager.TiletoWorld(current.position, 1));
                currentTiles.highlight(Color.green, false);
            }
        }
    }

    /// <summary>
    /// The purpose is to find the shortest path well taking into account the speed
    /// of the player. This uses a dictionary to key-pair with the speed and adds 
    /// the movecost of the tile to remove from speed.
    /// </summary>
    public void pathFinding(Vector3 startPosition)
    {   
        // Stores the current tile that we are on.
        Tile_Neighbors sourceTile;
        if (holdNeighbors.Count != 0)
            holdNeighbors.Clear();

        Vector3 tilePosition = gameManager.TiletoWorld(startPosition, -1);

        findingPath = true;

        int x = Mathf.RoundToInt(tilePosition.x);
        int z = Mathf.RoundToInt(tilePosition.z);

        // Sets all the dictionary pairs to zero based on the graph in game manager
        foreach (Tile_Neighbors current in gameManager.graph)
            cost[current] = 1000;

        // Sets the players position as the first location.
        sourceTile = gameManager.graph[x,z];

        cost[sourceTile] = 0;

        //Pushes the sourceTile into the stack
        holdNeighbors.Push(sourceTile);

        // As long as the stack as some elements then go through their
        // neighbors and calculate the cost of the speed.
         while(holdNeighbors.Count != 0)
         {
             Tile_Neighbors[] neighbors = sourceTile.neighbors;

             foreach (Tile_Neighbors next in neighbors)
             {
                currentTiles = gameManager.TileOn(gameManager.TiletoWorld(next.position, 1));

                tileInfo = currentTiles.property; 

                //If our cost[next] is greater then our new move cost then change it because that is not the shortest path
                //and push it to the top of the stack
                if (!tileInfo.isWalkable || currentTiles.UnitOn != null)
                    continue;
                else if (cost[next] > (cost[sourceTile] + tileInfo.move_cost) && (cost[sourceTile] + tileInfo.move_cost) <= Unit.speed)
                {
                    // Highlights the tiles. This is located in the game manager
                    currentTiles.highlight(Color.yellow, true);
                    cost[next] = cost[sourceTile] + tileInfo.move_cost;
                    holdNeighbors.Push(next);
                }
             }
             // Pop the next element from the stack.
             sourceTile = holdNeighbors.Pop();
         }

        Debug.Log("PathFinding Done");
    }

    /// <summary>
    /// Sets up the shortest path from where the player is to the tile that they clicked.
    /// This is called when the highlighted tile is clicked on.
    /// Remember that stacks FIRST IN LAST OUT.
    /// </summary>
    /// <param xPosition="x position of the tile"></param>
    /// <param yPosition="y"></param>
    public void pathSetup(float x, float z)
    {

        //Vector3 newPosition = gameManager.TiletoWorld(new Vector3(x, 0.0f, z));

        //int xTemp = Mathf.RoundToInt(newPosition.x);
        //int yTemp = Mathf.RoundToInt(newPosition.z);

        int xTemp = (int)x;
        int yTemp = (int)z;

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

            Debug.Log("Tile position: " + nextMoveTile.position);
            // Push the shortest tile that was found in the foreach loop above
            holdNeighbors.Push(nextMoveTile);
            startMoveTile = nextMoveTile;
        }

        RemovePath();

        Debug.Log("Path Setup done");

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
        Vector3 destination = gameManager.TiletoWorld(destinationTile.position, 1);//new Vector3(destinationTile.position.x, 1f, destinationTile.position.z);
        destination = new Vector3(destination.x, 1.0f, destination.z);

        // Sets the current tile to not have anything above it.
        currentTiles = gameManager.TileOn(destination);
        currentTiles.UnitOn = null;

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
            if (distance < distanceDifference)
            {
                transform.position = destination;
                destinationTile = holdNeighbors.Pop();
                destination = gameManager.TiletoWorld(destinationTile.position, 1);//new Vector3(destinationTile.position.x, 1f, destinationTile.position.z);
                destination = new Vector3(destination.x, 1.0f, destination.z);
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

            if (distance < distanceDifference)
            {
                transform.position = destination;
                    
                currentTiles = gameManager.TileOn(destination);
                currentTiles.UnitOn = Unit.gameObject;
            }
        }

        Debug.Log("Movement Done");

        findingPath = false;
        Unit.Active = false;
    }
}
