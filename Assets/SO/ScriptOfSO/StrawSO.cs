using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using Unity.Mathematics;
using UnityEditor;
using UnityEditor.Experimental.TerrainAPI;
using UnityEngine;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using Object = System.Object;

public class StrawSO : ScriptableObject
{
    

  
   public float damage = 1;
   public float timeValue;
   public int effectAllNumberShoot;
   public float dragRB = 0;
   public float range = 1;
   public bool loadingRateMode = false;
   public bool rateMainParameter = false;
       public bool rateSecondParameter = false;
       public float damageParameter = 0;
      public float rangeParameter =0;
      public float dragRBParameter =0;
      public float speedParameter = 0 ;
      public bool isDelay;
      public float delay = 0;
     
      public float delayParameter;
      [NamedArray("vector3")]
      public Vector3[] basePositionParameter; 

 [NamedArray("vector3")]
   public Vector3[] basePosition;
  
   public float speedBullet;
   public GameObject prefabBullet;
   
   public virtual void OnValidate()
   {
      damage = Mathf.Max(damage, 0);
      timeValue = Mathf.Max(timeValue, 0);
      dragRB= Mathf.Max(dragRB, 0);
      range = Mathf.Max(range, 0);
      delay = Mathf.Max(delay, 0);
      speedBullet = Mathf.Max(speedBullet, 0);
          effectAllNumberShoot= Mathf.Max(effectAllNumberShoot, 0);
   
      
   }
   public virtual void Shoot(GameManager.Effect effect1, GameManager.Effect effect2, Transform parentBulletTF , MonoBehaviour script, float currentTimeValue = 1)
   {
    
   }

   public virtual IEnumerator ShootDelay(GameManager.Effect effect1, GameManager.Effect effect2, Transform parentBulletTF , float currentTimeValue = 1 )
   {
       yield return null;
   }

   public virtual void SetParameter(GameObject bullet, float currentTimeValue, GameManager.Effect effect1, GameManager.Effect effect2, Vector3 currentBasePosition)
   {  ScriptBullet scriptBullet = bullet.GetComponent<ScriptBullet>();
       if (rateMainParameter == true)
       {
        
           scriptBullet.damage = damage+damageParameter * currentTimeValue;
           scriptBullet.range =range +rangeParameter * currentTimeValue;
           scriptBullet.rb.drag = dragRB + dragRBParameter * currentTimeValue;
       }
else if (rateMainParameter == false)
       {
           scriptBullet.damage = damage ;
           scriptBullet.range = range ;
           scriptBullet.rb.drag = dragRB;
       }
      
       scriptBullet.firstEffect = effect1;
       scriptBullet.secondEffect = effect2;
   }
   
}




