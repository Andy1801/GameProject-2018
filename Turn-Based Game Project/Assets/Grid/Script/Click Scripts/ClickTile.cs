using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickTile : MonoBehaviour {

    private Tiles tile;

    private PathFinding path;

    private void Start()
    {
        tile = GetComponent<Tiles>();
        path = GameObject.FindWithTag("GameManager").GetComponent<PathFinding>();
    }

    private void OnMouseDown()
    {
        if (tile.Highlighted)
        {
            path.pathSetup(tile.xPosition, tile.zPosition);
        }
        // Call unity UI system script to showcase information
    }
}
