using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerlinNoise : MonoBehaviour {

    public int width = 256;
    public int height = 256;
    public int scale = 20;

    public float offsetX = 100f;
    public float offsetY = 100f;

    private void Start()
    {
        offsetX = Random.Range(0, 999999f);
        offsetY = Random.Range(0, 999999f);
    }

    private void Update()
    {
        Renderer renderer = GetComponent<Renderer>();
        renderer.material.mainTexture = GenerateTexture();

    }

    Texture2D GenerateTexture()
    {
        Texture2D texture = new Texture2D(width, height);

        // genreate perlin noise map fot the texture

        for(int x=0;x<width;x++)
        {
            for(int y=0;y<height;y++)
            {
                Color color = CalculateColor(x, y);
                texture.SetPixel(x, y, color);
            }
        }

        texture.Apply();
        return texture;
    }

    Color CalculateColor(int x, int y)
    {
        float xCoord = (float) x / width * scale+ offsetX;
        float yCoord = (float) y / height * scale+ offsetY;
        float sample= Mathf.PerlinNoise(xCoord, yCoord); // need write my own perlinnoise
        return new Color (sample,sample,sample);
    }
}
