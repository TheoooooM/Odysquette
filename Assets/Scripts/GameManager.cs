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
        public int sizeUltimatePool;
        public StrawSO ultimateStrawSO;
      public  Transform spawnerTransform; // Transform ou spawn les balles
      
        public int sizeShootPool; //Taille du nombre de prefabs a instancier au lancement
    }
    
    Vector3 lastInput;
    #endregion
     float ultimateTimeValue;
    //mouse
    [SerializeField]
    private float offsetPadViewFinder;
    private Vector2 mousepos; //position de la souris sur l'écran
    public  float angle; //angle pour orienter la paille
    public float viewFinderDistance;
    [SerializeField] private Camera main;
    //Juices
    [SerializeField] Effect _firstEffect;
    public Effect firstEffect => _firstEffect;
    
    [SerializeField] Effect _secondEffect;
    public Effect secondEffect => _secondEffect;
    
    //Straw
    public Straw actualStraw;
    public List<StrawClass> strawsClass; //Liste de toute les pailles

    public bool shooting;
    public bool utlimate;
    private int countShootRate ;
   
    private float shootLoading;

    private bool EndLoading;


    public float shootCooldown;
    
    //Input
    public bool isMouse = true;
    public Vector2 ViewPad;

    
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
        lastInput = Vector3.right*viewFinderDistance;
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
        if (shooting) 
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
                 shootCooldown = 0;
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
                                          
                        
                        
                    }
                    break;
                }
              
            }

           
        }
       if (actualStrawClass.ultimateStrawSO.rateMode == StrawSO.RateMode.Ultimate && utlimate)
       {
           if (ultimateTimeValue >= actualStrawClass.ultimateStrawSO.timeValue)
           {
               actualStrawClass.ultimateStrawSO.Shoot(actualStrawClass.spawnerTransform, this, 0);
               ultimateTimeValue = 0;
           }

           utlimate = false;
       }

        if (!shooting)
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
  
        ultimateTimeValue = Mathf.Min(ultimateTimeValue, actualStrawClass.ultimateStrawSO.timeValue); 

   
        
        //---------------- Oriente la paille ------------------------
        if (isMouse)
        {
            Vector2 Position = new Vector2(actualStrawClass.StrawParent.transform.position.x, actualStrawClass.StrawParent.transform.position.y);
            _lookDir = new Vector2(mousepos.x, mousepos.y) - Position ;
            angle = Mathf.Atan2(_lookDir.y, _lookDir.x) * Mathf.Rad2Deg;
            UIManager.Instance.viewFinder.transform.position =  main.WorldToScreenPoint(mousepos);
            

            actualStrawClass.StrawParent.transform.rotation = Quaternion.Euler(0f, 0f, angle);

        }
        else
        {

            if (ViewPad.magnitude > 0.5f)
            {
                  angle = Mathf.Atan2(ViewPad.y, ViewPad.x) * Mathf.Rad2Deg;
                         
                               
                           
                                          
                                            lastInput = ViewPad.normalized;
                                          
                                            
                                                Debug.Log("test");
                                                
                                                UIManager.Instance.viewFinder.transform.position = main.WorldToScreenPoint(actualStrawClass.spawnerTransform.position +(Vector3) ViewPad.normalized*viewFinderDistance);
                                                lastInput = ViewPad.normalized; 
            }
          
                                
            UIManager.Instance.viewFinder.transform.position = main.WorldToScreenPoint(actualStrawClass.spawnerTransform.position +(Vector3) lastInput.normalized*viewFinderDistance);        
                     
                      
           
   
        }
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
   

    
}


