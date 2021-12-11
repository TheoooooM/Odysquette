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
        image = GetComponent<Image>();
        basicSprite = image.sprite;
    }

    private void Update()
    {
        if (GameManager.Instance != null)
        {
            if (firstEffect)
                image.sprite = GameManager.Instance.firstEffect switch
                {
                    GameManager.Effect.bounce => SO.bounceSprite,
                    GameManager.Effect.pierce => SO.pierceSprite,
                    GameManager.Effect.explosion => SO.explosionSprite,
                    GameManager.Effect.poison => SO.poisonSprite,
                    _ => basicSprite,
                };
            else image.sprite = GameManager.Instance.secondEffect switch
                {
                    GameManager.Effect.bounce => SO.bounceSprite,
                    GameManager.Effect.pierce => SO.pierceSprite,
                    GameManager.Effect.explosion => SO.explosionSprite,
                    GameManager.Effect.poison => SO.poisonSprite,
                    _ => basicSprite,
                };
        }
    }
}
