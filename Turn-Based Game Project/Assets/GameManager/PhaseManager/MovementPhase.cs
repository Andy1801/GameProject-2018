using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementPhase : PhaseParent {

    private TacticPhase tacticPhase;
    private ChooseEnemy chooseEnemy;

    private new void Start()
    {
        base.Start();
        tacticPhase = (TacticPhase)phaseSwap.PhaseUpdate((int)CurrentPhase.tactic);
        chooseEnemy = (ChooseEnemy)phaseSwap.PhaseUpdate((int)CurrentPhase.ChooseEnemy);
    }

    public override void PhaseSetup()
    {
        Debug.Log("Movement Phase ");

        gameManager.enterAction = moveUnit;
        gameManager.phaseReversal = PhaseReversal;
    }

    public override void PhaseReversal()
    {
        Debug.Log("Movement Phase Reversal");

        gameManager.ActiveUnit.SetInActive();
        gameManager.GetPathFinder.RemovePath();
        gameManager.GetTracker.CheckTileTracker();
        tacticPhase.PhaseSetup();

    }

    private void moveUnit()
    {
        Debug.Log("Enter Activate Movement Phase");

        Tiles currentTile = gameManager.TileOn(gameManager.GetTracker.transform.position);

        if (currentTile.Highlighted)
        {
            Game_Manager.instance.GetMovement.SetUp(currentTile.xPosition, currentTile.zPosition);
            gameManager.ActiveUnit.Moved = true;
            //The next Phase setup is called inside the movement script.
        }
    }
}
