using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathHeartLifeEnemy : MonoBehaviour
{
    [SerializeField]
    private GameObject heartItem;
    [SerializeField]
    private Vector3 position = Vector3.zero;

    private GameObject gam;
    [SerializeField] private bool nextToEnemy;
    public void DropHeartItem()
    {
     
        
        if (nextToEnemy)
        {
               gam =  Instantiate(heartItem, position, Quaternion.identity);
        }
        else
        {
            gam = Instantiate(heartItem, transform.position + position, Quaternion.identity);
        }
   
        gam.GetComponent<Items>().SpawnObject(true);
    }
}
