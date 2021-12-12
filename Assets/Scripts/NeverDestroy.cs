using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeverDestroy : MonoBehaviour
{
    public static NeverDestroy Instance;
    
    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
        life = 6;
    }

    public GameManager.Effect firstEffect;
    public GameManager.Effect secondEffect;

    public GameManager.Straw actualStraw;

    public int level = 0;

    public int ressources = 0;
    public int life;

    public float ultimateValue = 0;

    public void AddRessource(int amount = 1)
    {
        ressources += amount;
        if (UIManager.Instance != null) UIManager.Instance.ressourceText.text = "Ressources : " + ressources;
    }
}
