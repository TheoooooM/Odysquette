using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Singleton
    public static GameManager Instance;

    private void Awake()
    {
        Instance = this;
    }
    #endregion

    #region Enum

    public enum Effect {none,bounce, pierce, explosion, poison, ice}
    public enum Straw {basic, bubble, snipaille, eightPaille, fourDir, tripaille, mitra}
    
    #endregion
    
    #region StrawClass
    [System.Serializable]
    public class StrawClass
    {
        public Straw StrawType;
        public String StrawName;
        public GameObject StrawParent;
        public GameObject prefabs;
        public BulletStat Scriptable;
        public List<Transform> spawnerTransform;
        public int size;
    }
    #endregion
    
    //mouse
    private Vector2 mousepos;
    public  float angle;
    
    
    //Juices
    [SerializeField] Effect firstEffect;
    [SerializeField] Effect secondEffect;
    
    //Straw
    public Straw actualStraw;
    public List<StrawClass> strawsClass;

    private float shootCooldown;
    
    //Bullet
    [Header("Settings")]
    [SerializeField] int ShootRate;
    [SerializeField] private float bulletSize;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float bulletSpray;
    [SerializeField] private float damage;
    [SerializeField] private float Range;
    
    //Player
    public GameObject Player;

    //
    private float damageModifier;
    private float firerateModifier;
    private float speedModifier;
    
    

    
    [Header("----------------DEBUG---------------")]
    public Vector2 _lookDir;
    public StrawClass actualStrawClass;
    
    
    private void Start()
    {
        actualStrawClass = strawsClass[(int) actualStraw];
        foreach (StrawClass str in strawsClass)
        {
            if (str == actualStrawClass)
            {
                str.StrawParent.SetActive((true));
            }
            else
            {
                str.StrawParent.SetActive((false));
            }
        }
    }

    private void Update()
    {
        mousepos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (Input.GetKey(KeyCode.Mouse0))
        {
            if (shootCooldown <= 0f)
            {
                PoolManager.Instance.SpawnFromPool();
                shootCooldown = 1.5f;
            }
        }
        shootCooldown -= Time.deltaTime * ShootRate;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            ChangeStraw(Straw.tripaille);
        }
        
    }

    void FixedUpdate()
    {
        Vector2 Position = new Vector2(actualStrawClass.StrawParent.transform.position.x, actualStrawClass.StrawParent.transform.position.y);
        _lookDir = new Vector2(mousepos.x, mousepos.y) - Position ;
        angle = Mathf.Atan2(_lookDir.y, _lookDir.x) * Mathf.Rad2Deg;
        actualStrawClass.StrawParent.transform.rotation = Quaternion.Euler(0f, 0f, angle);
        
    }

    void ChangeStraw(Straw straw)
    {
        actualStrawClass.StrawParent.SetActive(false);
        actualStrawClass = strawsClass[(int) straw];
        actualStrawClass.StrawParent.SetActive(true);
        ShootRate = actualStrawClass.Scriptable.ShootRate;

    }
    
    
}


