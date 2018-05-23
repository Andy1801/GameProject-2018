using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickTile : MonoBehaviour {

    public Tile_Properties property;

    private float x;
    private float y;

    private bool highlighted;

    private PathFinding path;

    public bool Highlighted
    {
        get { return highlighted; }
        set { highlighted = value; }
    }

    private void Start()
    {
        path = GameObject.FindWithTag("GameManager").GetComponent<PathFinding>();
    }

    private void OnMouseDown()
    {
        x = transform.position.x;
        y = transform.position.z;


        if (highlighted)
        {
            path.pathSetup(x,y);
        }
        // Call unity UI system script to showcase information
    }
}
