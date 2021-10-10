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
    
    public override void Shoot(GameManager.Effect effect1,GameManager.Effect effect2, Transform parentBulletTF, MonoBehaviour script, float currentTimeValue = 1 )
    {
        
        List<Vector3> vector3 = new List<Vector3>();
        angleDivision += Mathf.RoundToInt(angleDivisionParameter * currentTimeValue);
        angle += angleParameter * currentTimeValue;
        if (isDelay)
        {
            for (int i = 0; i < angleDivision + 2; i++)
            {
                Vector3 currentBasePosition = new Vector3();
                float currentAngle = -angle / 2 + (angle / angleDivision) * i;
                Vector3 rotation = Quaternion.Euler(0, currentAngle, 0) * parentBulletTF.transform.forward;
                GameObject bullet = Instantiate(prefabBullet, parentBulletTF.position, parentBulletTF.rotation);
                currentBasePosition = bullet.transform.position;

                if (basePosition.Length != null)
                {
                    bullet.transform.position += basePosition[i];
                    currentBasePosition = bullet.transform.position + basePositionParameter[i] * currentTimeValue;

                }

                bullet.GetComponent<Rigidbody>().AddForce(rotation * speedBullet, ForceMode.Force);
                SetParameter(bullet, currentTimeValue, effect1, effect2, bullet.transform.position += basePosition[i]);
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
        angleDivisionParameter = Mathf.Max(angleDivisionParameter, 0);
    
        effectAllNumberShoot= Mathf.Max(effectAllNumberShoot, 0);
    }

    public override IEnumerator ShootDelay(GameManager.Effect effect1, GameManager.Effect effect2, Transform parentBulletTF, float currentTimeValue)
    {
        for (int i = 0; i < angleDivision + 2; i++)
        {
            Vector3 currentBasePosition = new Vector3();
            float currentAngle = -angle / 2 + (angle / angleDivision) * i;
            Vector3 rotation = Quaternion.Euler(0, currentAngle, 0) * parentBulletTF.transform.forward;
            GameObject bullet = Instantiate(prefabBullet, parentBulletTF.position, parentBulletTF.rotation);
            currentBasePosition = bullet.transform.position;

            if (basePosition.Length != null)
            {
                bullet.transform.position += basePosition[i];
                currentBasePosition = bullet.transform.position + basePositionParameter[i] * currentTimeValue;

            }

            bullet.GetComponent<Rigidbody>().AddForce(rotation * speedBullet, ForceMode.Force);
            SetParameter(bullet, currentTimeValue, effect1, effect2, bullet.transform.position += basePosition[i]); 
            yield return new WaitForSeconds(delay+delayParameter*currentTimeValue);
        }
       
    }

    //save pool
           
            //set la cadence de tir
            //set les bangs

        

 

    }

   

