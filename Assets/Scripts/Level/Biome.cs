using Pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

/*
 * Class: Biome
 * 
 * Description: Holds the necessary tiles for a 
 * biome, the number of rooms in biome, and a list
 * of rooms
 * 
 * 
 */
public class Biome : MonoBehaviour
{

    #region Biome Objects
    //Arrays that hold the biomes prefab tiles
    public GameObject[] groundTiles;
    public GameObject[] cornerTiles;
    public GameObject[] edgeTiles;
    public GameObject[] items;
    public GameObject[] enemies;
    public GameObject[] obstacles;

    //Holds the portal to allow transitions between rooms
    public GameObject exit;

    //The premade challenge/boss arena
    public GameObject bossRoomArea;
    #endregion

    //Number of rooms this biome holds
    public int numOfRooms;

    //Room dictating where the player starts
    private Room startRoom;

    //Final room to exit the Biome
    private Room bossRoom;
    
    //Transform holding all relevant objects belonging to the biome
    private Transform biomeBoard;

    //Dictionary which holds all rooms and their associated locations within the biome
    private Dictionary<Vector2, Room> biomeRooms = new Dictionary<Vector2, Room>();

    #region Challenge Room
    public int challengeMin = 5; //Min enemies per round
    public int challengeMax = 8; //Max enemies per round
    public int challengeRounds = 3; //Number of rounds for the room
    int challengeCurRounds = 0; //Current room round
    public int challengeTimer = 5000; //Timer for the next round
    int challengeCurTimer = 0; //Counter for next round
    #endregion

    // Start is called before the first frame update
    void Start()
    {

    }

//Challenge room values

    // Update is called once per frame
    void Update()
    {
        if(challengeCurRounds > 0)
        {
            challengeCurTimer++;
            if(challengeCurTimer > challengeTimer)
            {
                ChallengeLevelSpawnEnemies();
                challengeCurTimer = 0;
                challengeCurRounds--;
            }
        }
    }

    internal void StartFirstLevel()
    {
        biomeBoard = new GameObject("BiomeBoard").transform;
        biomeBoard.SetParent(gameObject.transform);
        GameObject.FindWithTag("Player").transform.position = new Vector3(0f, 0f, 0f);
        ActivateRoom(startRoom);
    }

    public void TravelNextRoom(List<Vector2> rooms)
    {
        DeactivateRoom(GetRoom(rooms[0]));
        GameObject.FindWithTag("Player").transform.position = new Vector3(0f, 0f, 0f);
        ActivateRoom(GetRoom(rooms[1]));
    }

    /*
     * Function: GenerateRooms()
     * 
     * Description: Generates a new room of semi-random size and 
     * adds it to the list of rooms within this biome
     * 
     * @maxRows Maximum number of rows allowed per room
     * @maxColumns Maximum number of columns allowed per room
     */
    public void GenerateRooms(int maxRows, int maxColumns)
    {
        for (int i = 0; i < numOfRooms; i++)
        {
            Vector2 randomPosition = new Vector2(Random.Range(0, maxRows * numOfRooms), Random.Range(0, maxColumns * numOfRooms));

            while (biomeRooms.ContainsKey(randomPosition))
            {
                randomPosition = new Vector2(Random.Range(0, maxRows * numOfRooms), Random.Range(0, maxColumns * numOfRooms));
            }

            biomeRooms.Add(randomPosition, new Room(Random.Range(maxRows / 2, maxRows), Random.Range(maxColumns / 2, maxColumns), randomPosition));

        }
        SetBeginEnd();
    }

