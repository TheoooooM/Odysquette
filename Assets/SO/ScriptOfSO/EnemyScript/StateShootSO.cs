using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class StateShootSO : StateEnemySO
{
    //
    public float rangeForShoot;
//
    public float rangeForBullet;

    public int damage;

    public float speedBullet;

    public float dragRB;

    public bool hasRange;
    
    public ExtensionMethods.EnemyTypeShoot enemyTypeShoot;
    
    public bool isDelayBetweenShoot;
    
    public bool isDelayBetweenWaveShoot;
    
    public float delayBetweenShoot = 0;
    
    public float delayBetweenWaveShoot = 0;
   
    public Vector3[] basePosition = new Vector3[0];

    public int numberWaveShoot = 1;

    public bool isAimPlayer;
    public float offSetDistance;
    public LayerMask layerMaskRay; 
   
    public Vector3 extentsRangeDetection;
 
    public virtual IEnumerator ShootDelay(GameObject prefabBullet, Transform parentBulletTF, Transform transformPlayer)
    {
      
        yield return null;
    }

    public override bool CheckCondition(Dictionary<ExtensionMethods.ObjectInStateManager, Object> objectDictionary)
    {
        Rigidbody2D rbPlayer = (Rigidbody2D) objectDictionary[ExtensionMethods.ObjectInStateManager.RigidBodyPlayer];
        Rigidbody2D rbEnemy = (Rigidbody2D) objectDictionary[ExtensionMethods.ObjectInStateManager.RigidBodyEnemy];
        if (Vector2.Distance(rbPlayer.position, rbEnemy.position) <= rangeForShoot)
        {
            
         
           
           Vector2 direction = (rbPlayer.position - rbEnemy.position);
           
           RaycastHit2D hit = Physics2D.BoxCast(rbEnemy.position, extentsRangeDetection, 0,
               direction.normalized, direction.magnitude, layerMaskRay);
           Debug.Log(hit.collider.gameObject.name);
          ExtDebug.DrawBoxCastBox(rbEnemy.position, extentsRangeDetection/2, Quaternion.identity, direction.normalized, direction.magnitude, Color.red);
           if(hit.collider.gameObject.layer == 9 )
           {
               return true;
           }
           

        }
            
        
      
        return false;

    }

    public virtual void SetParameter(GameObject bullet)
    {
       EnemyBullet scriptBullet = bullet.GetComponent<EnemyBullet>();
       scriptBullet.damage = damage;
       scriptBullet.damage = damage;
       scriptBullet.hasRange = hasRange;
       scriptBullet.range = rangeForBullet;
       scriptBullet.rb.drag = dragRB;
    }
}