using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeverDestroy : MonoBehaviour
{
    public static NeverDestroy Instance;
    
    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

}
