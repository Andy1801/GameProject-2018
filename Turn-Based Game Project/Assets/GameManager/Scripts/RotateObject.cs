using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// Rotates objects around a pivot that is in the center of the grid.
/// 
/// TODO:
/// 1. Check why we no longer needed to recalculate the path after rotation
/// could be due to our tiletoworld function.
/// </summary>
public class RotateObject : MonoBehaviour {

    private Movement movement;
    private StateManager stateManager;
    private GameObject parentGrid;

    private int[] doNotStates;

    private float direction;
    private float rotation;

    private Vector3 axisPosition;
    private Vector3 newPosition;

    public int[] GetDoNotStates
    {
        get { return doNotStates; }
    }

	// Use this for initialization
	void Start () {
        stateManager = Game_Manager.instance.GetState;
        parentGrid = Game_Manager.instance.parentGrid;

        doNotStates = new int[] { (int)CurrentState.moving, (int)CurrentState.tracking };

        direction = 0;
        rotation = transform.eulerAngles.y;

        float gridCenter = Game_Manager.instance.dimensions / 2;

        axisPosition = new Vector3(gridCenter, 0.0f, gridCenter);
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.A) && stateManager.CantDoState(doNotStates))
        {
            direction = 1;
            rotation = rotation + 90;
        }
        
        if (Input.GetKeyDown(KeyCode.D) && stateManager.CantDoState(doNotStates))
        {
            direction = -1;
            rotation = rotation - 90;
        }

        if (rotation % 360 == 0)
            rotation = 0;

        RotatePivot();
	}

    /// <summary>
    /// Deteremines the difference between the objects position and the axis position
    /// to rotate it around that pivot. Then sets the new position and the new rotation
    /// of the object.
    /// </summary>
    private void RotatePivot()
    {
        if (tag == "Parent")
            stateManager.Rotating = true;

        newPosition = transform.position - axisPosition;
        newPosition = Quaternion.Euler(0.0f, 90 * direction, 0.0f) * newPosition;
        newPosition = newPosition + axisPosition;

        transform.position = newPosition;
        transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);

        direction = 0;

        if (tag == "Parent")
            stateManager.Rotating = false;
    }

    /*//LateUpdate is called after update in every frame
    private void LateUpdate()
    {
        /*if (done)
        {
            if (gameManager.Path.FindingPath && tag == "Unit")
            {
                Debug.Log("Rotational pathfinding");
                gameManager.Path.pathFinding(newPosition);
                done = false;
            }
            else
                done = false;
        }
    }*/



    /*private void RotatePivot()
    {
        //Direction relative to the pivot
        /*if (!turn && direction != 0)
        {
            dir = transform.position - axisPosition;
            dir = Quaternion.Euler(0.0f, 90 * direction, 0.0f) * dir;
            dir = dir + axisPosition;
            turn = true;
        }

        if (turn)
        { 
            Vector3 change = Vector3.Lerp(transform.position, dir, 0.5f);
            transform.position = change;

            float distance = Vector3.Distance(transform.position, dir);

            if (distance < 0.001)
            {
                direction = 0;
                transform.position = dir;
                transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);

                turn = false;

                if (tag == "Parent")
                    done = true;
            }
        }
    }*/
}

