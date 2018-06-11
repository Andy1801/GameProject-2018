using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickParent : MonoBehaviour {

    protected Game_Manager gameManager;
    protected StateManager stateManager;

    protected void Start()
    {
        gameManager = Game_Manager.instance;
        stateManager = gameManager.GetState;
    }

    protected virtual void OnMouseDown()
    {
        //Call UI and show the information needed.
    }

}
