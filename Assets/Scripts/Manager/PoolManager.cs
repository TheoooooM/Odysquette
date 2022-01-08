using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PoolManager : MonoBehaviour {
    public Dictionary<GameManager.Straw, Queue<GameObject>[]> poolDictionary;
    public Queue<GameObject> PoisonQueue;
    public GameObject poisonPrefab;
    public GameObject piercePrefab;
    public Queue<GameObject> pierceQueue;
    public GameObject impactPrefab;
    public Queue<GameObject> impactQueue;
    public Queue<GameObject> explosionQueue;
    public GameObject explosionPrefab;

   


    #region Singleton

    public static PoolManager Instance;

    private void Awake() {
        Instance = this;
    }

    #endregion

    private void Start() {
        poolDictionary = new Dictionary<GameManager.Straw, Queue<GameObject>[]>(); //Créer un dictionnaire regroupant chaque pool
        PoisonQueue = new Queue<GameObject>();
        explosionQueue = new Queue<GameObject>();
        impactQueue = new Queue<GameObject>();
        pierceQueue = new Queue<GameObject>();

        if (GameManager.Instance != null) {
            foreach (GameManager.StrawClass pol in GameManager.Instance.strawsClass) {
                //---------------------Génère les pool et les bullets de base------------------------- 
                Queue<GameObject> objectPool = new Queue<GameObject>();
                Queue<GameObject> ultimatePool = new Queue<GameObject>();

                for (int i = 0; i < pol.sizeShootPool; i++) {
                    if (pol.strawSO.prefabBullet == null) continue;
                    GameObject obj = Instantiate(pol.strawSO.prefabBullet, transform);
                    obj.name = pol.strawSO.strawName + " " + i;
                    obj.SetActive(false);
                    objectPool.Enqueue(obj);
                }
                
                for (int i = 0; i < pol.sizeUltimatePool; i++) {
                    if (pol.ultimateStrawSO.prefabBullet == null) continue;
                    GameObject obj = Instantiate(pol.ultimateStrawSO.prefabBullet, transform);
                    obj.name = pol.ultimateStrawSO.strawName + " " + i;
                    obj.SetActive(false);

                    ultimatePool.Enqueue(obj);
                }

                //---------------------------------------------------------------------------------------
                poolDictionary.Add(pol.StrawType, new[] {objectPool, ultimatePool});
            }
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

            obj = Instantiate(prefabBullet, parentBulletTF.transform.position, parentBulletTF.rotation);
            

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

    public void SpawnPoisonPool(Transform bullet, Vector2 hit) {
        GameObject obj = SpawnEffectBulletPool(bullet, PoisonQueue, poisonPrefab);
        obj.GetComponent<Poison>().bulletTransform = bullet;
        obj.GetComponent<Poison>().hit = hit;
        Debug.Log(obj.GetComponent<Poison>().hit );
        obj.transform.rotation = bullet.rotation;
    }


    public void SpawnExplosionPool(Transform bullet)
    {
        SpawnEffectBulletPool(bullet, explosionQueue, explosionPrefab);
    }
    public void SpawnPiercePool(Transform bullet)
    {
       GameObject obj = SpawnEffectBulletPool(bullet, pierceQueue, piercePrefab);
        obj.transform.rotation = bullet.rotation;
    }
    public void SpawnImpactPool(Transform bullet)
    {
        SpawnEffectBulletPool(bullet, impactQueue, impactPrefab);
    }
    

    public  GameObject SpawnEffectBulletPool(Transform bullet, Queue<GameObject> QueueEffect, GameObject prefabBullet)
    {
        GameObject obj;
        if (QueueEffect.Count == 0)
        {
            obj = Instantiate(prefabBullet, bullet.position, quaternion.identity);
        }
        else {
            obj = QueueEffect.Dequeue();
            obj.transform.position = bullet.position;
            obj.SetActive(true);
        }
        return obj;
    }


   
}