using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickUnit : MonoBehaviour {

    private PathFinding path;
    private Allies unit;

    private void Start()
    {
        path = GameObject.FindWithTag("GameManager").GetComponent<PathFinding>();
        unit = GetComponent<Allies>();
    }

    private void OnMouseDown()
    {
        if (!path.FindingPath)
        {
            unit.Active = true;
            path.Unit = unit;
            path.pathFinding(new Vector3(transform.position.x, 1.0f, transform.position.z));
        }
        else if(path.FindingPath && !unit.Active)
        {
            unit.Active = true;
            path.Unit.Active = false;
            path.Unit = unit;
            path.RemovePath();
            path.pathFinding(new Vector3(transform.position.x, 1.0f, transform.position.z));
        }
    }
}
