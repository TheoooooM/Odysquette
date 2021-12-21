using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class LightColor : MonoBehaviour {
    [SerializeField] private List<LigthDataSO> matDataList = new List<LigthDataSO>();

    public List<LigthDataSO> MatDataList => matDataList;

    [Space]
    [SerializeField] private gameobjectLightList gamLightList = null;
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
        
        //GameObject Light A
        foreach (GameObject gam in gamLightList.GamLightA) {
            gam.GetComponent<SpriteRenderer>().material = light.ColorMat.MatA;
        }
        
        //GameObject Light B
        foreach (GameObject gam in gamLightList.GamLightB) {
            gam.GetComponent<SpriteRenderer>().material = light.ColorMat.MatB;
        }
        
        //GameObject Light C
        foreach (GameObject gam in gamLightList.GamLightC) {
            gam.GetComponent<SpriteRenderer>().material = light.ColorMat.MatC;
        }
        
        //GameObject Light D
        foreach (GameObject gam in gamLightList.GamLightD) {
            gam.GetComponent<SpriteRenderer>().material = light.ColorMat.MatD;
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
    [SerializeField] private Material matC = null;
    [SerializeField] private Material matD = null;
    public Material MatA => matA;
    public Material MatB => matB;
    public Material MatC => matC;
    public Material MatD => matD;
}

/// <summary>
/// List of the lights
/// </summary>
[Serializable]
public class gameobjectLightList {
    [SerializeField] private List<GameObject> gamLightA = null;
    [SerializeField] private List<GameObject> gamLightB = null;
    [SerializeField] private List<GameObject> gamLightC = null;
    [SerializeField] private List<GameObject> gamLightD = null;

    public List<GameObject> GamLightA => gamLightA;
    public List<GameObject> GamLightB => gamLightB;
    public List<GameObject> GamLightC => gamLightC;
    public List<GameObject> GamLightD => gamLightD;
}