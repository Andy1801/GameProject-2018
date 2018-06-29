using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Skill", menuName = "Property/Skill")]
public class Skill_Properties : ScriptableObject {

    public string skillName;
    public string type;
    public int damage;
    public int heal;
    public int range;
    public Tiles createdTile;
}
