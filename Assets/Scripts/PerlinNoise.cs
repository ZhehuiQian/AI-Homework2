﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerlinNoise : MonoBehaviour {

    public int width = 256;
    public int height = 256;
    public int scale = 20;

    public float offsetX = 100f;
    public float offsetY = 100f;

    private float[,] perlinNoise;

   void Start()
    {
        offsetX = Random.Range(0, 999999f);
        offsetY = Random.Range(0, 999999f);
        perlinNoise = PerlinNoiseGenerator();
        //print("perlin:" + perlinNoise[25,68]);
    }

    private float[,] PerlinNoiseGenerator()
    {
        return GeneratePerlinNoise(GenerateWhiteNoise(width, height), 2);
    }

    void Update()
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
        // from tutorial
        //float xCoord = (float)x / width * scale + offsetX;
        //float yCoord = (float)y / height * scale + offsetY;
        //float sample = Mathf.PerlinNoise(xCoord, yCoord); // need write my own perlinnoise
        //return new Color(sample, sample, sample);
        float sample = perlinNoise[x, y];
        return new Color(sample, sample, sample);

    }


    float[,] GenerateWhiteNoise(int width, int height)
    {
        // generate white noise: create an array with random values between 0 and 1
        //float random = Random.Range(0, 1);
        float[,] noise = new float[width, height];
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                noise[i, j] = Random.Range(0.0f,1.0f);
                //print("noise" + noise[i, j]);
            }
        }
        return noise;
    }

    float[,] GenerateSmoothNoise(float[,] baseNoise, int octave)
    {
        int width = baseNoise.GetLength(0);
        int height = baseNoise.GetLength(1);
        float[,] smoothNoise = new float[width, height];

        int samplePeriod = 1 << octave; // calculates 2^k
        float sampleFrequency = 1.0f / samplePeriod;

        for (int i = 0; i < width; i++)
        {
            // calculate the horizontal sampling indices
            int sample_i0 = (i / samplePeriod) * samplePeriod;
            int sample_i1 = (sample_i0 + samplePeriod) % width; // wrap around
            float horizontal_blend = (i - sample_i0) * sampleFrequency;

            for (int j = 0; j < height; j++)
            {
                // calculate the vertical sampling indices
                int sample_j0 = (j / samplePeriod) * samplePeriod;
                int sample_j1 = (sample_j0 + samplePeriod) % height; // wrap around
                float vertical_blend = (j - sample_j0) * sampleFrequency;

                // blend the top twp corners
                float top = Interpolate(baseNoise[sample_i0, sample_j0],
                    baseNoise[sample_i1, sample_j0], horizontal_blend);

                // blend the bottom two corners
                float bottom = Interpolate(baseNoise[sample_i0, sample_i1],
                    baseNoise[sample_i1, sample_j1], horizontal_blend);

                // final blend
                smoothNoise[i, j] = Interpolate(top, bottom, vertical_blend);
            }
        }

        return smoothNoise;
    }

    float Interpolate(float x0, float x1, float alpha)
    {
        return x0 * (1 - alpha) + alpha * x1;
    }

    float[,] GeneratePerlinNoise(float[,] baseNoise, int octaveCount)
    {
        int width = baseNoise.GetLength(0);
        int height = baseNoise.GetLength(1);

        float[][,] smoothNoise = new float[octaveCount][,]; // an array of 2D array containing
        float persistance = 0.5f;

        // generate smooth noise
        for (int i = 0; i < octaveCount; i++)
        {
            smoothNoise[i] = GenerateSmoothNoise(baseNoise, i);
        }
        float[,] perlinNoise = new float[width, height];
        float amplitude = 1.0f;
        float totalAmplitude = 0.0f;

        // blend noise together
        for (int octave = octaveCount - 1; octave >= 0; octave--)
        {
            amplitude *= persistance;
            totalAmplitude += amplitude;

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    perlinNoise[i, j] += smoothNoise[octave][i, j] * amplitude;
                }
            }
        }

        //normalization
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                perlinNoise[i, j] /= totalAmplitude;
            }
        }

        return perlinNoise;
    }

}
