using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Holds information about this current ally unit.
/// 
/// Information held:
///     A scriptable object with the basics of the unit
///     Whether the unit is currently active (moving) or not
///     
/// TODO: Have to setup so that when a new turn starts it resets all the players to false.
/// </summary>
public class Allies : Human {

    public Unit_Properties property; 

    private bool active;

    public bool Active
    {
        get { return active; }
        set { active = value; }
    }

    private new void Start()
    {
        base.Start();
        active = false;
        moved = false;
        attacked = false;

        //Gets the tile the unit is on in the beginning of the level and sets its self
        Game_Manager.instance.TileOn(transform.position).UnitOn = this;
    }

    //To be removed later
    private void changeColor(Color color, bool toHighlight)
    {
        //Change the color of the current active unit
        Renderer playerMat = GetComponent<Renderer>();

        if (toHighlight)
        {
            playerMat.material.color = color;
        }
        else
        {
            playerMat.material.color = color;
        }
    }

    /// <summary>
    /// If the unit is not active and we are not in any particular state then call pathfinding.
    /// </summary>
    public override void CallPathFinding()
    {
        if (!active && stateManager.CantDoState(pathFinder.GetDoNotStates))
            pathFinder.pathFinding(this);
    }
    //Sets this unit to active and the state to pending. This is called through the tracker script
    public void SetActive()
    {
        Active = true;
        changeColor(Color.black, true);
        Game_Manager.instance.ActiveUnit = this;
        stateManager.Pending = true;
    }

    //Sets the unit that is currently held in the game manager to false. Typically this is called through the
    //tracker script.
    public void SetInActive()
    {
        Active = false;
        stateManager.Pending = false;
        changeColor(Color.white, false);
    }
}
