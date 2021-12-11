using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarAfterSprite : MonoBehaviour
{
    [SerializeField] private float activeTime = 0.1f;
    private float timeActivated;
    private float alpha;
    [SerializeField] private float alphaSet = 0.8f;
    private float alphaMultiplier = 0.99f;

    public Transform car;

    private SpriteRenderer SR;
    private SpriteRenderer carSR;

    private Color color;

    // Start is called before the first frame update
    private void OnEnable()
    {
        SR = GetComponent<SpriteRenderer>();
        carSR = car.GetComponent<SpriteRenderer>();

        alpha = alphaSet;
        SR.sprite = carSR.sprite;
        transform.position = car.position;
        transform.rotation = car.rotation;
        timeActivated = Time.time;
    }

    private void Update()
    {
        alpha *= alphaMultiplier;
        color = new Color(1f, 1f, 1f, alpha);
        SR.color = color;
        if (Time.time >= (timeActivated + activeTime))
        {
            EnemySpawnerManager.Instance.AddToPool(gameObject);
        }
    }
}
