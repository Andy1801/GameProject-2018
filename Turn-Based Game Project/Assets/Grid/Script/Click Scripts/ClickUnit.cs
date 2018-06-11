using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickUnit : ClickParent {

    private Allies unit;
    private PathFinding pathFinder;

    private new void Start()
    {
        base.Start();
        unit = GetComponent<Allies>();
        pathFinder = gameManager.GetPathFinder;
    }

    protected override void OnMouseDown()
    {
        if (!unit.Active  && stateManager.CanDoState(pathFinder.GetDoNotStates))
        {
            unit.Active = true;

            if (gameManager.ActiveUnit != null)
            {
                gameManager.ActiveUnit.Active = false;
                pathFinder.RemovePath();
            }
             
            gameManager.ActiveUnit = unit;
            pathFinder.pathFinding(new Vector3(transform.position.x, 1.0f, transform.position.z));
        }
    }
}
