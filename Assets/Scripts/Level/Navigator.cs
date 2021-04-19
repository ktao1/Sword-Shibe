using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Navigator : MonoBehaviour
{

    //Variables for room vectors
    public Vector2 currentRoom = Vector2.negativeInfinity;
    public Vector2 nextRoom = Vector2.negativeInfinity;

    //0 for W, 1 for N, 2 for E, 3 for S
    public int portalLocation = 0;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    //On Collision send message for room transition
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            object[] args = new object[4];
            args[0] = currentRoom;
            args[1] = nextRoom;
            args[2] = portalLocation;

            /*
            List<Vector2> rooms = new List<Vector2>();

            rooms.Add(currentRoom);
            rooms.Add(nextRoom);
            */

            SendMessageUpwards("TravelNextRoom", args);
        }
    }
}
