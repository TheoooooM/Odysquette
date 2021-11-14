using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraControllers : MonoBehaviour
{
    public float speed;
 public float maxDistance;
  [SerializeField] private Transform player;
  private Vector3 offSet;
  
  private void FixedUpdate()
  {
  
    if (GameManager.Instance.isMouse)
    {


      Vector3 test = Vector3.ClampMagnitude(GameManager.Instance._lookDir, maxDistance);
   
   
     offSet = (player.position+test);
   
      
    
     
   
     
    }
    else
    {
      offSet = (player.position+(Vector3)GameManager.Instance.ViewPad*maxDistance);
    

    } 
 
   
    transform.position =  Vector3.MoveTowards(transform.position, offSet, speed*Time.deltaTime);
  }
}
