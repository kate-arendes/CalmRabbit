using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeMap : MonoBehaviour
{
    
    public GameObject GroundCube, WaterCube;
    public int width = 10, height = 10;
    public float waterChance = 0.5f;

    // Start is called before the first frame update
    void Start()
    {

        GameObject [,] tileArray = new GameObject[width, height];

        float currentX = 0.0f, currentZ = 0.0f;

        GameObject water = Instantiate<GameObject>(WaterCube);

        for(int xIndex = 0; xIndex < width; xIndex++)
        {
            for(int zIndex = 0; zIndex < height; zIndex++)
            {
                float tileType = Random.Range(0.0f, 1.0f);

                if(tileType > waterChance)
                {
                    tileArray[xIndex, zIndex] = Instantiate<GameObject>(GroundCube);
                    float height = Random.Range(0.5f, 1.5f);

                    tileArray[xIndex, zIndex].transform.position = new Vector3(currentX, height, currentZ);
                }

                

                currentZ += 10f;
            }

            currentX += 10f;
            currentZ = 0.0f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
