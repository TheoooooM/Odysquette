using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class LightColor : MonoBehaviour {
    [SerializeField] private List<LigthDataSO> matDataList = new List<LigthDataSO>();

    public List<LigthDataSO> MatDataList => matDataList;

    [Space]
    [SerializeField] private List<GameObject> doorLight = null;
    [SerializeField] private List<GameObject> windowLight = null;
    [SerializeField] private List<GameObject> flickerGam = null;
    [Space]
    [SerializeField] private List<GameObject> gamColor = new List<GameObject>();
    [SerializeField] private List<GameObject> gamColorLighter = new List<GameObject>();

    public void ChangeLightPink() => ChangeLight(matDataList[1]);
    public void ChangeLightRed() => ChangeLight(matDataList[2]);

    public void ChangeLight(LigthDataSO light) {
        //Dark Color
        foreach (GameObject gam in gamColor) {
            gam.GetComponent<Light2D>().color = light.BaseColor;
        }
        
        //Light Color
        foreach (GameObject gam in gamColorLighter) {
            gam.GetComponent<SpriteRenderer>().color = light.LightColor;
        }
        
        //Flicker Light
        foreach (GameObject gam in flickerGam) {
            gam.GetComponent<SpriteRenderer>().material = light.FlickerMat;
        }
        
        //Door Light
        foreach (GameObject gam in doorLight) {
            gam.GetComponent<SpriteRenderer>().material = light.ColorMat.MatA;
        }
        
        //Window Light
        foreach (GameObject gam in windowLight) {
            gam.GetComponent<SpriteRenderer>().material = light.ColorMat.MatB;
        }
    }
}

/// <summary>
/// List of the material
/// </summary>
[Serializable]
public class MaterialList {
    [SerializeField] private Material matA = null;
    [SerializeField] private Material matB = null;
    public Material MatA => matA;
    public Material MatB => matB;
}