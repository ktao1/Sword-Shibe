using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

/*
*   Class: GenerateMap
*
*   Programmers: Samuel Tayman, Maggie Poletti, Kecheng Tao
*
*   Description: Using a list of tiles this class generates a map at random 
*
*/
public class GenerateMap : MonoBehaviour
{

    //Dictates the amount of tiles that will be generated
    public int rows = 10;
    public int columns = 10;

    //Start tile determines where the player will spawn
    public GameObject startTile;
    //Player object to place in the map
    public GameObject player;

    /*
    *   NOTE: The following arrays may need to be split into biome specific arrays
    *   
    *   TODO: 
    *   - Redefine arrays per biome 
    *   - Min and max range for biome room count
    *   - Connect rooms together through corridors
    *   - Connect biomes together
    */

    //Array of objects holding placeable tiles
    public GameObject[] groundTiles;
    public GameObject[] cornerTiles;
    public GameObject[] edgeTiles;

    //Array of objects holding items to be placed onto tiles
    public GameObject[] items;

    //Array of objects holding enemies to be placed onto tiles
    public GameObject[] enemies;
    public GameObject[] obstacles;

    public GameObject portal;

    public int seed = 0;

    private Transform board;
    private List<Vector3> grid = new List<Vector3>();
    private int numOfPortals;
    private int numOfObstacles;
    private int numOfOpenTiles;

    private float obstacleProbability = 0f;

