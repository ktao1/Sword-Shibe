using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class testing : MonoBehaviour
{ 
    void Start()
    {
        AstarData data = AstarPath.active.data;

        GridGraph gg = data.gridGraph;

        int width = 50;
        int depth = 50;
        float nodeSize = 1;

        gg.center = new Vector3(100, 10, 10);

        gg.SetDimensions(width, depth, nodeSize);

        AstarPath.active.Scan();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
