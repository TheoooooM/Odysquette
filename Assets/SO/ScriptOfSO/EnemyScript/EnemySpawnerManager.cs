using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class EnemySpawnerManager : MonoBehaviour {
    [SerializeField] private GameObject afterImagePrefab;
    public Dictionary<ExtensionMethods.EnemyTypeShoot, Queue<GameObject>> enemypoolDictionary;
    private Queue<GameObject> availableObjects = new Queue<GameObject>();
    public static EnemySpawnerManager Instance;
    public  Queue<GameObject> bossShootQueue = new Queue<GameObject>();
    public GameObject bossShootBullet;
    public  Queue<GameObject> fxDashQueue = new Queue<GameObject>();
    public  Queue<GameObject> fxSmokeQueue  = new Queue<GameObject>();
    public GameObject fxDashPrefab;
    public GameObject fxSmokeDashPrefab;
    private void Awake() {
        Instance = this;
    }

    private void Start()
    {
        enemypoolDictionary = new Dictionary<ExtensionMethods.EnemyTypeShoot, Queue<GameObject>>();
        foreach (EnemyShootPool enemyShootPool in EnemySpawnerManager.Instance.enemyShootPools) {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < enemyShootPool.sizePool; i++)
            {
                GameObject obj = Instantiate(enemyShootPool.bulletPrefab, transform); 
                obj.name = enemyShootPool.enemyTypeShoot+" "+i;
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            enemypoolDictionary.Add(enemyShootPool.enemyTypeShoot, objectPool);
        }
    }

    public List<EnemyShootPool> enemyShootPools;

    private void GrowPool() {
        for (int i = 0; i < 10; i++) {
            var instanceToAdd = Instantiate(afterImagePrefab);
            instanceToAdd.GetComponent<CarAfterSprite>().car = transform;
            instanceToAdd.transform.SetParent(transform);
            AddToPool(instanceToAdd);
        }
    }

    public void AddToPool(GameObject instance) {
        instance.SetActive(false);
        availableObjects.Enqueue(instance);
    }

    public GameObject GetFromPool(Transform transform) {
        if (availableObjects.Count == 0) {
            GrowPool();
        }

        var instance = availableObjects.Dequeue();
        instance.GetComponent<CarAfterSprite>().car = transform;
        instance.SetActive(true);

        return instance;
    }
    public GameObject SpawnEnnemyShoot(ExtensionMethods.EnemyTypeShoot enemyTypeShoot, GameObject prefabBullet, Transform parentBulletTF) {
        GameObject obj;
        if (enemypoolDictionary[enemyTypeShoot].Count == 0) // Instancie une balle si il n'y en a plus dans la queue
        {
            obj = Instantiate(prefabBullet,  parentBulletTF.transform.position, parentBulletTF.rotation, transform);
         
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

    public void SpawnBossShootPool(Vector3 bullet)
    {
        SpawnEnemyPool(bullet, bossShootQueue, bossShootBullet);
    }

    public GameObject SpawnEnemyPool(Vector3 bullet, Queue<GameObject> currentQueue, GameObject currentObj)
    {
          GameObject obj;
                if (currentQueue.Count == 0)
                {
                    obj = Instantiate(currentObj, bullet, quaternion.identity);
                }
                else {
                    obj = currentQueue.Dequeue();
                    obj.transform.position = bullet;
                    obj.SetActive(true);
                }

                return obj;
    }
    public GameObject SpawnFxDash(Vector3 position)
    {
      GameObject obj =  SpawnEnemyPool(position, fxDashQueue, fxDashPrefab);
      return obj;
    }
    
}

[Serializable]
public class EnemyShootPool {
    public ExtensionMethods.EnemyTypeShoot enemyTypeShoot;
    public GameObject bulletPrefab;
    public int sizePool;
}
