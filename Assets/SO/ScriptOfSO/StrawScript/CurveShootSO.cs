using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
[CreateAssetMenu(fileName = "CurveShootSO", menuName = "ShootMode/CurveShootSO", order = 2)]
public class CurveShootSO : StrawSO
{

  
    public List<PointsForBezierCurve> trajectories = new List<PointsForBezierCurve>();
    [NamedArray("int", true)]
    public int[] stepOfCurve;
    public int[] stepOfCurveParameter;
    public Vector3[] pointsForBezierCurve;
    public float speedBounce;
    

    public PointsForBezierCurve[] trajectoriesParameters ;
    private int indexBullet;
    public override void  Shoot(Transform parentBulletTF, MonoBehaviour script, float currentTimeValue = 1)
    {
        if (!isDelayBetweenShoot && !isDelayBetweenWaveShoot)
        {
          
            for (int i = 0; i < trajectories.Count; i++)
            {

                if (stepOfCurve != null)
                {
                    Vector3 currentBasePosition = new Vector3();
                    AudioManager.Instance.PlayShootStraw(typeSoundShoot, shootSoundScale);
                    GameObject bullet = PoolManager.Instance.SpawnFromPool(parentBulletTF, prefabBullet, rateMode);
                    bullet.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                    indexBullet = i;
                    if (basePosition.Length != 0)
                    {
                        bullet.transform.position += basePosition[i];
                        if (basePositionParameter.Length == i +1 )
                        {
                            bullet.transform.position +=basePositionParameter[i]*currentTimeValue;
                        }
                   
                    }

                    SetParameter(bullet, currentTimeValue, parentBulletTF);
                  
                }
            }
        }
        else
        {
            script.StartCoroutine(ShootDelay( parentBulletTF, currentTimeValue));
        }

                               
                                   
                          
    }

#if UNITY_EDITOR
    public override void OnValidate()
    {
        base.OnValidate();
        for (int i = 0; i < stepOfCurve.Length; i++)
        {
            stepOfCurve[i] = Mathf.Max(stepOfCurve[i], 0);
        }
        for (int i = 0; i < stepOfCurve.Length; i++)
        {
            stepOfCurve[i] = Mathf.Max(stepOfCurve[i], 0);
        } 
        effectAllNumberShoot= Mathf.Max(effectAllNumberShoot, 0);
    }
#endif

    public override IEnumerator ShootDelay( Transform parentBulletTF, float currentTimeValue)
    {
        for (int j = 0; j < numberWaveShoot; j++)
        {
            for (int i = 0; i < trajectories.Count; i++)
            {

                if (stepOfCurve != null)
                {
                    AudioManager.Instance.PlayShootStraw(typeSoundShoot, shootSoundScale);
                    Vector3 currentBasePosition = new Vector3();
                    GameObject bullet = PoolManager.Instance.SpawnFromPool(parentBulletTF, prefabBullet, rateMode);
                    bullet.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                    indexBullet = i;
                    if (basePosition.Length != 0)
                    {
                        bullet.transform.position += basePosition[i];
                        if (basePositionParameter.Length == i + 1)
                        {
                            bullet.transform.position += basePositionParameter[i] * currentTimeValue;
                        }

                    }

                    SetParameter(bullet, currentTimeValue, parentBulletTF);
                    if(isDelayBetweenShoot)
                    yield return new WaitForSeconds(delayBetweenShoot + delayParameter * currentTimeValue);
                }
            }
            if(isDelayBetweenWaveShoot)
            yield return new WaitForSeconds(delayBetweenWaveShoot);
        }

    }

    public override void SetParameter(GameObject bullet, float currentTimeValue,  Transform PBtransform)
    {
        base.SetParameter(bullet, currentTimeValue);
        CurveBullet curveBullet = bullet.GetComponent<CurveBullet>();
     
        if (rateSecondParameter)
        {
            if (stepOfCurve != null)
            {
                if (stepOfCurve[indexBullet] != null)
                {
                        curveBullet.stepOfCurve = Mathf.RoundToInt(stepOfCurve[indexBullet]+stepOfCurveParameter[indexBullet]*currentTimeValue);
                }
            }
            else
            {
                curveBullet.stepOfCurve = stepOfCurve[indexBullet];
            }

            if (trajectoriesParameters != null)
            {
                if (trajectoriesParameters[indexBullet] != null)
                {
                    if (trajectoriesParameters[indexBullet].pointsForBezierCurve != null)
                    {
                      for (int i = 0; i < trajectories[indexBullet].pointsForBezierCurve.Count; i++)
                              {
                                  Vector3 vector3 = new Vector3();
                                  vector3 = PBtransform.rotation.normalized*(trajectories[indexBullet].pointsForBezierCurve[i]+trajectoriesParameters[indexBullet].pointsForBezierCurve[i]*currentTimeValue);
                                  curveBullet.trajectories.Add(vector3);
                              }

                      curveBullet.isParameterTrajectories = true;
                      curveBullet.range=   Vector3.Distance( curveBullet.trajectories[0], curveBullet.trajectories[curveBullet.trajectories.Count-1]);
                    }
                }
            }
        
            
        
        }
        else if (!rateSecondParameter)
            
        { 
            curveBullet.stepOfCurve = stepOfCurve[indexBullet];
           
            for (int i = 0; i < trajectories[indexBullet].pointsForBezierCurve.Count; i++)
            {
                Vector3 vector3 = new Vector3();
                
                vector3 += PBtransform.rotation.normalized* (trajectories[indexBullet].pointsForBezierCurve[i]);
                
                curveBullet.trajectories.Add(vector3); 
                                                                     
            }

          
        }

        if (rateMainParameter)
        {
            curveBullet.speed = currentTimeValue * speedParameter + speedBullet;
            
            
        }
        else if (!rateMainParameter)
        {
            curveBullet.speed =  speedBullet;
        }

        curveBullet.speedBounce = speedBounce;
        
        bullet.SetActive(true);
    }
 
}
     
       //set les bangs
     
    

[Serializable]
public class PointsForBezierCurve
{
    public List<Vector3> pointsForBezierCurve;
   
    
}












