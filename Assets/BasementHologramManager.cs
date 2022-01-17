using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasementHologramManager : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer firstBasement;
    [SerializeField]
    private SpriteRenderer secondBasement;
    [SerializeField]
    private EnemyStateManager hologram;
    private float currentHealth;
 
    private Vector3 currentPosition;
    [SerializeField]   private int index;

    private void Start()
    {
        currentHealth = hologram.health;
    }

    void Update()
    {
        if (currentHealth != hologram.health)
        {
            switch (index)
            {
                case 0:
                {
                    firstBasement.material.SetFloat("_HitTime", Time.time);
                    break;
                }
                case 1:
                {
                  secondBasement.material.SetFloat("_HitTime", Time.time);
                    break;
                }
            }
            currentHealth = hologram.health;
        }

        if (transform.position != currentPosition)
        {
            switch (index)
            {
                case 0:
                {
                    index = 1;
                    break;
                }
                case 1:
                {
                    index = 0;
                    break;
                }
            }
            currentPosition = transform.position;
        }
    }
}
