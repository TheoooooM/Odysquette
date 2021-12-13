using System;
using Cinemachine;
using UnityEngine;

public class CameraControllers : MonoBehaviour {
    #region Instance
    public static CameraControllers Instance;
    private void Awake() => Instance = this;
    #endregion Instance

    [Header("MAIN DATA")] 
    [SerializeField] private Transform player;
    [SerializeField] private CameraShake cameraShake;
    [SerializeField] private CinemachineVirtualCamera _cinemachineVirtualCamera;
    [SerializeField] private Camera cameraMain = null;
    [Space]

    [Header("CAMERA RAIL")] 
    [SerializeField] private bool useCameraAsRail = false;
    [SerializeField] private Transform minPosX, maxPosX = null;
    
    [HideInInspector] public Rect currentRectLimitation;
    private Vector3 offSet;

    private void Start() { if (useCameraAsRail) cameraMain.transform.position = new Vector3(minPosX.position.x, 0, -10); }

    private void Update() {
        if (!player.gameObject.activeSelf) return;
        
        if (!useCameraAsRail) {
            Vector3 mousePos = cameraMain.ScreenToWorldPoint(Input.mousePosition);

            float camPosX = (mousePos.x - cameraMain.transform.position.x) * 1 / 3;
            float camPosY = (mousePos.y - cameraMain.transform.position.y) * 1 / 3;
            
            
            
            if (GameManager.Instance == null) {
                offSet = new Vector3(player.position.x, player.position.y, -10) + cameraShake.CameraShakeOffset;
            }
            else {
                if (GameManager.Instance.isMouse) offSet = player.position + new Vector3(camPosX, camPosY, -10) + cameraShake.CameraShakeOffset;
                //else offSet = (player.position + (Vector3) GameManager.Instance.ViewPad * distanceCurveY.Evaluate(1));
            }
        }
        else {
            offSet = new Vector3(Mathf.Clamp(player.transform.position.x, minPosX.position.x, maxPosX.position.x), cameraMain.transform.position.y, -10);
        }
        
        cameraMain.transform.position = offSet;
    }
    
    private void OnDrawGizmos() {
        if (minPosX != null && maxPosX != null) {
            Gizmos.DrawLine(minPosX.position, maxPosX.position);
        }
    }
}