using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAfterImageSprite : MonoBehaviour
{
    [SerializeField] private float activeTime = 0.1f;
    private float timeActivated;
    private float alpha;
    [SerializeField] private float alphaSet = 0.8f;
    [SerializeField] private float alphaMultiplier = 0.9f;

    private Transform player;

    private SpriteRenderer SR;
    private SpriteRenderer playerSR;

    private Color color;

    private void OnEnable()
    {
        SR = GetComponent<SpriteRenderer>();
        player = HealthPlayer.Instance.transform; //check if this is the right tag for the player
        playerSR = player.GetComponent<SpriteRenderer>();

        alpha = alphaSet;
        SR.sprite = playerSR.sprite;
        transform.position = player.position;
        transform.rotation = player.rotation;
        timeActivated = Time.time;
    }

    private void Update()
    {
        alpha *= alphaMultiplier;
        color = new Color (1f, 1f, 1f, alpha);
        SR.color = color;
        if(Time.time >= (timeActivated + activeTime))
        {
            
            PlayerAfterImagePool.Instance.AddToPool(gameObject);
        } 
    }
    
}
