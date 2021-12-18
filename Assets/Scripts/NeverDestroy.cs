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

    private float time;
    private int minute = 0;
    private string minuteText;
    private int second = 0;
    private string secondText;
    

    private void Update()
    {
        time += Time.deltaTime;
        second = (int) time;
        if (second == 60)
        {
            time = 0;
            minute++;
        }

        if (minute < 10) minuteText = "0" + minute;
        else minuteText = minute.ToString();
        if (second < 10) secondText = "0" + second;
        else secondText = second.ToString();

        UIManager.Instance.Timer.text = minuteText + " : " + secondText;
    }


    public void AddRessource(int amount = 1)
    {
        ressources += amount;
        if (UIManager.Instance != null) UIManager.Instance.ressourceText.text = "Ressources : " + ressources;
    }
}
