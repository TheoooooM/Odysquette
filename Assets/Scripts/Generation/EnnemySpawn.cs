using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnnemySpawn : MonoBehaviour
{
    [Serializable]
    public class Spawn {
        public GameObject ennemy;
        public float spawnRate;
    }
    
    //private Enemy.Enemies enemyType;
    private GameObject MobToSpawn;
    public Spawn[] SpawnerList;
    private float rate;
    private int index = 0;
    

    [SerializeField] private RoomContainer part;

    private void Start()
    {
       
            int randomMax = 0;
                    for (int i = 0; i < SpawnerList.Length; i++) {
                        randomMax += (int) SpawnerList[i].spawnRate;
                    }
                    rate =Random.Range(0, randomMax);
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
        
        
        //Debug.Log(index-1);
        GameObject GO = Instantiate(MobToSpawn, transform.position, Quaternion.identity, part.transform);
        part.room.ennemiesList.Add(GO);
        GO.GetComponentInChildren<EnemyStateManager>().roomParent = part.room;
        
    }
}
