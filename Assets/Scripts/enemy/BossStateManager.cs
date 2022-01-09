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

    public override void OnDeath(bool byFall = false)
    {
        base.OnDeath(byFall);

        if ((NeverDestroy.Instance.minute + NeverDestroy.Instance.second) != 0) GameManager.Instance.Score += (20 * 60 / (NeverDestroy.Instance.minute * 60 + NeverDestroy.Instance.second));
        else GameManager.Instance.Score *= 2;
        
        GameManager.Instance.SetND();
        NeverDestroy.Instance.Score = GameManager.Instance.Score;
        
        UIManager.Instance.GameOver(true);
    }

    public override void TakeDamage(float damage, Vector2 position, float knockUpValue, bool knockup, bool isExplosion, bool isPoison = false)
   {
       if (!shieldCollider.enabled && BossManager.instance.currentBossPhase != ExtensionMethods.PhaseBoss.Begin)
       {
             base.TakeDamage(damage, position, knockUpValue, knockup, isExplosion);
                 BossManager.instance.UpdateSlider(health);
       }
    
   }
   
}
