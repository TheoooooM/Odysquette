using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyFeedBackDelay : MonoBehaviour
{
    [SerializeField]
    private UnityEventDelay[] unityEventDelaysList;

  

   public void StartUnityEventWithDelay(int index)
   {
       StartCoroutine(InvokeUnityEventWithDelay(index));
   }
    private IEnumerator InvokeUnityEventWithDelay(int index)
    {
        yield return new WaitForSeconds(unityEventDelaysList[index].delay);
        unityEventDelaysList[index].unityEvent.Invoke();
    }
    [Serializable]
   public class UnityEventDelay
    {
        public UnityEvent unityEvent;
        public float delay;
     
    }
}
