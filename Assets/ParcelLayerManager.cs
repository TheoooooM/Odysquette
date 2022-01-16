using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParcelLayerManager : MonoBehaviour
{
    [SerializeField]
    private string layerName;

    [SerializeField] private SpriteRenderer spriteRenderer;
    public void ChangeLayerLaunch()
    {
        spriteRenderer.sortingLayerName = layerName;
    }
}
