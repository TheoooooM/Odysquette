using System;
using System.Collections;
using System.Collections.Generic;
using LootLocker.Requests;
using UnityEngine;

public class NeverDestroy : MonoBehaviour
{
    #region Instance
    public static NeverDestroy Instance;
    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
        
        life = 6;
        
        
    }
    #endregion

    public GameManager.Effect firstEffect; 
    
    public GameManager.Effect secondEffect;

    public GameManager.Straw actualStraw;

    public int level = 0;

    public int ressources = 0;
    public int life;

    public float ultimateValue = 0;

    public int Score;
    
    //TIMER
    public int minute = 0;
    public int second = 0;
    
    private float time;
    private string minuteText;
    private string secondText;
    private bool canTimer = false;

    public bool isAWin = false;
    private void Start() {
        if (minute < 10) minuteText = "0" + minute;
        else minuteText = minute.ToString();
        if (second < 10) secondText = "0" + second;
        else secondText = second.ToString();
        if(UIManager.Instance != null) UIManager.Instance.Timer.text = minuteText + " : " + secondText;
    }

    private void Update() {
        if (UIManager.Instance != null && canTimer) {
            time += Time.deltaTime;
            second = (int) time;
            if (second == 60)
            {
                time += Time.deltaTime;
                second = (int) time;
                if (second == 60)
                {
                    time = 0;
                    minute++;
                }
            }

            if (minute < 10) minuteText = "0" + minute;
            else minuteText = minute.ToString();
            if (second < 10) secondText = "0" + second;
            else secondText = second.ToString();
            
            UIManager.Instance.Timer.text = minuteText + " : " + secondText;
        }
    }


    public void AddRessource(int amount = 1) {
        ressources += amount;
        if (UIManager.Instance != null) UIManager.Instance.ressourceText.text = ressources.ToString();
    }
    
    public void SubmitScore(string playerName, int ID)
    {
        LootLockerSDKManager.SubmitScore(playerName, Score, ID, (response) =>
        {
            if (response.success)Debug.Log("Submit Score");
            else Debug.Log("Failed Submit Score");
        });
    }

    public void StartTimer() {
        canTimer = true;
        if (UIManager.Instance != null) {
            UIManager.Instance.StartTimer();
            UIManager.Instance.Timer.text = minuteText + " : " + secondText;
        }
    }

    public void PauseTimer() {
        canTimer = false;
        if (UIManager.Instance != null) {
            UIManager.Instance.PauseTimer();
            UIManager.Instance.Timer.text = minuteText + " : " + secondText;
        }
    }

    /// <summary>
    /// Win the fight
    /// </summary>
    public void GoHUbWin() {
        isAWin = true;
    }
}
