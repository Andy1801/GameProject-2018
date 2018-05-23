using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickUnit : MonoBehaviour {

    public Unit_Properties property;
    public Game_Manager temp;

    private PathFinding path;

    private bool active;

    private void Start()
    {
        path = GameObject.FindWithTag("GameManager").GetComponent<PathFinding>();
    }

    private void OnMouseDown()
    {
        Debug.Log("Clicked Unit");

        path.pathFinding(new Vector3(transform.position.x, 1.0f, transform.position.z));
    }
}
