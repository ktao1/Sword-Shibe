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
    private List<Room> biomeRooms;

    private Room[] frontier;
    private List<Room> inRooms;

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
            biomeRooms.Add(new Room(Random.Range(maxRows / 2, maxRows), Random.Range(maxColumns / 2, maxColumns)));
        }
    }

    /*
     * Function: LinkRooms()
     * 
     * Description: Links all rooms within the biome
     * 
     * NOTE: Use Prim's or possibly Delaunay Triangulation
     * 
     */
    public void LinkRooms()
    {
        Room start = biomeRooms[Random.Range(0, biomeRooms.Count)];

        inRooms.Add(start);
        float distance = Vector3.Distance(new Vector3(0,1,2), new Vector3(0,40,3));
        


    }

    
}
