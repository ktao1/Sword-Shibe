using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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


    //Array of objects holding placeable tiles
    public GameObject[] groundTiles;
    public GameObject[] cornerTiles;
    public GameObject[] edgeTiles;


    /*
    *   NOTE: The following arrays may need to be split into biome specific arrays
    *
    */

    //Array of objects holding items to be placed onto tiles
    public GameObject[] items;
    //Array of objects holding enemies to be placed onto tiles
    public GameObject[] enemies;
    private Transform board;

    // Start is called before the first frame update
    void Start()
    {
        InstantiateTiles();

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void InstantiateTiles()
    {
        board = new GameObject("Board").transform;

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
                    selectedTile = edgeTiles[Random.Range(0, edgeTiles.Length)];

                    GameObject tileInstance = Instantiate(selectedTile, new Vector3(i, j, 0f), Quaternion.identity) as GameObject;

                    tileInstance.transform.SetParent(board);
                    tileInstance.transform.eulerAngles = new Vector3(tileInstance.transform.eulerAngles.x, tileInstance.transform.eulerAngles.y, tileInstance.transform.eulerAngles.z + 90);
                }
                //-90-degree Z rotation edge tiles, top-edge
                else if (j == rows * 3.5)
                {
                    selectedTile = edgeTiles[Random.Range(0, edgeTiles.Length)];

                    GameObject tileInstance = Instantiate(selectedTile, new Vector3(i, j, 0f), Quaternion.identity) as GameObject;

                    tileInstance.transform.SetParent(board);
                    tileInstance.transform.eulerAngles = new Vector3(tileInstance.transform.eulerAngles.x, tileInstance.transform.eulerAngles.y, tileInstance.transform.eulerAngles.z - 90);
                }
                else
                {
                    GameObject tileInstance = Instantiate(selectedTile, new Vector3(i, j, 0f), Quaternion.identity) as GameObject;

                    tileInstance.transform.SetParent(board);
                }
                j += 3.5f;
            }
            i += 3.5f;
        }

        /*
        for (int i = 0; i < columns; i++)
        {
            for(int j = 0; j < rows; j++)
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
                else if (i == 0 && j == rows - 1)
                {
                    selectedTile = cornerTiles[Random.Range(0, cornerTiles.Length)];

                    GameObject tileInstance = Instantiate(selectedTile, new Vector3(i, j, 0f), Quaternion.identity) as GameObject;

                    tileInstance.transform.SetParent(board);

                }
                //180-degree Z rotation corner tile, bottom-right
                else if (i == columns - 1 && j == 0)
                {
                    selectedTile = cornerTiles[Random.Range(0, cornerTiles.Length)];

                    GameObject tileInstance = Instantiate(selectedTile, new Vector3(i, j, 0f), Quaternion.identity) as GameObject;

                    tileInstance.transform.SetParent(board);
                    tileInstance.transform.eulerAngles = new Vector3(tileInstance.transform.eulerAngles.x, tileInstance.transform.eulerAngles.y, tileInstance.transform.eulerAngles.z + 180);
                }
                //-90-degree Z rotation corner tile, top-right
                else if (i == columns - 1 && j == rows - 1)
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
                else if(i == columns - 1)
                {
                    selectedTile = edgeTiles[Random.Range(0, edgeTiles.Length)];

                    GameObject tileInstance = Instantiate(selectedTile, new Vector3(i, j, 0f), Quaternion.identity) as GameObject;

                    tileInstance.transform.SetParent(board);
                    tileInstance.transform.eulerAngles = new Vector3(tileInstance.transform.eulerAngles.x, tileInstance.transform.eulerAngles.y, tileInstance.transform.eulerAngles.z + 180);
                }
                //90-degree Z rotation edge tiles, bottom-edge
                else if(j == 0)
                {
                    selectedTile = edgeTiles[Random.Range(0, edgeTiles.Length)];

                    GameObject tileInstance = Instantiate(selectedTile, new Vector3(i, j, 0f), Quaternion.identity) as GameObject;

                    tileInstance.transform.SetParent(board);
                    tileInstance.transform.eulerAngles = new Vector3(tileInstance.transform.eulerAngles.x, tileInstance.transform.eulerAngles.y, tileInstance.transform.eulerAngles.z + 90);
                }
                //-90-degree Z rotation edge tiles, top-edge
                else if (j == rows - 1)
                {
                    selectedTile = edgeTiles[Random.Range(0, edgeTiles.Length)];

                    GameObject tileInstance = Instantiate(selectedTile, new Vector3(i, j, 0f), Quaternion.identity) as GameObject;

                    tileInstance.transform.SetParent(board);
                    tileInstance.transform.eulerAngles = new Vector3(tileInstance.transform.eulerAngles.x, tileInstance.transform.eulerAngles.y, tileInstance.transform.eulerAngles.z - 90);
                }
                else
                {
                    GameObject tileInstance = Instantiate(selectedTile, new Vector3(i, j, 0f), Quaternion.identity) as GameObject;

                    tileInstance.transform.SetParent(board);
                }
            }
        }
        */
    }
}
