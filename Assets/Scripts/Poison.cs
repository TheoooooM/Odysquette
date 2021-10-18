using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poison : MonoBehaviour
{
    public float Timer;
    public float _Timer;
    

    private void OnEnable()
    {
        _Timer = Timer;
    }

    void FixedUpdate()
    {
        if (_Timer>0)
        {
            _Timer -= 1;
        }
        else
        {
            gameObject.SetActive(false);
            PoolManager.Instance.PoisonQueue.Enqueue(gameObject);
        }
    }
}
