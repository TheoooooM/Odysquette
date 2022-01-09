using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LockSliderManager : MonoBehaviour
{
    
    
    [SerializeField] private Image fill;
    

    public void ActivateFill()
    {
        fill.enabled = true;
    }
    public void DesactivateFill()
    {
        fill.enabled = false;
    }
}
