using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(EnemyFeedBack))]
public class EnemyFeedBackOtherObject : MonoBehaviour
{
  [Serializable]
  public class AnimationOfOtherObject
  {
    public string stateName;
    public Animator animator;
  }

  public AnimationOfOtherObject[] animationForOtherObjectList;
  void PlayAnimationOfOtherObject(int index)
  {
    if (animationForOtherObjectList[index].animator.GetCurrentAnimatorStateInfo(0).IsName(animationForOtherObjectList[index].stateName))
      return;
    animationForOtherObjectList[index].animator.Play(animationForOtherObjectList[index].stateName);
  }

 public void PlayAnimationOfOtherObjectDestroy(int index)
  {
    PlayAnimationOfOtherObject(index);
    Debug.Log(animationForOtherObjectList[index].animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
    Destroy( animationForOtherObjectList[index].animator.gameObject,  animationForOtherObjectList[index].animator.GetCurrentAnimatorStateInfo(0).length);
  }
 
  
  

}
