using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FXBossShootManager : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer spriteRenderer;

    private bool isUse;
    [SerializeField] private Animator animator;

    public void EndAnimation()
    {

        animator.Play("ShootBossFX_None");

    }

    public void BeginAnimation()
    {
        if (!isUse)
        {
            isUse = true;
                  animator.Play("ShootBossFX_Idle");  
        }

    }

    public void CancelUse()
    {
        isUse = false;
    }
}
