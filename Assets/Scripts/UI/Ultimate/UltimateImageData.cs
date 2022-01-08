using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UltimateImageData : MonoBehaviour {
    [SerializeField] private float minPosY = 0;
    public float MinPosY => minPosY;
    [SerializeField] private float minHeight = 0;
    public float MinHeight => minHeight;
    [Space]
    [SerializeField] private float maxPosY = 0;
    public float MaxPosY => maxPosY;
    [SerializeField] private float maxHeight = 0;
    public float MaxHeight => maxHeight;
}
