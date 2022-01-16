using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndSpash : MonoBehaviour
{
    public void WSendPlayerDeath()
    {
        Time.timeScale = 1;
        Debug.Log(Time.timeScale);
        GetComponent<Animator>().enabled = false;
        HealthPlayer.Instance.OnDeathPlayer();
    }
}
