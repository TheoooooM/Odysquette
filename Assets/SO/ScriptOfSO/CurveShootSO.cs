using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
[CreateAssetMenu(fileName = "CurveShootSO", menuName = "ShootMode/CurveShootSO", order = 2)]
public class CurveShootSO : StrawSO
{
   

    public List<PointsForBezierCurve> trajectories = new List<PointsForBezierCurve>();
    [NamedArray("int")]
    public int[] stepOfCurve;
    public int[] stepOfCurveParameter;
    public Vector3[] pointsForBezierCurve;
    
    public PointsForBezierCurve[] trajectoriesParameters;
    private int indexBullet;
    public override void  Shoot(GameManager.Effect effect1, GameManager.Effect effect2, Transform parentBulletTF, MonoBehaviour script, float currentTimeValue = 1)
    {
        if (isDelay)
        {
            for (int i = 0; i < trajectories.Count; i++)
            {

                if (stepOfCurve != null)
                {
                    Vector3 currentBasePosition = new Vector3();
                    GameObject bullet = Instantiate(prefabBullet, parentBulletTF.position,
                        parentBulletTF.rotation);
                    currentBasePosition = bullet.transform.position;
                    indexBullet = i;
                    if (basePosition.Length != null)
                    {
                        bullet.transform.position +=
                            basePosition[i] + basePositionParameter[i] * currentTimeValue;
                        currentBasePosition = bullet.transform.position;
                    }

                    SetParameter(bullet, currentTimeValue, effect1, effect2, currentBasePosition);
                }
            }
        }
        else
        {
            script.StartCoroutine(ShootDelay(effect1, effect2, parentBulletTF, currentTimeValue));
        }

                               
                                   
                          
    }

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
    public override IEnumerator ShootDelay(GameManager.Effect effect1, GameManager.Effect effect2, Transform parentBulletTF, float currentTimeValue)
    {
        for (int i = 0; i < trajectories.Count; i++)
        {

            if (stepOfCurve != null)
            {
                Vector3 currentBasePosition = new Vector3();
                GameObject bullet = Instantiate(prefabBullet, parentBulletTF.position,
                    parentBulletTF.rotation);
                currentBasePosition = bullet.transform.position;
                indexBullet = i;
                if (basePosition.Length != null)
                {
                    bullet.transform.position +=
                        basePosition[i] + basePositionParameter[i] * currentTimeValue;
                    currentBasePosition = bullet.transform.position;
                }

                SetParameter(bullet, currentTimeValue, effect1, effect2, currentBasePosition);  
                yield return new WaitForSeconds(delay+delayParameter*currentTimeValue);
            }
        }
    
    }

    public override void SetParameter(GameObject bullet, float currentTimeValue, GameManager.Effect effect1, GameManager.Effect effect2, Vector3 currentBasePosition)
    {
        base.SetParameter(bullet, currentTimeValue, effect1, effect2,  currentBasePosition);
        CurveBullet curveBullet = bullet.GetComponent<CurveBullet>();
        if (rateSecondParameter)
        {
            curveBullet.stepOfCurve = Mathf.RoundToInt(stepOfCurve[indexBullet]+stepOfCurveParameter[indexBullet]*currentTimeValue);
            
            for (int i = 0; i < trajectories[indexBullet].pointsForBezierCurve.Count; i++)
            {
                Vector3 vector3 = new Vector3();
                vector3 = trajectories[indexBullet].pointsForBezierCurve[i]+trajectoriesParameters[indexBullet].pointsForBezierCurve[i]*currentTimeValue;
                curveBullet.trajectories.Add(vector3);
            }
        }
        else if (!rateSecondParameter)
        {
            curveBullet.stepOfCurve = stepOfCurve[indexBullet];
            for (int i = 0; i < trajectories[indexBullet].pointsForBezierCurve.Count; i++)
            {
                Vector3 vector3 = new Vector3();
                vector3 = trajectories[indexBullet].pointsForBezierCurve[i];
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
            
    }
 
}
     
       //set les bangs
     
    

[Serializable]
public class PointsForBezierCurve
{
    public List<Vector3> pointsForBezierCurve;
   
    
}












