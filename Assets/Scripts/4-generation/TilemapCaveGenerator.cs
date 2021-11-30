using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;


/**
 * This class demonstrates the CaveGenerator on a Tilemap.
 * 
 * By: Erel Segal-Halevi
 * Since: 2020-12
 */

public class TilemapCaveGenerator: MonoBehaviour {
    [SerializeField] Tilemap tilemap = null;

    [Tooltip("The tile that represents a wall (an impassable block)")]
    [SerializeField] TileBase wallTile = null;

    [Tooltip("The tile that represents a floor (a passable block)")]
    [SerializeField] TileBase floorTile = null;

    [Tooltip("The percent of walls in the initial random map")]
    [Range(0, 1)]
    [SerializeField] float randomFillPercent = 0.5f;

    [Tooltip("Length and height of the grid")]
    [SerializeField] int gridSize = 100;

    [Tooltip("How many steps do we want to simulate?")]
    [SerializeField] int simulationSteps = 20;

    [Tooltip("For how long will we pause between each simulation step so we can look at the result?")]
    [SerializeField] float pauseTime = 1f;

    private CaveGenerator caveGenerator;

    // How much % to enlarge the map by
    [SerializeField] float enlargeBy;

    void Start()  {
        //To get the same random numbers each time we run the script
        Random.InitState(100);

        caveGenerator = new CaveGenerator(randomFillPercent, gridSize);
        caveGenerator.RandomizeMap();
                
        //For testing that init is working
        GenerateAndDisplayTexture(caveGenerator.GetMap());
            
        //Start the simulation
        StartCoroutine(SimulateCavePattern());

        placeCharacters();
    }

    // CHANGE
    public void enlargeMap()
    {
        int toAdd = (int)(gridSize * enlargeBy / 100f);
        gridSize += toAdd;
        Debug.Log("old new " + gridSize + " " + toAdd);

        caveGenerator.setGridSize(gridSize);

        caveGenerator.RandomizeMap();

        //For testing that init is working
        GenerateAndDisplayTexture(caveGenerator.GetMap());

        //Start the simulation
        StartCoroutine(SimulateCavePattern());

        placeCharacters();
    }

    //Do the simulation in a coroutine so we can pause and see what's going on
    private IEnumerator SimulateCavePattern()  {
        for (int i = 0; i < simulationSteps; i++)   {
            yield return new WaitForSeconds(pauseTime);

            //Calculate the new values
            caveGenerator.SmoothMap();

            //Generate texture and display it on the plane
            GenerateAndDisplayTexture(caveGenerator.GetMap());
        }
        Debug.Log("Simulation completed!");
    }



    //Generate a black or white texture depending on if the pixel is cave or wall
    //Display the texture on a plane
    private void GenerateAndDisplayTexture(int[,] data) {
        for (int y = 0; y < gridSize; y++) {
            for (int x = 0; x < gridSize; x++) {
                var position = new Vector3Int(x, y, 0);
                // Change
                var tile = data[x, y] == 1 ? floorTile: wallTile;
                tilemap.SetTile(position, tile);
            }
        }
    }

    public void placeCharacters()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        GameObject[] Enemies = GameObject.FindGameObjectsWithTag("Enemy");

        player.transform.position = tilemap.GetCellCenterWorld(new Vector3Int(0, 0, 0));
        Enemies[0].transform.position = tilemap.GetCellCenterWorld(new Vector3Int(gridSize - 1, 0, 0));
        Enemies[1].transform.position = tilemap.GetCellCenterWorld(new Vector3Int(0, gridSize - 1, 0));
    }

    public int getTileMapSize()
    {
        return gridSize;
    }
}
