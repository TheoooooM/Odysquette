using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public Dictionary<GameManager.Straw, Queue<GameObject>[]> poolDictionary;
    public Queue<GameObject> PoisonQueue;
    public GameObject poisonPrefab;
    
    public Queue<GameObject> explosionQueue;
    public GameObject explosionPrefab;

    public Dictionary<ExtensionMethods.EnemyTypeShoot, Queue<GameObject>> enemypoolDictionary;


    #region  Singleton
    public static PoolManager Instance;

    private void Awake()
    {
        Instance = this;
    }
    #endregion
    
    private void Start()
    {
        poolDictionary = new Dictionary<GameManager.Straw, Queue<GameObject>[]>(); //Créer un dictionnaire regroupant chaque pool
        PoisonQueue = new Queue<GameObject>();
        explosionQueue = new Queue<GameObject>();
        foreach (GameManager.StrawClass pol in GameManager.Instance.strawsClass)
        {
            //---------------------Génère les pool et les bullets de base------------------------- 
            Queue<GameObject> objectPool = new Queue<GameObject>();
            Queue<GameObject> ultimatePool = new Queue<GameObject>();
            for (int i = 0; i < pol.sizeShootPool; i++)
            {
                GameObject obj = Instantiate(pol.strawSO.prefabBullet); 
                obj.name = pol.strawSO.strawName+" "+i;
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            for (int i = 0; i < pol.sizeUltimatePool; i++)
            {
             GameObject obj = Instantiate(pol.ultimateStrawSO.prefabBullet); 
                obj.name = pol.ultimateStrawSO.strawName+" "+i;
                obj.SetActive(false); 
                ultimatePool.Enqueue(obj);

               
            }
          
            //---------------------------------------------------------------------------------------
            poolDictionary.Add(pol.StrawType, new []{objectPool, ultimatePool});
         
        }

        enemypoolDictionary = new Dictionary<ExtensionMethods.EnemyTypeShoot, Queue<GameObject>>();
        foreach (EnemyShootPool enemyShootPool in EnemySpawnerManager.Instance.enemyShootPools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();
            for (int i = 0; i < enemyShootPool.sizePool; i++)
            {
                GameObject obj = Instantiate(enemyShootPool.bulletPrefab); 
                obj.name = enemyShootPool.enemyTypeShoot+" "+i;
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }
            enemypoolDictionary.Add(enemyShootPool.enemyTypeShoot,objectPool);
        }
    }

    public GameObject SpawnFromPool(Transform parentBulletTF, GameObject prefabBullet, StrawSO.RateMode rateMode) //Active ou instancie une balle sur le spawn bullet
    {


        GameObject obj;
        int index = 0;
        if (rateMode == StrawSO.RateMode.Ultimate)
            index = 1;
            
        if (poolDictionary[GameManager.Instance.actualStraw][index].Count ==
            0) // Instancie une balle si il n'y en a plus dans la queue
        {
            obj = Instantiate(prefabBullet, parentBulletTF.position, parentBulletTF.rotation);
            

            //poolDictionary[GameManager.Instance.actualStraw].Enqueue(obj);
         
        }
        else // Sinon active la première balle se trouvant dans la queue
        {

          
            obj = poolDictionary[GameManager.Instance.actualStraw][index].Dequeue();

            obj.transform.position = parentBulletTF.position;

            obj.transform.rotation = parentBulletTF.rotation;
          // Debug.Log(obj.name);
        }
 return obj;

    }

    public void SpawnPoisonPool(Transform bullet)
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
      
       obj.transform.rotation = bullet.rotation;
    }
    
    
    public void SpawnExplosionPool(Transform bullet)
    {
        if (explosionQueue.Count == 0)
        {
        
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

    public GameObject SpawnEnnemyShoot(ExtensionMethods.EnemyTypeShoot enemyTypeShoot, GameObject prefabBullet, Transform parentBulletTF)
    {
        GameObject obj;
        if (enemypoolDictionary[enemyTypeShoot].Count == 0) // Instancie une balle si il n'y en a plus dans la queue
        {
            obj = Instantiate(prefabBullet, parentBulletTF.position, parentBulletTF.rotation);
        }
        else // Sinon active la première balle se trouvant dans la queue
        {

            obj = enemypoolDictionary[enemyTypeShoot].Dequeue();

            obj.transform.position = parentBulletTF.position;

            obj.transform.rotation = parentBulletTF.rotation;
            // Debug.Log(obj.name);
        }
        return obj;
    }

    
    
    
}
