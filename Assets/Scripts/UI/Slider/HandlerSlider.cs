using UnityEngine;
using UnityEngine.EventSystems;

public class HandlerSlider : MonoBehaviour, IDragHandler {
    [SerializeField] private CustomSliderUI slider = null;
    [Space]
    [SerializeField] private Transform pointA = null;
    [SerializeField] private Transform pointB = null;
    
    /// <summary>
    /// Move the Handler point
    /// </summary>
    /// <param name="eventData"></param>
    public void OnDrag(PointerEventData eventData) {
        GetComponent<RectTransform>().position = new Vector3(eventData.position.x, GetComponent<RectTransform>().position.y, GetComponent<RectTransform>().position.z);
        Vector3 pos = GetComponent<RectTransform>().position;
        pos = new Vector3(Mathf.Clamp(pos.x, pointA.transform.position.x, pointB.transform.position.x), pos.y, pos.z);
        GetComponent<RectTransform>().position = pos;
        float value = 0;
        value = ((pointB.transform.position.x - pointA.transform.position.x) - (pointB.transform.position.x - pos.x)) / ((pointB.transform.position.x - pointA.transform.position.x));
        slider.updateSliderAmountFromHandler(value);
    }
}
