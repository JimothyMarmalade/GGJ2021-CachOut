  
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;

public class LevelBuilder : MonoBehaviour
{
    [Header("Level Build Data")]
    public GameObject PlayerPointer;
    public int LevelSize = 5;

    public int ObstacleCount = 1;

    public int TreasureCount = 1;

    [Header("Tile Graphics")]
    public Tilemap BaseTilemap;
    public Tilemap ObstacleTilemap;
    public Tilemap TreasureTilemap;
    public Tile[] GroundTiles;
    public Tile[] ObstacleTiles;

    public Tile[] TreasureTiles;

    [Header("Calculation Variables")]
    public int[,] ObstacleLocations;
    public int[,] TreasureLocations;


    //Whenever the new level is loaded, it needs to generate ground tiles, obstacles, and treasures
    public void Start()
    {
        DrawNewBoard();
    }

    public void Update()
    {
        if (Input.GetKeyDown("p"))
            DrawNewBoard();
    }


    public void DrawNewBoard()
    {
        BaseTilemap.ClearAllTiles();
        ObstacleTilemap.ClearAllTiles();
        TreasureTilemap.ClearAllTiles();

        //Instantiate Obstacle and Treasure location guides
        ObstacleLocations = new int[LevelSize, LevelSize];
        TreasureLocations = new int[LevelSize, LevelSize];
        int gridOffset = (LevelSize-1)/2;
        
        //First, get the levelsize and calculate where the edges of the tilemap will be.
        int levelBounds = (LevelSize-1)/2;

        //First calculate where obstacles will be placed.
        for (int i = 0; i < ObstacleCount; i++)
        {
            bool locationFound = false;
            int xPos = Random.Range(-levelBounds, levelBounds+1);
            int yPos = Random.Range(-levelBounds, levelBounds+1);

            while (!locationFound)
            {
                if (ObstacleLocations[xPos+gridOffset, yPos+gridOffset] != 1)
                {
                    locationFound = true;
                }
                else
                {
                    xPos = Random.Range(-levelBounds, levelBounds+1);
                    yPos = Random.Range(-levelBounds, levelBounds+1);
                }
            }

            ObstacleLocations[xPos+gridOffset, yPos+gridOffset] = 1;
            Debug.Log("ObstacleLocations marked at [" + (xPos+gridOffset) + "," + (yPos+gridOffset) + "]");
        }

        //Now, calculate where the treasures will go
        for (int i = 0; i < TreasureCount; i++)
        {
            bool locationFound = false;
            int xPos = Random.Range(-levelBounds, levelBounds+1);
            int yPos = Random.Range(-levelBounds, levelBounds+1);

            while (!locationFound)
            {
                if (ObstacleLocations[xPos+gridOffset, yPos+gridOffset] != 1 && TreasureLocations[xPos+gridOffset, yPos+gridOffset] != 1)
                {
                    locationFound = true;
                }
                else
                {
                    xPos = Random.Range(-levelBounds, levelBounds+1);
                    yPos = Random.Range(-levelBounds, levelBounds+1);
                }
            }
            TreasureLocations[xPos+gridOffset, yPos+gridOffset] = 1;
            Debug.Log("TreasureLocations marked at [" + (xPos+gridOffset) + "," + (yPos+gridOffset) + "]");
        }
        
        //With the bounds known, begin drawing the ground tiles
        DrawGroundTiles(-levelBounds, levelBounds);

        //Next draw obstacles on all the assigned tiles
        DrawObstacleTiles(ObstacleLocations, gridOffset);

        //Draw the treasure tiles on all assigned tiles
        DrawTreasureTiles(TreasureLocations, gridOffset);

        //Finally, Spawn the Player somewhere that isn't on top of an obstacle
        ReplacePlayer(BaseTilemap, gridOffset);
    }

    private void DrawGroundTiles(int negBound, int posBounds)
    {
        for (int i = negBound; i <= posBounds; i++)
        {
            for (int j = negBound; j <= posBounds; j++)
            {
                Vector3Int location = new Vector3Int(i, j, 0);
                Tile drawnTile = GroundTiles[Random.Range(0, GroundTiles.Length)];
                BaseTilemap.SetTile(location, drawnTile);
                //Debug.Log("Drew Ground Tile at " + i + "," + j);
            }
        }
    }

    private void DrawObstacleTiles(int[,] obs, int offset)
    {
        for (int i = 0; i < LevelSize; i++)
        {
            for (int j = 0; j < LevelSize; j++)
            {
                //Debug.Log("Trying ObstacleLocations[" + i + "," + j + "]");
                if(obs[i, j] == 1)
                {
                    Vector3Int location = new Vector3Int(i-offset, j-offset, 0);
                    Tile drawnTile = ObstacleTiles[Random.Range(0, ObstacleTiles.Length)];
                    ObstacleTilemap.SetTile(location, drawnTile);
                    Debug.Log("Drew Obstacle Tile at " + (i-2) + "," + (j-2));
                }
            }
        }
    }
    private void DrawTreasureTiles(int[,] obs, int offset)
    {
        for (int i = 0; i < LevelSize; i++)
        {
            for (int j = 0; j < LevelSize; j++)
            {
                //Debug.Log("Trying TreasureLocations[" + i + "," + j + "]");
                if(obs[i, j] == 1)
                {
                    Vector3Int location = new Vector3Int(i-offset, j-offset, 0);
                    Tile drawnTile = TreasureTiles[Random.Range(0, TreasureTiles.Length)];
                    TreasureTilemap.SetTile(location, drawnTile);
                    Debug.Log("Drew Treasure Tile at " + (i-2) + "," + (j-2));
                }
            }
        }
    }

    private void ReplacePlayer(Tilemap map, int gridOffset)
    {
        //Get valid point on grid
        bool locationFound = false;
        int xPos = Random.Range(-gridOffset, gridOffset+1);
        int yPos = Random.Range(-gridOffset, gridOffset+1);

        while (!locationFound)
        {
            if (ObstacleLocations[xPos+gridOffset, yPos+gridOffset] != 1)
            {
                locationFound = true;
            }
            else
            {
                xPos = Random.Range(-gridOffset, gridOffset+1);
                yPos = Random.Range(-gridOffset, gridOffset+1);
            }
        }

        //Valid point found, convert to cell, then world position
        Vector3Int cellPos = new Vector3Int(xPos, yPos, 0);
        Vector3 offset = new Vector3(0.5f, 0.5f, 0);
        Vector3 spawn = map.GetComponentInParent<GridLayout>().CellToWorld(cellPos) + offset;
        Quaternion rot = new Quaternion(0, 0, 0, 0);

        PlayerPointer.transform.SetPositionAndRotation(spawn, rot);
    }
}