    // Start is called before the first frame update
    void Start()
    {
        LoadMapGrid();
        numOfPortals = 1;
        numOfObstacles = Random.Range(5, 20);
        numOfOpenTiles = rows * columns - (rows * 2 + ((columns - 2) * 2));
        InstantiateTiles(numOfObstacles, numOfOpenTiles);

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void NextLevel()
    {
        foreach(Transform child in board)
        {
            GameObject.Destroy(child.gameObject);
        }

        GameObject.Destroy(board.gameObject);
        numOfObstacles = Random.Range(5, 20);
        numOfOpenTiles = rows * columns - (rows * 2 + ((columns - 2) * 2));
        InstantiateTiles(numOfObstacles, numOfOpenTiles);
    }

    private void LoadMapGrid()
    {
        float i = 0f;
        while (i < (columns + 1) * 3.5f)
        {
            float j = 0f;
            while (j < (rows + 1) * 3.5f)
            {
                grid.Add(new Vector3(i, j, 0f));
                j += 3.5f;
            }
            i += 3.5f;
        }
    }

    /*
     *  Function: InstantiateTiles()
     * 
     *  Description: Generates the board based on the given rows and columns
     *  
     *  TODO: 
     *  - Noise algorithm to add randomness to map
     *  - Store points into a List and decide tile type at each (Floor, Wall, Enemy, Item)
     *  - During loop decide to place decoration pieces (Grass, Trees, Water)
     * 
     */
    void InstantiateTiles(int obstacleCount, int tileCount)
    {
        board = new GameObject("Board").transform;
        board.SetParent(gameObject.transform);

        obstacleProbability = ((float)numOfObstacles) / numOfOpenTiles;

        float i = 0f;
        while(i < (columns + 1) * 3.5f)
        {
            float j = 0f;
            while(j < (rows + 1) * 3.5f)
            {
                GameObject selectedTile = groundTiles[Random.Range(0, groundTiles.Length)];

                //Check for edges and place edge/corner tiles appropriately

                //90-degree Z rotation corner tile, bottom-left
                if (i == 0 && j == 0)
                {
                    selectedTile = cornerTiles[Random.Range(0, cornerTiles.Length)];

                    GameObject tileInstance = Instantiate(selectedTile, new Vector3(i, j, 0f), Quaternion.identity) as GameObject;

                    tileInstance.transform.SetParent(board);
                    tileInstance.transform.eulerAngles = new Vector3(tileInstance.transform.eulerAngles.x, tileInstance.transform.eulerAngles.y, tileInstance.transform.eulerAngles.z + 90);
                }
                //Base corner tile, top-left
                else if (i == 0 && j == rows * 3.5)
                {
                    selectedTile = cornerTiles[Random.Range(0, cornerTiles.Length)];

                    GameObject tileInstance = Instantiate(selectedTile, new Vector3(i, j, 0f), Quaternion.identity) as GameObject;

                    tileInstance.transform.SetParent(board);

                }
                //180-degree Z rotation corner tile, bottom-right
                else if (i == columns * 3.5 && j == 0)
                {
                    selectedTile = cornerTiles[Random.Range(0, cornerTiles.Length)];

                    GameObject tileInstance = Instantiate(selectedTile, new Vector3(i, j, 0f), Quaternion.identity) as GameObject;

                    tileInstance.transform.SetParent(board);
                    tileInstance.transform.eulerAngles = new Vector3(tileInstance.transform.eulerAngles.x, tileInstance.transform.eulerAngles.y, tileInstance.transform.eulerAngles.z + 180);
                }
                //-90-degree Z rotation corner tile, top-right
                else if (i == columns * 3.5 && j == rows * 3.5)
                {
                    selectedTile = cornerTiles[Random.Range(0, cornerTiles.Length)];

                    GameObject tileInstance = Instantiate(selectedTile, new Vector3(i, j, 0f), Quaternion.identity) as GameObject;

                    tileInstance.transform.SetParent(board);
                    tileInstance.transform.eulerAngles = new Vector3(tileInstance.transform.eulerAngles.x, tileInstance.transform.eulerAngles.y, tileInstance.transform.eulerAngles.z - 90);
                }

                //Edge tiles, left-edge
                else if (i == 0)
                {

                    selectedTile = edgeTiles[Random.Range(0, edgeTiles.Length)];

                    GameObject tileInstance = Instantiate(selectedTile, new Vector3(i, j, 0f), Quaternion.identity) as GameObject;

                    tileInstance.transform.SetParent(board);
                }
                //180-degree Z rotation edge tiles, right-edge
                else if (i == columns * 3.5)
                {

                    selectedTile = edgeTiles[Random.Range(0, edgeTiles.Length)];

                    GameObject tileInstance = Instantiate(selectedTile, new Vector3(i, j, 0f), Quaternion.identity) as GameObject;

                    tileInstance.transform.SetParent(board);
                    tileInstance.transform.eulerAngles = new Vector3(tileInstance.transform.eulerAngles.x, tileInstance.transform.eulerAngles.y, tileInstance.transform.eulerAngles.z + 180);
                }
                //90-degree Z rotation edge tiles, bottom-edge
                else if (j == 0)
                {

                    if (i == columns * 3.5 / 2)
                    {
       
                        GameObject playerInstance = GameObject.FindWithTag("Player");
                        playerInstance.transform.position = new Vector3(i, j, 0f);

                    }

                    selectedTile = edgeTiles[Random.Range(0, edgeTiles.Length)];

                    GameObject tileInstance = Instantiate(selectedTile, new Vector3(i, j, 0f), Quaternion.identity) as GameObject;

                    tileInstance.transform.SetParent(board);
                    tileInstance.transform.eulerAngles = new Vector3(tileInstance.transform.eulerAngles.x, tileInstance.transform.eulerAngles.y, tileInstance.transform.eulerAngles.z + 90);
                }
                //-90-degree Z rotation edge tiles, top-edge
                else if (j == rows * 3.5)
                {

                    if (i == columns * 3.5 / 2)
                    {
                        //Place portal
                        GameObject portalInstance = Instantiate(portal, new Vector3(i, j, 0f), Quaternion.identity);

                        portalInstance.transform.SetParent(board);
                    }

                    selectedTile = edgeTiles[Random.Range(0, edgeTiles.Length)];

                    GameObject tileInstance = Instantiate(selectedTile, new Vector3(i, j, 0f), Quaternion.identity) as GameObject;

                    tileInstance.transform.SetParent(board);
                    tileInstance.transform.eulerAngles = new Vector3(tileInstance.transform.eulerAngles.x, tileInstance.transform.eulerAngles.y, tileInstance.transform.eulerAngles.z - 90);
                }
                //Add tiles randomly into scene
                else
                {
                    GameObject tileInstance = Instantiate(selectedTile, new Vector3(i, j, 0f), Quaternion.identity) as GameObject;
                    
                    if(Random.Range(0f, 1f) <= obstacleProbability)
                    {
                        GameObject obstacleTile;
                        if(Random.Range(0f, 1f) <= 0.55f)
                        {
                            
                            obstacleTile = Instantiate(enemies[Random.Range(0, enemies.Length)], new Vector3(i, j, 0f), Quaternion.identity);

                            obstacleTile.transform.SetParent(board);
                          
                            numOfObstacles = numOfObstacles - 1;
                        }
                        else
                        {
                            obstacleTile = Instantiate(obstacles[Random.Range(0, obstacles.Length)], new Vector3(i, j, 0f), Quaternion.identity);
                            
                        }

                        obstacleTile.transform.SetParent(board);
                        obstacleCount = obstacleCount - 1;

                    }

                    tileInstance.transform.SetParent(board);
                    tileCount = tileCount - 1;
                }
                j += 3.5f;
            }
            i += 3.5f;
            if(tileCount != 0)
                obstacleProbability = ((float)obstacleCount) / tileCount;
        }

       
    }
}
