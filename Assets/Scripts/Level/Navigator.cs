using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Navigator : MonoBehaviour
{

    //Variables for room vectors
    public Vector2 currentRoom = Vector2.negativeInfinity;
    public Vector2 nextRoom = Vector2.negativeInfinity;

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
            List<Vector2> rooms = new List<Vector2>();
            rooms.Add(currentRoom);
            rooms.Add(nextRoom);

            SendMessageUpwards("TravelNextRoom", rooms);
        }
    }
}
