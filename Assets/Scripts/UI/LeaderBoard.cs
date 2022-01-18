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
    
    public int[] scoreArray = new int[9];
    public string[] nameArray = new string[9];
    private string[] nameArrayString = new string[9];
    public string[] NameArrayString => nameArrayString;
    private string[] scoreArrayString = new string[9];
    public string[] ScoreArrayString => scoreArrayString;
    
    
    private void Start()
    {
        UpdateArray();
    }

    public void UpdateArray() {
        nameArrayString[0] = "nameOne";
        nameArrayString[1] = "nameTwo";
        nameArrayString[2] = "nameThree";
        nameArrayString[3] = "nameFour";
        nameArrayString[4] = "nameFive";
        nameArrayString[5] = "nameSix";
        nameArrayString[6] = "nameSeven";
        nameArrayString[7] = "nameEight";
        nameArrayString[8] = "nameNine";
        
        scoreArrayString[0] = "scoreOne";
        scoreArrayString[1] = "scoreTwo";
        scoreArrayString[2] = "scoreThree";
        scoreArrayString[3] = "scoreFour";
        scoreArrayString[4] = "scoreFive";
        scoreArrayString[5] = "scoreSix";
        scoreArrayString[6] = "scoreSeven";
        scoreArrayString[7] = "scoreEight";
        scoreArrayString[8] = "scoreNine";
        
        for (int i = 0; i < scoreArray.Length-1; i++) {
            scoreArray[i] = PlayerPrefs.GetInt(scoreArrayString[i], 0);
            nameArray[i] = PlayerPrefs.GetString(nameArrayString[i], "");
        }
    }
    
    public void SetScore(string name, int score) {
        for (int i = nameArray.Length - 1; i >= 0; i--) {
            if (score > scoreArray[i]) {
                if (i == nameArray.Length - 1) continue;
                PlayerPrefs.SetInt(scoreArrayString[i + 1], scoreArray[i]);
                PlayerPrefs.SetString(nameArrayString[i + 1], nameArray[i]);

                if (i == 0) {
                    PlayerPrefs.SetInt(scoreArrayString[0], score);
                    PlayerPrefs.SetString(nameArrayString[0], name);
                }
            }
            else {
                if (i == nameArray.Length - 1) continue;
                PlayerPrefs.SetInt(scoreArrayString[i + 1], score);
                PlayerPrefs.SetString(nameArrayString[i + 1], name);
                break;
            }
        }
        
        
        /*
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
        }*/
    }
}
