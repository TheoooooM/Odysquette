using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStateManager : EnemyStateManager
{
    private Collider2D shieldCollider; 
    public override void Start()
    {
        shieldCollider = BossManager.instance.shieldBoss.GetComponent<Collider2D>();
        base.Start();
        
    }

  

    public override void TakeDamage(float damage, Vector2 position, float knockUpValue, bool knockup, bool isExplosion)
   {
       if (!shieldCollider.enabled && BossManager.instance.currentBossPhase != ExtensionMethods.PhaseBoss.Begin)
       {
             base.TakeDamage(damage, position, knockUpValue, knockup, isExplosion);
                 BossManager.instance.UpdateSlider(health);
       }
    
   }
   
}
