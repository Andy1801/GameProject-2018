using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Moves the tracker to it's next position grid by grid.
/// Checks if the tile it is on has a player.
/// </summary>
public class Tracker : MonoBehaviour {

    public float lerpTime = 0.5f;

    private StateManager stateManager;

    private Tiles currentTile;

    private int[] doNotStates;

    private float horizontalMovement;
    private float verticalMovement;

    private bool atLocation;
    private bool scan;

    public int[] GetDoNotStates { get { return doNotStates; } }

    private void Start()
    {
        stateManager = Game_Manager.instance.GetState;
        currentTile = null;

        doNotStates = new int[] { (int)CurrentState.moving };

        atLocation = false;

        //This is true so that the tracker can get the tile it started on.
        scan = true; 
    }

    private void Update()
    {
        //Update the movement of the tracker every frame

        if ((Input.GetButton("Horizontal") || Input.GetButton("Vertical")) && !atLocation && stateManager.CantDoState(doNotStates))
        {
            horizontalMovement = Input.GetAxisRaw("Horizontal");
            verticalMovement = Input.GetAxisRaw("Vertical");

            if (currentTile != null)
                RemoveNonActivePath();

            StartCoroutine("TrackerMoving");
        }

        if (scan)
        {
            scan = false;

            CheckTileTracker();
            //Call UI to present the character and the tile you are on.
        }
        else if ( Input.GetKeyDown(KeyCode.Return))
            Game_Manager.instance.enterAction();
        else if(Input.GetKeyDown(KeyCode.Q))
            Game_Manager.instance.phaseReversal();
    }

    public void CheckTileTracker()
    {
        currentTile = Game_Manager.instance.TileOn(transform.position);

        if (currentTile.UnitOn != null)
            currentTile.UnitOn.CallPathFinding();
    }

    private void RemoveNonActivePath()
    {
        if(currentTile.UnitOn != null && !stateManager.Pending)
        {
            Game_Manager.instance.GetPathFinder.RemovePath();
        }
    }

    private float TrackInGrid(float axisPosition , float originalPosition)
    {
        // Needed due to a slight rounding error that occurs when you rotate.
        axisPosition = Mathf.Round(axisPosition);

        bool inGrid = axisPosition < Game_Manager.instance.dimensions && axisPosition >= 0.0f;

        if (!inGrid)
            return originalPosition;

        return axisPosition;

    }

    private IEnumerator TrackerMoving()
    {
        stateManager.Tracking = true;

        Vector3 newPosition = new Vector3(verticalMovement + transform.position.x,transform.position.y, -horizontalMovement + transform.position.z);

        newPosition.x = TrackInGrid(newPosition.x, transform.position.x);
        newPosition.z = TrackInGrid(newPosition.z, transform.position.z);

        float distance = Vector3.Distance(transform.position, newPosition);

        while (distance > 0.1)
        {
            atLocation = true;

            transform.position = Vector3.Lerp(transform.position, newPosition, lerpTime);

            distance = Vector3.Distance(transform.position, newPosition);

            yield return null;
        }

        transform.position = newPosition;
        stateManager.Tracking = false;
        atLocation = false;
        scan = true;
    }

}
