using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    GameObject[,] map; //Quadrillage de la map
    GameObject[] UsableRoom; //Liste des Room disponible pour l'emplacement voulu



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateStage(int dimension)
    {
        map = new GameObject[dimension,dimension];

    }
}
