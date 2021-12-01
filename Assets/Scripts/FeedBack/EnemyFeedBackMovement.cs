using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFeedBackMovement : MonoBehaviour
{
    public List<MultipleAnimationList> AnimationStatesList;
    private Rigidbody2D rb;
    private Animator animator;
 
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }
    [Serializable]
    public class MultipleAnimationList 
    {
        public string  currentStatePlayed;

        public ExtensionMethods.AngleAnimation[] angleAnimation;
    }

    public void UpdateMovement(int index)
    {
                Vector2 direction = rb.velocity.normalized;
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
                                  
                        PlayAnimation(multipleAnimationList.angleAnimation[i].stateName, index);
                        
                    }
                } 
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
                                  
                PlayAnimation(multipleAnimationList.angleAnimation[i].stateName, index);
                
                        
            }
        } 
    }

    private void PlayAnimation(string stateName, int index)
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName(stateName))
                return; 
            animator.Play(stateName);
            AnimationStatesList[index].currentStatePlayed = stateName;
    }

}
