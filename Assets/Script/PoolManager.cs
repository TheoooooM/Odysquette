using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public List<Pool> bullets;
    public Dictionary<string, Queue<GameObject>> poolDictionary;
    
    public class Pool
    {
        public GameManager.Effect effect;
        public bool doubleEffect; //Est ce que la balle a un double effet
        public GameManager.Effect secondEffect;
        public GameObject prefabs;
        public int size;
    }

    #region  Singleton
    public static PoolManager Instance;

    private void Awake()
    {
        Instance = this;
    }
    #endregion
    
    
    
    private void Start()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (Pool pol in bullets)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();
            
            for (int i = 0; i < pol.size; i++)
            {
                GameObject obj = Instantiate(pol.prefabs, transform);
                obj.name = tag;
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }
            
            poolDictionary.Add(pol.effect.ToString(), objectPool);
        }
    }
    
    virtual public void SpawnFromPool(string tag)
    {
        GameObject objToSpawn = poolDictionary[tag].Dequeue();
        objToSpawn.SetActive(true);
        
        poolDictionary[tag].Enqueue(objToSpawn);
    }
    
}
