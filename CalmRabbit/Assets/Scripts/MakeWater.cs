using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeWater : MonoBehaviour
{
    public GameObject WaterCube, Rabbit;

    public int width, height;
    private float playerXCoord, playerZCoord, currentTopBound, currentBottomBound, currentLeftBound, currentRightBound;

    private GameObject[,] waterArray;
    private GameObject[] waterHeightPool;
    private GameObject[] waterWidthPool;

    // Start is called before the first frame update
    void Start()
    {

        waterArray = new GameObject[width, height];
        waterHeightPool = new GameObject[height];
        waterWidthPool = new GameObject[width];

        float currentX = 0f, currentZ = 0f;

        for(int xIndex = 0; xIndex < width; xIndex++)
        {
            for(int zIndex = 0; zIndex < height; zIndex++)
            {
                Vector3 pos = new Vector3(currentX, 0f, currentZ);
                Vector3 rot = new Vector3(0f, 0f, 0f);

                waterArray[xIndex, zIndex] = Instantiate<GameObject>(WaterCube, pos, Quaternion.Euler(rot));

                currentZ += 100f;
            }

            currentX += 100f;
            currentZ = 0f;
        }

        currentRightBound = waterArray[width - 1, height - 1].transform.localPosition.x;
        currentLeftBound = waterArray[0, 0].transform.localPosition.x;

        currentTopBound = waterArray[0, 0].transform.localPosition.z;
        currentBottomBound = waterArray[width - 1, height - 1].transform.localPosition.z;
    }

    // Update is called once per frame
    void Update()
    {
        playerXCoord = Rabbit.transform.localPosition.x;
        playerZCoord = Rabbit.transform.localPosition.z;

        if (System.Math.Abs(playerXCoord - currentRightBound) > (0.75f * width * 100f))
        {
            ShiftWaterLeft();
        }

        if (System.Math.Abs(playerXCoord - currentLeftBound) > (0.75f * width * 100f))
        {
            ShiftWaterRight();
        }

        if (System.Math.Abs(playerZCoord - currentBottomBound) > (0.75f * height * 100f))
        {
            ShiftWaterUp();

        }

        if (System.Math.Abs(playerZCoord - currentTopBound) > (0.75f * height * 100f))
        {

            ShiftWaterDown();
        }
    }

    void ShiftWaterLeft()
    {
        print("Water shifted left");

        // Put right row in pool 

        for (int zIndex = 0; zIndex < height; zIndex++)
        {
            waterHeightPool[zIndex] = waterArray[width - 1, zIndex];
        }

        // Shift tiles one row right in array

        for (int xIndex = width - 1; xIndex > 0; xIndex--)
        {
            for (int zIndex = 0; zIndex < height; zIndex++)
            {
                waterArray[xIndex, zIndex] = waterArray[xIndex - 1, zIndex];
            }
        }

        float currentX = waterArray[0, 0].transform.localPosition.x - 100f;

        // Add right row to array and transform

        for (int zIndex = 0; zIndex < height; zIndex++)
        {
            waterArray[0, zIndex] = waterHeightPool[zIndex];
            float currentZ = waterArray[0, zIndex].transform.localPosition.z;

            waterArray[0, zIndex].transform.localPosition = new Vector3(currentX, 0f, currentZ);

        }

        // Update horizontal bounds

        currentRightBound = waterArray[width - 1, height - 1].transform.localPosition.x;
        currentLeftBound = waterArray[0, 0].transform.localPosition.x;
    }

    void ShiftWaterRight()
    {
        print("Water shifted right");

        // Add left tiles to pool

        for (int zIndex = 0; zIndex < height; zIndex++)
        {
            waterHeightPool[zIndex] = waterArray[0, zIndex];
        }

        // Shift tiles left in array

        for (int xIndex = 0; xIndex < width - 1; xIndex++)
        {
            for (int zIndex = 0; zIndex < height; zIndex++)
            {
                waterArray[xIndex, zIndex] = waterArray[xIndex + 1, zIndex];
            }
        }

        float currentX = waterArray[width - 1, 0].transform.localPosition.x + 100f;

        // Add right row to array and transform

        for (int zIndex = 0; zIndex < height; zIndex++)
        {
            waterArray[width - 1, zIndex] = waterHeightPool[zIndex];
            float currentZ = waterArray[width - 1, zIndex].transform.localPosition.z;

            waterArray[width - 1, zIndex].transform.localPosition = new Vector3(currentX, 0f, currentZ);

        }

        // Update horizontal bounds

        currentLeftBound = waterArray[0, 0].transform.localPosition.x;
        currentRightBound = waterArray[width - 1, height - 1].transform.localPosition.x;
    }

    void ShiftWaterUp()
    {
        print("Water shifted up");

        // Pool bottom column

        for (int xIndex = 0; xIndex < width; xIndex++)
        {
            waterWidthPool[xIndex] = waterArray[xIndex, height - 1];
        }

        // Shift tiles down in array

        for (int zIndex = height - 1; zIndex > 0; zIndex--)
        {
            for (int xIndex = 0; xIndex < width; xIndex++)
            {
                waterArray[xIndex, zIndex] = waterArray[xIndex, zIndex - 1];
            }
        }

        // Add pooled tiles to top and transform

        float currentZ = waterArray[0, 0].transform.localPosition.z - 100f;

        for (int xIndex = 0; xIndex < width; xIndex++)
        {
            waterArray[xIndex, 0] = waterWidthPool[xIndex];
            float currentX = waterArray[xIndex, 0].transform.localPosition.x;

            waterArray[xIndex, 0].transform.localPosition = new Vector3(currentX, 0f, currentZ);

        }

        // Update vertical bounds

        currentBottomBound = waterArray[0, height - 1].transform.localPosition.z;
        currentTopBound = waterArray[0, 0].transform.localPosition.z;
    }

    void ShiftWaterDown()
    {
        print("Water shifted down");

        // Add top tiles to pool

        for (int xIndex = 0; xIndex < width; xIndex++)
        {
            waterWidthPool[xIndex] = waterArray[xIndex, 0];
        }

        // Shift tiles up in array

        for (int zIndex = 0; zIndex < height - 1; zIndex++)
        {
            for (int xIndex = 0; xIndex < width; xIndex++)
            {
                waterArray[xIndex, zIndex] = waterArray[xIndex, zIndex + 1];
            }
        }

        // Add pooled tiles to bottom and transform

        float currentZ = waterArray[0, height - 1].transform.localPosition.z + 100f;

        for (int xIndex = 0; xIndex < width; xIndex++)
        {
            waterArray[xIndex, height - 1] = waterWidthPool[xIndex];
            float currentX = waterArray[xIndex, height - 1].transform.localPosition.x;

            waterArray[xIndex, height - 1].transform.localPosition = new Vector3(currentX, 0f, currentZ);

        }

        // Update vertical bounds

        currentTopBound = waterArray[0, 0].transform.localPosition.z;
        currentBottomBound = waterArray[width - 1, height - 1].transform.localPosition.z;

    }
}
