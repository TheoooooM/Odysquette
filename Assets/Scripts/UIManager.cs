using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public Plane Planeg = new Plane(Vector3.zero, new Vector3(1,0), new Vector3(-1,0));

    public GameObject viewFinder;
    public GameObject[] HeartsLife;
    
    // Start is called before the first frame update
    public static UIManager Instance;

    private void Awake()
    {
        
        Instance = this;
    }

 

    // Update is called once per frame
    private void Update()
    {
       Debug.Log( Planeg.GetSide(new Vector3(0.5f,0)));
    }
}
