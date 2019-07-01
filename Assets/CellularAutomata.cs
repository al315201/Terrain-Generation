using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellularAutomata : MonoBehaviour
{

    public GameObject Casilla;
    public int Width = 50;
    public int Height = 50;
    GameObject[,] Mapa;
    GameObject Padre;
    int[,] vecinos;
    bool[,] FueMuro;

    public bool delay;
    public float delayAmount = 0f;
    public int iteraciones = 1;
    public int condicionCambio = 5;
    public int murosIniciales = 45;        //PORCENTAJE
                                                        //LOS MUROS DE LA CUEVA SON LOS QUADS INACTIVOS

    // Use this for initialization
    void Start()
    {
        Padre = GameObject.FindGameObjectWithTag("Mapa");
        Mapa = new GameObject[Width, Height];
        vecinos = new int[Width, Height];
        FueMuro = new bool[Width, Height];

        for (int i = 0; i < Width; i++)
        {
            for (int j = 0; j < Height; j++)
            {
                Vector3 pos = new Vector3(i, j, 0);
                GameObject tile = Instantiate(Casilla, pos, Casilla.GetComponent<Transform>().rotation, Padre.GetComponent<Transform>());
                tile.SetActive(true);
                Mapa[i, j] = tile;
                vecinos[i, j] = 0;
                FueMuro[i, j] = false;
            }
        }
  
    }

    // Update is called once per frame
    void Update()
    {

    }


    private void OnGUI()
    {
        if (GUI.Button(new Rect(30, 30, 200, 30), "Cellular Automata"))
        {
            Restart();
            ExecuteCellularAutomata( iteraciones );

        }

    }



    void ExecuteCellularAutomata(int iter)
    {
        CavarMurosIniciales();
        
        for(int t = 0; t < iter; t++)
        {
            for (int i = 0; i < Width; i++)             //COMPROBAMOS LOS VECINOS Y LOS ANOTAMOS
            {
                for (int j = 0; j < Height; j++)
                {
                    vecinos[i, j] = ComprobarVecinos(i, j);
                    //if(vecinos[i, j]>=5) Debug.Log("vecinos: " + vecinos[i,j]);
                }
            }

            StartCoroutine(ActualizarMapa());
           
        }
        

    }

    IEnumerator ActualizarMapa()
    {
        Debug.Log("Iniciada Corutina");
        for (int i = 0; i < Width; i++)                  //UNA VEZ COMPROBADOS CAMBIAMOS LO QUE TOQUE
        {
            for (int j = 0; j < Height; j++)
            {
                if (vecinos[i, j] >= condicionCambio)
                {
                    Mapa[i, j].SetActive(false);            //HACER MURO
                    FueMuro[i, j] = true;
                }
                else
                {
                    Mapa[i, j].SetActive(true);     //DESHACER MURO
                }
                if(delay) yield return new WaitForSeconds(delayAmount);
            }
        }
    }

    void CavarMurosIniciales()
    {
        Debug.Log("Cavar huecos iniciales");
        for (int i = 0; i < Width; i++)
        {
            for (int j = 0; j < Height; j++)
            {
                if (Random.Range(0, 100) > murosIniciales)
                {
                    Mapa[i, j].SetActive(false);
                    FueMuro[i, j] = true;
                }
            }
        }
    }

    int ComprobarVecinos(int x, int y)
    {
        int respuesta = 0;
        int bajox = x - 1;
        int altox = x + 1;
        int bajoy = y - 1;
        int altoy = y + 1;

        if (x == 0) bajox++;
        if (y == 0) bajoy++;
        if (x == Width - 1) altox--;
        if (y == Height - 1) altoy--;

        for(int i = bajox; i <= altox; i++)
        {
            for (int j = bajoy; j <= altoy; j++)
            {
                if (i == x && j == y) continue;
                if (!Mapa[i, j].activeInHierarchy)      //ES MURO
                {
                    respuesta++;
                }
            }
        }
        if (FueMuro[x, y]) respuesta++;



        return respuesta;
    }

    void Restart()
    {
        for (int i = 0; i < Width; i++)
        {
            for (int j = 0; j < Height; j++)
            {
                
                Mapa[i, j].SetActive(true);
                vecinos[i, j] = 0;
                FueMuro[i, j] = false;
            }
        }
    }
}
