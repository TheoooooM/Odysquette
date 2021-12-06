using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Light/Custom")]
public class LigthDataSO : ScriptableObject
{
    [SerializeField] private string colorName = "";
    [SerializeField] private MaterialList colorMat = null;
    [SerializeField] private Material flickerMat = null;
    [SerializeField] private Color baseColor = new Color();
    [SerializeField] private Color lightColor = new Color();

    public string ColorName => colorName;
    public MaterialList ColorMat => colorMat;
    public Material FlickerMat => flickerMat;
    public Color BaseColor => baseColor;
    public Color LightColor => lightColor;
}
