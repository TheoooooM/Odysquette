using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public bool hasRange;
    public int damage;
    public float range;
    public Rigidbody2D rb;
    public Vector3 basePosition;
    public bool isEnable;
    public bool isDesactive = false;
    public bool dontCollideWall;
    public ExtensionMethods.EnemyTypeShoot enemyTypeShoot;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void OnEnable()
    {
        isDesactive = false;
        isEnable = false;
        basePosition = transform.position;
        Invoke(nameof(DelayforDrag),0.5f);
        GetComponent<SpriteRenderer>().color = GameManager.Instance.currentColor;
    }

    // Update is called once per frame
    void Update()
    {
        if (hasRange)
        {
            if (Vector3.Distance(basePosition, transform.position) >=range)
            {
                DesactiveBullet();
                         
            }
        }

      
               if (rb.velocity.magnitude <= 0.1 && rb.drag > 0 && isEnable)
                    {
                        DesactiveBullet();
                     
                    } 
        
     
    }
    void DelayforDrag()
    {
        isEnable = true;
    }

    void DesactiveBullet()
    {
       
        if (isDesactive == false)
        {
            gameObject.SetActive(false); 

            PoolManager.Instance.enemypoolDictionary[enemyTypeShoot].Enqueue(gameObject);
            isDesactive = true;
        }
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other.name);
        if (!HealthPlayer.Instance.playerController.InDash)
        {
             if (other.CompareTag("Player"))
                    {
                        HealthPlayer.Instance.TakeDamagePlayer(damage);
                        DesactiveBullet();
                    }
        }
        
         if (other.CompareTag("Walls") && !dontCollideWall)

        {
           
            DesactiveBullet();
        }
    }
}
