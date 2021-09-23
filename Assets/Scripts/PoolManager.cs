using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public Dictionary<GameManager.Straw, Queue<GameObject>> poolDictionary;
    
    
    #region  Singleton
    public static PoolManager Instance;

    private void Awake()
    {
        Instance = this;
    }
    #endregion
    
    private void Start()
    {
        poolDictionary = new Dictionary<GameManager.Straw, Queue<GameObject>>();

        foreach (GameManager.StrawClass pol in GameManager.Instance.strawsClass)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();
            
            for (int i = 0; i < pol.size; i++)
            {
                GameObject obj = Instantiate(pol.prefabs, transform);
                obj.name = GameManager.Instance.actualStraw.ToString();
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }
            
            poolDictionary.Add(pol.StrawType, objectPool);
        }
    }
    
    virtual public void SpawnFromPool()
    {
        int count = 0;
        foreach (Transform spawn in GameManager.Instance.actualStrawClass.spawnerTransform)
        {
            if (poolDictionary[GameManager.Instance.actualStraw].Count == 0)
            {
                GameObject obj = Instantiate(GameManager.Instance.actualStrawClass.prefabs, transform);
                obj.name = GameManager.Instance.actualStraw.ToString();
                obj.transform.position = spawn.position;
                obj.transform.rotation = Quaternion.Euler(0f, 0f, GameManager.Instance.angle);
                poolDictionary[GameManager.Instance.actualStraw].Enqueue(obj);
            }
            else
            {
                count++;
                Debug.Log("Count : "+count);    
                GameObject objToSpawn = poolDictionary[GameManager.Instance.actualStraw].Dequeue();
                objToSpawn.SetActive(true);
                objToSpawn.transform.position = spawn.position;
                objToSpawn.transform.rotation = Quaternion.Euler(0f, 0f, GameManager.Instance.angle);
            }
        }
    }
    
}
