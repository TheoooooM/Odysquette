using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WIN : MonoBehaviour {
    [SerializeField] private GameObject victoryPanel = null;
    [SerializeField] private LeaderBoard leaderBoard = null;
    [SerializeField] private TMP_InputField playerName = null;
    [SerializeField] private TextMeshProUGUI finalScore = null;
    
    private void Start() {
        victoryPanel.SetActive(true);
        finalScore.text = $"Final Score : {NeverDestroy.Instance.Score}";
    }
    
    public void SubmitScore() {
        leaderBoard.UpdateArray();
        leaderBoard.SetScore(playerName.text, NeverDestroy.Instance.Score);
        
        NeverDestroy.Instance.ResetNeverDestroy();
    }
}
