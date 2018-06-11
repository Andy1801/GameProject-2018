using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CurrentState
{
    rotating,
    moving
};

public class StateManager : MonoBehaviour {

    // These determine the current action occuring in the game
    private bool rotating = false;
    private bool moving = false;

    // These are the properties related to the variables aboves.
    public bool Rotating { set { rotating = value; } }
    public bool Moving { set { moving = value; } }

    public bool CanDoState(int[] actions)
    {
        foreach (int current in actions)
        {
            if (GetState(current))
                return false;
        }
        return true;
    }

    /// <summary>
    /// Returns the specfic actions bool to see if it is active or not.
    /// </summary>
    /// <param name="action"></param>
    private bool GetState(int action)
    {
        switch (action)
        {
            case (int)CurrentState.rotating:
                return rotating;
            case (int)CurrentState.moving:
                return moving;
            default:
                return false;
        }
    }
}
