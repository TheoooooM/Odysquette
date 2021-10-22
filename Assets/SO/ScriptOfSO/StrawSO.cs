using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
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
    public bool hasRange;
    public string strawName;
    public Sprite strawRenderer;
   public float damage = 1;
   public float timeValue = 0;
   public int effectAllNumberShoot = 0;
   public float dragRB = 0;
   public float range = 1;
   public RateMode rateMode;
  
   public bool rateMainParameter = false;
       public bool rateSecondParameter = false;
       public float damageParameter = 0;
      public float rangeParameter =0;
      public float dragRBParameter =0;
      public float speedParameter = 0 ;
      public bool isDelayBetweenShoot;
      public bool isDelayBetweenWaveShoot;
      public int numberWaveShoot = 1;
      public float delayBetweenShoot = 0;
      public float delayBetweenWaveShoot = 0;
     
      public float delayParameter = 0;
      [NamedArray("vector3", true)]
      public Vector3[] basePositionParameter = new Vector3[0]; 

 [NamedArray("vector3", true)]
   public Vector3[] basePosition = new Vector3[0];
  
   public float speedBullet = 0;
   public GameObject prefabBullet;
   
   public virtual void OnValidate()
   {
    
      
     
      damage = Mathf.Max(damage, 0);
      timeValue = Mathf.Max(timeValue, 0);
      dragRB= Mathf.Max(dragRB, 0);
      range = Mathf.Max(range, 0);
      delayBetweenShoot = Mathf.Max(delayBetweenShoot, 0);
      speedBullet = Mathf.Max(speedBullet, 0);
          effectAllNumberShoot= Mathf.Max(effectAllNumberShoot, 0);
   
      
   }
   public virtual void Shoot( Transform parentBulletTF , MonoBehaviour script, float currentTimeValue = 1)
   {
    
   }

   public virtual IEnumerator ShootDelay( Transform parentBulletTF , float currentTimeValue = 1 )
   {
       yield return null;
   }

   public virtual void SetParameter(GameObject bullet, float currentTimeValue, Transform transform = null )
   {
       Bullet scriptBullet = bullet.GetComponent<Bullet>();
       if (rateMainParameter == true)
       {
           scriptBullet.hasRange = hasRange;
           scriptBullet.damage = damage + damageParameter * currentTimeValue;
           if(hasRange)
           scriptBullet.range = range + rangeParameter * currentTimeValue;
           scriptBullet.rb.drag = dragRB + dragRBParameter * currentTimeValue;
       }
       else if (rateMainParameter == false)
       {
           scriptBullet.damage = damage;
           scriptBullet.hasRange = hasRange;
           scriptBullet.range = range;
         
           scriptBullet.rb.drag = dragRB;
           scriptBullet.rateMode = rateMode;
       }


   }
public enum  RateMode
{
    Ultimate, FireRate, FireLoading
}
}




