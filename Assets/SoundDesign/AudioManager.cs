using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public AudioSource audioSource;

    public AudioClip shootPlayer;
    public AudioClip impactShoot;
    public AudioClip explosion;
    public AudioClip ennemiDeath;
    public AudioClip kodakFlash;
    public AudioClip damageTaken;
    

    private void Awake()
    {
        instance = this;
    }

    public void PlaySoundEffect(int index)
    {
        if (index == 1)
        {
            Debug.Log("Shoot Player SFX");
            audioSource.PlayOneShot(shootPlayer,Random.Range(0.8f,1));
        }
        else if (index == 2)
        {
            Debug.Log("Shoot Player SFX");
            audioSource.PlayOneShot(impactShoot,Random.Range(0.9f,1));
        }
        else if (index == 3)
        {
            Debug.Log("Shoot Player SFX");
            audioSource.PlayOneShot(explosion,Random.Range(0.9f,1));
        }
        else if (index == 4)
        {
            Debug.Log("Shoot Player SFX");
            audioSource.PlayOneShot(ennemiDeath,Random.Range(0.9f,1));
        }
        else if (index == 5)
        {
            Debug.Log("Shoot Player SFX");
            audioSource.PlayOneShot(damageTaken,Random.Range(0.9f,1));
        }
        else if (index == 6)
        {
            Debug.Log("Shoot Player SFX");
            audioSource.PlayOneShot(kodakFlash,Random.Range(0.9f,1));
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            PlaySoundEffect(3);
        }
    }
}
