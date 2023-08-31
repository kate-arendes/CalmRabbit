using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeMap : MonoBehaviour
{
    
    public GameObject GroundCube, WaterCube, EmptyObject, Rabbit;
    public int width = 10, height = 10;
    public float waterChance = 0.5f;

    private float playerXCoord, playerZCoord, currentTopBound, currentBottomBound, currentLeftBound, currentRightBound;

    private GameObject[,] tileArray;
    private GameObject[] tilePoolHeight;
    private GameObject[] tilePoolWidth;
    private GameObject water;

    // Start is called before the first frame update
    void Start()
    {

        // Instantiate tileArray and pools

        tileArray = new GameObject[width, height];
        tilePoolHeight = new GameObject[height];
        tilePoolWidth = new GameObject[width];


        float currentX = 0.0f, currentZ = 0.0f;

        water = Instantiate<GameObject>(WaterCube);
        water.transform.position = new Vector3(0, 0, 0);

        for (int xIndex = 0; xIndex < width; xIndex++)
        {
            for(int zIndex = 0; zIndex < height; zIndex++)
            {
                float tileType = Random.Range(0.0f, 1.0f);
                float tileHeight = Random.Range(0.5f, 1.5f);
                Vector3 pos = new Vector3(currentX, tileHeight, currentZ);
                Vector3 rot = new Vector3(0, 0, 0);

                if (tileType > waterChance)
                {

                    tileArray[xIndex, zIndex] = Instantiate<GameObject>(GroundCube, pos, Quaternion.Euler(rot));

                }
                else
                {
                    tileArray[xIndex, zIndex] = Instantiate<GameObject>(EmptyObject, pos, Quaternion.Euler(rot));
               
                }

                

                currentZ += 10f;
            }

            currentX += 10f;
            currentZ = 0.0f;
        }

        // Set initial bounds

        currentRightBound = tileArray[width - 1, height - 1].transform.localPosition.x;
        currentLeftBound = tileArray[0, 0].transform.localPosition.x;

        currentTopBound = tileArray[0, 0].transform.localPosition.z;
        currentBottomBound = tileArray[width - 1, height - 1].transform.localPosition.z;
    }

    // Update is called once per frame
    void Update()
    {
        playerXCoord = Rabbit.transform.localPosition.x;
        playerZCoord = Rabbit.transform.localPosition.z;

        if(System.Math.Abs(playerXCoord - currentRightBound) > (0.65f * width * 10f))
        {
            ShiftMapLeft();
        }

        if(System.Math.Abs(playerXCoord - currentLeftBound) > (0.65f * width * 10f))
        {
            ShiftMapRight();
        }

        if(System.Math.Abs(playerZCoord - currentBottomBound) > (0.65f * height * 10f))
        {
            ShiftMapUp();

        }

        if (System.Math.Abs(playerZCoord - currentTopBound) > (0.65f * height * 10f))
        {
            
            ShiftMapDown();
        }


        water.transform.localPosition = new Vector3(playerXCoord, 0f, playerZCoord);
    }

    void ShiftMapLeft()
    {

        // Put right row in pool 

        for (int zIndex = 0; zIndex < height; zIndex++)
        {
            tilePoolHeight[zIndex] = tileArray[width - 1, zIndex];
        }

        // Shift tiles one row right in array

        for (int xIndex = width - 1; xIndex > 0; xIndex--)
        {
            for(int zIndex = 0; zIndex < height; zIndex++)
            {
                tileArray[xIndex, zIndex] = tileArray[xIndex - 1, zIndex];
            }
        }

        float currentX = tileArray[0, 0].transform.localPosition.x - 10f;

        // Add right row to array and transform

        for (int zIndex = 0; zIndex < height; zIndex++)
        {
            tileArray[0, zIndex] = tilePoolHeight[zIndex];
            float tileHeight = Random.Range(0.5f, 1.5f);
            float currentZ = tileArray[0, zIndex].transform.localPosition.z;

            tileArray[0, zIndex].transform.localPosition = new Vector3(currentX, tileHeight, currentZ);
            
        }

        // Update right bound

        if(tileArray[width - 1, height - 1] != null)
        {
            currentRightBound = tileArray[width - 1, height - 1].transform.localPosition.x;
        }

        
    }


    void ShiftMapRight()
    {
        // Add left tiles to pool

        for (int zIndex = 0; zIndex < height; zIndex++)
        {
            tilePoolHeight[zIndex] = tileArray[0, zIndex];
        }

        // Shift tiles left in array

        for (int xIndex = 0; xIndex < width - 1; xIndex++)
        {
            for (int zIndex = 0; zIndex < height; zIndex++)
            {
                tileArray[xIndex, zIndex] = tileArray[xIndex + 1, zIndex];
            }
        }

        float currentX = tileArray[width - 1, 0].transform.localPosition.x + 10f;

        // Add right row to array and transform

        for (int zIndex = 0; zIndex < height; zIndex++)
        {
            tileArray[width - 1, zIndex] = tilePoolHeight[zIndex];
            float tileHeight = Random.Range(0.5f, 1.5f);
            float currentZ = tileArray[width - 1, zIndex].transform.localPosition.z;

            tileArray[width - 1, zIndex].transform.localPosition = new Vector3(currentX, tileHeight, currentZ);

        }

        // Update left bound

        if (tileArray[0, 0])
        {
            currentLeftBound = tileArray[0, 0].transform.localPosition.x;
        }

    }

    void ShiftMapDown()
    {
        // Add top tiles to pool

        for (int xIndex = 0; xIndex < width; xIndex++)
        {
            tilePoolWidth[xIndex] = tileArray[xIndex, 0];
        }

        // Shift tiles up in array

        for (int zIndex = 0; zIndex < height - 1; zIndex++)
        {
            for (int xIndex = 0; xIndex < width; xIndex++)
            {
                tileArray[xIndex, zIndex] = tileArray[xIndex, zIndex + 1];
            }
        }

        // Add pooled tiles to bottom and transform

        float currentZ = tileArray[0, height - 1].transform.localPosition.z + 10f;

        for (int xIndex = 0; xIndex < width; xIndex++)
        {
            tileArray[xIndex, height - 1] = tilePoolWidth[xIndex];
            float tileHeight = Random.Range(0.5f, 1.5f);
            float currentX = tileArray[xIndex, height - 1].transform.localPosition.x;

            tileArray[xIndex, height - 1].transform.localPosition = new Vector3(currentX, tileHeight, currentZ);

        }

        // Update top bound

        currentTopBound = tileArray[0, 0].transform.localPosition.z;

    }

    void ShiftMapUp()
    {

        // Pool bottom column

        for (int xIndex = 0; xIndex < width; xIndex++)
        {
            tilePoolWidth[xIndex] = tileArray[xIndex, height - 1];
        }

        // Shift tiles down in array

        for (int zIndex = height - 1; zIndex > 0; zIndex--)
        {
            for (int xIndex = 0; xIndex < width; xIndex++)
            {
                tileArray[xIndex, zIndex] = tileArray[xIndex, zIndex - 1];
            }
        }

        // Add pooled tiles to top and transform

        float currentZ = tileArray[0, 0].transform.localPosition.z - 10f;

        for (int xIndex = 0; xIndex < width; xIndex++)
        {
            tileArray[xIndex, 0] = tilePoolWidth[xIndex];
            float tileHeight = Random.Range(0.5f, 1.5f);
            float currentX = tileArray[xIndex, 0].transform.localPosition.x;

            tileArray[xIndex, 0].transform.localPosition = new Vector3(currentX, tileHeight, currentZ);

        }

        // Update bottom bound

        currentBottomBound = tileArray[0, height - 1].transform.localPosition.z;

    }
}
