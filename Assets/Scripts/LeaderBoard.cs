using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderBoard : MonoBehaviour
{
    #region SINGLETON
    public static LeaderBoard Instance;

    private void Awake()
    {
        Instance = this;
    }
    #endregion

    public int[] scoreArray = new int[5];
    public string[] nameArray = new string[5];
    
    private void Start()
    {
        UpdateArray();
    }

    public void UpdateArray()
    {
        scoreArray[0] = PlayerPrefs.GetInt("scoreOne");
        scoreArray[1] = PlayerPrefs.GetInt("scoreTwo");
        scoreArray[2] = PlayerPrefs.GetInt("scoreThree");
        scoreArray[3] = PlayerPrefs.GetInt("scoreFour");
        scoreArray[4] = PlayerPrefs.GetInt("scoreArray[5]");
        
        nameArray[0] = PlayerPrefs.GetString("nameOne");
        nameArray[1] = PlayerPrefs.GetString("nameTwo");
        nameArray[2] = PlayerPrefs.GetString("nameThree");
        nameArray[3] = PlayerPrefs.GetString("nameFour");
        nameArray[4] = PlayerPrefs.GetString("nameFive");
    }
    
    public void SetScore(string name, int score)
    {

        if (score > scoreArray[4])
        {
            if (score > scoreArray[3])
            {
                PlayerPrefs.SetInt("scoreFive", scoreArray[3]);
                PlayerPrefs.SetString("nameFive", nameArray[3]);
                
                if (score > scoreArray[2])
                {
                    PlayerPrefs.SetInt("scoreFour", scoreArray[2]);
                    PlayerPrefs.SetString("nameFour", nameArray[2]);

                    if (score > scoreArray[1])
                    {
                        PlayerPrefs.SetInt("scoreThree", scoreArray[1]);
                        PlayerPrefs.SetString("nameThree", nameArray[1]);

                        if (score > scoreArray[0])
                        {
                            PlayerPrefs.SetInt("scoreTwo", scoreArray[0]);
                            PlayerPrefs.SetString("nameTwo", nameArray[0]);
                            
                            
                            PlayerPrefs.SetInt("scoreOne", score);
                            PlayerPrefs.SetString("nameOne", name);

                        }
                        else
                        {
                            PlayerPrefs.SetInt("scoreTwo", score);
                            PlayerPrefs.SetString("nameTwo", name);
                        }
                    }
                    else
                    {
                        PlayerPrefs.SetInt("scoreThree", score);
                        PlayerPrefs.SetString("nameThree", name);
                    }
                }
                else
                {
                    PlayerPrefs.SetInt("scoreFour", score);
                    PlayerPrefs.SetString("nameFour", name);
                }
            }
            else
            {
                PlayerPrefs.SetInt("scoreFive", score);
                PlayerPrefs.SetString("nameFive", name);
            }
        }
    }
}
