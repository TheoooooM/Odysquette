using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemySpawn : MonoBehaviour
{
    private EnnemyVar.Ennemies ennemyType;
    
    void Start()
    {
        Instantiate(EnnemyVar.EnnemiesList[(int) ennemyType]);
    }

    void Update()
    {
        
    }
}
