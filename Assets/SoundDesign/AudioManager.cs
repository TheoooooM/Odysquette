using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
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
    
    [Header("----STRAW SOUND")]
    [SerializeField] private StrawSoundData strawSound = null;

    public enum StrawSoundEnum { Shoot, Impact, Explosion };

    [Header("----ENNEMY SOUND")]
    public AudioClip ennemiDeath;
    public AudioClip kodakFlash;
    public AudioClip damageTaken;


    public enum EnemySoundEnum { Death, KodakFlash, Damage };
    
    [Header("----MUSIC SOUND")]
    [SerializeField] private MusicSoundData musicSound = null;

    [Header("----AUDIOSOURCE")]
    [SerializeField] private AudioSource musicAudioSource;
    [SerializeField] private AudioSource sfxAudioSource;
    [SerializeField] private AudioSource calmSfxAudioSource;
    
    [Header("----SOUND DATA")]
    [SerializeField] private float timeBetweentwoExplosions = 0.075f;
    [SerializeField] private float timeBetweentwoDamages = 0.07f;
    [SerializeField] private float timeBetweentwoImpact = 0.07f;
    [Space]
    [SerializeField] private float maxDistanceExplosion = 15;
    [SerializeField] private float maxDistanceImpact = 15;
    
    #endregion VARIABLES

    private GameObject lastDeathGam = null;
    //EXPLOSION
    private bool canExplose = true;
    private float explosionTimer = 0;
    
    //DAMAGE
    private bool canDamage = true;
    private float damageTimer = 0;

    //IMPACT
    private bool canImpact = true;
    private float impactTimer = 0;
    
    private void Update() {
        if (canExplose == false) {
            explosionTimer += Time.deltaTime;
            if (explosionTimer >= timeBetweentwoExplosions) canExplose = true;
        }
        
        if (canDamage == false) {
            damageTimer += Time.deltaTime;
            if (damageTimer >= timeBetweentwoDamages) canDamage = true;
        }
        
        if (canImpact == false) {
            impactTimer += Time.deltaTime;
            if (impactTimer >= timeBetweentwoImpact) canImpact = true;
        }
    }


    public void OnEnable() => UnityEngine.SceneManagement.SceneManager.sceneLoaded += ChangeMusicForNewScene;

    /// <summary>
    /// Change the actual music
    /// </summary>
    /// <param name="scene"></param>
    /// <param name="mode"></param>
    private void ChangeMusicForNewScene(Scene scene, LoadSceneMode mode){
        musicAudioSource.Stop();

        //HUB
        if (musicSound.hubMusic.sceneMusic != null && scene.name.ToUpper() == musicSound.hubMusic.sceneMusic.name.ToUpper() && musicSound.hubMusic.music != null) {
            musicAudioSource.clip = musicSound.hubMusic.music;
            musicAudioSource.Play();
        }
        //TRANSITION
        else if (musicSound.transitionLevelMusic.sceneMusic !=null && scene.name.ToUpper() == musicSound.transitionLevelMusic.sceneMusic.name.ToUpper() && musicSound.transitionLevelMusic.music != null) {
            musicAudioSource.clip = musicSound.transitionLevelMusic.music;
            musicAudioSource.Play();
        }
        //IN LEVEL
        else if (musicSound.inLevelCalmMusic.sceneMusic != null && scene.name.ToUpper() == musicSound.inLevelCalmMusic.sceneMusic.name.ToUpper() && (musicSound.inLevelCalmMusic.music != null && musicSound.inLevelbattleMusic.music != null)) {
            musicAudioSource.clip = musicSound.inLevelCalmMusic.music;
            musicAudioSource.Play();
        }
        //BOSS
        else if (musicSound.bossMusic.sceneMusic !=null && scene.name.ToUpper() == musicSound.bossMusic.sceneMusic.name.ToUpper() && musicSound.bossMusic.music != null) {
            musicAudioSource.clip = musicSound.bossMusic.music;
            musicAudioSource.Play();
        }
    }

    /// <summary>
    /// Play a sound link to a straw/bullet
    /// </summary>
    /// <param name="sound"></param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public void PlayStrawSound(StrawSoundEnum sound, Vector3 pos = new  Vector3()) {
        switch (sound) {
            case StrawSoundEnum.Shoot:
                //if(strawSound.shootPlayer[0] != null) calmSfxAudioSource.PlayOneShot(strawSound.shootPlayer[0], Random.Range(calmSfxAudioSource.volume - .1f, calmSfxAudioSource.volume + .1f));
                break;
            case StrawSoundEnum.Impact:
                if (strawSound.impactShoot[0] != null && canImpact) {
                    float distanceToImpact = Mathf.Abs(Vector3.Distance(pos, Playercontroller.Instance.transform.position));
                    float distanceSubstract = Mathf.Clamp(maxDistanceImpact - distanceToImpact, 0.05f, maxDistanceImpact);
                    float impactRatio = distanceSubstract / maxDistanceImpact;
                    
                    calmSfxAudioSource.PlayOneShot(strawSound.impactShoot[0], Random.Range(calmSfxAudioSource.volume - .1f, calmSfxAudioSource.volume) * impactRatio);
                                        
                    canImpact = false;
                    impactTimer = 0;
                    
                }
                break;
            case StrawSoundEnum.Explosion:
                //Base the volume on the distance of the explosion
                if (strawSound.explosion[0] != null && canExplose == true) {
                    float distanceToExplosion = Mathf.Abs(Vector3.Distance(pos, Playercontroller.Instance.transform.position));
                    float distanceSubstract = Mathf.Clamp(maxDistanceExplosion - distanceToExplosion, 0.05f, maxDistanceExplosion);
                    float explosionRatio = distanceSubstract / maxDistanceExplosion;
                    
                    calmSfxAudioSource.PlayOneShot(strawSound.explosion[0], Random.Range(calmSfxAudioSource.volume - .2f, calmSfxAudioSource.volume - .1f) * explosionRatio);
                    
                    canExplose = false;
                    explosionTimer = 0;
                }
                break;
            default: throw new ArgumentOutOfRangeException(nameof(sound), sound, null);
        }
    }

    /// <summary>
    /// Play a sound link to an enemy
    /// </summary>
    /// <param name="sound"></param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public void PlayEnemySound(EnemySoundEnum sound, GameObject objectCall) {
        switch (sound) {
            case EnemySoundEnum.Death:
                if(ennemiDeath != null && lastDeathGam != objectCall) sfxAudioSource.PlayOneShot(ennemiDeath);
                lastDeathGam = objectCall;
                Debug.LogWarning("death");
                break;
            case EnemySoundEnum.KodakFlash:
                break;
            case EnemySoundEnum.Damage:
                if (damageTaken != null && canDamage) {
                    calmSfxAudioSource.PlayOneShot(damageTaken, Random.Range(calmSfxAudioSource.volume, calmSfxAudioSource.volume + .1f));
                                        
                    canDamage = false;
                    damageTimer = 0;
                }
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(sound), sound, null);
        }
    }
}

[System.Serializable]
public class StrawSoundData {
    public List<AudioClip> shootPlayer = new List<AudioClip>();
    public List<AudioClip> impactShoot = new List<AudioClip>();
    public List<AudioClip> explosion = new List<AudioClip>();
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
    public SceneAsset sceneMusic;
    public AudioClip music = null;
}