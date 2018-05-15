using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Tile", menuName = "Tile property/Tile")]
public class Tile_Properties : ScriptableObject {

	public string tileName;
	public int move_cost = 1;
	public bool isWalkable = true;
	public int damage = 0;
	public GameObject tile_Prefab;
}
