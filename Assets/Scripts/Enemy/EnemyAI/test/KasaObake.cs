using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class KasaObake : MonoBehaviour
{ 

    // sote the start position
    private Vector2 startingPosition;
    private Vector3 randomPosition;
    // Start is called before the first frame update
    void Start()
    {
        startingPosition = transform.position;
        randomPosition = GetRandomPosition();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private Vector2 GetRandomPosition()
    {
        Vector2 RandomPosition = new Vector2(Random.Range(10.0f, 10.0f), Random.Range(10.0f, 10.0f));
        return startingPosition + RandomPosition;
    }
}
