using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Bullet : MonoBehaviour {
    public bool isBounce;
    public bool hasRange;
    public float damage;
    public float range;
    public Color colorBang;
    Vector3 basePosition;
    public float knockUpValue;
    public StrawSO.RateMode rateMode;
    public bool isColliding;
    public Rigidbody2D rb;
    public Vector3 lastVelocity;

    public float ammountUltimate;

    [Header("==============Effects Stat===============")]
    public int pierceCount = 3;

    private int _pierceCount;

    public int bounceCount = 2;
    public int _bounceCount;

    public bool isEnable;
    public bool isDesactive = false;

    public float distance;
    private SpriteRenderer bulletSpriteRenderer;
    private GameObject lastEnemyHit = null;
    
    private void Start() {
        rb = GetComponent<Rigidbody2D>();
        _pierceCount = GameManager.Instance.HasEffect(GameManager.Effect.piercing) ? pierceCount : 0;
        _bounceCount = GameManager.Instance.HasEffect(GameManager.Effect.bouncing) ? bounceCount : 0;
        bulletSpriteRenderer = GetComponent<SpriteRenderer>();
    }

    public virtual void OnEnable() {
        isColliding = false;
        isBounce = false;
        isDesactive = false;
        isEnable = false;
        basePosition = transform.position;
        _pierceCount = pierceCount;
        Invoke(nameof(DelayforDrag), 0.5f);

        StartCoroutine(WaitForDestroy());

        _pierceCount = GameManager.Instance.HasEffect(GameManager.Effect.piercing) ? pierceCount : 0;
        _bounceCount = GameManager.Instance.HasEffect(GameManager.Effect.bouncing) ? bounceCount : 0;
        GetComponent<SpriteRenderer>().color = GameManager.Instance.currentColor; 
    }

    public virtual void Update() {
        if (hasRange) {
            if (Vector3.Distance(basePosition, transform.position) >= range) {
                if(GameManager.Instance.HasEffect(GameManager.Effect.explosive)) Explosion();
                if(GameManager.Instance.HasEffect(GameManager.Effect.poison)) PoolManager.Instance.SpawnPoisonPool(transform, Vector2.zero);
                DesactiveBullet();
            }
        }
        
        if (rb.velocity.magnitude <= 0.1 && rb.drag > 0 && isEnable) {
            if(GameManager.Instance.HasEffect(GameManager.Effect.explosive)) Explosion();
            if(GameManager.Instance.HasEffect(GameManager.Effect.poison)) PoolManager.Instance.SpawnPoisonPool(transform, Vector2.zero);
            DesactiveBullet();
        } 
    }


    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("DestructableObject")) return;
        if (isColliding) {
            DesactiveBullet();
            return;
        }

        bool doExplosion = false;
        switch (GameManager.Instance.firstEffect) {
            case GameManager.Effect.explosive:
                if(!other.CompareTag("Walls")) doExplosion = true;
                break;

            case GameManager.Effect.poison :
                if(!other.CompareTag("Walls")) PoolManager.Instance.SpawnPoisonPool(transform, Vector2.zero);
                break;
            
        }

        switch (GameManager.Instance.secondEffect) {
            case GameManager.Effect.explosive:
                if(!other.CompareTag("Walls")) doExplosion = true;
                break;
            
            case GameManager.Effect.poison :
                if(!other.CompareTag("Walls")) PoolManager.Instance.SpawnPoisonPool(transform, Vector2.zero);
                break;
          
        }

        //Anti double explosion
        if (doExplosion) {
            Explosion();
        }

        if (other.CompareTag("Enemy")) {
            EnemyStateManager enemyStateManager = other.GetComponent<EnemyStateManager>();
            if(enemyStateManager.enabled)
            enemyStateManager.TakeDamage(damage, rb.position, knockUpValue, true, false);
            if (rateMode != StrawSO.RateMode.Ultimate) {
                GameManager.Instance.ultimateValue += enemyStateManager.EMainStatsSo.coeifficentUltimateStrawPoints * ammountUltimate;
            }
            
            if (_pierceCount > 0 && lastEnemyHit != other.gameObject && GameManager.Instance.HasEffect(GameManager.Effect.piercing)) {
                _pierceCount--;
                PoolManager.Instance.SpawnPiercePool(transform);
                lastEnemyHit = other.gameObject;
                //PoolManager.Instance.SpawnImpactPool(transform);
            }
            else if(pierceCount == 0 || !GameManager.Instance.HasEffect(GameManager.Effect.piercing)){
                DesactiveBullet();
            }
        }
        else if (!other.CompareTag("Walls")) {
            DesactiveBullet();
        }
    }


    public virtual void OnCollisionEnter2D(Collision2D other) {
        isColliding = true;
        
        if (_bounceCount > 0 && (other.gameObject.CompareTag("Walls")||other.gameObject.CompareTag("ShieldEnemy"))){ 
            if (GameManager.Instance.HasEffect(GameManager.Effect.poison)) PoolManager.Instance.SpawnPoisonPool(transform, other.contacts[0].normal);
            if (GameManager.Instance.HasEffect(GameManager.Effect.explosive)) Explosion();
            
            StartCoroutine(DestroyBulletIfStuck());
            
            _bounceCount--;
            AudioManager.Instance.PlayImpactStraw(AudioManager.StrawSoundEnum.Bounce, transform.position);
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
            if (GameManager.Instance.HasEffect(GameManager.Effect.poison) && !other.gameObject.CompareTag("Walls")) PoolManager.Instance.SpawnPoisonPool(transform, other.contacts[0].normal);
            if (GameManager.Instance.HasEffect(GameManager.Effect.explosive) && !other.gameObject.CompareTag("Walls")) Explosion();
            DesactiveBullet();
        }
    }
    
    protected virtual void OnCollisionExit2D(Collision2D other) {
        rb.velocity = lastVelocity;
        isColliding = false;
    }


    protected virtual IEnumerator  DestroyBulletIfStuck() {
        yield return new WaitForSeconds(0.15f);
        if(isColliding || rb.velocity.magnitude <= .025f) DesactiveBullet();
    }

    private float maxDistance = 20;
    protected void Explosion() {
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
            AudioManager.Instance.PlayImpactStraw(AudioManager.StrawSoundEnum.Impact, transform.position);
                
                

            isDesactive = true;
        }
    }

    private IEnumerator WaitForDestroy() {
        yield return new WaitForSeconds(10);
        DesactiveBullet();
    }
}