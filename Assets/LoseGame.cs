using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class LoseGame : MonoBehaviour
{
    [SerializeField] Tilemap tileMap;
    private GameObject[] enemies;

    private void Start()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
    }
    void Update()
    {
        if (transform.position == enemies[0].transform.position ||
            transform.position == enemies[1].transform.position)
        {
            tileMap.GetComponent<TilemapCaveGenerator>().placeCharacters();
        }
    }
}
