using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room
{
    //Multiplier value to adjust for sprite size
    private float spriteMultiplier = 3.5f;

    //x, y position of the room
    public Vector2 point;

    //Number of rows of room
    private int rows;

    //Number of columns of room
    private int columns;

    //Parent asset of room for readability/debugging
    private Transform board;

    //Room grid to place tiles
    private Dictionary<Vector3, int> grid;


    public Vector2 N = Vector2.negativeInfinity;
    public Vector2 S = Vector2.negativeInfinity;
    public Vector2 E = Vector2.negativeInfinity;
    public Vector2 W = Vector2.negativeInfinity;

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


    /*
    internal bool MarkN(Room position)
    {
        if (position.point.y - point.y > 0 &&
            position.point.y - point.y >= position.point.x - point.x &&
            Vector2.Distance(point, position.point) < Vector2.Distance(point, N.point) &&
            Vector2.Distance(point, position.point) < Vector2.Distance(position.point, position.S.point) && 
            !isNeighbor(position))
            return true;
        return false;
    }

    
    internal bool MarkS(Room position)
    {
        if (position.point.y - point.y < 0 &&
            position.point.y - point.y <= position.point.x - point.x &&
            Vector2.Distance(point, position.point) < Vector2.Distance(point, S.point) &&
            Vector2.Distance(point, position.point) < Vector2.Distance(position.point, position.N.point) && 
            !isNeighbor(position))
            return true;
        return false;
    }

    
    internal bool MarkE(Room position)
    {
        if (position.point.x - point.x > 0 &&
            position.point.x - point.x >= position.point.y - point.y &&
            Vector2.Distance(point, position.point) < Vector2.Distance(point, E.point) &&
            Vector2.Distance(point, position.point) < Vector2.Distance(position.point, position.W.point) && 
            !isNeighbor(position))
            return true;
        return false;
    }

    internal bool MarkW(Room position)
    {
        if (position.point.x - point.x < 0 &&
            position.point.x - point.x <= position.point.y - point.y &&
            Vector2.Distance(point, position.point) < Vector2.Distance(point, W.point) &&
            Vector2.Distance(point, position.point) < Vector2.Distance(position.point, position.E.point) && 
            !isNeighbor(position))
            return true;
        return false;
    }*/

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
