using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room
{
    //Multiplier value to adjust for sprite size
    private float spriteMultiplier = 3.5f;

    //Number of rows of room
    private int rows;

    //Number of columns of room
    private int columns;

    //Parent asset of room for readability/debugging
    private Transform board;

    //Room grid to place tiles
    private List<Vector3> grid;

    public Room(int _rows, int _columns)
    {
        rows = _rows;
        columns = _columns;
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
     * Function: GenerateRoom()
     * 
     * Description: Use variation of Perlin Noise algorithm
     * to mark the rest of the room grid with floor, wall,
     * enemy and item tiles
     * 
     * 
     */
     public void GenerateRoom()
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
     public void InstantiateRoom()
    {

    }

}
