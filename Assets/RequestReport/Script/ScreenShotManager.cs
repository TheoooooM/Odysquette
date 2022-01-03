using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace ReportRequest
{
public class ScreenShotManager : MonoBehaviour
{
    
     [HideInInspector] public bool isOpenScreenShotPanel;
    
     [HideInInspector][SerializeField]
    private GameObject screenShotPanel;

    [HideInInspector]
    [SerializeField]
    private Image screenShotPrevisualisation;
    private Vector2 pivot = new Vector2(0.5f, 0.5f);
    WaitForEndOfFrame frameEnd = new WaitForEndOfFrame();            
    int width = 1920;
    int height = 1080; 
    Rect texRect = new Rect(0, 0, 1920, 1080);
    private ReportRequestManager reportRequestManager;
    private void Start()
    {
        reportRequestManager = GetComponent<ReportRequestManager>();
    }
    
    public  IEnumerator MakeScreenShot()
    {
        if (!reportRequestManager.isOpenReportRequestPanel)
        {
            yield return frameEnd;
            Texture2D tex = new Texture2D(width, height, TextureFormat.RGB24, false);
            // Read screen contents into the texture
            tex.ReadPixels(new Rect(0, 0, width, height), 0, 0);
            tex.Apply();
            Sprite spriteForPreview =  Sprite.Create(tex, texRect, pivot);
            screenShotPrevisualisation.sprite = spriteForPreview;
            isOpenScreenShotPanel = true;
            screenShotPanel.SetActive(true);
            Time.timeScale = 0;
        }
    }

    public void SaveScreenShot()
    {
        reportRequestManager.currentScreenShot = screenShotPrevisualisation.sprite;
        isOpenScreenShotPanel = false;
        screenShotPanel.SetActive(false);
        reportRequestManager.OpenRequestReportPanel();
    }

    public void CloseScreenShotPanel()
    {
        isOpenScreenShotPanel = false;
        screenShotPanel.SetActive(false);
        Time.timeScale = 1;
    }
    

}
}
