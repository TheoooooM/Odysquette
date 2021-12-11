using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPlayer : MonoBehaviour {
    public Rigidbody2D rb;
    [SerializeField] private CameraShake cameraShake;

    [SerializeField] public int maxHealth;
    [SerializeField] public int healthPlayer;
    [SerializeField] private float timeInvincible;
    private SpriteRenderer spriteRenderer;
    [SerializeField] private bool isInvincible;
    [SerializeField] private float timerInvincible;

    // Start is called before the first frame update
    public static HealthPlayer Instance;
    public Playercontroller playerController;
    public bool isGameOver;

    private void Awake() {
        Instance = this;
        if (Camera.main != null) cameraShake = Camera.main.GetComponent<CameraShake>();
    }

    private void Start() {
        playerController = GetComponent<Playercontroller>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (NeverDestroy.Instance != null) healthPlayer = NeverDestroy.Instance.life;
        else healthPlayer = maxHealth;
        rb = GetComponent<Rigidbody2D>();
        for (int i = 0; i < healthPlayer; i++) {
            if (UIManager.Instance != null) {
                UIManager.Instance.HeartsLife[i].SetActive(true);
            }
        }
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

                //spriteRenderer.color = Color.red;
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

            healthPlayer -= damage;
            if(cameraShake != null) cameraShake.CreateCameraShake(0.1f, .25f);
            
            if (UIManager.Instance == null) return;
            
            for (int i = UIManager.Instance.HeartsLife.Length - 1; i > -1; i--) {
                if (i >= healthPlayer) UIManager.Instance.HeartsLife[i].SetActive(false);
                else break;
                spriteRenderer.material.SetFloat("_HitTime", Time.time);
                isInvincible = true;
            }
        }
    }

    /// <summary>
    /// Heal the player
    /// </summary>
    /// <param name="heal"></param>
    public void TakeHealPlayer(int heal) {
        healthPlayer += heal;
        for (int i = 0; i < healthPlayer; i++) {
            if (i <= healthPlayer) {
                UIManager.Instance.HeartsLife[i].SetActive(true);
            }
            else {
                break;
            }
        }
    }

    private void OnDeathPlayer() {
        if (GameManager.Instance != null) GameManager.Instance.enabled = false;
        gameObject.SetActive(false);
        if (UIManager.Instance != null) UIManager.Instance.GameOver();
    }

    private void OnTriggerStay2D(Collider2D other) {
        if (other.CompareTag("ShieldEnemy")) TakeDamagePlayer(1);
    }
}