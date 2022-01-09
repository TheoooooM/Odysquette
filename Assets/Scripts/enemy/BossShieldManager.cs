using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossShieldManager : MonoBehaviour
{
    private Collider2D collider2D;
   
    private void Start()
    {
        collider2D = GetComponent<Collider2D>();
    }

    public void UpShield()
    {
        collider2D.enabled = true;
    }

    public void DownShield()
    {
        collider2D.enabled = false;
    }
}
