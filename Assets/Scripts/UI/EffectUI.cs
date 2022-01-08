using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using UnityEngine;
using UnityEngine.UI;

public class EffectUI : MonoBehaviour
{
    public bool firstEffect;
    private Image image;
    private Sprite basicSprite;
    [SerializeField]itemSO SO;

    private void Start()
    {
        image = transform.GetChild(0).GetComponent<Image>();
        basicSprite = image.sprite;
    }

    private void Update() {
        if (GameManager.Instance != null) {
            if (firstEffect)
                image.sprite = GameManager.Instance.firstEffect switch {
                    GameManager.Effect.bouncing => SO.bounceSprite,
                    GameManager.Effect.piercing => SO.pierceSprite,
                    GameManager.Effect.explosive => SO.explosionSprite,
                    GameManager.Effect.poison => SO.poisonSprite,
                    _ => basicSprite,
                };
            else image.sprite = GameManager.Instance.secondEffect switch {
                    GameManager.Effect.bouncing => SO.bounceSprite,
                    GameManager.Effect.piercing => SO.pierceSprite,
                    GameManager.Effect.explosive => SO.explosionSprite,
                    GameManager.Effect.poison => SO.poisonSprite,
                    _ => basicSprite,
                };
        }
    }
}
