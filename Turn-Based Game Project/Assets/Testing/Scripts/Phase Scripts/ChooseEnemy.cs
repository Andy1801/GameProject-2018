﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseEnemy : PhaseParent {

    private TacticPhase tacticPhase;
    private ChooseAttack chooseAttackPhase;

    private new void Start()
    {
        base.Start();
        tacticPhase = (TacticPhase)phaseSwap.PhaseUpdate((int)CurrentPhase.tactic);
        chooseAttackPhase = (ChooseAttack)phaseSwap.PhaseUpdate((int)CurrentPhase.ChooseAttack);
    }

    public override void PhaseSetup()
    {
        //Create the function that the enter activate delegate will be set to

        gameManager.GetPathFinder.pathFinding(gameManager.ActiveUnit);

        gameManager.enterAction = SetAttackTile;
        gameManager.phaseReversal = PhaseReversal;
    }

    public override void PhaseReversal()
    {
        gameManager.ActiveUnit.SetInActive();
        gameManager.GetPathFinder.RemovePath();
        gameManager.GetTracker.CheckTileTracker();
        tacticPhase.PhaseSetup();
    }

    public void SetAttackTile()
    {
        Tiles currentTile = gameManager.TileOn(gameManager.GetTracker.transform.position);

        if (currentTile.Highlighted)
        {
            gameManager.attackTile = currentTile;
            //Call the choose Attack phase.
            chooseAttackPhase.PhaseSetup();
        }
    }
}