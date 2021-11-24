using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.Mathematics;
using UnityEngine;


public class CameraControllers : MonoBehaviour
{
    public float speed;
 public float maxDistance;
  [SerializeField] private Transform player;
  private Vector3 offSet;
  [SerializeField] private CinemachineVirtualCamera _cinemachineVirtualCamera;
  
  private void FixedUpdate()
  {
  
    if (GameManager.Instance.isMouse)
    {


      Vector3 lookDir = Vector3.ClampMagnitude(GameManager.Instance._lookDir, maxDistance);
   
   
     offSet = player.position+(Vector3)lookDir;
     
     
    }
    else
    {
      offSet = (player.position+(Vector3)GameManager.Instance.ViewPad*maxDistance);
    

    }

    offSet.z = -0.7f;
    
 
    _cinemachineVirtualCamera.ForceCameraPosition( Vector3.MoveTowards(new Vector3(transform.position.x, transform.position.y, -0.7f), offSet, speed*Time.deltaTime), quaternion.identity);  
  }
}
