using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;

[RequireComponent(typeof(EnemyFeedBack))]
public class EnemyFeedBackMovement : MonoBehaviour
{
    public List<MultipleAnimationList> AnimationStatesList;
    public List<MultipleAnimationListOneTime> AnimationStatesListOneTime;
    private Rigidbody2D rb;
    private Animator animator;
    public List<AnimationForSpecificPosition> animationForSpecificPositionsList;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        if (AnimationStatesListOneTime.Count != 0)
        {
                  for (int i = 0; i < AnimationStatesListOneTime.Count; i++)
                    {
                        if (AnimationStatesListOneTime[i].destination == null)
                        {
                            AnimationStatesListOneTime[i].destination = HealthPlayer.Instance.transform;
                        }
                      
                    }
        }
  
    }
    [Serializable]
    public class MultipleAnimationList 
    {
        public string  currentStatePlayed;

        public ExtensionMethods.AngleAnimation[] angleAnimation;
    }    [Serializable]
    public class MultipleAnimationListOneTime 
    {
        public bool ApplyState;
        public Transform destination;
        public ExtensionMethods.AngleAnimation[] angleAnimation;
    }
    [Serializable]
    public class AnimationForSpecificPosition
    {
        public Transform tranformPosition;
        public string stateName;
        public float toleranceDistance;
    }


    public void UpdateSpecificPosition(int index)
    {
        if (Vector3.Distance(transform.position, animationForSpecificPositionsList[index].tranformPosition.position)<animationForSpecificPositionsList[index].toleranceDistance)
        {
         PlayAnimation(animationForSpecificPositionsList[index].stateName, index, true);   
        };
    }
    public void UpdatePositionOnTime(int index)
    {
        if (!AnimationStatesListOneTime[index].ApplyState)
        {
 
            Vector2 direction = (AnimationStatesListOneTime[index].destination.position - transform.position).normalized;
                            float currentInputAngle =  Mathf.Atan2(direction.y, direction.x)*Mathf.Rad2Deg;
                            if (Mathf.Sign(currentInputAngle) == -1)
                            {
                                currentInputAngle = 360 + currentInputAngle;
                            }
            
                            MultipleAnimationListOneTime multipleAnimationList = AnimationStatesListOneTime[index];
                            for (int i = 0; i < multipleAnimationList.angleAnimation.Length; i++)
                            {
                    
                                if (currentInputAngle >=  multipleAnimationList.angleAnimation[i].angleMin &&
                                    currentInputAngle <=  multipleAnimationList.angleAnimation[i].angleMax)
                                {
                                              
                                    PlayAnimation(multipleAnimationList.angleAnimation[i].stateName, index, true);
                                    AnimationStatesListOneTime[index].ApplyState = true;
                                }
                            } 
        }
                
    }

    public void CancelBoolOneTime(int index)
    {
        AnimationStatesListOneTime[index].ApplyState = false;
    }
    public void UpdatePosition(int index)
    {
    
        Vector2 direction = (HealthPlayer.Instance.transform.position - transform.position).normalized;
        float currentInputAngle =  Mathf.Atan2(direction.y, direction.x)*Mathf.Rad2Deg;
        if (Mathf.Sign(currentInputAngle) == -1)
        {
            currentInputAngle = 360 + currentInputAngle;
        }

        MultipleAnimationList multipleAnimationList = AnimationStatesList[index];
        for (int i = 0; i < multipleAnimationList.angleAnimation.Length; i++)
        {
        
            if (currentInputAngle >=  multipleAnimationList.angleAnimation[i].angleMin &&
                currentInputAngle <=  multipleAnimationList.angleAnimation[i].angleMax)
            {
                                  
                PlayAnimation(multipleAnimationList.angleAnimation[i].stateName, index, false);
                
                        
            }
        } 
    }

    private void PlayAnimation(string stateName, int index, bool animationOneTime)
    {  
        if (animator.GetCurrentAnimatorStateInfo(0).IsName(stateName))
                return; 
        
            animator.Play(stateName);
            Debug.Log(stateName);
            if(!animationOneTime)
            AnimationStatesList[index].currentStatePlayed = stateName;
    }

}
