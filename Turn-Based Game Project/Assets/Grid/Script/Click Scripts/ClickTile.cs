using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickTile : ClickParent {

    private Tiles tile;
    private Movement movement;

    private new void Start()
    {
        base.Start();
        tile = GetComponent<Tiles>();
        movement = gameManager.GetMovement;
    }

    /*private void OnMouseEnter()
    {
        tile.information();
    }*/

    protected override void OnMouseDown()
    {
        if (tile.Highlighted && stateManager.CanDoState(movement.GetDoNotStates))
            movement.SetUp(tile.xPosition, tile.zPosition);
    }
}
