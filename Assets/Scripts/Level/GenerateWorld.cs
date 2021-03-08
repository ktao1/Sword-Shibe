using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GenerateWorld : MonoBehaviour
{

    //Seed value to reproduce worlds and vary randomization
    public int seed = 0;

    //Max rows and columns to dictate the maximum size of a room
    public int maxRows = 10;
    public int maxColumns = 10;

    //Maximum number of rooms per biome
    public int roomsPerBiome = 2;

    //List of biomes on map
    private List<Biome> biomes;
    

    // Start is called before the first frame update
    void Start()
    {
        LoadBiomes();
        ConnectBiomes();

        for(int i = 0; i < biomes.Count; i++)
        {
            if (i < biomes.Count - 1)
                biomes[i].GenerateRooms(maxRows, maxColumns, biomes[i + 1].id);
            else
                biomes[i].GenerateRooms(maxRows, maxColumns, 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*
     *  Function: ConnectBiomes()
     * 
     *  Description: Uses a specified list of Biomes and
     *  connects them together as a path
     *  
     * 
     */
    private void ConnectBiomes()
    {
        List<Biome> temp = new List<Biome>(); 

        while(biomes.Count > 0)
        {
            int index = Random.Range(0, biomes.Count);
            temp.Add(biomes[index]);
            biomes.RemoveAt(index);
        }

        biomes = temp;

    }

    private void LoadBiomes()
    {
        Biome[] biomeList = GetComponents<Biome>();

        for(int i = 0; i < biomeList.Length; i++)
        {
            biomes.Add(biomeList[i]);
        }

    }

}
