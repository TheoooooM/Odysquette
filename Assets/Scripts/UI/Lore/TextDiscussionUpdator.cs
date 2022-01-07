using System;
using TMPro;
using UnityEngine;

public class TextDiscussionUpdator : MonoBehaviour {
    [SerializeField] private DisquetteDiscussionManager discussionManager = null;
    [SerializeField] private TextMeshProUGUI text = null;

    private void Start() => text.text = discussionManager.UpdateText();
}
