using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public List<Pool> bullets;
    public Dictionary<GameManager.Straw, Queue<GameObject>> poolDictionary;
    
    [System.Serializable]
    public class Pool
    {
        [SerializeField] private String name;
        public GameManager.Straw Straw;
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
        poolDictionary = new Dictionary<GameManager.Straw, Queue<GameObject>>();

        foreach (Pool pol in bullets)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();
            
            for (int i = 0; i < pol.size; i++)
            {
                GameObject obj = Instantiate(pol.prefabs, transform);
                obj.name = GameManager.Instance.actualStraw.ToString();
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }
            
            poolDictionary.Add(pol.Straw, objectPool);
        }
    }
    
    virtual public void SpawnFromPool()
    {
        int count = 0;
        foreach (Transform spawn in GameManager.Instance.actualStrawClass.spawnerTransform)
        {
            if (poolDictionary[GameManager.Instance.actualStraw].Count == 0)
            {
                GameObject obj = Instantiate(bullets[(int) GameManager.Instance.actualStraw].prefabs, transform);
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
