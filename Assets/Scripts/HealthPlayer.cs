using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPlayer : MonoBehaviour
{
    

    public Collider waveCollider;
    [SerializeField]
    public int maxHealth;
    [SerializeField]
    public int healthPlayer ;
    [SerializeField]
    private float timeInvincible;
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private bool isInvincible;
    public bool  isDead;
     [SerializeField]
    private float timerInvincible;
    // Start is called before the first frame update
    public static HealthPlayer Instance;
    public Playercontroller playerController;

    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        playerController = GetComponent<Playercontroller>();
        spriteRenderer = GetComponent<SpriteRenderer>();
      healthPlayer = maxHealth;
        for (int i = 0; i < healthPlayer; i++)
        {
             UIManager.Instance.HeartsLife[i].SetActive(true); }

    



    }

    private void Update()
    {
        if (isInvincible)
        {
             if (timerInvincible > timeInvincible)
                    {
                      
                        timerInvincible = 0;
                        isInvincible = false;
                        spriteRenderer.color = Color.white;
                        waveCollider.enabled = true;
                        
                    }
             else
             { 
                 timerInvincible += Time.deltaTime;
                 
                 spriteRenderer.color = Color.red;
             }
        }
       
    }

  public  void TakeDamagePlayer(int damage)
    {

        
             if (!isInvincible)
                    {
                       
                        if (healthPlayer - damage <= 0)
                        {
                            OnDeathPlayer();
                        }
                        healthPlayer -= damage;
                                    for (int i = UIManager.Instance.HeartsLife.Length-1; i > -1; i--)
                                    {
                                        if(i >= healthPlayer)
                                        {
                                            waveCollider.enabled = false;
                                            UIManager.Instance.HeartsLife[i].SetActive(false); 
                                        }
                                        else
                                        {
                                            break;
                                        }
            
                                        isInvincible = true;
                                    }
                   
                    }
        
       
    }

    void TakeHealPlayer(int heal)
    {
        healthPlayer += heal;
        for (int i = 0; i < healthPlayer; i++)
        {
            if (i <= healthPlayer)
            {
                
                UIManager.Instance.HeartsLife[i].SetActive(true);
            }
            else
            {
                break;
            }
        }
        
    }

    void OnDeathPlayer()
    {
        GameManager.Instance.enabled = false;
        Destroy(gameObject);
    

    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("ShieldEnemy"))
        {
            TakeDamagePlayer(1);
        }
    }
}
