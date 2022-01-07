using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Bullet : MonoBehaviour {
    public bool isBounce;
    public bool colliding;
    public bool hasRange;
    public float damage;
    public float range;
    public Color colorBang;
    Vector3 basePosition;
    public float knockUpValue;
    public StrawSO.RateMode rateMode;
    public Vector3 oldPositionPoison;
    [SerializeField] private bool isColliding;
    public Rigidbody2D rb;
    public Vector3 lastVelocity;

    public float ammountUltimate;

    [Header("==============Effects Stat===============")]
    public int pierceCount = 2;

    private int _pierceCount;

    public int bounceCount = 2;
    public int _bounceCount;

    public float poisonCooldown = 5;
    float _poisonCooldown = 0;
    public bool isEnable;
    public bool isDesactive = false;

    public float distance;
    private SpriteRenderer bulletSpriteRenderer;

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
        if (GameManager.Instance.firstEffect == GameManager.Effect.piercing || GameManager.Instance.secondEffect == GameManager.Effect.piercing) _pierceCount = pierceCount;
        else _pierceCount = 0;

        if (GameManager.Instance.firstEffect == GameManager.Effect.bouncing || GameManager.Instance.secondEffect == GameManager.Effect.bouncing) _bounceCount = bounceCount;
        else _bounceCount = 0;
        bulletSpriteRenderer = GetComponent<SpriteRenderer>();
    }

    public virtual void OnEnable() {
        isColliding = false;
        isBounce = false;
        isDesactive = false;
        isEnable = false;
        basePosition = transform.position;
        _pierceCount = pierceCount;
        //canBounce = false; 
        lastVelocity = rb.velocity;
        Invoke(nameof(DelayforDrag), 0.5f);

        if (GameManager.Instance.firstEffect == GameManager.Effect.piercing || GameManager.Instance.secondEffect == GameManager.Effect.piercing) _pierceCount = pierceCount;
        else _pierceCount = 0;

        if (GameManager.Instance.firstEffect == GameManager.Effect.bouncing || GameManager.Instance.secondEffect == GameManager.Effect.bouncing) _bounceCount = bounceCount;
        else _bounceCount = 0;
        GetComponent<SpriteRenderer>().color = GameManager.Instance.currentColor;
    }

    public virtual void Update() {
        if (hasRange) {
            if (Vector3.Distance(basePosition, transform.position) >= range) {
                if(GameManager.Instance.firstEffect == GameManager.Effect.explosive || GameManager.Instance.secondEffect == GameManager.Effect.explosive) Explosion();
                DesactiveBullet();
            }
        }


        if (rb.velocity.magnitude <= 0.1 && rb.drag > 0 && isEnable) {
            if(GameManager.Instance.firstEffect == GameManager.Effect.explosive || GameManager.Instance.secondEffect == GameManager.Effect.explosive) Explosion();
            DesactiveBullet();
        }
    }

    private void FixedUpdate() {
        if (GameManager.Instance.firstEffect == GameManager.Effect.poison || GameManager.Instance.secondEffect == GameManager.Effect.poison) {
            /*if (_poisonCooldown < distance / rb.velocity.magnitude) {
                _poisonCooldown += Time.fixedDeltaTime;
            }*/
            // v= d/t => t = d/v => d = vt
            
            
            if (Vector2.Distance(oldPositionPoison, transform.position) > distance) {
                oldPositionPoison = transform.position;
                PoolManager.Instance.SpawnPoisonPool(transform);
                //_poisonCooldown = poisonCooldown;
            }
            
            /*else {
                oldPositionPoison = transform.position;
                //Debug.Log(_poisonCooldown);
                PoolManager.Instance.SpawnPoisonPool(transform);
                _poisonCooldown = poisonCooldown;
            }*/
        }
        if(!isColliding)lastVelocity = rb.velocity;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("DestructableObject")) return;
        if (isColliding) return;
        
        isColliding = true;
        // Rest of the code
        StartCoroutine(Reset());
        
        switch (GameManager.Instance.firstEffect) {
            case GameManager.Effect.explosive:
                Explosion();
                break;

            /*case GameManager.Effect.ice:
                Ice(other.gameObject);
                break;*/
        }

        switch (GameManager.Instance.secondEffect) {
            case GameManager.Effect.explosive:
                Explosion();
                break;

            /*case GameManager.Effect.ice:
                Ice(other.gameObject);
                break;*/
        }

        if (other.CompareTag("Enemy")) {
            EnemyStateManager enemyStateManager = other.GetComponent<EnemyStateManager>();
            if(enemyStateManager.enabled)
            enemyStateManager.TakeDamage(damage, rb.position, knockUpValue, true, false);
            if (rateMode != StrawSO.RateMode.Ultimate) {
                GameManager.Instance.ultimateValue += enemyStateManager.EMainStatsSo.coeifficentUltimateStrawPoints * ammountUltimate;
            }
            
            if (_pierceCount > 0) {
                _pierceCount--;
                PoolManager.Instance.SpawnPiercePool(transform);
                PoolManager.Instance.SpawnImpactPool(transform);
            }
            else {
                
                DesactiveBullet();
            }
        }
        else if (!other.CompareTag("Walls")) {
            if (GameManager.Instance.firstEffect != GameManager.Effect.explosive && GameManager.Instance.secondEffect != GameManager.Effect.explosive) AudioManager.Instance.PlayStrawSound(AudioManager.StrawSoundEnum.Impact, transform.position);
            DesactiveBullet();
        }
    }

    public virtual void OnCollisionEnter2D(Collision2D other) {
        colliding = true;
        //Debug.Log("collide with " + rb.velocity);
        
        if (_bounceCount > 0 && (other.gameObject.CompareTag("Walls")||other.gameObject.CompareTag("ShieldEnemy")) && lastVelocity.x != 0 && lastVelocity.y != 0) {
            AudioManager.Instance.PlayStrawSound(AudioManager.StrawSoundEnum.Impact);

            _bounceCount--;
            var speed = lastVelocity.magnitude;
            //Debug.Log(lastVelocity);

            var direction = Vector3.Reflect(lastVelocity.normalized, other.contacts[0].normal);
            rb.velocity = direction * Mathf.Max(speed, 0f);
            lastVelocity = rb.velocity;
            var angle = (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);
            transform.rotation = Quaternion.Euler(0, 0, angle);


            isBounce = true;
            Debug.Log(isBounce);
            PoolManager.Instance.SpawnImpactPool(transform);
        }
        else {
            if(other.gameObject.CompareTag("Walls")) AudioManager.Instance.PlayStrawSound(AudioManager.StrawSoundEnum.Impact, transform.position);
            DesactiveBullet();
        }
    }

    private void OnCollisionStay2D(Collision2D other) {
        _bounceCount--;
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        colliding = false;
    }


    private float maxDistance = 15;
    void Explosion() {
        PoolManager.Instance.SpawnExplosionPool(transform);
        if (Camera.main != null) {
            float distanceToCamera = Mathf.Abs(Vector2.Distance(Camera.main.transform.position, transform.position));
            float distanceSubstract = Mathf.Clamp(maxDistance - distanceToCamera, 0, maxDistance);
            float ratio = distanceSubstract / maxDistance;
            
            Camera.main.GetComponent<CameraShake>().CreateCameraShake(.085f * ratio, .15f * ratio);
        }
    }

    void Ice(GameObject gam) {
        Debug.Log("ice");
        gam.GetComponent<enemy>().freezeTime = 5;
    }

    void DelayforDrag() {
        isEnable = true;
    }

   public void DesactiveBullet() {
        if (isDesactive == false) {
            StopAllCoroutines();
            gameObject.SetActive(false);
            PoolManager.Instance.SpawnImpactPool(transform);
            isEnable = false;
            //if(GameManager.Instance.firstEffect == GameManager.Effect.explosion && GameManager.Instance.secondEffect == GameManager.Effect.explosion) PoolManager.Instance.SpawnExplosionPool(transform);
            if (rateMode == StrawSO.RateMode.Ultimate) {
                PoolManager.Instance.poolDictionary[GameManager.Instance.actualStraw][1].Enqueue(gameObject);
            }
            else {
                PoolManager.Instance.poolDictionary[GameManager.Instance.actualStraw][0].Enqueue(gameObject);
            }

            colliding = false;

            isDesactive = true;
        }
    }

    IEnumerator Reset() {
        yield return new WaitForEndOfFrame();
        isColliding = false;
    }
}