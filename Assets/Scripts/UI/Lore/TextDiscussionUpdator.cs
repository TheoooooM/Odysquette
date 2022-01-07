using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextDiscussionUpdator : MonoBehaviour {
    [SerializeField] private DisquetteDiscussionManager discussionManager = null;
    [SerializeField] private TextMeshProUGUI text = null;
    [SerializeField] private List<AudioClip> discussionSound = new List<AudioClip>();
    [SerializeField] private AudioSource sound = null;

    private void Start() {
        sound.clip = discussionSound[Random.Range(0, discussionSound.Count)];
        text.text = discussionManager.UpdateText();
    }
}
