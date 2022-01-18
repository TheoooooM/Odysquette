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
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
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
    
    public enum BossSoundEnum     {ShootBoss, ShootTurret, ShieldOn, ShieldOff, DashBoss}

    public enum PlayerSoundEnum {
        Move,
        Death,
        Damage,
        Dash,
        Fall,
        TakeItem,
        OpenChest,
        Drone, 
        UltimateReady,
        Ressources,
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
     [SerializeField] private AudioClip dashBoss;
    
    public enum EnemySoundEnum {
        Death,
        KodakFlash,
        CarDash,
        WalkmanShoot, 
        HologramTP,
        HologramShoot
   
    };
    public enum UISoundEnum {OpenSound, CloseSound}

    [Header("----UI SOUND")] [SerializeField]
    private AudioClip closeSound;

     [SerializeField]private AudioClip openSound;

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
    [SerializeField] float maxDroneSound;
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

    private float sfxSoundMultiplier = 0;
    public float SfxSoundMultiplier => sfxSoundMultiplier;
    private float musicSoundMultiplier = 0;

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

        sfxSoundMultiplier = Mathf.Clamp(PlayerPrefs.GetFloat("sfx", .2f) * .7f, 0, 1);
        musicSoundMultiplier = Mathf.Clamp(PlayerPrefs.GetFloat("music", .2f) * .75f, 0, 1);
        musicAudioSource.volume = musicSoundMultiplier;
    }


    public void OnEnable() => UnityEngine.SceneManagement.SceneManager.sceneLoaded += ChangeMusicForNewScene;

    /// <summary>
    /// Change the actual music
    /// </summary>
    /// <param name="scene"></param>
    /// <param name="mode"></param>
    private void ChangeMusicForNewScene(Scene scene, LoadSceneMode mode) {
        if (musicAudioSource == null) return;

        //HUB
        if (musicSound.hubMusic.sceneMusicName != "" && scene.name.ToUpper() == musicSound.hubMusic.sceneMusicName.ToUpper() && musicSound.hubMusic.music != null) {
            musicAudioSource.Stop();
            musicAudioSource.clip = musicSound.hubMusic.music;
            musicAudioSource.Play();
        }
        //TRANSITION
        else if (musicSound.transitionLevelMusic.sceneMusicName != "" && scene.name.ToUpper() == musicSound.transitionLevelMusic.sceneMusicName.ToUpper() && musicSound.transitionLevelMusic.music != null) {
            if (musicAudioSource.clip == musicSound.transitionLevelMusic.music) return;
            musicAudioSource.clip = musicSound.transitionLevelMusic.music;
            musicAudioSource.Play();
        }
        //SHOP
        else if (musicSound.shopMusic.sceneMusicName != "" && scene.name.ToUpper() == musicSound.shopMusic.sceneMusicName.ToUpper() && musicSound.shopMusic.music != null) {
            musicAudioSource.Stop();
            musicAudioSource.clip = musicSound.shopMusic.music;
            musicAudioSource.Play();
        }
        //IN LEVEL
        else if (musicSound.inLevelCalmMusic.sceneMusicName != "" && scene.name.ToUpper() == musicSound.inLevelCalmMusic.sceneMusicName.ToUpper() && musicSound.inLevelCalmMusic.music != null) {
            musicAudioSource.Stop();
            musicAudioSource.clip = musicSound.inLevelCalmMusic.music;
            musicAudioSource.Play();
        }
        else if (musicSound.inLevelbattleMusic.sceneMusicName != "" && scene.name.ToUpper() == musicSound.inLevelbattleMusic.sceneMusicName.ToUpper() && musicSound.inLevelbattleMusic.music != null) {
            musicAudioSource.Stop();
            musicAudioSource.clip = musicSound.inLevelbattleMusic.music;
            musicAudioSource.Play();
        }
        //BOSS
        else if (musicSound.bossMusic.sceneMusicName != "" && scene.name.ToUpper() == musicSound.bossMusic.sceneMusicName.ToUpper() && musicSound.bossMusic.music != null) {
            musicAudioSource.Stop();
            musicAudioSource.clip = musicSound.bossMusic.music;
            musicAudioSource.Play();
        }
    }

    public void  PlayUISound(UISoundEnum sound) {
        switch (sound) {
            case  UISoundEnum.OpenSound : {
                playerMovementAudioSource.PlayOneShot(openSound, calmSfxAudioSource.volume * sfxSoundMultiplier);
            }
                break;
            
            case  UISoundEnum.CloseSound : {
                playerMovementAudioSource.PlayOneShot(closeSound, calmSfxAudioSource.volume * sfxSoundMultiplier);
            } 
                break;
        }
    }
    
    public void PlayPlayerSound(PlayerSoundEnum sound) {
        switch (sound) {
            case PlayerSoundEnum.Move:{
                if (!playerMovementAudioSource.isPlaying) {
                    playerMovementAudioSource.PlayOneShot(playerSound.moveSound, playerMovementAudioSource.volume * .25f * sfxSoundMultiplier);
                }
            }
                break;
            case PlayerSoundEnum.Ressources:
            {
                int rand = Random.Range(0, playerSound.ressourcesSoundList.Length);
             calmSfxAudioSource.PlayOneShot(playerSound.ressourcesSoundList[rand], playerMovementAudioSource.volume  * sfxSoundMultiplier * .25f);
                }
                break;
            case PlayerSoundEnum.Death:{
                calmSfxAudioSource.PlayOneShot(playerSound.deathSound, calmSfxAudioSource.volume * sfxSoundMultiplier);
            }
                break;
            case PlayerSoundEnum.Damage:{
                calmSfxAudioSource.PlayOneShot(playerSound.damageSound, calmSfxAudioSource.volume * sfxSoundMultiplier * .5f);
            }
                break;
            case PlayerSoundEnum.Dash:{
                calmSfxAudioSource.PlayOneShot(playerSound.dashSound, calmSfxAudioSource.volume * 1 * sfxSoundMultiplier);
            }
                break;
            case PlayerSoundEnum.Fall:{
                calmSfxAudioSource.PlayOneShot(playerSound.fallSound, calmSfxAudioSource.volume * sfxSoundMultiplier * .1f);
            }
                break;
            case PlayerSoundEnum.TakeItem:{
                calmSfxAudioSource.PlayOneShot(playerSound.takeItem, calmSfxAudioSource.volume * sfxSoundMultiplier * .75f);
            }
                break;
            case PlayerSoundEnum.OpenChest:{
                calmSfxAudioSource.PlayOneShot(playerSound.openChest, calmSfxAudioSource.volume * sfxSoundMultiplier * .65f);
            }
                break;
            
            case PlayerSoundEnum.Drone:{
                float distanceToDrone = Mathf.Abs(Vector3.Distance(DroneManager.Instance.transform.position, Playercontroller.Instance.transform.position));
                float distanceSubstract = Mathf.Clamp(maxDroneSound - distanceToDrone, 0.05f, maxDroneSound);
                float droneRatio = distanceSubstract / maxDroneSound;
                calmSfxAudioSource.PlayOneShot(playerSound.droneSound, droneRatio * maxDroneSound * 0.01f * sfxSoundMultiplier);
            }
                break;
            case PlayerSoundEnum.UltimateReady:{
                calmSfxAudioSource.PlayOneShot(playerSound.ultimateReadySound, calmSfxAudioSource.volume * sfxSoundMultiplier);
            }
                break;
        }
    }

    public void PlayShootStraw(StrawSoundEnum sound, float soundScale, Vector3 pos = new Vector3()) {
        if (canShoot) {
            switch (sound) {
                case StrawSoundEnum.BasicShoot:
                    calmSfxAudioSource.PlayOneShot(strawSound.basicShoot[Random.Range(0, strawSound.basicShoot.Count - 1)], soundScale * calmSfxAudioSource.volume * .375f * sfxSoundMultiplier);
                    break;
                case StrawSoundEnum.BubbleShoot:
                    calmSfxAudioSource.PlayOneShot(strawSound.bubbleShoot[Random.Range(0, strawSound.bubbleShoot.Count - 1)], soundScale * calmSfxAudioSource.volume * .375f * sfxSoundMultiplier);
                    Debug.Log("test st jqdfjqsf bublek");
                    break;
                case StrawSoundEnum.SniperShoot:
                    calmSfxAudioSource.PlayOneShot(strawSound.snipShoot, soundScale * calmSfxAudioSource.volume * .375f * sfxSoundMultiplier);
                    break;

                default: throw new ArgumentOutOfRangeException(nameof(sound), sound, null);
            }
        }
    }

    public void PlayBossSound(BossSoundEnum sound) {
        switch (sound) {
            case BossSoundEnum.ShieldOff:{
                calmSfxAudioSource.PlayOneShot(cancelShieldBoss,  calmSfxAudioSource.volume * sfxSoundMultiplier * 85f);
            }
                break;
            case BossSoundEnum.ShieldOn:{
                calmSfxAudioSource.PlayOneShot(activateShieldBoss,  calmSfxAudioSource.volume * sfxSoundMultiplier * 85f);
            }
                break;
            case BossSoundEnum.ShootBoss:{
                StartCoroutine(PlaySoundDelay(delayTimeShootBoss, shootBoss, calmSfxAudioSource));
            }
                break;
            case BossSoundEnum.ShootTurret:{
                calmSfxAudioSource.PlayOneShot(shootTurret,  calmSfxAudioSource.volume * sfxSoundMultiplier * 85f);
            }
                break;
            case BossSoundEnum.DashBoss:{
                calmSfxAudioSource.PlayOneShot(shootTurret,  calmSfxAudioSource.volume * (sfxSoundMultiplier -0.2f)  * 85f);
            }
                break;
        }
    }

    IEnumerator PlaySoundDelay(float delay, AudioClip audioClip, AudioSource audioSource)
    {
        yield return new WaitForSeconds(delay); 
        audioSource.PlayOneShot(audioClip, audioSource.volume * sfxSoundMultiplier);
    }
    
    private void ImpactSound(AudioClip _impact, bool _canImpact, float _maxDistanceSound, Vector3 pos = new Vector3()) {
        if (_impact != null && _canImpact) {
            float distanceToImpact = Mathf.Abs(Vector3.Distance(pos, Playercontroller.Instance.transform.position));
            float distanceSubstract = Mathf.Clamp(_maxDistanceSound - distanceToImpact, 0.05f, _maxDistanceSound);
            float impactRatio = distanceSubstract / maxDistanceImpact;

            calmSfxAudioSource.PlayOneShot(_impact, Random.Range(calmSfxAudioSource.volume, calmSfxAudioSource.volume + .1f) * impactRatio * sfxSoundMultiplier);
        }
    }

    public void PlayImpactStraw(StrawSoundEnum sound, Vector3 pos = new Vector3()) {
        switch (sound) {
            case StrawSoundEnum.Impact:{
                if (GameManager.Instance.firstEffect == GameManager.Effect.none && GameManager.Instance.secondEffect == GameManager.Effect.none) {
                    ImpactSound(strawSound.basicImpact, canImpact, maxDistanceImpact);
                    canImpact = false;
                    impactTimer = 0;
                }
            }

                break;
            case StrawSoundEnum.Explosion:
                //Base the volume on the distance of the explosion

                if (strawSound.explosion[0] != null && canExplosion == true) {
                    float explosionRatio = GetRatio(pos, maxDistancePoison);
                    calmSfxAudioSource.PlayOneShot(strawSound.explosion[0], Random.Range(calmSfxAudioSource.volume - .2f, calmSfxAudioSource.volume - .1f) * explosionRatio * .85f * sfxSoundMultiplier);
                    canExplosion = false;
                    explosionTimer = 0;
                }

                break;


            case StrawSoundEnum.Poison:
                //Base the volume on the distance of the poison
                    if (strawSound.poisonImpact != null && canPoison) {
                        float poisonRatio = GetRatio(pos, maxDistancePoison);
                        calmSfxAudioSource.PlayOneShot(strawSound.poisonImpact, Random.Range(calmSfxAudioSource.volume - .2f, calmSfxAudioSource.volume - .1f) * poisonRatio * .75f * sfxSoundMultiplier);
                        canPoison = false;
                        poisonTimer = 0;
                    }
                break;

            case StrawSoundEnum.Bounce:
                if (GameManager.Instance.HasEffect(GameManager.Effect.poison)) return;
                if (GameManager.Instance.HasEffect(GameManager.Effect.explosive)) return;
                
                //Base the volume on the distance of the bounce
                    if (strawSound.bounceImpact != null && canBounce == true) {
                        float bounceRatio = GetRatio(pos, maxDistanceBounce);
                        calmSfxAudioSource.PlayOneShot(strawSound.bounceImpact, Random.Range(calmSfxAudioSource.volume - .2f, calmSfxAudioSource.volume - .1f) * bounceRatio * .75f * sfxSoundMultiplier);
                        canBounce = false;
                        bounceTimer = 0;
                    }
                break;

            case StrawSoundEnum.Pierce:
                //Base the volume on the distance of the pierce
                    if (strawSound.pierceImpact != null && canPierce == true) {
                        float pierceRatio = GetRatio(pos, maxDistanceBounce);
                        calmSfxAudioSource.PlayOneShot(strawSound.pierceImpact, Random.Range(calmSfxAudioSource.volume - .2f, calmSfxAudioSource.volume - .1f) * pierceRatio * .75f * sfxSoundMultiplier);
                        canPierce = false;
                        explosionTimer = 0;
                    }
                break;


            default: throw new ArgumentOutOfRangeException(nameof(sound), sound, null);
        }
    }

    /// <summary>
    /// Get the ratio of the bullet distance over maxdistance
    /// </summary>
    /// <param name="bulletPos"></param>
    /// <param name="maxDistance"></param>
    /// <returns></returns>
    private float GetRatio(Vector3 bulletPos, float maxDistance) {
        float distanceToPlayer = Mathf.Abs(Vector3.Distance(bulletPos, Playercontroller.Instance.transform.position));
        float distanceSubstract = Mathf.Clamp(maxDistance - distanceToPlayer, 0.05f, maxDistance);
        return distanceSubstract / maxDistance;
    }
    
    public void  PlayEnemyDeathSound(EnemySoundEnum sound, GameObject objectCall) {
        switch (sound) {
            case EnemySoundEnum.Death:
                if (ennemiDeath != null && lastDeathGam != objectCall) sfxAudioSource.PlayOneShot(ennemiDeath, sfxAudioSource.volume * sfxSoundMultiplier * .35f);
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
                sfxAudioSource.PlayOneShot(kodakFlash, sfxAudioSource.volume * sfxSoundMultiplier);
                break;
            case EnemySoundEnum.WalkmanShoot:
                sfxAudioSource.PlayOneShot(walkmanShoot, sfxAudioSource.volume * sfxSoundMultiplier * .55f);
                break;
            case EnemySoundEnum.CarDash:
                sfxAudioSource.PlayOneShot(carDash, sfxAudioSource.volume * sfxSoundMultiplier * .215f);
                break;
            case EnemySoundEnum.HologramTP:
                sfxAudioSource.PlayOneShot(hologramTP, sfxAudioSource.volume * sfxSoundMultiplier * .75f);
                break;
            case EnemySoundEnum.HologramShoot:
                sfxAudioSource.PlayOneShot(hologramShoot, sfxAudioSource.volume * sfxSoundMultiplier * .75f);
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
    public AudioClip droneSound;
    public AudioClip ultimateReadySound;
    public AudioClip[] ressourcesSoundList;
}


[System.Serializable]
public class MusicSoundData {
    public MusicSoundBaseClass hubMusic = null;
    public MusicSoundBaseClass transitionLevelMusic = null;
    public MusicSoundBaseClass inLevelCalmMusic = null;
    public MusicSoundBaseClass shopMusic = null;
    public MusicSoundBaseClass inLevelbattleMusic = null;
    public MusicSoundBaseClass bossMusic = null;
}

[System.Serializable]
public class MusicSoundBaseClass {
    public string sceneMusicName;
    public AudioClip music = null;
}