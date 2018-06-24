using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is a singleton script that has access other scripts in the game
/// to share and use them with other components in the game.
/// </summary>

public enum Conversion
{
    worldToTile = -1,
    tileToWorld = 1
};

public class Game_Manager : MonoBehaviour {

    //This is a instance property to get the singleton
    public static Game_Manager instance { get; private set; }

    // Holds the property of the tiles
	public Tile_Properties[] tiles;

    [System.NonSerialized]
    public Tile_Neighbors[,] graph;

    [SerializeField]
    public GameObject parentGrid;

    [SerializeField]
	public int dimensions = 10;
	private int[,] tile_location;

    public Tiles previousTile;
    public Tiles attackTile;

    //Delegates used for phase setup, phase reversal, and key functions.
    public delegate void PhaseDelegate();
    public PhaseDelegate phaseSetup;
    public PhaseDelegate phaseReversal;
    public PhaseDelegate enterAction;
    public PhaseDelegate qAction;

    public Color TileColor { get; set; }

    private Allies activeUnit;
    private Tracker tracker;
    private StateManager stateManager;
    private PhaseSwapping phaseSwap;
    private PathFinding path;
    private Movement movement;

    // Properties: Getter and Setter functions for other scripts
    public Allies ActiveUnit
    {
        get { return activeUnit;  }
        set { activeUnit = value; }
    }
    public Tracker GetTracker { get { return tracker; } }
    public StateManager GetState { get { return stateManager; } }
    public PhaseSwapping GetPhase { get { return phaseSwap; } }
    public PathFinding GetPathFinder { get { return path; } }
    public Movement GetMovement { get { return movement; } }

    // Creates the grid based on the dimensions given and the basic grass tiles.
    void Awake () {

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            tile_location = new int[dimensions, dimensions];
            graph = new Tile_Neighbors[dimensions, dimensions];

            activeUnit = null;

            tracker = GameObject.FindGameObjectWithTag("Tracker").GetComponent<Tracker>();
            phaseSwap = GameObject.FindGameObjectWithTag("PhaseManager").GetComponent<PhaseSwapping>();

            stateManager = GetComponent<StateManager>();
            path = GetComponent<PathFinding>();
            movement = GetComponent<Movement>();

            //This is for testing
            tile_location[1, 0] = 1;
            tile_location[2, 1] = 1;

            for (int x = 0; x < dimensions; x++)
            {
                for (int y = 0; y < dimensions; y++)
                {
                    int temp = tile_location[x, y];
                    if (parentGrid)
                        Instantiate(tiles[temp].tile_Prefab, new Vector3(x, 0, y), Quaternion.identity, parentGrid.transform);
                }
            }

            SetNeighbors();
            FindNeighbors();
        }
        else
            Destroy(gameObject);
    }

    //Initilizes the array for each of the neighbors related to one of the tiles.
    private void SetNeighbors()
    {
        for (int x = 0; x < dimensions; x++)
        {
            for (int y = 0; y < dimensions; y++)
            {
                bool conditionX = (x == 0 || x == (dimensions - 1));
                bool conditionY = (y == 0 || y == (dimensions - 1));


                graph[x, y] = new Tile_Neighbors(conditionX, conditionY);
                graph[x, y].position = new Vector3(x, 0f,y);
            }
        }
    } 

    //Finds the neighbors and stores them.
    private void FindNeighbors() 
    {
        int counter = 0;

        //Goes through the x-axis
        for (int x = 0; x < dimensions; x++)
        {
            //Goes through the y-axis and checks for certain conditions to set the neighbor.
            for (int y = 0; y < dimensions; y++)
            {
                if (x != 0)
                {
                    graph[x, y].neighbors[counter] = graph[x - 1, y];
                    counter++;
                }
                if(x != (dimensions - 1))
                {
                    graph[x, y].neighbors[counter] = graph[x + 1, y];
                    counter++;
                }
                if (y != 0)
                {
                    graph[x, y].neighbors[counter] = graph[x, y - 1];
                    counter++;
                }
                if (y != (dimensions - 1))
                    graph[x, y].neighbors[counter] = graph[x, y + 1];

                counter = 0;
            }
        }
    }

    /// <summary>
    /// Gets the tile that the player or the tracker is standing by casting a 
    /// ray cast into the floor.
    /// </summary>
	public Tiles TileOn(Vector3 currentPosition)
    {
        RaycastHit hit;
        currentPosition = new Vector3(currentPosition.x, 1f, currentPosition.z);

        //if ray cast hit something then store it into hit
        if (Physics.Raycast(currentPosition, Vector3.down, out hit))
        {
            //if hit is not equal to null then set the current tile
            if (hit.collider != null)
            {
                return hit.collider.gameObject.GetComponent<Tiles>();
            }
            else
                Debug.LogError("The player is not standing on a platform");
        }

        return null;
    }

    /// <summary>
    /// Converts the tile's local position to a world position and vice versa.
    /// </summary>
    /// <param name="originalPosition">Either the tiles locals position or the tiles world position</param>
    /// <param name="worldToTile">Either converting from tile to world (1) or world to tile (-1)</param>
    /// <returns></returns>
    public Vector3 TiletoWorld(Vector3 originalPosition, int worldToTile)
    {
        Vector3 axisPoint = new Vector3(dimensions / 2, 0.0f, dimensions / 2);

        Vector3 rotatedPosition = originalPosition - axisPoint;

        rotatedPosition = Quaternion.Euler(0.0f, parentGrid.transform.eulerAngles.y * worldToTile, 0.0f) * rotatedPosition;

        rotatedPosition = rotatedPosition + axisPoint;

        return rotatedPosition;
    }
}
