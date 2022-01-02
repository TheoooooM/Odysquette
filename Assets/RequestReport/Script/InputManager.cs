using UnityEngine;
using UnityEngine.InputSystem;
using ReportRequest;
public class InputManager : MonoBehaviour
{
    private ScreenShotManager screenShotManager;
    private ReportRequestManager reportRequestManager;

    private void Start()
    {
        screenShotManager = GetComponent<ScreenShotManager>();
        reportRequestManager = GetComponent<ReportRequestManager>();
    }

    public void InputScreenShot(InputAction.CallbackContext callbackContext)
   {

      if(callbackContext.performed)
      StartCoroutine(screenShotManager.MakeScreenShot());
    }
    public void InputRequestReport(InputAction.CallbackContext callbackContext)
    {
       
        if(callbackContext.performed)
        reportRequestManager.OpenRequestReportPanel();
      
    }


}
