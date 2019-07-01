using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeScattering : MonoBehaviour {


    public int ArbolesTotales = 10;


    public GameObject CasillaBase;
    public GameObject ArbolBasePequeño;
    public GameObject ArbolBaseGrande;
    public GameObject ArbolBaseEnorme;
    public int Width = 50;
    public int Height = 50;
    public int MaxIntentos = 8;
    public int zonaExclusionAG = 1;         //RADIO, ADAPTAR AL TAMAÑO DEL ARBOL COMO MINIMO
    public int zonaExclusionAE = 3;
    GameObject[,] Mapa;
    bool[,] ArbolesPlantados;
    GameObject[,] Arboles;
    GameObject CasillaPadre;
    GameObject ArbolPadre;
    GameObject[,] ArbolesCA;
    int[,] SemillasCA;

    int iteracion = 0;
    public float Maxoffset = 1f;

    // Use this for initialization
    void Start () {
        CasillaPadre = GameObject.FindGameObjectWithTag("CasillaPadre");
        ArbolPadre = GameObject.FindGameObjectWithTag("ArbolPadre");
        Mapa = new GameObject[Width, Height];
        Arboles = new GameObject[Width, Height];
        ArbolesPlantados = new bool[Width, Height];
        for (int i = 0; i < Width; i++)
        {
            for (int j = 0; j < Height; j++)
            {
                Vector3 pos = new Vector3(i, j, 0);
                GameObject tile = Instantiate(CasillaBase, pos, Quaternion.identity, CasillaPadre.GetComponent<Transform>());
                tile.SetActive(true);
                Mapa[i, j] = tile;
                
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}



    private void OnGUI()
    {
        if (GUI.Button(new Rect(30, 30, 200, 30), "Tree Scattering 1"))
        {
            PlantarArboles1();

        }

        if (GUI.Button(new Rect(30, 130, 200, 30), "Reset Trees"))
        {
            ResetTrees();

        }


        if (GUI.Button(new Rect(30, 80, 200, 30), "Tree Scattering 2"))
        {
            PlantarArboles2();

        }

        if (GUI.Button(new Rect(30, 80, 200, 30), "Tree Scattering 3 CA"))
        {
            IterarCelullarAutomata();

        }

    }

    void ResetTrees()
    {
        for (int i = 0; i < Width; i++)
        {
            for (int j = 0; j < Height; j++)
            {
                if (ArbolesPlantados[i, j])
                {
                    Destroy(Arboles[i, j]);
                    ArbolesPlantados[i, j] = false;
                }

            }
        }
        iteracion = 0;
    }

    void PlantarArboles1()
    {
        for(int i=0; i<ArbolesTotales; i++)
        {
            bool plantado = false;
            int contador = 0;
            int Grande = Random.Range(0, 3);
            while (!plantado)
            {
                int x = Random.Range(0, Width);
                int y = Random.Range(0, Height);

                if (!ArbolesPlantados[x, y])
                {
                    if (Grande==1)
                    {
                        GameObject arbol = Instantiate(ArbolBasePequeño, new Vector3(x, y, -1), ArbolBasePequeño.GetComponent<Transform>().rotation, ArbolPadre.GetComponent<Transform>());
                        arbol.SetActive(true);
                        Arboles[x, y] = arbol;
                        ArbolesPlantados[x, y] = true;
                        plantado = true;
                    }
                    else if(Grande==2)
                    {
                        if (ComprobarArbolGrande(zonaExclusionAG, x, y))
                        {
                            GameObject arbol = Instantiate(ArbolBaseGrande, new Vector3(x, y, -1), ArbolBaseGrande.GetComponent<Transform>().rotation, ArbolPadre.GetComponent<Transform>());
                            arbol.SetActive(true);
                            Arboles[x, y] = arbol;
                            PlantarArbolGrande(zonaExclusionAG, x, y);
                            plantado = true;
                        }
                        else
                        {
                            //Debug.Log("ha fallado ComprobarArbolGrande");
                            contador++;
                        }

                    }
                    else
                    {
                        if (ComprobarArbolGrande(zonaExclusionAE, x, y))
                        {
                            GameObject arbol = Instantiate(ArbolBaseEnorme, new Vector3(x, y, -1), ArbolBaseEnorme.GetComponent<Transform>().rotation, ArbolPadre.GetComponent<Transform>());
                            arbol.SetActive(true);
                            Arboles[x, y] = arbol;
                            PlantarArbolGrande(zonaExclusionAE, x, y);
                            plantado = true;
                        }
                        else
                        {
                            //Debug.Log("ha fallado ComprobarArbolGrande");
                            contador++;
                        }
                    }
                }
                else
                {
                    //Debug.Log("ha fallado la comprobación normal");
                    contador++;
                }

                if (contador >= MaxIntentos) plantado = true;     
            }
        }
    }

    void PlantarArboles2()          //colocacion inicial con desplazamiento
    {
        for(int i = 1; i<Width-1; i+=3)
        {
            for(int j = 1; j<Height-1; j+=3)
            {
                GameObject arbol = Instantiate(ArbolBasePequeño, new Vector3(i, j, -1), ArbolBasePequeño.GetComponent<Transform>().rotation, ArbolPadre.GetComponent<Transform>());
                arbol.SetActive(true);
                Arboles[i, j] = arbol;
                ArbolesPlantados[i, j] = true;
                float x = Random.Range(-Maxoffset, Maxoffset+1);
                float y = Random.Range(-Maxoffset, Maxoffset+1);
                Vector3 offset = new Vector3(x, y, 0);
                Arboles[i, j].transform.position += offset;

            }
        }
    }

    bool ComprobarArbolGrande(int tam, int x, int y)
    {
        
        int bajox = x - tam;
        int altox = x + tam;
        int bajoy = y - tam;
        int altoy = y + tam;

        if (bajox < 0) return false;
        if (bajoy < 0) return false;
        if (altox > Width - 1) return false;
        if (altoy > Height - 1) return false;



        for (int i = bajox; i <= altox; i++)
        {
            for (int j = bajoy; j <= altoy; j++)
            {
                if(ArbolesPlantados[i, j]) return false;
            }
        }
        return true;                    //SITIO LIBRE PARA PLANTAR
    }

    void PlantarArbolGrande(int tam, int x, int y)
    {
        int bajox = x - tam;
        int altox = x + tam;
        int bajoy = y - tam;
        int altoy = y + tam;


        for (int i = bajox; i <= altox; i++)
        {
            for (int j = bajoy; j <= altoy; j++)
            {

                ArbolesPlantados[i, j] = true;     
                
            }
        }

    }

    void IterarCelullarAutomata()
    {
        if (iteracion==0)
        {
            PlantarSemillas();
        }
        else
        {
            for()
        }
    }
}
