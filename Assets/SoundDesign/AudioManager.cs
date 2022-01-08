using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;
using Vector3 = UnityEngine.Vector3;

public class AudioManager : MonoBehaviour {
    #region VARIABLES

    #region INSTANCE

    private static AudioManager instance = null;
    public static AudioManager Instance => instance;

    private void Awake() {
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    #endregion INSTANCE

    [Header("----Player SOUND")] [SerializeField]
    private PlayerSoundData playerSound = null;

    [Header("----STRAW SOUND")] [SerializeField]
    private StrawSoundData strawSound = null;


    public enum StrawSoundEnum {
        BasicShoot,
        BubbleShoot,
        SniperShoot,
        Impact,
        Explosion,
        Pierce,
        Bounce,
        Poison
    };
    
    public enum BossSoundEnum     {ShootBoss, ShootTurret, ShieldOn, ShieldOff}

    public enum PlayerSoundEnum {
        Move,
        Death,
        Damage,
        Dash,
        Fall,
        TakeItem,
        OpenChest
    }

    [Header("----ENNEMY SOUND")] public AudioClip ennemiDeath;
    public AudioClip kodakFlash;
    [SerializeField] private AudioClip carDash;
    [SerializeField] private AudioClip walkmanShoot;
    [SerializeField] private AudioClip hologramTP;
    [SerializeField] private AudioClip hologramShoot;

    [Header("----BOSS SOUND")] [SerializeField] AudioClip cancelShieldBoss;
     [SerializeField] AudioClip activateShieldBoss;
     [SerializeField] private AudioClip shootBoss;
     [SerializeField] private AudioClip shootTurret;
    
    public enum EnemySoundEnum {
        Death,
        KodakFlash,
        CarDash,
        WalkmanShoot, 
        HologramTP,
        HologramShoot
   
    };

    [Header("----MUSIC SOUND")] [SerializeField]
    private MusicSoundData musicSound = null;

    [Header("----AUDIOSOURCE")] [SerializeField]
    private AudioSource musicAudioSource;

    [SerializeField] private AudioSource sfxAudioSource;
    [SerializeField] private AudioSource calmSfxAudioSource;
    public AudioSource playerMovementAudioSource;

    [Header("----SOUND DATA")] [SerializeField]
    private float timeBetweentwoExplosions = 0.075f;

    [SerializeField] private float delayTimeShootBoss;
    [SerializeField] private float timeBetweentwoDamages = 0.07f;
    [SerializeField] private float timeBetweentwoImpact = 0.07f;
    [SerializeField] private float timeBetweentwoPoison = 0.07f;
    [SerializeField] private float timeBetweentwoBounce = 0.07f;
    [SerializeField] private float timeBetweentwoPierce = 0.07f;
    [SerializeField] private float timeBetweenTwoShoot = 0.1f;
    [Space] [SerializeField] private float maxDistanceExplosion = 15;
    [SerializeField] private float maxDistanceImpact = 15;
    [SerializeField] private float maxDistanceBounce = 35;
    [SerializeField] private float maxDistancePierce = 35;
    [SerializeField] private float maxDistancePoison = 45;

    #endregion VARIABLES

    private GameObject lastDeathGam = null;


    //DAMAGE
    private bool canDamage = true;
    private float damageTimer = 0;

    //IMPACT
    private bool canImpact = true;
    private bool canExplosion = true;
    private bool canBounce = true;
    private bool canPierce = true;
    private bool canPoison = true;
    private float impactTimer = 0;
    private float poisonTimer = 0;
    private float pierceTimer = 0;
    private float bounceTimer = 0;
    private float explosionTimer = 0;



    //SHOOT
    private bool canShoot = true;
    private float shootTimer;

    private void Update() {
        if (canExplosion == false) {
            explosionTimer += Time.deltaTime;
            if (explosionTimer >= timeBetweentwoExplosions) canExplosion = true;
        }

        if (canDamage == false) {
            damageTimer += Time.deltaTime;
            if (damageTimer >= timeBetweentwoDamages) canDamage = true;
        }

        if (canImpact == false) {
            impactTimer += Time.deltaTime;
            if (impactTimer >= timeBetweentwoImpact) canImpact = true;
        }

        if (canPoison == false) {
            poisonTimer += Time.deltaTime;
            if (poisonTimer >= timeBetweentwoPoison) canPoison = true;
        }

        if (canBounce == false) {
            bounceTimer += Time.deltaTime;
            if (bounceTimer >= timeBetweentwoBounce) canBounce = true;
        }

        if (canPierce == false) {
            pierceTimer += Time.deltaTime;
            if (pierceTimer >= timeBetweentwoPierce) canPierce = true;
        }

        if (!canShoot) {
            shootTimer += Time.deltaTime;
            if (shootTimer >= timeBetweenTwoShoot) canShoot = true;
        }
    }


    public void OnEnable() => UnityEngine.SceneManagement.SceneManager.sceneLoaded += ChangeMusicForNewScene;

    /// <summary>
    /// Change the actual music
    /// </summary>
    /// <param name="scene"></param>
    /// <param name="mode"></param>
    private void ChangeMusicForNewScene(Scene scene, LoadSceneMode mode) {
        if (musicAudioSource == null) return;

        musicAudioSource.Stop();

        //HUB
        if (musicSound.hubMusic.sceneMusicName != "" && scene.name.ToUpper() == musicSound.hubMusic.sceneMusicName.ToUpper() && musicSound.hubMusic.music != null) {
            musicAudioSource.clip = musicSound.hubMusic.music;
            musicAudioSource.Play();
        }
        //TRANSITION
        else if (musicSound.transitionLevelMusic.sceneMusicName != "" && scene.name.ToUpper() == musicSound.transitionLevelMusic.sceneMusicName.ToUpper() && musicSound.transitionLevelMusic.music != null) {
            musicAudioSource.clip = musicSound.transitionLevelMusic.music;
            musicAudioSource.Play();
        }
        //IN LEVEL
        else if (musicSound.inLevelCalmMusic.sceneMusicName != "" && scene.name.ToUpper() == musicSound.inLevelCalmMusic.sceneMusicName.ToUpper() && (musicSound.inLevelCalmMusic.music != null && musicSound.inLevelbattleMusic.music != null)) {
            musicAudioSource.clip = musicSound.inLevelCalmMusic.music;
            musicAudioSource.Play();
        }
        //BOSS
        else if (musicSound.bossMusic.sceneMusicName != "" && scene.name.ToUpper() == musicSound.bossMusic.sceneMusicName.ToUpper() && musicSound.bossMusic.music != null) {
            musicAudioSource.clip = musicSound.bossMusic.music;
            musicAudioSource.Play();
        }
    }

    public void PlayPlayerSound(PlayerSoundEnum sound) {
        switch (sound) {
            case PlayerSoundEnum.Move:{
                if (!playerMovementAudioSource.isPlaying) {
                    playerMovementAudioSource.PlayOneShot(playerSound.moveSound);
                }
            }
                break;
            case PlayerSoundEnum.Death:{
                calmSfxAudioSource.PlayOneShot(playerSound.deathSound);
            }
                break;
            case PlayerSoundEnum.Damage:{
                calmSfxAudioSource.PlayOneShot(playerSound.damageSound);
            }
                break;
            case PlayerSoundEnum.Dash:{
                calmSfxAudioSource.PlayOneShot(playerSound.dashSound, calmSfxAudioSource.volume * .5f);
            }
                break;
            case PlayerSoundEnum.Fall:{
                calmSfxAudioSource.PlayOneShot(playerSound.fallSound);
            }
                break;
            case PlayerSoundEnum.TakeItem:{
                calmSfxAudioSource.PlayOneShot(playerSound.takeItem);
            }
                break;
            case PlayerSoundEnum.OpenChest:{
                calmSfxAudioSource.PlayOneShot(playerSound.openChest);
            }
                break;
        }
    }

    public void PlayShootStraw(StrawSoundEnum sound, float soundScale, Vector3 pos = new Vector3()) {
        if (canShoot) {
            switch (sound) {
                case StrawSoundEnum.BasicShoot:
                    calmSfxAudioSource.PlayOneShot(strawSound.basicShoot[Random.Range(0, strawSound.basicShoot.Count - 1)], soundScale * calmSfxAudioSource.volume);

                    break;
                case StrawSoundEnum.BubbleShoot:
                    calmSfxAudioSource.PlayOneShot(strawSound.bubbleShoot[Random.Range(0, strawSound.bubbleShoot.Count - 1)], soundScale * calmSfxAudioSource.volume);
                    Debug.Log("test st jqdfjqsf bublek");
                    break;
                case StrawSoundEnum.SniperShoot:
                    calmSfxAudioSource.PlayOneShot(strawSound.snipShoot, soundScale * calmSfxAudioSource.volume);
                    break;

                default: throw new ArgumentOutOfRangeException(nameof(sound), sound, null);
            }
        }
    }

    public void PlayBossSound(BossSoundEnum sound)
    {
        switch (sound)
        {
            case BossSoundEnum.ShieldOff:
            {
                calmSfxAudioSource.PlayOneShot(cancelShieldBoss,  calmSfxAudioSource.volume);
            }
                break;
            case BossSoundEnum.ShieldOn:
            {
                calmSfxAudioSource.PlayOneShot(activateShieldBoss,  calmSfxAudioSource.volume);
            }
                break;
            case BossSoundEnum.ShootBoss:
            {
                StartCoroutine(PlaySoundDelay(delayTimeShootBoss, shootBoss, calmSfxAudioSource));
               
            }
                break;
            case BossSoundEnum.ShootTurret:
            {
                calmSfxAudioSource.PlayOneShot(shootTurret,  calmSfxAudioSource.volume);
            }
                break;
            
            
        }
    }

    IEnumerator PlaySoundDelay(float delay, AudioClip audioClip, AudioSource audioSource)
    {
        yield return new WaitForSeconds(delay); 
        audioSource.PlayOneShot(audioClip);
    }


    private void ImpactSound(AudioClip _impact, bool _canImpact, float _maxDistanceSound, Vector3 pos = new Vector3()) {
        if (_impact != null && _canImpact) {
            float distanceToImpact =
                Mathf.Abs(Vector3.Distance(pos, Playercontroller.Instance.transform.position));
            float distanceSubstract =
                Mathf.Clamp(_maxDistanceSound - distanceToImpact, 0.05f, _maxDistanceSound);
            float impactRatio = distanceSubstract / maxDistanceImpact;

            calmSfxAudioSource.PlayOneShot(_impact,
                Random.Range(calmSfxAudioSource.volume - .1f, calmSfxAudioSource.volume) * impactRatio);
        }
    }

    public void PlayImpactStraw(StrawSoundEnum sound, Vector3 pos = new Vector3()) {
        switch (sound) {
            case StrawSoundEnum.Impact:{
                if (GameManager.Instance.firstEffect == GameManager.Effect.none &&
                    GameManager.Instance.secondEffect == GameManager.Effect.none) {
                    ImpactSound(strawSound.basicImpact, canImpact, maxDistanceImpact);
                    canImpact = false;
                    impactTimer = 0;
                }
            }

                break;
            case StrawSoundEnum.Explosion:
                //Base the volume on the distance of the explosion

                if (strawSound.explosion[0] != null && canExplosion == true) {
                    float distanceToExplosion =
                        Mathf.Abs(Vector3.Distance(pos, Playercontroller.Instance.transform.position));
                    float distanceSubstract = Mathf.Clamp(maxDistanceExplosion - distanceToExplosion, 0.05f,
                        maxDistanceExplosion);
                    float explosionRatio = distanceSubstract / maxDistanceExplosion;
                    calmSfxAudioSource.PlayOneShot(strawSound.explosion[0],
                        Random.Range(calmSfxAudioSource.volume - .2f, calmSfxAudioSource.volume - .1f) *
                        explosionRatio);
                    canExplosion = false;
                    explosionTimer = 0;
                }

                break;


            case StrawSoundEnum.Poison:
                //Base the volume on the distance of the explosion
                if (GameManager.Instance.firstEffect == GameManager.Effect.explosive ||
                    GameManager.Instance.secondEffect == GameManager.Effect.explosive) {
                    if (strawSound.poisonImpact != null && canPoison == true) {
                        ImpactSound(strawSound.poisonImpact, canPoison, maxDistancePoison);
                        canPoison = false;
                        poisonTimer = 0;
                    }
                }

                break;

            case StrawSoundEnum.Bounce:
                //Base the volume on the distance of the explosion
                if (GameManager.Instance.firstEffect == GameManager.Effect.explosive ||
                    GameManager.Instance.secondEffect == GameManager.Effect.explosive ||
                    GameManager.Instance.secondEffect == GameManager.Effect.poison ||
                    GameManager.Instance.firstEffect == GameManager.Effect.poison) {
                    if (strawSound.bounceImpact != null && canBounce == true) {
                        ImpactSound(strawSound.bounceImpact, canBounce, maxDistanceBounce);
                        canBounce = false;
                        bounceTimer = 0;
                    }
                }

                break;

            case StrawSoundEnum.Pierce:
                //Base the volume on the distance of the explosion
                if (GameManager.Instance.firstEffect == GameManager.Effect.explosive ||
                    GameManager.Instance.secondEffect == GameManager.Effect.explosive ||
                    GameManager.Instance.secondEffect == GameManager.Effect.poison ||
                    GameManager.Instance.firstEffect == GameManager.Effect.poison) {
                    if (strawSound.pierceImpact != null && canPierce == true) {
                        ImpactSound(strawSound.pierceImpact, canPierce, maxDistancePierce);
                        canPierce = false;
                        explosionTimer = 0;
                    }
                }

                break;


            default: throw new ArgumentOutOfRangeException(nameof(sound), sound, null);
        }
    }


  public void  PlayEnemyDeathSound(EnemySoundEnum sound, GameObject objectCall)
    {
        switch (sound)
        {
            case EnemySoundEnum.Death:
                if (ennemiDeath != null && lastDeathGam != objectCall) sfxAudioSource.PlayOneShot(ennemiDeath);
                lastDeathGam = objectCall;
                Debug.LogWarning("death");
                break;
        }
    }
    /// <summary>
    /// Play a sound link to an enemy
    /// </summary>
    /// <param name="sound"></param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public void PlayEnemySound(EnemySoundEnum sound) {
        switch (sound) {
            case EnemySoundEnum.KodakFlash:
                sfxAudioSource.PlayOneShot(kodakFlash);
                break;
            case EnemySoundEnum.WalkmanShoot:
                sfxAudioSource.PlayOneShot(walkmanShoot);
                break;
            case EnemySoundEnum.CarDash:
                sfxAudioSource.PlayOneShot(carDash);
                break;
            case EnemySoundEnum.HologramTP:
                sfxAudioSource.PlayOneShot(hologramTP);
                break;
            case EnemySoundEnum.HologramShoot:
                sfxAudioSource.PlayOneShot(hologramShoot);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(sound), sound, null);
        }
    }
}

[System.Serializable]
public class StrawSoundData {
    public List<AudioClip> basicShoot = new List<AudioClip>();
    public List<AudioClip> bubbleShoot = new List<AudioClip>();
    public AudioClip snipShoot;
    public AudioClip basicImpact;
    public AudioClip poisonImpact;
    public AudioClip pierceImpact;
    public AudioClip bounceImpact;
    public List<AudioClip> explosion = new List<AudioClip>();
}

[System.Serializable]
public class PlayerSoundData {
    public AudioClip moveSound;
    public AudioClip damageSound;
    public AudioClip deathSound;
    public AudioClip dashSound;
    public AudioClip fallSound;
    public AudioClip openChest;
    public AudioClip takeItem;
}


[System.Serializable]
public class MusicSoundData {
    public MusicSoundBaseClass hubMusic = null;
    public MusicSoundBaseClass transitionLevelMusic = null;
    public MusicSoundBaseClass inLevelCalmMusic = null;
    public MusicSoundBaseClass inLevelbattleMusic = null;
    public MusicSoundBaseClass bossMusic = null;
}

[System.Serializable]
public class MusicSoundBaseClass {
    public string sceneMusicName;
    public AudioClip music = null;
}