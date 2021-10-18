using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptBullet : MonoBehaviour
{
    public float damage;
    public float range;
    public StrawSO strawSo;

    public Color colorBang;
    public Vector3 basePosition;
    
    
//pour faire la curve
    void Curve()
    {
//  
//
    }
    

    private void Update()
    {
       
    }

    private void OnTriggerEnter(Collider other)
    {
        //si tu touches tu l'ennemi ou si tu touches un wall tu depop
    }
}