    /*
     * Function: LinkRooms()
     * 
     * Description: Links all rooms within the biome through validating
     * the cardinal directions of each room 
     * 
     */
    public void LinkRooms()
    {
        foreach (var room in biomeRooms)
        {
            foreach (var tempRoom in biomeRooms)
            {
                if (tempRoom.Key == room.Key)
                    break;
                if (MarkN(room.Value, tempRoom.Value))
                {
                    if (!GetRoom(room.Value.N).point.Equals(Vector2.negativeInfinity))
                        Reestablish(GetRoom(room.Value.N), "S");
                    room.Value.N = tempRoom.Value.point;
                    if (!GetRoom(tempRoom.Value.S).point.Equals(Vector2.negativeInfinity))
                        Reestablish(GetRoom(tempRoom.Value.S), "N");
                    tempRoom.Value.S = room.Value.point;

                }
                if (MarkS(room.Value, tempRoom.Value))
                {
                    if (!GetRoom(room.Value.S).point.Equals(Vector2.negativeInfinity))
                        Reestablish(GetRoom(room.Value.S), "N");
                    room.Value.S = tempRoom.Value.point;
                    if (!GetRoom(tempRoom.Value.N).point.Equals(Vector2.negativeInfinity))
                        Reestablish(GetRoom(tempRoom.Value.N), "S");
                    tempRoom.Value.N = room.Value.point;
                }
                if (MarkE(room.Value, tempRoom.Value))
                {
                    if (!GetRoom(room.Value.E).point.Equals(Vector2.negativeInfinity))
                        Reestablish(GetRoom(room.Value.E), "W");
                    room.Value.E = tempRoom.Value.point;
                    if (!GetRoom(tempRoom.Value.W).point.Equals(Vector2.negativeInfinity))
                        Reestablish(GetRoom(tempRoom.Value.W), "E");
                    tempRoom.Value.W = room.Value.point;
                }
                if (MarkW(room.Value, tempRoom.Value))
                {
                    if (!GetRoom(room.Value.W).point.Equals(Vector2.negativeInfinity))
                        Reestablish(GetRoom(room.Value.W), "E");
                    room.Value.W = tempRoom.Value.point;
                    if (!GetRoom(tempRoom.Value.E).point.Equals(Vector2.negativeInfinity))
                        Reestablish(GetRoom(tempRoom.Value.E), "W");
                    tempRoom.Value.E = room.Value.point;
                }
            }
        }
    }

    //Reestablish a connection to room with broken link
    public void Reestablish(Room broken, string neighbor)
    {
        foreach (var room in biomeRooms)
        {
            if (room.Key != broken.point)
            {
                if (neighbor.Equals("N"))
                {
                    broken.N = Vector2.negativeInfinity;
                    MarkN(broken, room.Value);
                }
                else if (neighbor.Equals("S"))
                {
                    broken.S = Vector2.negativeInfinity;
                    MarkS(broken, room.Value);
                }
                else if (neighbor.Equals("E"))
                {
                    broken.E = Vector2.negativeInfinity;
                    MarkE(broken, room.Value);
                }
                else if (neighbor.Equals("W"))
                {
                    broken.W = Vector2.negativeInfinity;
                    MarkW(broken, room.Value);
                }
            }
        }
    }

    public void DisplayRooms()
    {
        foreach (var room in biomeRooms)
        {
            room.Value.DisplayNeighbors();
        }
    }

    /*
     * Function: MarkN()
     * 
     * Description: Validates a given point if it's in the North
     * direction and if it is the current closest point is set as
     * the new N
     * 
     * @position Vector2 position of a given room (in our case, bottom-left 
     * corner of room)
     * 
     */
    public bool MarkN(Room main, Room neighbor)
    {
        if (neighbor.point.y - main.point.y > 0 &&
            neighbor.point.y - main.point.y >= neighbor.point.x - main.point.x &&
            Vector2.Distance(main.point, neighbor.point) < Vector2.Distance(main.point, main.N) &&
            Vector2.Distance(main.point, neighbor.point) < Vector2.Distance(neighbor.point, neighbor.S) &&
            !main.isNeighbor(neighbor.point))
            return true;
        return false;
    }

    /*
     * Function: MarkS()
     * 
     * Description: Validates a given point if it's in the South
     * direction and if it is the current closest point is set as
     * the new S
     * 
     * @position Vector2 position of a given room (in our case, bottom-left 
     * corner of room)
     */
    public bool MarkS(Room main, Room neighbor)
    {
        if (neighbor.point.y - main.point.y < 0 &&
            neighbor.point.y - main.point.y <= neighbor.point.x - main.point.x &&
            Vector2.Distance(main.point, neighbor.point) < Vector2.Distance(main.point, main.S) &&
            Vector2.Distance(main.point, neighbor.point) < Vector2.Distance(neighbor.point, neighbor.N) &&
            !main.isNeighbor(neighbor.point))
            return true;
        return false;
    }

