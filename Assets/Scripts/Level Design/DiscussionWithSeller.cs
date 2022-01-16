using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DiscussionWithSeller : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private AudioSource source;
    [SerializeField] private CanvasGroup cvsGroup;
    [SerializeField] private int iteration = 0;
    
    [SerializeField] private List<string> textToShow = new List<string>();
    [SerializeField] private int textId = 2;

    private void Start() {
        source.enabled = false;
        cvsGroup.alpha = 0;
        text.text = "";
        textId = textToShow.Count;
    }

    private void Update() {
        cvsGroup.alpha = Mathf.Lerp(cvsGroup.alpha, source.enabled ? 1f : 0f, source.enabled ? 1f/5f : 1f / (float)iteration);
    }


    /// <summary>
    /// Start the discussion after bying
    /// </summary>
    public void StartDiscusssion() {
        StopAllCoroutines();
        
        source.enabled = true;
        text.text = textToShow[textToShow.Count - textId];
        textId--;

        StartCoroutine(endDiscussion());
    }

    /// <summary>
    /// End the discussion
    /// </summary>
    /// <returns></returns>
    IEnumerator endDiscussion() {
        yield return new WaitForSeconds(3);
        source.enabled = false;
    }
}
