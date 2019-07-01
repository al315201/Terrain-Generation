using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainPerlinNoise : MonoBehaviour {


    float offsetX, offsetY;
    public Terrain terreno;
   

	// Use this for initialization
	void Start () {
        offsetX = Random.Range(0f, 99999f);
        offsetY = Random.Range(0f, 99999f);

    }
	
	// Update is called once per frame
	void Update () {
		
	}


    private void OnGUI()
    {
        if(GUI.Button(new Rect(30,30, 200, 30 ),"Apply Perlin"))
        {
            offsetX = Random.Range(0f, 99999f);
            offsetY = Random.Range(0f, 99999f);

            int xRes = terreno.terrainData.heightmapHeight;
            int yRes = terreno.terrainData.heightmapWidth;
            float[,] heights = terreno.terrainData.GetHeights(0, 0, xRes, yRes);
            float tileSize = 10f;           //cuanto mayor el tilesize, más de lejos se ve el mapa

            for (int x =0; x<xRes; x++)
            {
                for (int y=0; y<yRes; y++)
                {
                    heights[x, y] = Mathf.PerlinNoise( ( (float)x / xRes) * tileSize + offsetX, ((float)y / yRes) * tileSize + offsetY) / 10.0f;

                }
            }

            terreno.terrainData.SetHeights(0, 0, heights);


        }


        if (GUI.Button(new Rect(30, 130, 200, 30), "Flatten Terrain"))
        {
            int xRes = terreno.terrainData.heightmapHeight;
            int yRes = terreno.terrainData.heightmapWidth;
            float[,] heights = terreno.terrainData.GetHeights(0, 0, xRes, yRes);

            for (int x = 0; x < xRes; x++)
            {
                for (int y = 0; y < yRes; y++)
                {
                    heights[x, y] = 0;

                }
            }

            terreno.terrainData.SetHeights(0, 0, heights);
            

        }
    }
}
