using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFeedBackFX : MonoBehaviour {
    [SerializeField] private List<FxOneTime> FxOneTimeList = new List<FxOneTime>();


    public void InstantiateOneTimeFx(int index) {
        if (!FxOneTimeList[index].isOneTime) {
            GameObject obj = EnemySpawnerManager.Instance.SpawnEnnemyShoot(FxOneTimeList[index].enemyTypeShootFX, FxOneTimeList[index].prefab, transform);
            obj.SetActive(true);
            FxOneTimeList[index].isOneTime = true;
        }
    }

    public void CancelInstantiateOneTimeFx(int index) {
        FxOneTimeList[index].isOneTime = false;
    }

    [Serializable]
    public class FxOneTime {
        public ExtensionMethods.EnemyTypeShoot enemyTypeShootFX;
        public bool isOneTime;
        public GameObject prefab;
    }
}