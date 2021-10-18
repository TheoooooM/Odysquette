using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test2 : test
{
    // Start is called before the first frame update
    void Start()
    {
        bonsoir();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void bonsoir()
    {
        Debug.Log("je suis lis avant ");
        base.bonsoir();
        
    }
    
}
