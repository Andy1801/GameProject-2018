using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Tile_Properties {

	public string name;
	public int move_cost = 1;
	public bool isWalkable = true;
	public int damage = 0;
	public GameObject tile_Prefab;
}
