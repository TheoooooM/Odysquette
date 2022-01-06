using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStateManager : EnemyStateManager
{
 
    
   public override void TakeDamage(float damage, Vector2 position, float knockUpValue, bool knockup, bool isExplosion)
   {
      base.TakeDamage(damage, position, knockUpValue, knockup, isExplosion);
      BossManager.instance.UpdateSlider(health);
   }
   
}
