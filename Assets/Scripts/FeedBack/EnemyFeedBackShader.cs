using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFeedBackShader : MonoBehaviour {
    private Material material;
    public PropertyNameWithValue[] propertyNameWithValuesList;

    private void Start() {
        material = GetComponent<SpriteRenderer>().material;
    }

    public void UpdateMaterialProperty(int index) {
        float speed = Time.deltaTime * propertyNameWithValuesList[index].value;
        float currentvalue = material.GetFloat(propertyNameWithValuesList[index].propertyName);
        material.SetFloat(propertyNameWithValuesList[index].propertyName, currentvalue + speed);
    }

    public void SetMaterialProperty(int index) {
        material.SetFloat(propertyNameWithValuesList[index].propertyName, propertyNameWithValuesList[index].value);
    }

    [Serializable]
    public class PropertyNameWithValue {
        public string propertyName;
        public float value;
    }
}