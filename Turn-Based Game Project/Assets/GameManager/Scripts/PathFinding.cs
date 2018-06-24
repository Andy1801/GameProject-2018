using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Finds all the paths that the user can take to get from one position to 
/// another based on the users speed.
/// </summary>

public class PathFinding : MonoBehaviour {

    private const int defaultCost = 1000;

    // Reference to the game manager script in order to access neighbors
    private Game_Manager gameManager;

    private int playerSpeed;

    //States in which this particular state can not occur
    private int[] doNotStates;

    // Going to hold the next positions to be visited.
    private Stack<Tile_Neighbors> holdNeighbors;

    //The tile that the player or the tracker is currently on
    private Tile_Properties tileInfo;
    private Tiles currentTiles;

    // Stores the cost to get to this tile related to the tile neighbor
    private Dictionary<Tile_Neighbors, int> cost;

    public int[] GetDoNotStates { get { return doNotStates; } }

    public Dictionary<Tile_Neighbors, int> Cost
    {
        get { return cost; }
    }

	// Use this for initialization
	void Start () {
        holdNeighbors = new Stack<Tile_Neighbors>();

        gameManager = Game_Manager.instance;

        doNotStates = new int[] { (int)CurrentState.rotating, (int)CurrentState.moving, (int)CurrentState.pending };

        cost = new Dictionary<Tile_Neighbors, int>();

        foreach (Tile_Neighbors current in gameManager.graph)
            cost[current] = defaultCost;
    }

    /// <summary>
    /// The purpose is to find the shortest path well taking into account the speed
    /// of the player. This uses a dictionary to key-pair with the speed and adds 
    /// the movecost of the tile to remove from speed.
    /// </summary>
    public void pathFinding(Human unit)
    {
        if (unit.Moved)
            playerSpeed = unit.attackDistance;
        else
            playerSpeed = unit.speed;

        // Stores the current tile that we are on.
        Tile_Neighbors sourceTile;

        if (holdNeighbors.Count != 0)
            holdNeighbors.Clear();

        Vector3 tilePosition = gameManager.TiletoWorld(unit.transform.position, (int)Conversion.worldToTile);

        int x = Mathf.RoundToInt(tilePosition.x);
        int z = Mathf.RoundToInt(tilePosition.z);

        // Sets all the dictionary pairs to 1000 based on the graph in game manager
        foreach (Tile_Neighbors current in gameManager.graph)
            cost[current] = defaultCost;

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
                currentTiles = gameManager.TileOn(gameManager.TiletoWorld(next.position, (int)Conversion.tileToWorld));

                tileInfo = currentTiles.property; 

                //If our cost[next] is greater then our new move cost then change it because that is not the shortest path
                //and push it to the top of the stack
                if (!tileInfo.isWalkable || currentTiles.UnitOn != null)
                    continue;
                else if (cost[next] > (cost[sourceTile] + tileInfo.move_cost) && (cost[sourceTile] + tileInfo.move_cost) <= playerSpeed)
                {
                    // Highlights the tiles. This is located in the game manager
                    currentTiles.highlight(true, unit);
                    cost[next] = cost[sourceTile] + tileInfo.move_cost;
                    holdNeighbors.Push(next);
                }
             }
             sourceTile = holdNeighbors.Pop();
         }
        Debug.Log("PathFinding Done");
    }

    /// <summary>
    /// Resets the cost of all the tiles and removes all the highlight from certain tiles.
    /// </summary>
    public void RemovePath()
    {
        foreach (Tile_Neighbors current in gameManager.graph)
        {
            // If the cost of the tile had been modified then find the tile and take away the highlight.
            if (cost[current] != defaultCost)
            {
                currentTiles = gameManager.TileOn(gameManager.TiletoWorld(current.position, (int)Conversion.tileToWorld));
                currentTiles.highlight(false, null);
            }
        }
    }
}
