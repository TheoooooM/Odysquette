using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CreateAssetMenu(fileName = "AngleAreaShootSO", menuName = "ShootMode/AngleAreaShootSO", order = 3)]
public class AngleAreaShootSO : StrawSO
{
    public int angleDivision = 0;
    public float angle = 0;
    
    public int angleDivisionParameter = 0 ;
    
    public float angleParameter = 0;
    private GameObject bullet;
    Vector3 rotation = new Vector3();float currentAngle = 0 ;
    public override void Shoot( Transform parentBulletTF, MonoBehaviour script, float currentTimeValue = 1 )
    {
        
      
     
        if (!isDelayBetweenShoot && !isDelayBetweenWaveShoot)
        {
            for (int i = 0; i < angleDivision+2+Mathf.RoundToInt(angleDivisionParameter * currentTimeValue); i++)
            {
            
            bullet = PoolManager.Instance.SpawnFromPool(parentBulletTF, prefabBullet, rateMode);
            bullet.SetActive(true);
            bullet.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                
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
                
               Debug.Log(rotation);
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
      
        for (int j = 0; j < numberWaveShoot; j++)
        {
            for (int i = 0; i < angleDivision + 2; i++)
            {
                GameObject bullet = PoolManager.Instance.SpawnFromPool(parentBulletTF, prefabBullet, rateMode);
                bullet.SetActive(true);
                bullet.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                float currentAngle = 0;
                Vector3 rotation = new Vector3();
                if (angleDivisionParameter != 0)
                {
                    currentAngle += -angle / 2 +
                                    (angle / (angleDivision +
                                              Mathf.RoundToInt(angleDivisionParameter * currentTimeValue))) * i;
                }
                else
                {
                    currentAngle = -angle / 2 + (angle / angleDivision) * i;
                }

                if (angleParameter != 0)
                {
                    rotation = Quaternion.Euler(0, 0, currentAngle + angleParameter * currentTimeValue) *
                               parentBulletTF.transform.right;
                }
                else
                {
                    rotation = Quaternion.Euler(0, 0, currentAngle) * parentBulletTF.transform.right;
                }
                
                if (basePosition.Length != 0)
                {
                    bullet.transform.position += basePosition[i];
                    if (basePositionParameter.Length >= 1)
                    {
                        bullet.transform.position += basePositionParameter[i] * currentTimeValue;
                    }

                }

                bullet.GetComponent<Rigidbody2D>()
                    .AddForce(rotation * (speedBullet + speedParameter * currentTimeValue), ForceMode2D.Force);
                SetParameter(bullet, currentTimeValue, null);
                if(isDelayBetweenShoot)
                yield return new WaitForSeconds(delayBetweenShoot + delayParameter * currentTimeValue);
            }

            if (isDelayBetweenWaveShoot)
            {
                Debug.Log("fjdls");
            yield return new WaitForSeconds(delayBetweenShoot);
                
            }
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

   

