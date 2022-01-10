using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEditor;
using UnityEngine;

public class HealthPlayer : MonoBehaviour {
    public Rigidbody2D rb;
    [SerializeField] private CameraShake cameraShake;
    [SerializeField] public int maxHealth;
    [SerializeField] public int healthPlayer;
    private int lastHealth = 0;
    
    [SerializeField] private float timeInvincible;
    private SpriteRenderer spriteRenderer;
    [SerializeField] private bool isInvincible;
    [SerializeField] private float timerInvincible;
    public bool playUltimateSound;
    [SerializeField] private Material defaultMaterial;
    [SerializeField] private Material ultimateMaterial;

    public GameObject ultimateAura;
    // Start is called before the first frame update
    public static HealthPlayer Instance;
    public Playercontroller playerController;
    public bool isGameOver;

    private void Awake() {
        Instance = this;
        if (Camera.main != null) cameraShake = Camera.main.GetComponent<CameraShake>();
    }

    private void Start() {
        AddCommandConsole();
        
        playerController = GetComponent<Playercontroller>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        if (NeverDestroy.Instance != null) healthPlayer = NeverDestroy.Instance.life;
        else healthPlayer = maxHealth;
        lastHealth = healthPlayer;
        
        rb = GetComponent<Rigidbody2D>();
        if (UIManager.Instance != null) {
            int actualLifeHeart = Mathf.CeilToInt(healthPlayer / 2f);
            for (int i = 0; i < actualLifeHeart; i++) {
                if (healthPlayer % 2 == 1 && i == actualLifeHeart - 1) {
                    UIManager.Instance.HeartsLifes[i].GetComponent<Animator>().Play("idleLifeHalf");
                    UIManager.Instance.HeartsLifes[i].GetComponent<Animator>().SetFloat("lifeValue", .5f);
                }
                else {
                    UIManager.Instance.HeartsLifes[i].GetComponent<Animator>().Play("idleLifeAll");
                    UIManager.Instance.HeartsLifes[i].GetComponent<Animator>().SetFloat("lifeValue", 1);
                }
            }

            if (actualLifeHeart < 3) {
                for (int i = 2; i > 3 - (4 - actualLifeHeart); i--) {
                    UIManager.Instance.HeartsLifes[i].GetComponent<Animator>().Play("idleLifeEmpty");
                    UIManager.Instance.HeartsLifes[i].GetComponent<Animator>().SetFloat("lifeValue", 0);
                }
            }
        }
    }

    /// <summary>
    /// Add the command to the console
    /// </summary>
    private void AddCommandConsole() {
        CommandConsole REGEN = new CommandConsole("life", "life : Take (<0) or Give (>0) life to the player", new List<CommandClass>() {new CommandClass(typeof(int))}, (value) => {
            int life = int.Parse(value[0]);
            
            if(life > 0) GiveHealthPlayer(life);
            else if(life < 0) TakeDamagePlayer(-life);
        });
        
        CommandConsoleRuntime.Instance.AddCommand(REGEN);
    }
    
    private void Update() {
        if (isInvincible) {
            if (timerInvincible > timeInvincible) {
                timerInvincible = 0;
                isInvincible = false;
                spriteRenderer.color = Color.white;
            }
            else {
                timerInvincible += Time.deltaTime;
            }
        }
        else
        {
            if (GameManager.Instance.ultimateValue == 125)
            {
                if (!playUltimateSound)
                {
                    AudioManager.Instance.PlayPlayerSound(AudioManager.PlayerSoundEnum.UltimateReady);
                    playUltimateSound = true;
                }
                spriteRenderer.material.SetTexture(spriteRenderer.sprite.name, spriteRenderer.sprite.texture);
                spriteRenderer.material = ultimateMaterial;
            }
        }
    }

    /// <summary>
    /// Deal damage to the player
    /// </summary>
    /// <param name="damage"></param>
    public void TakeDamagePlayer(int damage) {
        if (!isInvincible) {
            if (healthPlayer - damage <= 0) OnDeathPlayer();
            
            AudioManager.Instance.PlayPlayerSound(AudioManager.PlayerSoundEnum.Damage);
            healthPlayer -= damage;
            if(cameraShake != null) cameraShake.CreateCameraShake(.15f, .4f);
            
            if (UIManager.Instance == null) return;
            StartCoroutine(UpdateLife());
            
            for (int i = UIManager.Instance.HeartsLifes.Count - 1; i > -1; i--) {
                if(GameManager.Instance.ultimateValue == 125)
                    CancelUltimate();
                spriteRenderer.material.SetFloat("_HitTime", Time.time);
                if(GameManager.Instance != null && GameManager.Instance.strawSprite != null) GameManager.Instance.strawSprite.material.SetFloat("_HitTime", Time.time);
                isInvincible = true;
            }
        }
    }


    

