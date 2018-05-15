using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickTile : MonoBehaviour {

    public Tile_Properties property;
    private float x;
    private float y;

    private void OnMouseDown()
    {
        Debug.Log("Clicked");

        Debug.Log(property.tileName);

        x = transform.position.x;
        y = transform.position.z;
    }
}
