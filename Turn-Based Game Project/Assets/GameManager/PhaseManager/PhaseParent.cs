using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseParent : MonoBehaviour
{
    protected StateManager stateManager;
    protected Game_Manager gameManager;

    protected PhaseSwapping phaseSwap;

    public void Start()
    {
        gameManager = Game_Manager.instance;
        stateManager = gameManager.GetState;

        phaseSwap = gameManager.GetPhase;
    }

    public virtual void PhaseSetup()
    {

    }

    public virtual void PhaseReversal()
    {

    }
}
