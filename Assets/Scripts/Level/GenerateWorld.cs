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

    //Value that holds the current biome index
    private int currentBiome = 0;

    private Transform AllBullets;

    // Start is called before the first frame update
    void Start()
    {
        //Load all biome data
        LoadBiomes();

        //Generate rooms for each biome
        for (int i = 0; i < biomes.Count; i++)
        {
            if (i < biomes.Count - 1)
                biomes[i].GenerateRooms(maxRows, maxColumns);
            else
                biomes[i].GenerateRooms(maxRows, maxColumns);
        }

        //Link each biomes rooms together
        biomes[currentBiome].LinkRooms();

        //For debug purposes only
        //biomes[currentBiome].DisplayRooms();

        //Instantiate first biomes starting room
        biomes[currentBiome].StartFirstLevel();
        AllBullets = new GameObject("AllBullets").transform;
        AllBullets.SetParent(gameObject.transform);
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

        for (int i = 0; i < biomeList.Length; i++)
        {
            biomes.Add(biomeList[i]);
        }

        Debug.Log(biomes.Count);

    }

    //Loads the next biome once a boss is defeated, if no more biomes, game is over
    public void NextLevel()
    {
        foreach(Transform child in gameObject.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        Debug.Log("Moving to next biome");

        currentBiome += 1;
        if (currentBiome == biomes.Count)
            EndGame();

        //Link each biomes rooms together
        biomes[currentBiome].LinkRooms();
        biomes[currentBiome].DisplayRooms();

        Debug.Log("Starting first level");

        //Instantiate first biomes starting room
        biomes[currentBiome].StartFirstLevel();
    }

    //Quit the game
    private void EndGame()
    {
        /*
         * TODO:
         * - Setup ending scenes
         * - Add option to replay
         * - Or quit out
         */

        //Exit the game
        Application.Quit();
    }
}
