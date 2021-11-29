using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;
using UnityEditor.Timeline.Actions;
using Object = UnityEngine.Object;

public class StateEnemySO : ScriptableObject
{
    // deja utilisé
    public bool haveAnimationStartState;
    public bool haveFxStartState;
    public bool haveSoundStartState;
    public bool haveAnimationPlayState;
    public bool haveFxPlayState;
    public bool haveSoundPlayState;
    public bool haveShaderPlayState;
    public bool haveShaderStartState;
    public AnimationClip animationPlayState;
    public AnimationClip animationStartState;
    public AudioClip audioStartState;
    public AudioClip audioPlayState;
    public GameObject fxStartState;
    public GameObject fxPlayState;
    
    
    public bool needEnemyMovement;
    public bool openBasePanel;
    public bool openDebugPanel;
    public bool openSpecPanel;
    public float timeCondition;
    public float healthCondition;
    public bool openKnobDebugPanel;
    public bool useHealthCondition;
    public bool useTimeCondition;
    public bool isKnockUpInState =true;
    //debug
    public List<ExtensionMethods.ObjectInStateManager> objectInStateManagersCondition = new List<ExtensionMethods.ObjectInStateManager>();
    public List<ExtensionMethods.ObjectInStateManager> objectInStateManagersState = new List<ExtensionMethods.ObjectInStateManager>();
    //1
    //fixed state during default state
    //start parameter 
    //play state
    public bool oneStartState;
    public float startTime;
    public float playStateTime;
    public bool isFixedUpdate;
    public bool haveStartState;
    private float timeForTimerState;
    public bool duringDefaultState;
    public virtual bool CheckCondition (Dictionary<ExtensionMethods.ObjectInStateManager, Object> objectDictionary)
    {
        Animator animator = new Animator();
        AnimationClip animationClip = null;
        animator.Play(animationClip.name);
        return true;
    }

    public virtual void StartState( Dictionary<ExtensionMethods.ObjectInStateManager, Object> objectDictionary,  out bool endStep)
    {
        
        endStep = false;
    }
        
    public virtual void PlayState(Dictionary<ExtensionMethods.ObjectInStateManager, Object> objectDictionary, out bool endStep)
    {
        endStep = false;
    }

    public virtual void PlayShader(EnemyStateManager enemyStateManager)
    {
        if (enemyStateManager != null)
        {
            if (!enemyStateManager.currentShaderState)
            {
                return;
            }

            enemyStateManager.currentShaderState = true;
        }
       
    }

    public void PlayAnimation(Animator animator, string animationName)
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName(animationName))
            return;
        animator.Play(animationName);
    }

    public void PlaySound(AudioClip audioClip)
    {
        //PlayAudioClip 
    }

    public void PlayFX(GameObject fx, Vector3 position, EnemyStateManager enemyStateManager)
    {
        if (enemyStateManager != null)
        {
            if (!enemyStateManager.currentFxState)
            {
                return;
            }

            enemyStateManager.currentFxState = true;
        }
            
          
            Instantiate(fx, position, Quaternion.identity);
    }
        

 

   
}
