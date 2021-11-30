using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WinGame : MonoBehaviour
{
    [SerializeField] Tilemap tileMap;

    void Update()
    {
        int gridSize = tileMap.GetComponent<TilemapCaveGenerator>().getTileMapSize();
        if (transform.position == tileMap.GetCellCenterWorld(new Vector3Int(gridSize - 1, gridSize - 1, 0)))
        {
            tileMap.GetComponent<TilemapCaveGenerator>().enlargeMap();
        }
    }
}
