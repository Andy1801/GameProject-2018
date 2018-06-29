using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CurrentPhase
{
    placement,
    tactic,
    movement,
    ChooseEnemy,
    ChooseAttack,
    end
};

public class PhaseSwapping : MonoBehaviour {

    private TacticPhase tacticPhase;
    private MovementPhase movementPhase;
    private ChooseEnemy chooseEnemyPhase;
    private ChooseAttack chooseAttackPhase;

	// Use this for initialization
	void Awake () {
        tacticPhase = GetComponent<TacticPhase>();
        movementPhase = GetComponent<MovementPhase>();
        chooseEnemyPhase = GetComponent<ChooseEnemy>();
        chooseAttackPhase = GetComponent<ChooseAttack>();
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
            case (int)CurrentPhase.ChooseAttack:
                return chooseAttackPhase;
            default:
                return null;
        }
    }
}
