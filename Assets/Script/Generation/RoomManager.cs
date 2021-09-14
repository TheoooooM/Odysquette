using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    #region Variables

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

    int RoomLeft;
    GameObject CurrentRoom; //Pièce en train d'ête génré 
    List<GameObject> ToGenerate = new List<GameObject>();
    int x;
    int y;
    GameObject Temproom;
    #endregion


    // Start is called before the first frame update
    void Start()
    {
        GenerateStage(10, 40);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SetRoom(int DimOne, int DimTwo,GameObject Room ,GameObject Gen)
    {
        map[DimOne, DimTwo].GetComponent<RoomSlot>().IsUse = true;
        map[DimOne, DimTwo].GetComponent<RoomSlot>().Usable = true;
        map[DimOne, DimTwo].GetComponent<RoomSlot>().Room = Room;
        Instantiate(Room, Gen.transform);
    }

    void GenerateRoom(GameObject Gen, int Side)
    {
        
        bool Top = false;
        bool Bot = false;
        bool Left = false;
        bool Right = false;
        bool NeedTop = false;
        bool NeedBot = false;
        bool NeedLeft = false;
        bool NeedRight = false;
        List<GameObject> UsableRoom = new List<GameObject>();
        bool GoodRoom = false;

        switch (Side)
        {
            case 0:
                x = CurrentRoom.GetComponent<RoomClass>().x + 1;
                y = CurrentRoom.GetComponent<RoomClass>().y;
                break;
            case 1:
                x = CurrentRoom.GetComponent<RoomClass>().x - 1;
                y = CurrentRoom.GetComponent<RoomClass>().y;
                break;
            case 2:
                x = CurrentRoom.GetComponent<RoomClass>().x;
                y = CurrentRoom.GetComponent<RoomClass>().y - 1;
                break;
            case 3:
                x = CurrentRoom.GetComponent<RoomClass>().x;
                y = CurrentRoom.GetComponent<RoomClass>().y + 1;
                break;
        }


        if (RoomLeft > 0)
        {
            for(int i = 0; i<=4; i++)
            {
                switch (i)
                {
                    case 0:
                        if(map[x+1,y] != null)
                        {
                            if (map[x+1, y].GetComponent<RoomSlot>().Usable || map[x + 1, y].GetComponent<RoomSlot>().IsUse) Top = true;
                            if (map[x+1, y].GetComponent<RoomSlot>().IsUse) NeedTop = true;
                        }
                        break;

                    case 1:
                        if (map[x-1, y] != null)
                        {
                            if (map[x-1, y].GetComponent<RoomSlot>().Usable || map[x - 1, y].GetComponent<RoomSlot>().IsUse) Bot = true;
                            if (map[x-1, y].GetComponent<RoomSlot>().IsUse) NeedBot = true;
                        }
                        break;

                    case 2:
                        if (map[x-1, y] != null)
                        {
                            if (map[x, y - 1].GetComponent<RoomSlot>().Usable || map[x, y - 1].GetComponent<RoomSlot>().IsUse) Left = true;
                            if (map[x, y - 1].GetComponent<RoomSlot>().IsUse) NeedLeft = true;
                        }
                        break;

                    case 3:
                        if (map[x+1, y] != null)
                        {
                            if (map[x, y + 1].GetComponent<RoomSlot>().Usable || map[x, y + 1].GetComponent<RoomSlot>().IsUse) Right = true;
                            if (map[x, y + 1].GetComponent<RoomSlot>().IsUse) NeedRight = true;
                        }
                        break;
                }
            }

            int NoGlitch = 0;
            while(!GoodRoom)
            {
                bool restart = false;
                switch (Side)
                {
                    case 0:
                        Temproom = OpenBot[Random.Range(0, OpenBot.Length)];
                        break;

                    case 1:
                        Temproom = OpenTop[Random.Range(0, OpenTop.Length)];
                        break;

                    case 2:
                        Temproom = OpenRight[Random.Range(0, OpenRight.Length)];
                        break;

                    case 3:
                        Temproom = OpenLeft[Random.Range(0, OpenLeft.Length)];
                        break;
                }
                if (RoomLeft - Temproom.GetComponent<RoomClass>().NbrExit >= 0)
                {
                    for(int i=0; i <= 4; i++)
                    {
                        switch (i)
                        {
                            case 0:
                                if (!Top && Temproom.GetComponent<RoomClass>().OpenTop || NeedTop && !Temproom.GetComponent<RoomClass>().OpenTop) restart = true ;
                                break;

                            case 1:
                                if (!Bot && Temproom.GetComponent<RoomClass>().OpenBottom || NeedBot && !Temproom.GetComponent<RoomClass>().OpenBottom) restart = true;
                                break;

                            case 2:
                                if (!Right && Temproom.GetComponent<RoomClass>().OpenRight || NeedRight && !Temproom.GetComponent<RoomClass>().OpenRight) restart = true;
                                break;

                            case 3:
                                if (!Left && Temproom.GetComponent<RoomClass>().OpenLeft || NeedLeft && !Temproom.GetComponent<RoomClass>().OpenLeft) restart = true;
                                break;

                        }
                    }

                }
                if (!restart)
                {
                    GoodRoom = true;
                    SetRoom(x, y, Temproom, Gen);
                    if(!Temproom.GetComponent<RoomClass>().Closing)
                    break;
                }
                Debug.Log("retry room");
                NoGlitch++;
                if (NoGlitch >= 50)
                {
                    GoodRoom = true;
                    Debug.Log("Crash");
                    break;
                }

            }


        }
        else
        {
            switch (Side)
            {
                case 0:
                    foreach(GameObject Rom in Close)
                    {
                        if (Rom.GetComponent<RoomClass>().OpenBottom) UsableRoom.Add(Rom);
                    }
                    break;
                case 1:
                    foreach (GameObject Rom in Close)
                    {
                        if (Rom.GetComponent<RoomClass>().OpenTop) UsableRoom.Add(Rom);
                    }
                    break;
                case 2:
                    foreach (GameObject Rom in Close)
                    {
                        if (Rom.GetComponent<RoomClass>().OpenRight) UsableRoom.Add(Rom);
                    }
                    break;
                case 3:
                    foreach (GameObject Rom in Close)
                    {
                        if (Rom.GetComponent<RoomClass>().OpenLeft) UsableRoom.Add(Rom);
                    }
                    break;
            }


        }



    }

    void RestartRoom()
    {

    }

    public void GenerateStage(int dimension, int MaxRoom)
    {
        #region Variables
        bool FirstRoom = true; //Est ce que c'est la premère salle
        bool IsGood = false;
        RoomLeft = MaxRoom; //Le nombre de salle possible restantes
        int CurrentExit;// Nombrede sorties restantes dnas la CurrentRoom

        #endregion

        
        map = new GameObject[dimension,dimension];
        

        for (int i = 0; i < dimension; i++)
        {
            for(int u = 0; u <= 9; u++)
            {
                map[i, u] = RoomSlot;
            }
        }

        Instantiate(SpawnRoom, SpawningPoint.transform);
        CurrentRoom = SpawnRoom;
        map[dimension / 2 + 1, dimension / 2 + 1].GetComponent<RoomSlot>().IsUse = true;
        map[dimension / 2 + 1, dimension / 2 + 1].GetComponent<RoomSlot>().Usable = true;

        if (FirstRoom)
        {
            CurrentExit = 4;
            RoomLeft -= 5;
            FirstRoom = false;
        }
        else
        {
            CurrentExit = CurrentRoom.GetComponent<RoomClass>().NbrExit - 1;
        }

        int NoGlitch = 0;
        while (!IsGood)
        {
            Debug.Log("ok");
            for (int i = 0; i >= 4; i++)
            {
                Debug.Log("ok");
                if (CurrentExit > 0)
                {
                    Debug.Log("ok");
                    switch (i)
                    {
                        case 0:
                            Debug.Log("ok");
                            if (CurrentRoom.GetComponent<RoomClass>().GenTop != null) GenerateRoom(CurrentRoom.GetComponent<RoomClass>().GenTop, i); Debug.Log("0");
                            break;
                        case 1:
                            if (CurrentRoom.GetComponent<RoomClass>().GenBot != null) GenerateRoom(CurrentRoom.GetComponent<RoomClass>().GenBot, i); Debug.Log("1");
                            break;
                        case 2:
                            if (CurrentRoom.GetComponent<RoomClass>().GenLeft != null) GenerateRoom(CurrentRoom.GetComponent<RoomClass>().GenLeft, i); Debug.Log("2");
                            break;
                        case 3:
                            if (CurrentRoom.GetComponent<RoomClass>().GenRight != null) GenerateRoom(CurrentRoom.GetComponent<RoomClass>().GenRight, i); Debug.Log("3");
                            break;

                    }
                }
                else if (ToGenerate[0] != null)
                {
                    Debug.Log("ok");
                    CurrentRoom = ToGenerate[0];
                    ToGenerate.RemoveAt(0);
                }
                else
                {
                    Debug.Log("ok");
                    IsGood = true;
                    break;
                }

            }
            Debug.Log("Vraiment rien la");
            NoGlitch++;
            if (NoGlitch >= 50)
            {
                IsGood = true;
                Debug.Log("Crashh");
                break;
            }
        }

    }
}
