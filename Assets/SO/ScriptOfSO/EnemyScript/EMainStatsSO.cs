using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "EMainStatsSO", menuName = "EMainStatsSO", order = 1)]
public class EMainStatsSO : ScriptableObject
{
    private void OnValidate()
    {
        Debug.Log(timeCondition.Count);
        foreach (var element in timeCondition)
        {
            Debug.Log(element.Value);
        }
    }

    public string typeName;
   public float maxHealth;
   public float dragForKnockUp;
   public bool isKnockUp;
   public Sprite sprite;
   public List<StateEnemySO> stateEnnemList = new List<StateEnemySO>(4);
   public StateEnemySO baseState;
   public Dictionary<int, float> timeCondition = new Dictionary<int, float>(1);
 
   
   public Dictionary<int, float> healthCondition = new Dictionary<int, float>(1);
   public SpriteRenderer spriteRenderer;
    public float giverUltimateStrawPoints;
  

}
    
