using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The purpose of this script is to create the grid for the map based on the tiles 
/// we gave it as well as a few conditions.
/// 
/// FIXES NEEDED:
/// 1. Make the highlight transition slowly rather then instantly
/// </summary>

public class Game_Manager : MonoBehaviour {

    // Holds the property of the tiles
	public Tile_Properties[] tiles;
	
	[SerializeField]
	private int dimensions = 10;

	private int[,] tile_location;

    [System.NonSerialized]
	public Tile_Neighbors[,] graph;

    [SerializeField]
    private GameObject parentGrid;
	

    // Creates the grid based on the dimensions given and the basic grass tiles.
	void Start () {
		tile_location = new int[dimensions,dimensions];
        graph = new Tile_Neighbors[dimensions, dimensions];

		for(int x = 0; x < dimensions; x++)
		{
			for(int y = 0; y < dimensions; y++)
			{
				int temp = tile_location[x,y];
                if (parentGrid)
				    Instantiate(tiles[temp].tile_Prefab, new Vector3(x, 0, y), Quaternion.identity, parentGrid.transform);
			}
		}

        SetNeighbors();
        FindNeighbors();
    }

    //Initilizes the array for each of the neighbors related to one of the tiles.
    private void SetNeighbors()
    {
        for (int x = 0; x < dimensions; x++)
        {
            for (int y = 0; y < dimensions; y++)
            {
                bool conditionX = (x == 0 || x == (dimensions - 1));
                bool conditionY = (y == 0 || y == (dimensions - 1));


                graph[x, y] = new Tile_Neighbors(conditionX, conditionY);
                graph[x, y].position = new Vector2(x, y);
            }
        }
    }


    //Finds the neighbors and stores them.
    private void FindNeighbors()
    {
        int counter = 0;

        //Goes through the x-axis
        for (int x = 0; x < dimensions; x++)
        {
            //Goes through the y-axis and checks for certain conditions to set the neighbor.
            for (int y = 0; y < dimensions; y++)
            {
                if (x != 0)
                {
                    graph[x, y].neighbors[counter] = graph[x - 1, y];
                    counter++;
                }
                if(x != (dimensions - 1))
                {
                    graph[x, y].neighbors[counter] = graph[x + 1, y];
                    counter++;
                }
                if (y != 0)
                {
                    graph[x, y].neighbors[counter] = graph[x, y - 1];
                    counter++;
                }
                if (y != (dimensions - 1))
                    graph[x, y].neighbors[counter] = graph[x, y + 1];

                counter = 0;
            }
        }
    }

    public void highlight(GameObject tile, Tile_Properties tileInfo)
    {
        Renderer tileMat = tile.GetComponent<Renderer>();

        tileMat.material.color = tileInfo.moveableColor;

    }
}
