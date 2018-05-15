using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickUnit : MonoBehaviour {

    public Unit_Properties property;

    private void OnMouseDown()
    {
        Debug.Log("Clicked Unit");

        //Calls highlight tile script here
        //Calls movement script
        //Once movement is done unhighlight all the highlighted tiles.
    }
}
