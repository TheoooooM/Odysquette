using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FxBossSpinManager : MonoBehaviour
{

    private bool inSpin;
    [SerializeField] private AngleTrail[] angleTrailsList;
    [SerializeField]
    private DirectionTrail currentDirectionTrail;

    [SerializeField] private Transform trail;
    [SerializeField] private Transform dust;
    public void CancelBool()
    {
        currentDirectionTrail = DirectionTrail.None;
        inSpin = false;
        trail.gameObject.SetActive(false);
        dust.gameObject.SetActive(false);
    }

    public void DoneBool()
    {
        inSpin = true;
 
        dust.gameObject.SetActive(true);
    }

    private void Update()
    {
        
        if (inSpin)
        {
            Vector2 currentAngleVector = (Playercontroller.Instance.transform.position - transform.position).normalized;
         CheckAngles(currentAngleVector);   
        }
    }

    void CheckAngles(Vector2 angleVector)
    { 
   
   float currentAngle;
        currentAngle = Mathf.Atan2(angleVector.y, angleVector.x) * Mathf.Rad2Deg;
        if (Math.Abs(Mathf.Sign(currentAngle) - (-1)) < 0.05f) {
            currentAngle = 360 + currentAngle;
        }

        foreach (AngleTrail angleTrail in angleTrailsList) {
            if (currentAngle >= angleTrail.angleMin &&
                currentAngle <= angleTrail.angleMax) {
               SetTrail(angleTrail);
            }
        }
    }

    void SetTrail(AngleTrail angleTrail)
    {
        if (angleTrail.direction == currentDirectionTrail)
            return;
        dust.localPosition = angleTrail.positionDust;
        dust.rotation = Quaternion.Euler(0,0,angleTrail.angleDust);
    
        currentDirectionTrail = angleTrail.direction;
    }

    [Serializable]
    public class AngleTrail
    {
        public DirectionTrail direction;
        public int angleMin;
        public int angleMax;
        public Vector2 positionTrail;
        public Vector2 positionDust;
        public float angleDust;
    }

    public enum DirectionTrail
    {
        None,Front, Back, FrontRight, FrontLeft, BackLeft, BackRight, Right, Left 
    }
}
