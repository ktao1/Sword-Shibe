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
    private List<Biome> biomes = new List<Biome>();
    

    // Start is called before the first frame update
    void Start()
    {
        LoadBiomes();

        for(int i = 0; i < biomes.Count; i++)
        {
            if (i < biomes.Count - 1)
                biomes[i].GenerateRooms(maxRows, maxColumns, biomes[i + 1].id);
            else
                biomes[i].GenerateRooms(maxRows, maxColumns, 0);
        }

        biomes[0].LinkRooms();
        biomes[0].DisplayRooms();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    

    /*
     * Function: LoadBiomes()
     * 
     * Description: Loads biomes stored as components into a usable list
     * 
     */
    private void LoadBiomes()
    {
        Biome[] biomeList = GetComponents<Biome>();

        for(int i = 0; i < biomeList.Length; i++)
        {
            biomes.Add(biomeList[i]);
        }

        Debug.Log(biomes.Count);

    }

}
