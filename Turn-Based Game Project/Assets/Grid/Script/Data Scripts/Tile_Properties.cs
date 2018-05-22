using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Tile", menuName = "Property/Tile")]
public class Tile_Properties : ScriptableObject {

	public string tileName;
    public int move_cost = 1;
	public int damage = 0;
    public bool isWalkable = true;
    public Color moveableColor;
    public Color unMoveableColor;
	public GameObject tile_Prefab;
}
