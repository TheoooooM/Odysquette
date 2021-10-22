using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEditor;
[CreateAssetMenu(fileName = "BasicShootSO", menuName = "ShootMode/BasicShootSO", order = 1)]
public class BasicShootSO : StrawSO
{
  
    [NamedArray("float", true)]
    public float[] directions = Array.Empty<float>();
    [NamedArray("float", false)]
    public float[] directionParameter = Array.Empty<float>();
    

    public override void OnValidate()
    {
        base.OnValidate();
        
    }

    public override void Shoot(Transform parentBulletTF,MonoBehaviour script, float currentTimeValue = 1 ) 
    {

     


        if (!isDelayBetweenShoot && !isDelayBetweenWaveShoot)
        {
            for (int i = 0; i < directions.Length; i++)
            {
             
               
                GameObject bullet = PoolManager.Instance.SpawnFromPool(parentBulletTF, prefabBullet, rateMode );
                bullet.SetActive(true);
                Vector3 rotation = Vector3.zero;
 
                if (directionParameter.Length >= i +1 )
                {
            
                   rotation = Quaternion.Euler(0,0 , directions[i]+ directionParameter[i] * currentTimeValue) * parentBulletTF.transform.right;   
                    
                }
                else
                {
                     rotation = Quaternion.Euler(0, 0, directions[i]) * parentBulletTF.transform.right;
                }
              
                if (basePosition.Length != 0)
                {
                    bullet.transform.position += basePosition[i];
                    if (basePositionParameter.Length >= i +1 )
                    {
                        bullet.transform.position +=basePositionParameter[i]*currentTimeValue;
                    }
                   
                }
         
                bullet.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                bullet.GetComponent<Rigidbody2D>().AddForce(rotation* (speedBullet + speedParameter * currentTimeValue),
                    ForceMode2D.Force);
                SetParameter(bullet, currentTimeValue,null);
            }
        }
        else
        {
           script.StartCoroutine(ShootDelay(parentBulletTF, currentTimeValue));
        }
      
    }

 

    public override IEnumerator ShootDelay(Transform parentBulletTF, float currentTimeValue)
    {
        for (int j = 0; j < numberWaveShoot; j++)
        {
             for (int i = 0; i < directions.Length; i++)
                    {
                        Debug.Log("dfqsjdsmljidfqsjdfqsdfqs");
                  
                        GameObject bullet = PoolManager.Instance.SpawnFromPool(parentBulletTF, prefabBullet, rateMode);
                        bullet.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                        bullet.SetActive(true);
                        Vector3 rotation = Vector3.zero;
                        if (directionParameter.Length >= i +1 )
                        {
            
                            rotation = Quaternion.Euler(0,0 , directions[i]+ directionParameter[i] * currentTimeValue) * parentBulletTF.transform.right;   
                    
                        }
                        else
                        {
                            rotation = Quaternion.Euler(0, 0, directions[i]) * parentBulletTF.transform.right;
                        }        
                        
                        if (basePosition.Length != 0)
                        {
                            bullet.transform.position += basePosition[i];
                            if (basePositionParameter.Length == i +1 )
                            {
                                bullet.transform.position +=basePositionParameter[i]*currentTimeValue;
                            }
                          
                        }
                        //save pool
                                         
                        bullet.GetComponent<Rigidbody2D>().AddForce(rotation*(speedBullet+speedParameter*currentTimeValue), ForceMode2D.Force );
                   
                        SetParameter(bullet, currentTimeValue, null);
                        if(isDelayBetweenShoot)
                        yield return new WaitForSeconds(delayBetweenShoot + delayParameter * currentTimeValue);
                    }
            if(isDelayBetweenWaveShoot)
                    yield return new WaitForSeconds(delayBetweenShoot);
        }
       
    }



    public override void SetParameter(GameObject bullet, float currentTimeValue, Transform transform)
    {
        base.SetParameter(bullet, currentTimeValue);
        Bullet scriptBullet = bullet.GetComponent<Bullet>();
        
    }
}
