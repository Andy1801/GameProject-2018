using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TacticPhase : PhaseParent {

    private MovementPhase movementPhase;
    private ChooseEnemy chooseEnemyPhase;

    private new void Start()
    {
        base.Start();
        movementPhase = (MovementPhase)phaseSwap.PhaseUpdate((int)CurrentPhase.movement);
        chooseEnemyPhase = (ChooseEnemy)phaseSwap.PhaseUpdate((int)CurrentPhase.ChooseEnemy);
    }

    public override void PhaseSetup()
    {
        //Change the phaseswapping in the state manager to true.

        gameManager.enterAction = activatePlayer;

        //Changes the phaseswapping in the state manager to false with a small time delay.

        gameManager.phaseReversal = PhaseReversal; 
    }

    /// <summary>
    /// If the player has moved then revert the player back to the original tile that they were on
    /// and set the current tile to not have a unit on it.
    /// </summary>
    public override void PhaseReversal()
    {
        if (gameManager.ActiveUnit != null && gameManager.ActiveUnit.Moved && gameManager.previousTile != null)
        {
            gameManager.TileOn(gameManager.ActiveUnit.transform.position).UnitOn = null;

            Vector3 newPosition = new Vector3(gameManager.previousTile.transform.position.x, 1.0f, gameManager.previousTile.transform.position.z);

            gameManager.ActiveUnit.transform.position = newPosition;
            gameManager.TileOn(newPosition).UnitOn = gameManager.ActiveUnit;
            gameManager.GetPathFinder.RemovePath();

            gameManager.ActiveUnit.Moved = false;
        }
        
    }

    private void activatePlayer()
    {
        Tiles currentTile = gameManager.TileOn(gameManager.GetTracker.transform.position);

        if (currentTile.UnitOn != null && currentTile.UnitOn.tag == "Unit")
        {
            if (!currentTile.UnitOn.Moved)
            {
                currentTile.UnitOn.SetActive();
                gameManager.previousTile = gameManager.TileOn(currentTile.UnitOn.transform.position);
                movementPhase.PhaseSetup();
            }
            else if (!currentTile.UnitOn.Attacked)
            {
                //Set the battle Phase choose enemy up.
                Debug.Log("Tactic --> Battle");
                currentTile.UnitOn.SetActive();
                gameManager.previousTile = null;
                chooseEnemyPhase.PhaseSetup();
            }
        }
    }
}
