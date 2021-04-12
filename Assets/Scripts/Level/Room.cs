using Pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Room
{
    //Multiplier value to adjust for sprite size
    private float spriteMultiplier = 3.5f;

    //x, y position of the room
    public Vector2 point;

    //Number of rows of room
    public int rows;

    //Number of columns of room
    public int columns;

    //Parent asset of room for readability/debugging
    private Transform roomBoard;

    private int numOfObstacles;
    private int numOfOpenTiles;

    /*
     * Grid
     * @Vector3 Point in grid
     * @int Type of tile to place. 0 for ground, 1 for enemy, 2 for obstacle
     */
    private Dictionary<Vector3, GameObject> grid = new Dictionary<Vector3, GameObject>();

    //Neighbors to room
    public Vector2 N = Vector2.negativeInfinity;
    public Vector2 S = Vector2.negativeInfinity;
    public Vector2 E = Vector2.negativeInfinity;
    public Vector2 W = Vector2.negativeInfinity;

    //Defines if room has been generated with tiles placed
    internal bool isInstantiated = false;

    private float obstacleProbability;



    /*
     * Constructor: Room
     * 
     * @_rows Number of rows in room
     * @_columns Number of columns in room
     * @_point x,y coordinates of room
     */
    public Room(int _rows, int _columns, Vector2 _point)
    {
        rows = _rows;
        columns = _columns;
        point = _point;
        numOfObstacles = Random.Range(5, 20);
        numOfOpenTiles = rows * columns - (rows * 2 + ((columns - 2) * 2));

    }

    /*
     * Function: CarveCorePath()
     * 
     * Description: Use Prim's algorithm to mark the core
     * path player can follow to reach each exit of room
     * 
     */
    public void CarveCorePath()
    {

    }

    /*
     * Function: InstantiateRoom()
     * 
     * Description: Iterates through the room grid and 
     * instantiates tiles based on the coordinates marked
     * tile type
     * 
     */
    public void InstantiateRoom(GameObject[] groundTiles, GameObject[] cornerTiles, GameObject[] edgeTiles, GameObject[] enemies, GameObject[] obstacles, GameObject exit)
    {
        //Unique name for current room. This allows us to save room data
        roomBoard = new GameObject(point.ToString()).transform;
        roomBoard.SetParent(GameObject.Find("BiomeBoard").transform);


        //Following statements place exits at each direction to next room
        if (!N.Equals(Vector2.negativeInfinity))
        {
            if (columns % 2 == 1)
            {
                GameObject exitInstance = GameObject.Instantiate(exit, new Vector3((columns * spriteMultiplier / 2) + 1.75f, rows * spriteMultiplier, 0f), Quaternion.identity);
                exitInstance.transform.SetParent(roomBoard);

                //Add component to exit and modify to define neighbor
                Navigator nav = exitInstance.GetComponent<Navigator>();
                nav.currentRoom = point;
                nav.nextRoom = N;
            }
            else
            {
                GameObject exitInstance = GameObject.Instantiate(exit, new Vector3((columns * spriteMultiplier / 2), rows * spriteMultiplier, 0f), Quaternion.identity);
                exitInstance.transform.SetParent(roomBoard);

                //Add component to exit and modify to define neighbor
                Navigator nav = exitInstance.GetComponent<Navigator>();
                nav.currentRoom = point;
                nav.nextRoom = N;
            }

        }

        if (!S.Equals(Vector2.negativeInfinity))
        {
            if (columns % 2 == 1)
            {
                GameObject exitInstance = GameObject.Instantiate(exit, new Vector3((columns * spriteMultiplier / 2) + 1.75f, 0f, 0f), Quaternion.identity);
                exitInstance.transform.SetParent(roomBoard);

                //Add component to exit and modify to define neighbor
                Navigator nav = exitInstance.GetComponent<Navigator>();
                nav.currentRoom = point;
                nav.nextRoom = S;
            }
            else
            {
                GameObject exitInstance = GameObject.Instantiate(exit, new Vector3((columns * spriteMultiplier / 2), 0f, 0f), Quaternion.identity);
                exitInstance.transform.SetParent(roomBoard);

                //Add component to exit and modify to define neighbor
                Navigator nav = exitInstance.GetComponent<Navigator>();
                nav.currentRoom = point;
                nav.nextRoom = S;
            }

        }

        if (!E.Equals(Vector2.negativeInfinity))
        {
            //Sets the E exit
            if (rows % 2 == 1)
            {
                GameObject exitInstance = GameObject.Instantiate(exit, new Vector3(columns * spriteMultiplier, (rows * spriteMultiplier / 2) + 1.75f, 0f), Quaternion.identity);
                exitInstance.transform.SetParent(roomBoard);

                //Add component to exit and modify to define neighbor
                Navigator nav = exitInstance.GetComponent<Navigator>();
                nav.currentRoom = point;
                nav.nextRoom = E;
            }
            else
            {
                GameObject exitInstance = GameObject.Instantiate(exit, new Vector3(columns * spriteMultiplier, (rows * spriteMultiplier / 2), 0f), Quaternion.identity);
                exitInstance.transform.SetParent(roomBoard);

                //Add component to exit and modify to define neighbor
                Navigator nav = exitInstance.GetComponent<Navigator>();
                nav.currentRoom = point;
                nav.nextRoom = E;
            }

        }

        if (!W.Equals(Vector2.negativeInfinity))
        {
            if (rows % 2 == 1)
            {
                GameObject exitInstance = GameObject.Instantiate(exit, new Vector3(0f, (rows * spriteMultiplier / 2) + 1.75f, 0f), Quaternion.identity);
                exitInstance.transform.SetParent(roomBoard);

                //Add component to exit and modify to define neighbor
                Navigator nav = exitInstance.GetComponent<Navigator>();
                nav.currentRoom = point;
                nav.nextRoom = W;
            }
            else
            {
                GameObject exitInstance = GameObject.Instantiate(exit, new Vector3(0f, (rows * spriteMultiplier / 2), 0f), Quaternion.identity);
                exitInstance.transform.SetParent(roomBoard);

                //Add component to exit and modify to define neighbor
                Navigator nav = exitInstance.GetComponent<Navigator>();
                nav.currentRoom = point;
                nav.nextRoom = W;
            }
        }

        int obstacleCount = numOfObstacles;
        int tileCount = numOfOpenTiles;

        obstacleProbability = ((float)numOfObstacles) / numOfOpenTiles;

        float i = 0f;
        while (i < (columns + 1) * 3.5f)
        {
            float j = 0f;
            while (j < (rows + 1) * 3.5f)
            {
                /*
                 * TODO: 
                 * - Add additional edge tiles and perform calculations to
                 * best determine the type of edge to use
                 * 
                 * - Prefab the additional tiles
                 * 
                 */
                GameObject selectedTile = groundTiles[Random.Range(0, groundTiles.Length)];
                
                //90-degree Z rotation corner tile, bottom-left
                if (i == 0 && j == 0)
                {
                    selectedTile = cornerTiles[3];

                    GameObject tileInstance = GameObject.Instantiate(selectedTile, new Vector3(i, j, 0f), Quaternion.identity) as GameObject;
                    grid.Add(new Vector3(i, j, 0f), tileInstance);

                    tileInstance.transform.SetParent(roomBoard);
                }
                //Base corner tile, top-left
                else if (i == 0 && j == rows * 3.5)
                {
                    selectedTile = cornerTiles[0];

                    GameObject tileInstance = GameObject.Instantiate(selectedTile, new Vector3(i, j, 0f), Quaternion.identity) as GameObject;
                    grid.Add(new Vector3(i, j, 0f), tileInstance);

                    tileInstance.transform.SetParent(roomBoard);

                }
                //180-degree Z rotation corner tile, bottom-right
                else if (i == columns * 3.5 && j == 0)
                {
                    selectedTile = cornerTiles[2];

                    GameObject tileInstance = GameObject.Instantiate(selectedTile, new Vector3(i, j, 0f), Quaternion.identity) as GameObject;
                    grid.Add(new Vector3(i, j, 0f), tileInstance);

                    tileInstance.transform.SetParent(roomBoard);
                }
                //-90-degree Z rotation corner tile, top-right
                else if (i == columns * 3.5 && j == rows * 3.5)
                {
                    selectedTile = cornerTiles[1];

                    GameObject tileInstance = GameObject.Instantiate(selectedTile, new Vector3(i, j, 0f), Quaternion.identity) as GameObject;
                    grid.Add(new Vector3(i, j, 0f), tileInstance);

                    tileInstance.transform.SetParent(roomBoard);
                }

                //Edge tiles, left-edge
                else if (i == 0)
                {
                    List<GameObject> temp = new List<GameObject>();
                    temp.Add(edgeTiles[0]);
                    temp.Add(edgeTiles[4]);

                    List<GameObject> available = new List<GameObject>();

                    //Determine which tiles can be placed at current location
                    for (int k = 0; k < temp.Count; k++)
                    {
                        GameObject neighbor = temp[k];

                        if ((!grid.ContainsKey(new Vector3(i - 3.5f, j, 0f)) || (grid.ContainsKey(new Vector3(i - 3.5f, j, 0f)) && neighbor.GetComponent<Connector>().W.CompareTo(grid[new Vector3(i - 3.5f, j, 0f)].GetComponent<Connector>().E) == 0)) &&
                            (!grid.ContainsKey(new Vector3(i + 3.5f, j, 0f)) || (grid.ContainsKey(new Vector3(i + 3.5f, j, 0f)) && neighbor.GetComponent<Connector>().E.CompareTo(grid[new Vector3(i + 3.5f, j, 0f)].GetComponent<Connector>().W) == 0)) &&
                            (!grid.ContainsKey(new Vector3(i, j + 3.5f, 0f)) || (grid.ContainsKey(new Vector3(i, j + 3.5f, 0f)) && neighbor.GetComponent<Connector>().N.CompareTo(grid[new Vector3(i, j + 3.5f, 0f)].GetComponent<Connector>().S) == 0)) &&
                            (!grid.ContainsKey(new Vector3(i, j - 3.5f, 0f)) || (grid.ContainsKey(new Vector3(i, j - 3.5f, 0f)) && neighbor.GetComponent<Connector>().S.CompareTo(grid[new Vector3(i, j - 3.5f, 0f)].GetComponent<Connector>().N) == 0)))
                        {
                            available.Add(neighbor);
                        }
                    }

                    Debug.Log("<color=yellow>Tile Left Count" + available.Count + "</color>");

                    int selection = Random.Range(0, available.Count);
                    if (available.Count > 0)
                        selectedTile = available[selection];
                    available.Clear();

                    GameObject tileInstance = GameObject.Instantiate(selectedTile, new Vector3(i, j, 0f), Quaternion.identity) as GameObject;
                    grid.Add(new Vector3(i, j, 0f), tileInstance);

                    tileInstance.transform.SetParent(roomBoard);
                }
                //180-degree Z rotation edge tiles, right-edge
                else if (i == columns * 3.5)
                {

                    List<GameObject> temp = new List<GameObject>();
                    temp.Add(edgeTiles[2]);
                    temp.Add(edgeTiles[6]);

                    List<GameObject> available = new List<GameObject>();

                    //Determine which tiles can be placed at current location
                    for (int k = 0; k < temp.Count; k++)
                    {
                        GameObject neighbor = temp[k];

                        if ((!grid.ContainsKey(new Vector3(i - 3.5f, j, 0f)) || (grid.ContainsKey(new Vector3(i - 3.5f, j, 0f)) && neighbor.GetComponent<Connector>().W.CompareTo(grid[new Vector3(i - 3.5f, j, 0f)].GetComponent<Connector>().E) == 0)) &&
                            (!grid.ContainsKey(new Vector3(i + 3.5f, j, 0f)) || (grid.ContainsKey(new Vector3(i + 3.5f, j, 0f)) && neighbor.GetComponent<Connector>().E.CompareTo(grid[new Vector3(i + 3.5f, j, 0f)].GetComponent<Connector>().W) == 0)) &&
                            (!grid.ContainsKey(new Vector3(i, j + 3.5f, 0f)) || (grid.ContainsKey(new Vector3(i, j + 3.5f, 0f)) && neighbor.GetComponent<Connector>().N.CompareTo(grid[new Vector3(i, j + 3.5f, 0f)].GetComponent<Connector>().S) == 0)) &&
                            (!grid.ContainsKey(new Vector3(i, j - 3.5f, 0f)) || (grid.ContainsKey(new Vector3(i, j - 3.5f, 0f)) && neighbor.GetComponent<Connector>().S.CompareTo(grid[new Vector3(i, j - 3.5f, 0f)].GetComponent<Connector>().N) == 0)))
                        {
                            available.Add(neighbor);
                        }
                    }

                    Debug.Log("<color=yellow>Tile Right Count" + available.Count + "</color>");

                    int selection = Random.Range(0, available.Count);
                    if (available.Count > 0)
                        selectedTile = available[selection];
                    available.Clear();

                    GameObject tileInstance = GameObject.Instantiate(selectedTile, new Vector3(i, j, 0f), Quaternion.identity) as GameObject;
                    grid.Add(new Vector3(i, j, 0f), tileInstance);

                    tileInstance.transform.SetParent(roomBoard);
                }
                //90-degree Z rotation edge tiles, bottom-edge
                else if (j == 0)
                {

                    List<GameObject> temp = new List<GameObject>();
                    temp.Add(edgeTiles[3]);
                    temp.Add(edgeTiles[7]);

                    List<GameObject> available = new List<GameObject>();

                    //Determine which tiles can be placed at current location
                    for (int k = 0; k < temp.Count; k++)
                    {
                        GameObject neighbor = temp[k];

                        if ((!grid.ContainsKey(new Vector3(i - 3.5f, j, 0f)) || (grid.ContainsKey(new Vector3(i - 3.5f, j, 0f)) && neighbor.GetComponent<Connector>().W.CompareTo(grid[new Vector3(i - 3.5f, j, 0f)].GetComponent<Connector>().E) == 0)) &&
                            (!grid.ContainsKey(new Vector3(i + 3.5f, j, 0f)) || (grid.ContainsKey(new Vector3(i + 3.5f, j, 0f)) && neighbor.GetComponent<Connector>().E.CompareTo(grid[new Vector3(i + 3.5f, j, 0f)].GetComponent<Connector>().W) == 0)) &&
                            (!grid.ContainsKey(new Vector3(i, j + 3.5f, 0f)) || (grid.ContainsKey(new Vector3(i, j + 3.5f, 0f)) && neighbor.GetComponent<Connector>().N.CompareTo(grid[new Vector3(i, j + 3.5f, 0f)].GetComponent<Connector>().S) == 0)) &&
                            (!grid.ContainsKey(new Vector3(i, j - 3.5f, 0f)) || (grid.ContainsKey(new Vector3(i, j - 3.5f, 0f)) && neighbor.GetComponent<Connector>().S.CompareTo(grid[new Vector3(i, j - 3.5f, 0f)].GetComponent<Connector>().N) == 0)))
                        {
                            available.Add(neighbor);
                        }
                    }

                    Debug.Log("<color=yellow>Tile Bottom Count" + available.Count + "</color>");

                    int selection = Random.Range(0, available.Count);
                    if (available.Count > 0)
                        selectedTile = available[selection];
                    available.Clear();

                    GameObject tileInstance = GameObject.Instantiate(selectedTile, new Vector3(i, j, 0f), Quaternion.identity) as GameObject;
                    grid.Add(new Vector3(i, j, 0f), tileInstance);

                    tileInstance.transform.SetParent(roomBoard);
                }
                //-90-degree Z rotation edge tiles, top-edge
                else if (j == rows * 3.5)
                {

                    List<GameObject> temp = new List<GameObject>();
                    temp.Add(edgeTiles[1]);
                    temp.Add(edgeTiles[5]);

                    List<GameObject> available = new List<GameObject>();

                    //Determine which tiles can be placed at current location
                    for (int k = 0; k < temp.Count; k++)
                    {
                        GameObject neighbor = temp[k];
                        Debug.Log("Validating Tile: " + neighbor.name);
                        if ((!grid.ContainsKey(new Vector3(i - 3.5f, j, 0f)) || (grid.ContainsKey(new Vector3(i - 3.5f, j, 0f)) && neighbor.GetComponent<Connector>().W.CompareTo(grid[new Vector3(i - 3.5f, j, 0f)].GetComponent<Connector>().E) == 0)) &&
                            (!grid.ContainsKey(new Vector3(i + 3.5f, j, 0f)) || (grid.ContainsKey(new Vector3(i + 3.5f, j, 0f)) && neighbor.GetComponent<Connector>().E.CompareTo(grid[new Vector3(i + 3.5f, j, 0f)].GetComponent<Connector>().W) == 0)) &&
                            (!grid.ContainsKey(new Vector3(i, j + 3.5f, 0f)) || (grid.ContainsKey(new Vector3(i, j + 3.5f, 0f)) && neighbor.GetComponent<Connector>().N.CompareTo(grid[new Vector3(i, j + 3.5f, 0f)].GetComponent<Connector>().S) == 0)) &&
                            (!grid.ContainsKey(new Vector3(i, j - 3.5f, 0f)) || (grid.ContainsKey(new Vector3(i, j - 3.5f, 0f)) && neighbor.GetComponent<Connector>().S.CompareTo(grid[new Vector3(i, j - 3.5f, 0f)].GetComponent<Connector>().N) == 0)))
                        {
                            available.Add(neighbor);
                        }
                    }

                    Debug.Log("<color=yellow>Tile Top Count" + available.Count + "</color>");
                    Debug.Log("<color=green>Location: " + new Vector3(i, j, 0f).ToString() + "</color>");

                    int selection = Random.Range(0, available.Count);
                    if (available.Count > 0)
                        selectedTile = available[selection];
                    available.Clear();

                    GameObject tileInstance = GameObject.Instantiate(selectedTile, new Vector3(i, j, 0f), Quaternion.identity) as GameObject;
                    grid.Add(new Vector3(i, j, 0f), tileInstance);

                    tileInstance.transform.SetParent(roomBoard);
                }
                //Add tiles randomly into scene
                else
                {
                    List<GameObject> temp = new List<GameObject>(groundTiles);
                    List<GameObject> available = new List<GameObject>();
                    
                    //Determine which tiles can be placed at current location
                    for (int k = 0; k < temp.Count; k++)
                    {
                        GameObject neighbor = temp[k];
                        
                        if ((!grid.ContainsKey(new Vector3(i - 3.5f, j, 0f)) || (grid.ContainsKey(new Vector3(i - 3.5f, j, 0f)) && neighbor.GetComponent<Connector>().W.CompareTo(grid[new Vector3(i - 3.5f, j, 0f)].GetComponent<Connector>().E) == 0)) &&
                            (!grid.ContainsKey(new Vector3(i + 3.5f, j, 0f)) || (grid.ContainsKey(new Vector3(i + 3.5f, j, 0f)) && neighbor.GetComponent<Connector>().E.CompareTo(grid[new Vector3(i + 3.5f, j, 0f)].GetComponent<Connector>().W) == 0)) &&
                            (!grid.ContainsKey(new Vector3(i, j + 3.5f, 0f)) || (grid.ContainsKey(new Vector3(i, j + 3.5f, 0f)) && neighbor.GetComponent<Connector>().N.CompareTo(grid[new Vector3(i, j + 3.5f, 0f)].GetComponent<Connector>().S) == 0)) &&
                            (!grid.ContainsKey(new Vector3(i, j - 3.5f, 0f)) || (grid.ContainsKey(new Vector3(i, j - 3.5f, 0f)) && neighbor.GetComponent<Connector>().S.CompareTo(grid[new Vector3(i, j - 3.5f, 0f)].GetComponent<Connector>().N) == 0)))
                        {
                            available.Add(neighbor);
                        }
                    }
                    
                    int selection = Random.Range(0, available.Count);
                    if(available.Count > 0)
                        selectedTile = available[selection];
                    available.Clear();
                    
                    GameObject tileInstance = GameObject.Instantiate(selectedTile, new Vector3(i, j, 0f), Quaternion.identity) as GameObject;
                    grid.Add(new Vector3(i, j, 0f), tileInstance);

                    if (Random.Range(0f, 1f) <= obstacleProbability)
                    {
                        GameObject obstacleTile;
                        if (Random.Range(0f, 1f) <= 0.55f)
                        {

                            obstacleTile = GameObject.Instantiate(enemies[Random.Range(0, enemies.Length)], new Vector3(i, j, 0f), Quaternion.identity);

                            obstacleTile.transform.SetParent(roomBoard);

                            numOfObstacles = numOfObstacles - 1;
                        }
                        else
                        {
                            obstacleTile = GameObject.Instantiate(obstacles[Random.Range(0, obstacles.Length)], new Vector3(i, j, 0f), Quaternion.identity);

                        }

                        obstacleTile.transform.SetParent(roomBoard);
                        obstacleCount = obstacleCount - 1;

                    }

                    tileInstance.transform.SetParent(roomBoard);
                    tileCount = tileCount - 1;
                }
                j += 3.5f;
            }
            i += 3.5f;
            if (tileCount != 0)
                obstacleProbability = ((float)obstacleCount) / tileCount;
        }
        
        //Set room to active
        isInstantiated = true;
    }

    public void Activate()
    {
        //Set room board to active state
        roomBoard.gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        //Set room board to inactive state
        roomBoard.gameObject.SetActive(false);
    }

    internal void DisplayNeighbors()
    {
        Debug.Log("Room " + point + " Neighbors\n");
        Debug.Log("N: " + N.ToString() + " S: " + S.ToString() + " W: " + W.ToString() + " E: " + E.ToString());
    }

    public bool isNeighbor(Vector2 position)
    {
        if (position == N || position == S ||
            position == E || position == W)
            return true;
        return false;
    }
}
