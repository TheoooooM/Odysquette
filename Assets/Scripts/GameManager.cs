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

    public enum Effect {none, bounce, pierce, explosion, poison, ice}
    public enum Straw {basic, bubble, snipaille, eightPaille, fourDir, tripaille, mitra}
    
    #endregion
    
    #region StrawClass
    
    
    [System.Serializable]
    public class StrawClass // Class regroupant toute les informations concernant une paille
    {
        public String StrawName;
        public Straw StrawType; 
        public GameObject StrawParent; // GameObject de la paille
        public GameObject prefabs; // Prefabs de la balle tiré par la paille
        public BulletStat Scriptable; //Scriptable object regroupant les stats de la balle
        public List<Transform> spawnerTransform; // Transform ou spawn les balles
        public int size; //Taille du nombre de prefabs a instancier au lancement
    }
    #endregion
    
    //mouse
    private Vector2 mousepos; //position de la souris sur l'écran
    public  float angle; //angle pour orienter la paille
    
    
    //Juices
    [SerializeField] Effect _firstEffect;
    public Effect firstEffect => _firstEffect;
    
    [SerializeField] Effect _secondEffect;
    public Effect secondEffect => _secondEffect;
    
    //Straw
    public Straw actualStraw;
    public List<StrawClass> strawsClass; //Liste de toute les pailles

    public float shootCooldown;
    
    //Bullet
    [Header("Settings")]
    [SerializeField] int ShootRate;
    
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
        
        foreach (StrawClass str in strawsClass) //active la bonne paille au début
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

        if (Input.GetKeyDown(KeyCode.Space)) // Test pour changer de paille
        {
            ChangeStraw(Straw.tripaille);
        }
        
        //---------------- Oriente la paille ------------------------
        Vector2 Position = new Vector2(actualStrawClass.StrawParent.transform.position.x, actualStrawClass.StrawParent.transform.position.y);
        _lookDir = new Vector2(mousepos.x, mousepos.y) - Position ;
        angle = Mathf.Atan2(_lookDir.y, _lookDir.x) * Mathf.Rad2Deg;
        actualStrawClass.StrawParent.transform.rotation = Quaternion.Euler(0f, 0f, angle);
        //--------------------------------------------------------------
    }

    void FixedUpdate()
    {
    }

    void ChangeStraw(Straw straw) //change la paille 
    {
        actualStrawClass.StrawParent.SetActive(false);
        actualStrawClass = strawsClass[(int) straw];
        actualStrawClass.StrawParent.SetActive(true);
        ShootRate = actualStrawClass.Scriptable.ShootRate;

    }
    
    
}


