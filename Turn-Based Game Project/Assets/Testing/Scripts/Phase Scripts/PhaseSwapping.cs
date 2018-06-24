using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CurrentPhase
{
    placement,
    tactic,
    movement,
    ChooseEnemy, 
    end
};

public class PhaseSwapping : MonoBehaviour {

    private TacticPhase tacticPhase;
    private MovementPhase movementPhase;
    private ChooseEnemy chooseEnemyPhase;

	// Use this for initialization
	void Awake () {
        tacticPhase = GetComponent<TacticPhase>();
        movementPhase = GetComponent<MovementPhase>();
        chooseEnemyPhase = GetComponent<ChooseEnemy>();
	}

    private void Start()
    {
        tacticPhase.PhaseSetup();
    }

    public PhaseParent PhaseUpdate(int phase)
    {
        switch(phase)
        {
            case (int)CurrentPhase.tactic:
                return tacticPhase;
            case (int)CurrentPhase.movement:
                return movementPhase;
            case (int)CurrentPhase.ChooseEnemy:
                return chooseEnemyPhase;
            default:
                return null;
        }
    }
}
