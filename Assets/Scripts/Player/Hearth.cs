using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hearth : MonoBehaviour
{
    private HealthPlayer healthControl;
    [HideInInspector] public bool currentHearth = false;
    private bool isHalf=false;

    [Header("----Sprite----")] 
    [SerializeField] private Sprite hearth;
    [SerializeField] private Sprite halfHearth;

    private Animator animator;



    private Image image;

    private void Awake()
    {
        image = GetComponent<Image>();
        animator = GetComponent<Animator>();
    }


    public void LifeUpdate() {
        GetComponent<Animator>().enabled = true;
        if (currentHearth)
        {
            if (!isHalf)animator.Play("animLifeAffect");
            else animator.Play("animLifehalfAffect");
        }
        else animator.Play("animLifeNoneaffect");
    }
}
