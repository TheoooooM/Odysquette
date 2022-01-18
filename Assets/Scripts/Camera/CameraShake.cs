using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [SerializeField] private Vector2 cameraShakeOffset = Vector2.zero;
    public Vector3 CameraShakeOffset => cameraShakeOffset;

    /// <summary>
    /// Create a camera Shake
    /// </summary>
    /// <param name="duration"></param>
    /// <param name="magnitude"></param>
    public void CreateCameraShake(float duration, float magnitude) => StartCoroutine(ShakeCamera(duration, magnitude));
    
    /// <summary>
    /// Create a camera shake for the end of the boss
    /// </summary>
    /// <param name="magnitude"></param>
    public void CreateBossCameraShake(float magnitude) => StartCoroutine(ShakeCamera(7, magnitude));

    /// <summary>
    /// Camera Shake
    /// </summary>
    /// <param name="duration"></param>
    /// <param name="magnitude"></param>
    /// <returns></returns>
    private IEnumerator ShakeCamera(float duration, float magnitude) {
        cameraShakeOffset = Vector2.zero;
        float timePassed = 0f;

        while (timePassed < duration) {
            float x = Random.Range(-1, 1) * magnitude;
            float y = Random.Range(-1, 1) * magnitude;

            cameraShakeOffset = new Vector2(x, y);
            timePassed += Time.deltaTime;

            yield return null;
            yield return null;
        }
        
        cameraShakeOffset = Vector2.zero;
    }

}
