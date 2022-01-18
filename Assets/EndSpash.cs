using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndSpash : MonoBehaviour
{
    public void WSendPlayerDeath()
    {
        Debug.Log(Time.timeScale);
        GetComponent<Animator>().enabled = false;
        HealthPlayer.Instance.OnDeathPlayer();
    }
}
