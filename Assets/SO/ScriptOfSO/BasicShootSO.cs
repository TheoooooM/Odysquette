using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEditor;
[CreateAssetMenu(fileName = "BasicShootSO", menuName = "ShootMode/BasicShootSO", order = 1)]
public class BasicShootSO : StrawSO
{
  
    [NamedArray("float", true)]
    public float[] directions;
    [NamedArray("float", false)]
    public float[] directionParameter;

    public override void OnValidate()
    {
        base.OnValidate();
        
    }

    public override void Shoot(Transform parentBulletTF,MonoBehaviour script, float currentTimeValue = 1 ) 
    {

     


        if (!isDelay)
        {
            for (int i = 0; i < directions.Length; i++)
            {
             
               
                GameObject bullet = PoolManager.Instance.SpawnFromPool(parentBulletTF, prefabBullet );
                bullet.SetActive(true);
                Vector3 rotation;
 
                if (directionParameter.Length == i +1 )
                {
            
                   rotation = Quaternion.Euler(0,0 , directions[i]+ directionParameter[i] * currentTimeValue) * parentBulletTF.transform.right;   
                    
                }
                else
                {
                     rotation = Quaternion.Euler(0, directions[i], 0) * parentBulletTF.transform.right;
                }
              
                if (basePosition.Length != 0)
                {
                    bullet.transform.position += basePosition[i];
                    if (basePositionParameter.Length == i +1 )
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
        for (int i = 0; i < directions.Length; i++)
        {
            Vector3 currentBasePosition = new Vector3();
            GameObject bullet = PoolManager.Instance.SpawnFromPool(parentBulletTF, prefabBullet );
            bullet.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            bullet.SetActive(true);
            Vector3 rotation;
            if (directionParameter.Length == i +1 )
            {
            
                rotation = Quaternion.Euler(0,0 , directions[i]+ directionParameter[i] * currentTimeValue) * parentBulletTF.transform.right;   
                
            }
            else
            {
                rotation = Quaternion.Euler(0, directions[i], 0) * parentBulletTF.transform.right;
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
                             
            bullet.GetComponent<Rigidbody2D>().AddForce(rotation.normalized*(speedBullet+speedParameter*currentTimeValue), ForceMode2D.Force );
            bullet.transform.rotation = Quaternion.Euler(0,0, directions[i]+ directionParameter[i] * currentTimeValue);
            SetParameter(bullet, currentTimeValue, null);
            yield return new WaitForSeconds(delay + delayParameter * currentTimeValue);
        }
    }



    public override void SetParameter(GameObject bullet, float currentTimeValue, Transform transform)
    {
        base.SetParameter(bullet, currentTimeValue);
        Bullet scriptBullet = bullet.GetComponent<Bullet>();
        
    }
}
