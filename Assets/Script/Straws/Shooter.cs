using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    [SerializeField] private GameObject poolContainer;
    [SerializeField] private GameObject Spawner;
    
    float shootCooldown;
    private Vector2 mousepos;
    public Vector2 _lookDir;
    float angle;
    
    [Header("Settings")]
    [SerializeField] int ShootRate;
    [SerializeField] private float bulletSize;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float bulletSpray;
    [SerializeField] private float damage;
    [SerializeField] private float Range;
    
    
    [System.Serializable]
    public class Pool
    {
        public Effects Effect;
        public bool doubleEffect; //Est ce que la balle a un double effet
        public Effects secondEffect;
        public GameObject prefabs;
        public int size;
    }
    
    [System.Serializable]
    public class Straw
    {
        public Straws strawEffect;
        private Sprite strawSprite;
    }

    public enum Effects
    {
        none, basic, bounce, throught
    }

    public enum Straws
    {
        pierce, bounce, poison, explosion, blackHole, ice
    }
    
    /*#region Singletone

    public static Shooter Instance;

    private void Awake()
    {
        Instance = this;
    }

    #endregion*/

    public List<Pool> bullets;
    public Dictionary<string, Queue<GameObject>> poolDictionary;

    private void Start()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (Pool pol in bullets)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();
            
            for (int i = 0; i < pol.size; i++)
            {
                GameObject obj = Instantiate(pol.prefabs, poolContainer.transform);
                obj.name = tag;
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }
            
            poolDictionary.Add(pol.Effect.ToString(), objectPool);
        }
    }

    private void Update()
    {
        mousepos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (Input.GetKey(KeyCode.Mouse0))
        {
            if (shootCooldown <= 0f)
            {
                //SpawnFromPool("basic");
                shootCooldown = 1.5f;
            }
            else
            {
                Debug.Log("nope");
            }
        }
        shootCooldown -= Time.deltaTime*ShootRate;
    }
    void FixedUpdate()
    {
        Vector2 Position = new Vector2(transform.position.x, transform.position.y);
        _lookDir = new Vector2(mousepos.x, mousepos.y) - Position ;
        angle = Mathf.Atan2(_lookDir.y, _lookDir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
        
    }

    /*virtual public void SpawnFromPool(string tag)
    {
        GameObject objToSpawn = poolDictionary[tag].Dequeue();
        objToSpawn.SetActive(true);
        objToSpawn.transform.position = Spawner.transform.position;
        objToSpawn.transform.rotation = Quaternion.Euler(0, 0, angle);
        
        poolDictionary[tag].Enqueue(objToSpawn);
    }*/
    
}
