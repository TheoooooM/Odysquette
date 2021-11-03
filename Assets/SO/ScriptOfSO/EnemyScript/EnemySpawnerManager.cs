using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerManager : MonoBehaviour
{
   
    public static EnemySpawnerManager Instance;
    private void Awake()
    {
        Instance = this;
    }
    public List<EnemyShootPool> enemyShootPools;
}
[Serializable]
public class EnemyShootPool
{
    public ExtensionMethods.EnemyTypeShoot enemyTypeShoot;
    public GameObject bulletPrefab;
    public int sizePool;
}
