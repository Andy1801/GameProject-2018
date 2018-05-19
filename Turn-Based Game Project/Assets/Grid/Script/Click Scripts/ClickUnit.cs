using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickUnit : MonoBehaviour {

    public Unit_Properties property;
    public Game_Manager temp;

    private bool active;

    private void OnMouseDown()
    {
        Debug.Log("Clicked Unit");
        property.position = new Vector2(transform.position.x, transform.position.z);

        //Calls highlight tile script here
        //Calls movement script
        //Once movement is done unhighlight all the highlighted tiles.
    }
}
