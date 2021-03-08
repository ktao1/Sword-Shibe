using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Class: Biome
 * 
 * Description: Holds the necessary tiles for a 
 * biome, the number of rooms in biome, and a list
 * of rooms
 * 
 * 
 */
public class Biome
{

    //Arrays that hold the biomes prefab tiles
    private GameObject[] groundTiles;
    private GameObject[] cornerTiles;
    private GameObject[] edgeTiles;
    private GameObject[] items;
    private GameObject[] enemies;

    //Number of rooms this biome holds
    private int numOfRooms;

    //List of generated rooms
    private List<Room> biomeRooms;

    //Constructor for biome
    public Biome(GameObject[] _groundTiles, GameObject[] _cornerTiles, GameObject[] _edgeTiles, GameObject[] _items, GameObject[] _enemies, int _rooms)
    {
        groundTiles = _groundTiles;
        cornerTiles = _cornerTiles;
        edgeTiles = _edgeTiles;
        items = _items;
        enemies = _enemies;
        numOfRooms = Random.Range(_rooms / 2, _rooms);
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

    }

    
}
