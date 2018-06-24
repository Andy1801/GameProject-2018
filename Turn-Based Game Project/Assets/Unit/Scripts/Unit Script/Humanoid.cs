using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Human: MonoBehaviour {

    public string UnitName;
    public int attack;
    public int defense;
    public int speed;
    public int attackDistance;

    protected PathFinding pathFinder;
    protected StateManager stateManager;

    protected bool moved;
    protected bool attacked;

    public bool Moved
    {
        get { return moved; }
        set { moved = value; }
    }
    public bool Attacked
    {
        get { return attacked; }
        set { attacked = value; }
    }

    public void Start()
    {
        pathFinder = Game_Manager.instance.GetPathFinder;
        stateManager = Game_Manager.instance.GetState;
    }

    public virtual void CallPathFinding()
    {

    }
}
