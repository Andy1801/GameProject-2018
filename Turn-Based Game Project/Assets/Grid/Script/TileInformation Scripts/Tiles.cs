using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tiles : MonoBehaviour {

    public Tile_Properties property;

    private GameObject unitOn;

    private float x;
    private float z;

    private bool highlighted;

    public GameObject UnitOn
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
}
