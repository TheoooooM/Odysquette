using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OverrideDropdownImage : MonoBehaviour {
    [SerializeField] private Transform parentContent = null;
    [SerializeField] private Sprite darkSprite = null;
    [SerializeField] private Sprite lightSprite = null;

    private void Update() {
        if (parentContent.childCount == 4) {
            for (int i = 1; i < parentContent.GetChild(3).GetChild(0).GetChild(0).childCount; i++) {
                parentContent.GetChild(3).GetChild(0).GetChild(0).GetChild(i).GetChild(0).GetComponent<Image>().sprite = (i % 2 == 0 ? lightSprite : darkSprite);
            }
        }
    }
}
