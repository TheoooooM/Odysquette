using System.Collections.Generic;
using Cinemachine;
using SOHNE.Accessibility.Colorblindness;
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

    [Space] 
    [SerializeField] private GameObject reportGam = null;
    public GameObject ReportGam => reportGam;
    [SerializeField] private GameObject reporScreenshottGam = null;
    public GameObject ReporScreenshottGam => reporScreenshottGam;

    [HideInInspector] public Rect currentRectLimitation;
    private Vector3 offSet;
    private float baseCamY = 0;

    bool deathSet = false;
    private Vector3 deathCameraPos;
    
    private void Start() {
        if (useCameraAsRail) cameraMain.transform.position = new Vector3(minPosX.position.x, 0, -10);
        if (Colorblindness.Instance != null && CommandConsoleRuntime.Instance != null) {
            CommandConsole COLORBLIND = new CommandConsole("colorblind", "colorblind : change colorblind settings", new List<CommandClass>() {new CommandClass(typeof(ColorblindTypes))}, (value) => { Colorblindness.Instance.Change((int) System.Enum.Parse(typeof(ColorblindTypes), value[0])); });
            CommandConsoleRuntime.Instance.AddCommand(COLORBLIND);
        }

        baseCamY = cameraMain.transform.position.y;
    }

    private void Update() {
        if ((!player.gameObject.activeSelf  || (CommandConsoleRuntime.Instance != null && CommandConsoleRuntime.Instance.ObjectChild.activeSelf) || (UIManager.Instance != null && UIManager.Instance.PauseMenu.activeSelf) || reportGam.activeSelf || reporScreenshottGam.activeSelf)) return;
       
        if (!HealthPlayer.Instance.isDeath)
        {
            if (!useCameraAsRail) {
                Vector3 mousePos = cameraMain.ScreenToWorldPoint(Input.mousePosition);

                float camPosX = (mousePos.x - cameraMain.transform.position.x) * 1 / 3;
                float camPosY = (mousePos.y - cameraMain.transform.position.y) * 1 / 3;
            
                if (GameManager.Instance == null) {
                    offSet = new Vector3(player.position.x, player.position.y, -10) + cameraShake.CameraShakeOffset;
                }
                else {
                    if (GameManager.Instance.isMouse) offSet = player.position + new Vector3(camPosX, camPosY, -10) + cameraShake.CameraShakeOffset;
                    else offSet = (player.position + new Vector3(GameManager.Instance.ViewPad.x, GameManager.Instance.ViewPad.y, -10)) + cameraShake.CameraShakeOffset;
                }
            }
            else {
                offSet = new Vector3(Mathf.Clamp(player.transform.position.x, minPosX.position.x, maxPosX.position.x), baseCamY, -10) + cameraShake.CameraShakeOffset;
            }
            cameraMain.transform.position = offSet;
        }
        else
        {
            if (!deathSet)
            {
                //Debug.Log($"Player.position : {player.position}, offset : {offSet}, cameraMain.transform.position : {cameraMain.transform.position}");
                deathCameraPos = cameraMain.transform.position;
            }

            if (  (cameraMain.transform.position.x > player.position.x + 1 || cameraMain.transform.position.x < player.position.x - 1) ||(cameraMain.transform.position.y > player.position.y + 1 || cameraMain.transform.position.y < player.position.y - 1)) {
                offSet = ((Vector3) ((Vector2) player.position - (Vector2) cameraMain.transform.position).normalized * 0.3f + cameraMain.transform.position);
                cameraMain.transform.position = offSet;
            }
        }
    }
    
    
    private void OnDrawGizmos() {
        if (minPosX != null && maxPosX != null) {
            Gizmos.DrawLine(minPosX.position, maxPosX.position);
        }
    }
}