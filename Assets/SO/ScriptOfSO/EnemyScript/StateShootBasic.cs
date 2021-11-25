using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Object = UnityEngine.Object;
[CreateAssetMenu(fileName = "StateShootBasicSO", menuName = "EnnemyState/Shoot/StateShootBasic", order = 0)]
public class StateShootBasic : StateShootSO
{

    public float[] directions = Array.Empty<float>();

    
    public override void StartState(Dictionary<ExtensionMethods.ObjectInStateManager, Object> objectDictionary, out bool endStep)
    {
        Transform transformPlayer = (Transform) objectDictionary[ExtensionMethods.ObjectInStateManager.TransformPlayer];
        Transform parentBulletTF =
            (Transform) objectDictionary[ExtensionMethods.ObjectInStateManager.TransformShoot];
        Transform enemyTransform = (Transform) objectDictionary[ExtensionMethods.ObjectInStateManager.TransformEnemy];
        enemyTransform.GetComponent<SpriteRenderer>().color = Color.cyan;
        Vector3 direction =  (transformPlayer.position-enemyTransform.position).normalized;
        parentBulletTF.transform.position = enemyTransform.position + direction * offSetDistance;
        if (isFirstAimPlayer)
        {
            Transform previousTransformPlayer= (Transform) objectDictionary[ExtensionMethods.ObjectInStateManager.PreviousTransformPlayer];
            previousTransformPlayer.position = transformPlayer.position; 
        }
       
   
        
        endStep = false;
    }

    public override void PlayState(Dictionary<ExtensionMethods.ObjectInStateManager, Object> objectDictionary, out bool endStep)
    {
        
        Transform enemyTransform = (Transform) objectDictionary[ExtensionMethods.ObjectInStateManager.TransformEnemy];
        enemyTransform.GetComponent<SpriteRenderer>().color = Color.white;
  
        Transform parentBulletTF =
            (Transform) objectDictionary[ExtensionMethods.ObjectInStateManager.TransformShoot];
        EnemyStateManager enemyStateManager = (EnemyStateManager) objectDictionary[ExtensionMethods.ObjectInStateManager.EnemyStateManager];
        GameObject prefabBullet = null;
        Transform transformPlayer = null;
        Vector2 directionPlayer = new Vector2();
        if (isAimPlayer)
        {
            
             transformPlayer = (Transform) objectDictionary[ExtensionMethods.ObjectInStateManager.TransformPlayer];
            
        }

        if (isFirstAimPlayer)
        {
            transformPlayer = (Transform) objectDictionary[ExtensionMethods.ObjectInStateManager.PreviousTransformPlayer];
        }

            for (int i = 0; i <EnemySpawnerManager.Instance.enemyShootPools.Count ; i++)
        {
            if (enemyTypeShoot == EnemySpawnerManager.Instance.enemyShootPools[i].enemyTypeShoot)
            {
                prefabBullet = EnemySpawnerManager.Instance.enemyShootPools[i].bulletPrefab;
            }
        }
    
      
       
        if (!isDelayBetweenShoot && !isDelayBetweenWaveShoot)
        {
            for (int i = 0; i < directions.Length; i++)
            {
             
               
                GameObject bullet = PoolManager.Instance.SpawnEnnemyShoot(enemyTypeShoot, prefabBullet, parentBulletTF);
                bullet.SetActive(true);  
                if (basePosition.Length != 0)
                {
                    bullet.transform.position += basePosition[i];
                }
                Vector3 rotation = new Vector3();
                directionPlayer = parentBulletTF.position;
                if (isAimPlayer)
                {
                    directionPlayer = (transformPlayer.position - bullet.transform.position).normalized;
                    
                }
                rotation = Quaternion.Euler(0, 0, directions[i]) * directionPlayer;
                float angle = Mathf.Atan2(rotation.y, rotation.x)*Mathf.Rad2Deg;
                bullet.transform.rotation = Quaternion.Euler(0, 0, angle) ;
                
              

              
                  
           
         
             
                bullet.GetComponent<Rigidbody2D>().AddForce(rotation* speedBullet,
                    ForceMode2D.Force);
                SetParameter(bullet);
            }
        }
        else
        {
            enemyStateManager.StartCoroutine(ShootDelay(prefabBullet,parentBulletTF, transformPlayer));
        }

        endStep = true;
    }
     public override IEnumerator ShootDelay(GameObject prefabBullet,Transform parentBulletTF, Transform transformPlayer)
    {
        Vector2 directionPlayer = new Vector2();
      
        for (int j = 0; j < numberWaveShoot; j++)
        {
       
             for (int i = 0; i < directions.Length; i++)
                    {
                        GameObject bullet = PoolManager.Instance.SpawnEnnemyShoot(enemyTypeShoot, prefabBullet, parentBulletTF);
                        bullet.SetActive(true);  
                        if (basePosition.Length != 0)
                        {
                            bullet.transform.position += basePosition[i];
                        }
                        Vector3 rotation = new Vector3();
                        directionPlayer = parentBulletTF.position;
                   
                        if (isAimPlayer || isFirstAimPlayer)
                        {
                            directionPlayer = (transformPlayer.position - bullet.transform.position).normalized;
                    
                        }
                        rotation = Quaternion.Euler(0, 0, directions[i]) * directionPlayer;
                        float angle = Mathf.Atan2(rotation.y, rotation.x);
                        bullet.transform.rotation = Quaternion.Euler(0, 0, angle) ;
            
                        //save pool
                           bullet.GetComponent<Rigidbody2D>().velocity = Vector2.zero;              
                        bullet.GetComponent<Rigidbody2D>().AddForce(rotation*speedBullet, ForceMode2D.Force );
                   
                        SetParameter(bullet);
                        if(isDelayBetweenShoot)
                        yield return new WaitForSeconds(delayBetweenShoot);
                    }
            if(isDelayBetweenWaveShoot)
                    yield return new WaitForSeconds(delayBetweenWaveShoot);
        }
       
    }




    
}
