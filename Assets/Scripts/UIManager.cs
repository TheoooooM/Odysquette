using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    
    public GameObject[] HeartsLife;
    
    // Start is called before the first frame update
    public static UIManager Instance;

    private void Awake()
    {
        Instance = this;
    }

 

    // Update is called once per frame
    void Update()
    {
        
    }
}
