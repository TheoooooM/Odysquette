using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public Dictionary<GameManager.Straw, Queue<GameObject>> poolDictionary;
    public Queue<GameObject> PoisonQueue;
    public GameObject poisonPrefab;
    
    public Queue<GameObject> explosionQueue;
    public GameObject explosionPrefab;


    #region  Singleton
    public static PoolManager Instance;

    private void Awake()
    {
        Instance = this;
    }
    #endregion
    
    private void Start()
    {
        poolDictionary = new Dictionary<GameManager.Straw, Queue<GameObject>>(); //Créer un dictionnaire regroupant chaque pool
        PoisonQueue = new Queue<GameObject>();
        explosionQueue = new Queue<GameObject>();
        foreach (GameManager.StrawClass pol in GameManager.Instance.strawsClass)
        {
            //---------------------Génère les pool et les bullets de base------------------------- 
            Queue<GameObject> objectPool = new Queue<GameObject>();
            
            for (int i = 0; i < pol.size; i++)
            {
                GameObject obj = Instantiate(pol.prefabs, transform);
                obj.name = GameManager.Instance.actualStraw.ToString();
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }
            //---------------------------------------------------------------------------------------
            
            poolDictionary.Add(pol.StrawType, objectPool);
        }
    }
    
    virtual public void SpawnFromPool() //Active ou instancie une balle sur le spawn bullet
    {
        
            int count = 0;
            foreach (Transform spawn in GameManager.Instance.actualStrawClass.spawnerTransform)
            {
                if (poolDictionary[GameManager.Instance.actualStraw].Count == 0) // Instancie une balle si il n'y en a plus dans la queue
                {
                    GameObject obj = Instantiate(GameManager.Instance.actualStrawClass.prefabs, transform);
                    obj.name = GameManager.Instance.actualStrawClass.StrawName;
                    obj.transform.position = spawn.position;
                    //obj.transform.rotation = Quaternion.Euler(0f, 0f, GameManager.Instance.angle);
                    //poolDictionary[GameManager.Instance.actualStraw].Enqueue(obj);
                }
                else // Sinon active la première balle se trouvant dans la queue
                {
                    count++;
                    //Debug.Log("Count : "+count);    
                    GameObject objToSpawn = poolDictionary[GameManager.Instance.actualStraw].Dequeue();
                    objToSpawn.SetActive(true);
                    objToSpawn.transform.position = spawn.position;
                    //objToSpawn.transform.rotation = Quaternion.Euler(0f, 0f, GameManager.Instance.angle);
                }
            }
        
    }

    public void SpawnPoisonPool(Transform bullet)
    {
        if (PoisonQueue.Count == 0)
        {
            GameObject obj = Instantiate(poisonPrefab);
            obj.transform.position = bullet.position;
        }
        else
        {
            GameObject obj = PoisonQueue.Dequeue();
            obj.transform.position = bullet.position;
            obj.SetActive(true);
        }
    }
    
    
    public void SpawnExplosionPool(Transform bullet)
    {
        if (explosionQueue.Count == 0)
        {
            Debug.Log(bullet.position);
            GameObject obj = Instantiate(explosionPrefab, bullet.position, quaternion.identity);
            //obj.transform.position = bullet.position;
        }
        else
        {
            GameObject obj = explosionQueue.Dequeue();
            obj.transform.position = bullet.position;
            obj.SetActive(true);
        }
    }
    
    
}
