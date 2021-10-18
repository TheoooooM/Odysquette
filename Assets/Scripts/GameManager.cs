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
        public GameObject StrawParent;      // GameObject de la paille
        public StrawSO strawSO;
        public StrawSO ultimateStrawSO;
      public  Transform spawnerTransform; // Transform ou spawn les balles
      
        public int sizePool; //Taille du nombre de prefabs a instancier au lancement
    }
    #endregion
     float ultimateTimeValue;
    //mouse
    private Vector2 mousepos; //position de la souris sur l'écran
    public  float angle; //angle pour orienter la paille

    private float shootCooldownSeconds;
    //Juices
    [SerializeField] Effect _firstEffect;
    public Effect firstEffect => _firstEffect;
    
    [SerializeField] Effect _secondEffect;
    public Effect secondEffect => _secondEffect;
    
    //Straw
    public Straw actualStraw;
    public List<StrawClass> strawsClass; //Liste de toute les pailles
    private int countShootRate ;
    private float shootCooldown;
    private float shootLoading;

    private bool EndLoading;
    
    //Bullet
    [Header("Settings")]
    [SerializeField] int ShootRate;
    
    //Player
    public GameObject Player;
    
    [Header("----------------DEBUG---------------")]
    public Vector2 _lookDir;
    public StrawClass actualStrawClass;

    private void OnValidate()
    {
    
   
        foreach (StrawClass str in strawsClass)
        {
            
            if(str.strawSO != null)
             str.StrawName = str.strawSO.strawName;
        }
       
    }

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
            switch (actualStrawClass.strawSO.rateMode)
            {
                case StrawSO.RateMode.FireLoading:
                {
                  
                           shootLoading += Time.deltaTime;
                           if (shootLoading >= 0.25f)
                           {
                                EndLoading = true;
                           }
                           if (shootLoading >=actualStrawClass.strawSO.timeValue-0.1f )
                           {
                               EndLoading = false;
                           }
                           if (shootLoading >= actualStrawClass.strawSO.timeValue)
                           {
                                                                                                            
                               actualStrawClass.strawSO.Shoot(actualStrawClass.spawnerTransform, this, shootLoading);
                               shootLoading =0; 
                                                                          
                            
                                                                                                            
                           }
                    
               
                           break;
                }
                case StrawSO.RateMode.FireRate:
                { 
                    if ( shootCooldown >= actualStrawClass.strawSO.timeValue)
                    {
                 
                        if (countShootRate ==actualStrawClass.strawSO.effectAllNumberShoot && (actualStrawClass.strawSO.rateMainParameter || actualStrawClass.strawSO.rateSecondParameter))
                        {
                            actualStrawClass.strawSO.Shoot(actualStrawClass.spawnerTransform, this, 1);
                            countShootRate = 0;
                        }
                        else
                        {
                            actualStrawClass.strawSO.Shoot(actualStrawClass.spawnerTransform, this, 0);
                            countShootRate++;
                        }
                                          
                        shootCooldown = 0;
                        
                    }
                    break;
                }
              
            }

          
           
        }
       if (actualStrawClass.ultimateStrawSO.rateMode == StrawSO.RateMode.Ultimate && Input.GetKeyDown(KeyCode.Mouse1))
       {
           if (ultimateTimeValue >= actualStrawClass.strawSO.timeValue)
           {
               actualStrawClass.ultimateStrawSO.Shoot(actualStrawClass.spawnerTransform, this, 0);
               ultimateTimeValue = 0;
           }
       }

        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            if (EndLoading)
            {
              
                
                actualStrawClass.strawSO.Shoot(actualStrawClass.spawnerTransform, this, shootLoading);
                shootLoading =0; 
             
                EndLoading = false;
            }
        }
        shootCooldown += Time.deltaTime; 
      
        shootCooldown = Mathf.Min(shootCooldown, actualStrawClass.strawSO.timeValue); 
    
       ultimateTimeValue+= Time.deltaTime; 
        ultimateTimeValue = Mathf.Min(ultimateTimeValue, actualStrawClass.strawSO.timeValue); 

   
        
    }

    void FixedUpdate()
    {
        //---------------- Oriente la paille ------------------------
        Vector2 Position = new Vector2(actualStrawClass.StrawParent.transform.position.x, actualStrawClass.StrawParent.transform.position.y);
        _lookDir = new Vector2(mousepos.x, mousepos.y) - Position ;
        angle = Mathf.Atan2(_lookDir.y, _lookDir.x) * Mathf.Rad2Deg;
        actualStrawClass.StrawParent.transform.rotation = Quaternion.Euler(0f, 0f, angle);
        //--------------------------------------------------------------
    }

    void ChangeStraw(Straw straw) //change la paille 
    {
        //dictionnaire
        actualStrawClass.StrawParent.SetActive(false);
        actualStrawClass = strawsClass[(int) straw];
        actualStrawClass.StrawParent.GetComponent<SpriteRenderer>().sprite = actualStrawClass.strawSO.strawRenderer;
        actualStrawClass.StrawParent.SetActive(true);
        
        


    }
    public enum ShootMode
    {
        BasicShoot, CurveShoot,AreaShoot, AngleAreaShoot 
    }
    public enum RateMode
    {
        fireRate, LoadingRate,
    }

    
}


