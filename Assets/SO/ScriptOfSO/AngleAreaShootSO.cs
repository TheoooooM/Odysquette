using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CreateAssetMenu(fileName = "AngleAreaShootSO", menuName = "ShootMode/AngleAreaShootSO", order = 3)]
public class AngleAreaShootSO : StrawSO
{
    public int angleDivision;
    public float angle;
    
    public int angleDivisionParameter;
    
    public float angleParameter;
    private GameObject bullet;
    Vector3 rotation ;float currentAngle;
    public override void Shoot( Transform parentBulletTF, MonoBehaviour script, float currentTimeValue = 1 )
    {
        
      
     
        if (!isDelay)
        {
            for (int i = 0; i < angleDivision+2+Mathf.RoundToInt(angleDivisionParameter * currentTimeValue); i++)
            {
            
            bullet = PoolManager.Instance.SpawnFromPool(parentBulletTF, prefabBullet );
            bullet.SetActive(true);
                
                
                Vector3 rotation;   if (angleDivisionParameter != 0)
                                                 {
                                                     if (angleParameter != 0)
                                                     {
                                                         currentAngle = (-angle-angleParameter*currentTimeValue) / 2 + ((angle+angleParameter*currentTimeValue) / (angleDivision+Mathf.RoundToInt(angleDivisionParameter * currentTimeValue))) * i;
                                                     }
                                                     else
                                                     {
                                                         currentAngle = -angle / 2 + (angle / (angleDivision+Mathf.RoundToInt(angleDivisionParameter * currentTimeValue))) * i;
                                                     }
                                                      
                                                     
                                                 }
                                                 else
                                                 {
                                                     if (angleParameter != 0)
                                                     {
                                                         currentAngle =(-angle-angleParameter*currentTimeValue)/ 2 + ((angle+angleParameter*currentTimeValue)/ angleDivision) * i;
                                                     }
                                                     else
                                                     {
                                                          currentAngle = -angle / 2 + (angle / angleDivision) * i;
                                                     }
                                                       
                                                                                       
                                                 }

                  
                    
                  
               
                     
                     rotation = Quaternion.Euler(0, 0, currentAngle) * parentBulletTF.transform.right;  
                    
                 
             
               
                
                if (basePosition.Length != 0)
                {
                  
                    bullet.transform.position += basePosition[0];
                    if (basePositionParameter.Length == 1 )
                    {
                      
                        bullet.transform.position +=basePositionParameter[i]*currentTimeValue;
                    }
                 
                }
                
                bullet.transform.rotation = Quaternion.Euler(0,0, currentAngle );
                bullet.GetComponent<Rigidbody2D>().AddForce(rotation * (speedBullet + speedParameter * currentTimeValue),
                    ForceMode2D.Force);
                SetParameter(bullet, currentTimeValue,null);
                
            }
           
        }
        else
        {
            script.StartCoroutine(ShootDelay( parentBulletTF, currentTimeValue));
        }
       
        
            }
    public override void OnValidate()
    {
        base.OnValidate();
        angleDivisionParameter = Mathf.Max(angleDivisionParameter, 0);
    
        effectAllNumberShoot= Mathf.Max(effectAllNumberShoot, 0);
    }

    public override IEnumerator ShootDelay( Transform parentBulletTF, float currentTimeValue)
    {
        for (int i = 0; i < angleDivision + 2; i++)
        {
           GameObject bullet = PoolManager.Instance.SpawnFromPool(parentBulletTF, prefabBullet); 
         
            float currentAngle = 0;
            Vector3 rotation = new Vector3();   
            if (angleDivisionParameter != 0)
            {
                currentAngle += -angle / 2 + (angle / (angleDivision+Mathf.RoundToInt(angleDivisionParameter * currentTimeValue))) * i; 
            }
            else
            {
                currentAngle = -angle / 2 + (angle / angleDivision) * i;                                         
            }
            if (angleParameter != 0)
            {
                rotation = Quaternion.Euler(0, 0, currentAngle + angleParameter * currentTimeValue) * parentBulletTF.transform.right;
            }
            else
            {
                rotation = Quaternion.Euler(0, 0, currentAngle) * parentBulletTF.transform.right;
            }

         
          

            if (basePosition.Length != 0)
            {
                bullet.transform.position += basePosition[i];
                if (basePositionParameter.Length == 1 )
                {
                    bullet.transform.position +=basePositionParameter[i]*currentTimeValue;
                }
               
            }

            bullet.GetComponent<Rigidbody2D>().AddForce(rotation * (speedBullet+ speedParameter * currentTimeValue), ForceMode2D.Force);
            SetParameter(bullet, currentTimeValue,null); 
            yield return new WaitForSeconds(delay+delayParameter*currentTimeValue);
        }
       
    }

    //save pool
           
            //set la cadence de tir
            //set les bangs

            public override void SetParameter(GameObject bullet, float currentTimeValue, Transform p)
            {
                base.SetParameter(bullet, currentTimeValue);
                Bullet scriptBullet = bullet.GetComponent<Bullet>();
        
            }

 

    }

   

