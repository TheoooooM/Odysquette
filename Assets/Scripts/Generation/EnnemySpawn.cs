using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnnemySpawn : MonoBehaviour
{
    [Serializable]
    public class Spawn
    {
        public GameObject ennemy;
        public float spawnRate;
    }
    //private EnnemyVar.Ennemies ennemyType;
    private GameObject MobToSpawn;
    public Spawn[] SpawnerList;
    private float rate;
    private int index = 0;

    [SerializeField] private RoomContainer part;
    
    void Start()
    {
        rate =Random.Range(0, 100);
        //Debug.Log("rate : " + rate);
        while (rate>=0)
        {
            //Debug.Log("index : " +index);
            MobToSpawn = SpawnerList[index].ennemy;
            rate -= SpawnerList[index].spawnRate;
            index++;
            if (index == SpawnerList.Length)
            {
                break;
            }
        }

        part = GetComponentInParent<RoomContainer>();
        //Debug.Log(index-1);
        GameObject GO = Instantiate(MobToSpawn, transform.position, Quaternion.identity, part.transform);
        part.room.ennemiesList.Add(GO);
        GO.GetComponentInChildren<EnemyStateManager>().roomParent = part.room;
    }

    void Update()
    {
        
    }
}
