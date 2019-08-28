﻿using UnityEngine;
using Cinemachine;

public class MapHandlerExp : MonoBehaviour
{
    bool mapActive = false;

    [Range(0.1f, 1f)]
    public float actionTimerLength;
    float actionTimer = 0;

    [HideInInspector]
    public Tile[,] mapGrid;
    public GameObject levelTilemap;
    public GameObject floorTilemap;
    public CinemachineVirtualCamera vcam;

    public HeroHandler heroHandler;



    void Start()
    {
        float topBound = levelTilemap.transform.GetChild(0).transform.position.y;
        float bottomBound = levelTilemap.transform.GetChild(0).transform.position.y;
        float leftBound = levelTilemap.transform.GetChild(0).transform.position.x;
        float rightBound = levelTilemap.transform.GetChild(0).transform.position.x;

        //First, scrub through the entire tilemap to determine which tile is at the bottom left corner
        foreach (Transform child in levelTilemap.transform)
        {
            if (child.position.x < leftBound)
                leftBound = child.position.x;
            else if (child.position.x > rightBound)
                rightBound = child.position.x;

            if (child.position.y < bottomBound)
                bottomBound = child.position.y;
            else if (child.position.y > topBound)
                topBound = child.position.y;
        }

        int mapWidth = (int)(rightBound - leftBound) + 1;
        int mapHeight = (int)(topBound - bottomBound) + 1;

        float xOffset = Mathf.Abs(leftBound);
        float yOffset = Mathf.Abs(bottomBound);

        mapGrid = new Tile[mapWidth, mapHeight];

        //Now, populate the mapGrid, keeping what the offset of each tile should be in mind
        foreach (Transform child in levelTilemap.transform)
        {
            child.Translate(new Vector2(xOffset, yOffset));

            int x = (int)child.position.x;
            int y = (int)child.position.y;

            //If we're looking at the hero, do all the necessary hero setup
            if (child.name == "Hero")
            {
                HeroHandler.HeroDirections initDir = HeroHandler.HeroDirections.Up;

                //Top player spawn
                if (y == (mapGrid.GetLength(1) - 1))
                    initDir = HeroHandler.HeroDirections.Down;

                //Bottom player spawn
                else if (y == 0)
                    initDir = HeroHandler.HeroDirections.Up;

                //Left player spawn
                else if (x == 0)
                    initDir = HeroHandler.HeroDirections.Right;

                //Right player spawn
                else if (x == (mapGrid.GetLength(0) - 1))
                    initDir = HeroHandler.HeroDirections.Left;

                //Else illegal spawn
                else
                    print("Tried to spawn player at x=" + x + ", y=" + y + " which is ILLEGAL");

                heroHandler.Init(mapGrid, new Vector2(x, y), initDir);
            }

            //Otherwise, just slap it in the mapGrid
            else
            {
                mapGrid[x, y] = child.GetComponent<Tile>();
            }
        }

        //Lastly, offset the floor tilemap so it aligns with everything else
        floorTilemap.transform.Translate(new Vector2(xOffset, yOffset));

        //Lastly lastly, loop through all pit tiles and set their graphics depending on if there are adjacent pits
        GetComponent<PitConnector>().ConnectAllPits(mapGrid);

        //Set the map to active (only for testing, remove this later)
        ActivateMap();
    }



    void Update()
    {
        if (mapActive)
        {
            if (heroHandler) heroHandler.MapUpdate(actionTimer, actionTimerLength);

            actionTimer += Time.deltaTime;
            if (actionTimer >= actionTimerLength)
            {
                actionTimer = 0;    //This could probably be tweaked to subtract from timer, rather than setting it to zero, allowing multiple actions per frame if the timer is short enough
                if (heroHandler) heroHandler.MapAction();
            }
        }
    }



    void ActivateMap()
    {
        mapActive = true;
        heroHandler.enabled = true;
    }
}