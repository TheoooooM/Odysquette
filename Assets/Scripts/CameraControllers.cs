using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Cinemachine;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;


public class CameraControllers : MonoBehaviour
{
  public static CameraControllers Instance;
  private void Awake() => Instance = this;


  public Rect currentRectLimitation;
  public float speed;
  public float maxDistance;
  [SerializeField] private Transform player;
  [SerializeField]
  private Vector3 offSet;
  [SerializeField]
  private CinemachineVirtualCamera _cinemachineVirtualCamera;
  

  /*private void OnDrawGizmosSelected()
  {
    Handles.DrawSolidRectangleWithOutline(currentRectLimitation,new Color(01,1,1,0.01f), Color.red);
  }*/

  
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
    
    //offSet.x = Mathf.Clamp(offSet.x, currentRectLimitation.xMin+1.77f*_cinemachineVirtualCamera.m_Lens.OrthographicSize, currentRectLimitation.xMax-1.77f*_cinemachineVirtualCamera.m_Lens.OrthographicSize);
    //offSet.y = Mathf.Clamp(offSet.y, currentRectLimitation.yMin+_cinemachineVirtualCamera.m_Lens.OrthographicSize, currentRectLimitation.yMax-_cinemachineVirtualCamera.m_Lens.OrthographicSize);
    //offSet.z = -0.7f;
    _cinemachineVirtualCamera.ForceCameraPosition( Vector3.MoveTowards(new Vector3(transform.position.x, transform.position.y, -10), offSet, speed*Time.deltaTime), Quaternion.identity);  
  }
}
