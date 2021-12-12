using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerColliderWave : MonoBehaviour
{
    private ParticleSystem ps;
 
    private void Start()
    {
        
        ps = GetComponent<ParticleSystem>();
     // ps.trigger.AddCollider(HealthPlayer.Instance.waveCollider); 
      
    }

    void OnParticleTrigger()
    {   List<ParticleSystem.Particle> enter = new List<ParticleSystem.Particle>();
       
        int numEnter = ps.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, enter);
        Debug.Log(enter.Count); 
        if (numEnter != 0)
        {
            Debug.Log("alélouia");
              HealthPlayer.Instance.TakeDamagePlayer(GetComponent<EnemyBullet>().damage);
        }
      
        
    }
}
