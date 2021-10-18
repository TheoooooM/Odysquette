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
            
            for (int i = 0; i < pol.sizePool; i++)
            {
                GameObject obj = Instantiate(pol.strawSO.prefabBullet, transform);
                obj.name = i.ToString();
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }
            //---------------------------------------------------------------------------------------
            
            poolDictionary.Add(pol.StrawType, objectPool);
        }
    }

    public GameObject SpawnFromPool(Transform parentBulletTF, GameObject prefabBullet) //Active ou instancie une balle sur le spawn bullet
    {


        GameObject obj;

        if (poolDictionary[GameManager.Instance.actualStraw].Count ==
            0) // Instancie une balle si il n'y en a plus dans la queue
        {
            obj = Instantiate(prefabBullet, parentBulletTF.position, parentBulletTF.rotation);
            

            //poolDictionary[GameManager.Instance.actualStraw].Enqueue(obj);
         
        }
        else // Sinon active la première balle se trouvant dans la queue
        {

          
            obj = poolDictionary[GameManager.Instance.actualStraw].Dequeue();

            obj.transform.position = parentBulletTF.position;

            obj.transform.rotation = parentBulletTF.rotation;
          // Debug.Log(obj.name);
        }
 return obj;

    }

    public void SpawnPoisonPool(Transform bullet, float speed)
    {
        GameObject obj;
        if (PoisonQueue.Count == 0)
        {
             obj = Instantiate(poisonPrefab);
            obj.transform.position = bullet.position;
        }
        else
        {
            obj = PoisonQueue.Dequeue();
            obj.transform.position = bullet.position;
            obj.SetActive(true);
        }

        obj.GetComponent<Poison>().rbBullet = bullet.GetComponent<Rigidbody2D>();
       obj.GetComponent<Poison>().speed = speed;
       obj.transform.rotation = bullet.rotation;
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