   public void CancelUltimate()
   {
     
       spriteRenderer.material.SetTexture(spriteRenderer.sprite.name, spriteRenderer.sprite.texture);
       spriteRenderer.material = defaultMaterial;
   }

    /// <summary>
    /// Update the life
    /// </summary>
    private IEnumerator UpdateLife() {
        int lifeChange = healthPlayer - lastHealth;
        int lifeChangeValueTo1 = (lifeChange > 0 ? 1 : -1);
        
        for (int i = 0; i < Mathf.Abs(lifeChange); i++) {
            int oldHeartImg = Mathf.CeilToInt((lastHealth / 2f) - 1);
            int newHeartImg = Mathf.CeilToInt(((lastHealth + (1 * lifeChangeValueTo1)) / 2f) - 1);
            //Debug.Log(oldHeartImg + " " + newHeartImg);
            
            //int newHeartImg = Mathf.CeilToInt((healthPlayer / 2f) - 1);

            if (oldHeartImg == newHeartImg) {
                UIManager.Instance.HeartsLifes[newHeartImg].GetComponent<Animator>().SetFloat("lifeValue", UIManager.Instance.HeartsLifes[newHeartImg].GetComponent<Animator>().GetFloat("lifeValue") + (0.5f * lifeChangeValueTo1));
                for (int j = 0; j < 3; j++) {
                    if(j != newHeartImg) UIManager.Instance.HeartsLifes[j].GetComponent<Animator>().SetTrigger("UpdateLife");
                }
                lastHealth += 1 * lifeChangeValueTo1;
            }
            else {
                if (lifeChange > 0) {
                    UIManager.Instance.HeartsLifes[newHeartImg].GetComponent<Animator>().SetFloat("lifeValue", UIManager.Instance.HeartsLifes[newHeartImg].GetComponent<Animator>().GetFloat("lifeValue") + (0.5f * lifeChangeValueTo1));
                    for (int j = 0; j < 3; j++) {
                        if(j != newHeartImg) UIManager.Instance.HeartsLifes[j].GetComponent<Animator>().SetTrigger("UpdateLife");
                    }
                }
                else {
                    UIManager.Instance.HeartsLifes[oldHeartImg].GetComponent<Animator>().SetFloat("lifeValue", UIManager.Instance.HeartsLifes[oldHeartImg].GetComponent<Animator>().GetFloat("lifeValue") + (0.5f * lifeChangeValueTo1));
                    for (int j = 0; j < 3; j++) {
                        if(j != oldHeartImg) UIManager.Instance.HeartsLifes[j].GetComponent<Animator>().SetTrigger("UpdateLife");
                    }
                }
                
                lastHealth += 1 * lifeChangeValueTo1;
            }

            yield return new WaitForSeconds(2);
        }
    }


    /// <summary>
    /// Heal the player
    /// </summary>
    /// <param name="heal"></param>
    public void GiveHealthPlayer(int heal) {
        healthPlayer = Mathf.Clamp(healthPlayer + heal, 0, 6);

        StartCoroutine(UpdateLife());
    }

    private void OnDeathPlayer() {
        //GameManager.Instance.Score += (20*60/ (NeverDestroy.Instance.minute*60+ NeverDestroy.Instance.second));
        GameManager.Instance.SetND();
        NeverDestroy.Instance.Score = GameManager.Instance.Score;
        
        if (GameManager.Instance != null) GameManager.Instance.enabled = false;
        gameObject.SetActive(false);
        
        if (UIManager.Instance != null) UIManager.Instance.GameOver();
        AudioManager.Instance.PlayPlayerSound(AudioManager.PlayerSoundEnum.Death);
    }

    private void OnTriggerStay2D(Collider2D other) {
        if (other.CompareTag("ShieldEnemy")) TakeDamagePlayer(1);
    }
}