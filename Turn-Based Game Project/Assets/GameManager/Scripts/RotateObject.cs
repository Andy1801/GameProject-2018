using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Rewrite the code to be more organized
/// 
/// Also when at 360 degrees the path is not calculated again.
/// </summary>
public class RotateObject : MonoBehaviour {

    /*private GameObject gameManager;
    private Game_Manager managerScript;
    private PathFinding path;*/

    private Game_Manager gameManager;

    private GameObject parentGrid;

    private float direction;
    private float rotation;

    private bool done;
    private bool turn;

    private Vector3 axisPosition;
    private Vector3 dir;
    private Quaternion rot;
	// Use this for initialization
	void Start () {
        /*gameManager = GameObject.FindWithTag("GameManager"); 
        managerScript = gameManager.GetComponent<Game_Manager>();
        path = gameManager.GetComponent<PathFinding>();*/

        gameManager = GameObject.FindWithTag("GameManager").GetComponent<Game_Manager>();

        parentGrid = gameManager.parentGrid;

        direction = 0;
        rotation = transform.eulerAngles.y;

        turn = false;

        float gridCenter = gameManager.dimensions / 2;

        axisPosition = new Vector3(gridCenter, 0.0f, gridCenter);
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.A) && !turn)
        {
            direction = 1;
            rotation = rotation + 90;
        }
        
        if (Input.GetKeyDown(KeyCode.D) && !turn)
        {
            direction = -1;
            rotation = rotation - 90;
        }

        if (rotation % 360 == 0)
            rotation = 0;

        RotatePivot();
	}

    private void LateUpdate()
    {
        if (done && parentGrid.transform.eulerAngles.y % 90 == 0)
        {
            if (gameManager.Path.FindingPath && tag == "Unit")
            {
                gameManager.Path.pathFinding(dir);
                done = false;
                Debug.Log("Found path in rotation");
            }
            else
                done = false;
        }
    }

    private void RotatePivot()
    {
        //Direction relative to the pivot
        if (!turn && direction != 0)
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
                turn = false;
                done = true;
                direction = 0;
                transform.position = dir;
                transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);

                Debug.Log("Rotation Done");
            }
        }
    }
}
