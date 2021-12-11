using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerManager : MonoBehaviour {
    [SerializeField] private GameObject afterImagePrefab;

    private Queue<GameObject> availableObjects = new Queue<GameObject>();

    public static EnemySpawnerManager Instance;

    private void Awake() {
        Instance = this;
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
}

[Serializable]
public class EnemyShootPool {
    public ExtensionMethods.EnemyTypeShoot enemyTypeShoot;
    public GameObject bulletPrefab;
    public int sizePool;
}