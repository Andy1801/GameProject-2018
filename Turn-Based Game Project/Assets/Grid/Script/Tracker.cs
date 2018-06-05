using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tracker : MonoBehaviour {

    private Tiles tile;
    private Game_Manager gameManager;

    private void Start()
    {
        gameManager = GameObject.FindWithTag("GameManager").GetComponent<Game_Manager>();

        tile = gameManager.TileOn(transform.position);
    }

    private void Update()
    {
        if(Input.GetButtonDown("Horizontal"))
        {
            Debug.Log("Horizontal");
        }
    }

}
