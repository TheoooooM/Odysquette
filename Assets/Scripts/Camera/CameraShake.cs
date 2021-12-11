using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [SerializeField] private Vector2 cameraShakeOffset = Vector2.zero;
    public Vector3 CameraShakeOffset => cameraShakeOffset;

    public void CreateCameraShake(float duration, float magnitude) => StartCoroutine(ShakeCamera(duration, magnitude));
    
    private IEnumerator ShakeCamera(float duration, float magnitude) {
        cameraShakeOffset = Vector2.zero;
        float timePassed = 0f;

        while (timePassed < duration) {
            float x = Random.Range(-1, 1) * magnitude;
            float y = Random.Range(-1, 1) * magnitude;

            cameraShakeOffset = new Vector2(x, y);
            timePassed += Time.deltaTime;

            yield return null;
        }
        
        cameraShakeOffset = Vector2.zero;
    }
}
