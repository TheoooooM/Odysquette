using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    [SerializeField] GameObject[] OpenTop; //Toute les sallles ouverte en haut
    [SerializeField] GameObject[] OpenBot; //Toute les sallles ouverte en bas
    [SerializeField] GameObject[] OpenLeft; //Toute les sallles ouverte à gauche
    [SerializeField] GameObject[] OpenRight; //Toute les sallles ouverte à droite
    [SerializeField] GameObject[] Close; //Toute les salles à 1 issue

    [SerializeField] GameObject SpawnRoom;
    [SerializeField] GameObject SpawningPoint;


    [SerializeField] GameObject[,] map; //Quadrillage de la map
    GameObject[] UsableRoom; //Liste des Room disponible pour l'emplacement voulu

    [SerializeField] GameObject RoomSlot;



    // Start is called before the first frame update
    void Start()
    {
        GenerateStage(10, 40);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GenerateStage(int dimension, int MaxRoom)
    {
        #region Variables
        bool FirstRoom = true; //Est ce que c'est la premère salle
        int RoomLeft = MaxRoom; //Le nombre de salle possible restantes
        GameObject CurrentRoom; //Pièce en train d'ête génré 
        int CurrentExit;// Nombrede sorties restantes dnas la CurrentRoom
        GameObject[] ToGenerate; //Liste des salles a génré
        #endregion

        
        map = new GameObject[dimension,dimension];
        

        for (int i = 0; i < dimension; i++)
        {
            Debug.Log(i);
            for(int u = 0; u <= 9; u++)
            {
                Debug.Log(u);
                map[i, u] = RoomSlot;
            }
        }

        Instantiate(SpawnRoom, SpawningPoint.transform);
        map[dimension / 2 + 1, dimension / 2 + 1].GetComponent<RoomSlot>().IsUse = true;
        Debug.Log(dimension / 2 + 1 + "  Etat IsUse map[5,5] : " + map[5, 5].GetComponent<RoomSlot>().IsUse);

    }
}