    /*
     * Function: MarkE()
     * 
     * Description: Validates a given point if it's in the East
     * direction and if it is the current closest point is set as
     * the new E
     * 
     * @position Vector2 position of a given room (in our case, bottom-left 
     * corner of room)
     */
    public bool MarkE(Room main, Room neighbor)
    {
        if (neighbor.point.x - main.point.x > 0 &&
            neighbor.point.x - main.point.x >= neighbor.point.y - main.point.y &&
            Vector2.Distance(main.point, neighbor.point) < Vector2.Distance(main.point, main.E) &&
            Vector2.Distance(main.point, neighbor.point) < Vector2.Distance(neighbor.point, neighbor.W) &&
            !main.isNeighbor(neighbor.point))
            return true;
        return false;
    }

    /*
     * Function: MarkW()
     * 
     * Description: Validates a given point if it's in the West
     * direction and if it is the current closest point is set as
     * the new W
     * 
     * @position Vector2 position of a given room (in our case, bottom-left 
     * corner of room)
     */
    public bool MarkW(Room main, Room neighbor)
    {
        if (neighbor.point.x - main.point.x < 0 &&
            neighbor.point.x - main.point.x <= neighbor.point.y - main.point.y &&
            Vector2.Distance(main.point, neighbor.point) < Vector2.Distance(main.point, main.W) &&
            Vector2.Distance(main.point, neighbor.point) < Vector2.Distance(neighbor.point, neighbor.E) &&
            !main.isNeighbor(neighbor.point))
            return true;
        return false;
    }

    /*
     * Function: GetRoom()
     * 
     * Description: Returns a room based on specified
     * room location
     * 
     * @position Location of room
     */
    private Room GetRoom(Vector2 position)
    {
        if (position.Equals(Vector2.negativeInfinity) || !biomeRooms.ContainsKey(position))
        {
            return new Room(0, 0, Vector2.negativeInfinity);
        }
        return biomeRooms[position];
    }

    public void SetBeginEnd()
    {
        float farthest = 0f;

        foreach (var room1 in biomeRooms)
        {
            foreach (var room2 in biomeRooms)
            {
                if (farthest < Vector2.Distance(room1.Value.point, room2.Value.point))
                {
                    farthest = Vector2.Distance(room1.Value.point, room2.Value.point);
                    startRoom = room1.Value;
                    bossRoom = room2.Value;
                }
            }
        }
    }

    public void ActivateRoom(Room room)
    {
        if(room.point == bossRoom.point)
        {
            ProduceChallengeLevel();
        }   
        else if (room.isInstantiated)
        {
            room.Activate();
        }
        else
        {
            room.InstantiateRoom(groundTiles, cornerTiles, edgeTiles, enemies, obstacles, exit);
        }

        AstarData data = AstarPath.active.data;
        GridGraph gg = data.gridGraph;
        gg.center = new Vector3((float)room.rows * 3.5f / 2, (float)room.columns * 3.5f / 2, 0f);
        AstarPath.active.Scan();
    }

    public void DeactivateRoom(Room room)
    {
        room.Deactivate();
    }


    //Loads the premade boss level
    public void ProduceChallengeLevel()
    {
        GameObject roomInstance = Instantiate(bossRoomArea, new Vector3(0f, 0f, 0f), Quaternion.identity) as GameObject;
        roomInstance.transform.SetParent(GameObject.Find("BiomeBoard").transform);
        GameObject.FindWithTag("Player").transform.position = new Vector3(0f, 0f, 0f);
        ChallengeLevelSpawnEnemies();
        challengeCurRounds = challengeRounds;
    }
    public void ChallengeLevelSpawnEnemies()
    {
        int enemyAmount = Random.Range(challengeMin, challengeMax);
        Debug.Log("Spawning");

        for(int i = 0; i < enemyAmount; i++)
        {
            Vector3 newEnemy = new Vector3(Random.Range(-9.2f, 9.2f), Random.Range(-5f, 5f), 0f);
            Instantiate(enemies[Random.Range(0,enemies.Length)], newEnemy, Quaternion.identity);    
        }
    }
}