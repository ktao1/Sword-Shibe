using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class KasaObakaGFX : MonoBehaviour
{

    public AIPath aiPath;


    // Update is called once per frame
    void Update()
    {
        // if monster move to the right
        if(aiPath.desiredVelocity.x >= 1f)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
        // if monster move to the left 
        else if(aiPath.desiredVelocity.x <= 1f)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
    }
}
