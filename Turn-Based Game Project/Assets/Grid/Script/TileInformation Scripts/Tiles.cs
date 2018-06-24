using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Holds information about a specific tile on the grid.
/// 
/// Information held:
///     A scriptable object with basics about the tile
///     The unit on the tile or lack there of
///     The position that the tile is on
///     Whether the tile is active or not
/// </summary>
public class Tiles : MonoBehaviour {

    public Tile_Properties property;

    private Allies unitOn;

    private float x;
    private float z;

    private bool highlighted;

    // Properties: Getter and Setter functions
    public Allies UnitOn
    {
        get { return unitOn; }
        set { unitOn = value; }
    }

    public float xPosition
    {
        get { return x; }
    }

    public float zPosition
    {
        get { return z; }
    }

    public bool Highlighted
    {
        get { return highlighted; }
        set { highlighted = value;}
    }

	// Use this for initialization
	private void Start () {
        x = transform.position.x;
        z = transform.position.z;
	}

    //Highlights the tiles that the player can walk on and then lets the tiles know that they are highlighted.
    public void highlight(bool toHighlight, Human unit)
    {
        Renderer tileMat = GetComponent<Renderer>();

        if (toHighlight)
        {
            if (unit.Moved)
                tileMat.material.color = Color.red;
            else
                tileMat.material.color = Color.yellow;

            Highlighted = true;
        }
        else
        {
            tileMat.material.color = property.originalColor;
            Highlighted = false;
        }
    }
}
