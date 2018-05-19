using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Used to hold the negihbors of a particular location. e.g (Positions (0,0) has (0, 1) and (1,0) as neighbors)
/// </summary>
[System.Serializable]
public class Tile_Neighbors{
    public Tile_Neighbors[] neighbors;
    public Vector2 position;  

    public int counter;

    public Tile_Neighbors( bool conditionX, bool conditionY)
    {
        if (conditionX && conditionY)
            counter = 2;
        else if (conditionX || conditionY)
            counter = 3;
        else
            counter = 4;

        neighbors = new Tile_Neighbors[counter];
    }
}
