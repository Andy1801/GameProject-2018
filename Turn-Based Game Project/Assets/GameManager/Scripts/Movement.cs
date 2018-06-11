using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {

    public float lerpTime;
    public float distanceDifference;

    private Game_Manager gameManager;

    //States in which this particular state can not occur
    private int[] doNotStates;

    private Stack<Tile_Neighbors> movingPath;

    public int[] GetDoNotStates { get { return doNotStates; } }

    // Use this for initialization
    private void Start()
    {
        gameManager = Game_Manager.instance;

        doNotStates = new int[] { (int)CurrentState.rotating };

        movingPath = new Stack<Tile_Neighbors>();
    }

    /// <summary>
    /// Sets up the path from inital position of the player to the
    /// current x and z coordinate
    /// </summary>
    /// <param name="x"> The new x position of where you want to move</param>
    /// <param name="z"> The new z position of where you want to move</param>
    public void SetUp(float x, float z)
    {
        gameManager.GetState.Moving = true;

        int xTemp = Mathf.RoundToInt(x);
        int zTemp = Mathf.RoundToInt(z);

        Tile_Neighbors startMoveTile = gameManager.graph[xTemp, zTemp];

        movingPath.Push(startMoveTile);

        while(gameManager.GetPathFinder.Cost[startMoveTile] != 0)
        {
            // The next tile that you move to
            Tile_Neighbors nextMoveTile = null;

            foreach(Tile_Neighbors neighbor in startMoveTile.neighbors)
            {
                if (nextMoveTile == null)
                    nextMoveTile = neighbor;
                else if (gameManager.GetPathFinder.Cost[nextMoveTile] > gameManager.GetPathFinder.Cost[neighbor])
                    nextMoveTile = neighbor;
            }

            // Push the shortest tile that was found in the foreach loop above
            movingPath.Push(nextMoveTile);
            startMoveTile = nextMoveTile;
        }

        gameManager.GetPathFinder.RemovePath();

        Debug.Log("Moving path setup done");

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
        Tile_Neighbors destinationTile = movingPath.Pop();

        //The position of the tile you are moving to.
        Vector3 destination = gameManager.TiletoWorld(destinationTile.position, (int)Conversion.tileToWorld);
        destination = new Vector3(destination.x, 1.0f, destination.z);

        // Sets the current tile to not have anything above it.
        Tiles currentTiles = gameManager.TileOn(destination);
        currentTiles.UnitOn = null;

        // While the stack is not empty keep moving the unit.
        while (movingPath.Count != 0)
        {
            // Get the position in between the to vectors at the rate of the lerpTime
            Vector3 newPosition = Vector3.Lerp(gameManager.ActiveUnit.transform.position, destination, lerpTime);

            //Set the new position to the lerp position
            gameManager.ActiveUnit.transform.position = newPosition;

            distance = Vector3.Distance(gameManager.ActiveUnit.transform.position, destination);

            //If distance is less then a certain number then you have reached the current position
            //set the next one and move to it.
            // Should not be a magic number
            if (distance < distanceDifference)
            {
                gameManager.ActiveUnit.transform.position = destination;
                destinationTile = movingPath.Pop();
                destination = gameManager.TiletoWorld(destinationTile.position, (int)Conversion.tileToWorld);
                destination = new Vector3(destination.x, 1.0f, destination.z);
            }

            //Wait until the end of frame to run the while loop again.
            yield return new WaitForEndOfFrame();
        }

        distance = Vector3.Distance(gameManager.ActiveUnit.transform.position, destination);

        while (distance > distanceDifference)
        {
            Vector3 newPosition = Vector3.Lerp(gameManager.ActiveUnit.transform.position, destination, lerpTime);

            gameManager.ActiveUnit.transform.position = newPosition;

            distance = Vector3.Distance(gameManager.ActiveUnit.transform.position, destination);

            yield return new WaitForEndOfFrame();
        }

        gameManager.ActiveUnit.transform.position = destination;

        currentTiles = gameManager.TileOn(destination);
        currentTiles.UnitOn = gameManager.ActiveUnit.gameObject;

        Debug.Log("Movement Done");

        gameManager.ActiveUnit.Active = false;
        gameManager.GetState.Moving = false;
    }
}
