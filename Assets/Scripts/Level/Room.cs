using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room
{
    //Multiplier value to adjust for sprite size
    private float spriteMultiplier = 3.5f;

    //x, y position of the room
    private Vector2 point;

    //Number of rows of room
    private int rows;

    //Number of columns of room
    private int columns;

    //Parent asset of room for readability/debugging
    private Transform board;

    //Room grid to place tiles
    private Dictionary<Vector3, int> grid;

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
        for(int i = 0; i < rows; i++)
        {
            for(int j = 0; j < columns; j++)
            {
                grid.Add(new Vector3(i * spriteMultiplier, j * spriteMultiplier, 0f), 0);
            }
        }
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
