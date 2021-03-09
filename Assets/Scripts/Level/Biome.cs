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

    //Arrays that hold the biomes prefab tiles
    public GameObject[] groundTiles;
    public GameObject[] cornerTiles;
    public GameObject[] edgeTiles;
    public GameObject[] items;
    public GameObject[] enemies;

    //Number of rooms this biome holds
    public int numOfRooms;

    //List of generated rooms
    //private List<Room> biomeRooms;

    private Dictionary<Vector2, Room> biomeRooms;

    public int id;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

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
    public void GenerateRooms(int maxRows, int maxColumns, int id)
    {
        for(int i = 0; i < numOfRooms; i++)
        {
            Vector2 randomPosition = new Vector2(Random.Range(0, maxRows * numOfRooms), Random.Range(0, maxColumns * numOfRooms));

            while(biomeRooms.ContainsKey(randomPosition))
            {
                randomPosition = new Vector2(Random.Range(0, maxRows * numOfRooms), Random.Range(0, maxColumns * numOfRooms));
            }

            biomeRooms.Add(randomPosition, new Room(Random.Range(maxRows / 2, maxRows), Random.Range(maxColumns / 2, maxColumns), randomPosition));
            
        }
        
    }

    /*
     * Function: LinkRooms()
     * 
     * Description: Links all rooms within the biome
     * 
     * NOTE: Use Delaunay Triangulation with minimum spanning tree
     * 
     */
    public void LinkRooms()
    {
        foreach(var room in biomeRooms)
        {
            foreach(var tempRoom in biomeRooms)
            {
                room.Value.MarkN(tempRoom.Value.point);
                room.Value.MarkS(tempRoom.Value.point);
                room.Value.MarkE(tempRoom.Value.point);
                room.Value.MarkW(tempRoom.Value.point);
            }
        }
    }

    
}
