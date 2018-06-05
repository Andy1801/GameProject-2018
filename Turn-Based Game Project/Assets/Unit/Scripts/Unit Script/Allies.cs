using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Allies : Human {

    public Unit_Properties property;

    private bool active;

    public bool Active
    {
        get { return active; }
        set { active = false; }
    }

    private void Start()
    {
        active = false;
    }
}
