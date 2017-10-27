using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGeneration : MonoBehaviour {

    public int width = 256;
    public int height = 256;
    public int depth = 20;
    public float scale = 20;

    public float offsetX = 100f;
    public float offsetY = 100f;

    // Use this for initialization
    void Start () {
        offsetX = Random.Range(0, 999999f);
        offsetY = Random.Range(0, 999999f);
        Terrain terrain = GetComponent<Terrain>();
        terrain.terrainData = GenerateTerrain(terrain.terrainData);
	}
	

    TerrainData GenerateTerrain(TerrainData terrainData)
    {
        terrainData.heightmapResolution = width + 1;
        terrainData.size = new Vector3(width, depth, height);
        terrainData.SetHeights(0, 0, GenerateHeights());
        return terrainData;
    }

    float[,] GenerateHeights()
    {
        float[,] heights = new float[width, height];
        for(int x=0;x<width;x++)
        {
            for(int y=0;y<height;y++)
            {
                heights[x,y] =CalculateHeight(x, y);
            }
        }
        return heights;
    }

    float CalculateHeight(int x,int y)
    {
        float xCoord = (float)x / width * scale + offsetX;
        float yCoord = (float)y / height * scale + offsetY;
        float sample = Mathf.PerlinNoise(xCoord, yCoord); // need write my own perlinnoise
        return sample;
    }
}
