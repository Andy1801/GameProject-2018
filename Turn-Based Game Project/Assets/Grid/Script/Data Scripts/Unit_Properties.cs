using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Unit", menuName = "Property/Unit")]
public class Unit_Properties : ScriptableObject {

    public string unitName;

    public float attack;
    public float defense;
    public float speed;

    //Portential category for a specific skill here. Using a C# script that gets called whenever you used a skill.

    public Vector2 position = Vector2.zero;

}
