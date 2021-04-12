using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Connector : MonoBehaviour
{

    //Define type of edge
    public enum Edge
    {
        Type0,
        Type1,
        Type2,
        Type3

    }

    public Edge N = Edge.Type0;
    public Edge S = Edge.Type1;
    public Edge E = Edge.Type1;
    public Edge W = Edge.Type2;

    public int NN;
    public int SN;
    public int EN;
    public int WN;

    void Start()
    {


    }

    void Update()
    {



    }

}