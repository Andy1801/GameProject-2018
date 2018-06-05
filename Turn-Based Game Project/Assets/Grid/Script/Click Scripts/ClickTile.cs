using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickTile : MonoBehaviour {

    private Tiles tile;

    //private PathFinding path;

    private Game_Manager gameManager;

    private void Start()
    {
        tile = GetComponent<Tiles>();
        gameManager = GameObject.FindWithTag("GameManager").GetComponent<Game_Manager>();
    }

    /*private void OnMouseEnter()
    {
        tile.information();
    }*/

    private void OnMouseDown()
    {
        if (tile.Highlighted)
            gameManager.Path.pathSetup(tile.xPosition, tile.zPosition);
        // Call unity UI system script to showcase information
    }
}
