using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeightMap : MonoBehaviour {


    // Create a texture and fill it with Perlin noise.
    // Try varying the xOrg, yOrg and scale values in the inspector
    // while in Play mode to see the effect they have on the noise.

    // Width and height of the texture in pixels.
    public int pixWidth;
    public int pixHeight;

    // The origin of the sampled area in the plane.
    public float xOrg;
    public float yOrg;

    // The number of cycles of the basic noise pattern that are repeated
    // over the width and height of the texture.
    public float scale = 0.005f;

    private Texture2D noiseTex;
    private Color[] pix;
    private Renderer rend;

    public Terrain Terreno;
    int xRes;
    int yRes;
    float[,] heights;

    void CalcNoise()
    {
        heights = Terreno.terrainData.GetHeights(0, 0, xRes, yRes);
        Debug.Log("heights antes de meterle Perlin, se supone, en 0,0  " + heights[0, 0] + " en 10,10: " + heights[10, 10] + " en 40,40: " + heights[40, 40] + " en 60,60: " + heights[60, 60] + " en 100,100: " + heights[100, 100]);
        // For each pixel in the texture...
        float y = 0f;
        Debug.Log("la width de noisetex " + noiseTex.width);
        while (y < noiseTex.height)
        {
            float x = 0f;
            while (x < noiseTex.width)
            {
                float xCoord = xOrg + x / noiseTex.width * scale;
                float yCoord = yOrg + y / noiseTex.height * scale;
                
                float sample = Mathf.PerlinNoise(xCoord, yCoord);   //antes xCoord e yCoord
                heights[(int)x, (int)y] = sample;
                pix[(int)y * noiseTex.width + (int)x] = new Color(sample, sample, sample);
                x++;
            }
            y++;
        }
        Terreno.terrainData.SetHeights(0, 0, heights);                  //AQUI ES DONDE SE MODELA EL TERRENO SEGUN PERLIN
        // Copy the pixel data to the texture and load it into the GPU.
        noiseTex.SetPixels(pix);
        noiseTex.Apply();

        Debug.Log("heights despues de meterle Perlin, se supone, en 0,0  " + heights[0, 0] + " en 10,10: " + heights[10, 10] + " en 40,40: " + heights[40, 40] + " en 60,60: " + heights[60, 60] + " en 100,100: " + heights[100, 100]);

    }

    void Aplanar()
    {
        heights = Terreno.terrainData.GetHeights(0, 0, xRes, yRes);
        for(int i = 0; i< 513; i++)
        {
            for(int j=0; j< 513; j++ )
            {
                heights[i, j] = 0f;
            }
            
        }
        Terreno.terrainData.SetHeights(0, 0, heights);
    }



    // Use this for initialization
    void Start () {
        rend = GetComponent<Renderer>();

        pixWidth=513;
        pixHeight=513;

        xOrg = 0;
        yOrg = 0;


        // Set up the texture and a Color array to hold pixels during processing.
        noiseTex = new Texture2D(pixWidth, pixHeight);
        pix = new Color[noiseTex.width * noiseTex.height];
        rend.material.mainTexture = noiseTex;

        xRes = 513;
        yRes = 513;

        //xRes = Terreno.terrainData.heightmapWidth;
        //yRes = Terreno.terrainData.heightmapHeight;
        heights = Terreno.terrainData.GetHeights(0, 0, xRes, yRes);
        //heights[100, 100] = 1f;
        Debug.Log("100,100: "+ heights[100, 100]);
        Debug.Log("200,200: " + heights[200, 200]);
        //heights[100, 100] = 0f;
        //heights[200, 200] = 0f;

        Terreno.terrainData.SetHeights(0, 0, heights);

        //Debug.Log("100,100 después: " + heights[100, 100]);
        //Debug.Log("200,200 después: " + heights[200, 200]);

        //CalcNoise();
    }

    void Update()
    {

        

        //CalcNoise();
    }

    private void OnGUI()
    {
        if (GUI.Button(new Rect(30, 30, 200, 30), "Lanzar Perlin"))
        {
            CalcNoise();
        }


        if (GUI.Button(new Rect(330, 30, 200, 30), "Aplanar"))
        {
            Aplanar();
        }
    }


}
