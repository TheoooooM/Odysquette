using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CustomSliderUI : MonoBehaviour {
    [SerializeField] private Image sliderImage = null;
    [SerializeField] private GameObject HandleObject = null;
    [Space]
    [SerializeField] private Transform pointA = null;
    [SerializeField] private Transform pointB = null;
    [Space]
    [SerializeField, Range(0,1)] private float fillAmount = 0;
    public float FillAmount => fillAmount;

    [Space] 
    [SerializeField] private UnityEvent OnValueChanged = null;
    
#if UNITY_EDITOR
    /// <summary>
    /// Update in Editor
    /// </summary>
    private void OnValidate() {
        UpdateSliderValue(fillAmount);
        HandleObject.transform.position = Vector3.Lerp(pointA.position, pointB.position, fillAmount);
    }
#endif

    /// <summary>
    /// Called at start
    /// </summary>
    private void Start() {
        HandleObject.GetComponent<RectTransform>().position = pointA.GetComponent<RectTransform>().position;
        UpdateSliderValue(0);
    }

    /// <summary>
    /// Update the slider
    /// </summary>
    /// <param name="value"></param>
    private void UpdateSliderValue(float value) {
        fillAmount = value;
        sliderImage.fillAmount = Mathf.Clamp(value, 0,1);
        OnValueChanged.Invoke();
    }

    /// <summary>
    /// Update the variable of fill Amount
    /// </summary>
    /// <param name="HandlerPos"></param>
    public void updateSliderAmountFromHandler(float newFillAmount) => UpdateSliderValue(newFillAmount);
}
