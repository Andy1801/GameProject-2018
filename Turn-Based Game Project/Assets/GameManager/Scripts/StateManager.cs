using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Keeps check of what is happening on the grid and is called when
/// a script wants to know if it could do a certain action well another 
/// is performing.
/// 
/// A State is:
///     A certain action that is occuring in the game that does not allow
///     certain other actions to occur.
/// 
/// Created this for the purpose of being able to check more then one
/// condition at a time without writing code.
/// </summary>
public enum CurrentState
{
    chooseAttack,
    rotating,
    tracking,
    pending,
    moving
};

public class StateManager : MonoBehaviour {

    // These determine the current state occuring in the game
    private bool phaseSwapping = false;
    private bool tacticPhase = false;
    private bool chooseAttack = false;
    private bool rotating = false;
    private bool tracking = false;
    private bool pending = false;
    private bool moving = false;

    // These are the properties related to the variables aboves.
    public bool Tactic
    {
        get { return tacticPhase; }
        set { tacticPhase = value; }
    }
    public bool ChoosingAttack
    {
        get { return chooseAttack; }
        set { chooseAttack = value; }
    }

    public bool Rotating
    {
        get { return rotating; }
        set { rotating = value; }
    }
    public bool Tracking
    {
        get { return tracking; }
        set { tracking = value; }
    }
    public bool Pending
    {
        get { return pending; }
        set { pending = value; }
    }
    public bool Moving
    {
        get { return moving; }
        set { moving = value; }
    }

    /// <summary>
    /// Checks whether any of the actions sent through a int array are
    /// active. If so then the action can not be performed.
    /// </summary>
    /// <param name="states">Holds all states that need to be checked</param>
    /// <returns></returns>
    public bool CantDoState(int[] states)
    {
        //If phaseSwapping is happening then don't do any action
        if (phaseSwapping)
            return false;

        foreach (int current in states)
        {
            if (GetState(current))
                return false;
        }
        return true;
    }

    /// <summary>
    /// Returns the specfic actions bool to see if it is active or not.
    /// </summary>
    /// <param name="state"></param>
    private bool GetState(int state)
    {
        switch (state)
        {
            case (int)CurrentState.chooseAttack:
                return ChoosingAttack;
            case (int)CurrentState.rotating:
                return rotating;
            case (int)CurrentState.tracking:
                return tracking;
            case (int)CurrentState.pending:
                return pending;
            case (int)CurrentState.moving:
                return moving;
            default:
                return false;
        }
    }
}
