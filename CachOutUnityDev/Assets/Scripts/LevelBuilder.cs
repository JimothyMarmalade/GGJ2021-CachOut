using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;

public class LevelBuilder : MonoBehaviour
{
    [Header("Level Build Data")]
	public int stageNo = 1;
	public static int MaxHorizSize = 60;
	public static int MaxVertSize = 40;
    public int LevelHorizSize = Math.Max(5+(stageNo-1)*3/2, MaxHorizSize);
	public int LevelVertSize = Math.Max(5+(stageNo-1), MaxVertSize);

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
        //Instantiate Obstacle and Treasure location guides
        ObstacleLocations = new int[LevelHorizSize, LevelVertSize];
        TreasureLocations = new int[LevelHorizSize, LevelVertSize];
        //int gridOffset = (LevelSize-1)/2;
        
        //First, get the levelsize and calculate where the edges of the tilemap will be.
        //int levelBounds = (LevelSize-1)/2;

        //First calculate where obstacles will be placed.
        for (int i = 0; i < ObstacleCount; i++)
        {
            bool locationFound = false;
            int xPos = Random.Range(0, LevelHorizSize-1);
            int yPos = Random.Range(0, LevelVertSize-1);

            while (!locationFound)
            {
                if (ObstacleLocations[xPos, yPos] != 1)
                {
                    locationFound = true;
                }
                else
                {
                    xPos = Random.Range(0, LevelHorizSize-1);
                    yPos = Random.Range(0, LevelVertSize-1);
                }
            }

            ObstacleLocations[xPos, yPos] = 1;
            Debug.Log("ObstacleLocations marked at [" + (xPos) + "," + (yPos) + "]");
        }

        //Now, calculate where the treasures will go
        for (int i = 0; i < TreasureCount; i++)
        {
            bool locationFound = false;
            int xPos = Random.Range(0, LevelHorizSize-1);
            int yPos = Random.Range(0, LevelVertSize-1);

            while (!locationFound)
            {
                if (ObstacleLocations[xPos, yPos] != 1 && TreasureLocations[xPos, yPos] != 1)
                {
                    locationFound = true;
                }
                else
                {
                    xPos = Random.Range(0, LevelHorizSize-1);
                    yPos = Random.Range(0, LevelVertSize-1);
                }
            }
            TreasureLocations[xPos, yPos] = 1;
            Debug.Log("TreasureLocations marked at [" + (xPos) + "," + (yPos) + "]");
        }
        
        //With the bounds known, begin drawing the ground tiles
        DrawGroundTiles(LevelHorizSize, LevelVertSize);

        //Next draw obstacles on all the assigned tiles
        DrawObstacleTiles(ObstacleLocations);

        //Finally, draw the treasure tiles on all assigned tiles
        DrawTreasureTiles(TreasureLocations);

    }

    private void DrawGroundTiles(int horizBounds, int vertBounds)
    {
        for (int i = 0; i < horizBounds; i++)
        {
            for (int j = 0; j < vertBounds; j++)
            {
                Vector3Int location = new Vector3Int(i, j, 0);
                Tile drawnTile = GroundTiles[Random.Range(0, GroundTiles.Length-1)];
                BaseTilemap.SetTile(location, drawnTile);
                //Debug.Log("Drew Ground Tile at " + i + "," + j);
            }
        }
    }

    private void DrawObstacleTiles(int[,] obs)
    {
        for (int i = 0; i < LevelHorizSize; i++)
        {
            for (int j = 0; j < LevelVertSize; j++)
            {
                //Debug.Log("Trying ObstacleLocations[" + i + "," + j + "]");
                if(obs[i, j] == 1)
                {
                    Vector3Int location = new Vector3Int(i, j, 0);
                    Tile drawnTile = ObstacleTiles[Random.Range(0, ObstacleTiles.Length-1)];
                    ObstacleTilemap.SetTile(location, drawnTile);
                    Debug.Log("Drew Obstacle Tile at " + (i) + "," + (j));
                }
            }
        }
    }
    private void DrawTreasureTiles(int[,] tre)
    {
        for (int i = 0; i < LevelHorizSize; i++)
        {
            for (int j = 0; j < LevelVertSize; j++)
            {
                //Debug.Log("Trying TreasureLocations[" + i + "," + j + "]");
                if(tre[i, j] == 1)
                {
                    Vector3Int location = new Vector3Int(i, j, 0);
                    Tile drawnTile = TreasureTiles[Random.Range(0, TreasureTiles.Length-1)];
                    TreasureTilemap.SetTile(location, drawnTile);
                    Debug.Log("Drew Treasure Tile at " + (i) + "," + (j));
                }
            }
        }
    }

}