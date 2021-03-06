using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "EMainStatsSO", menuName = "EMainStatsSO", order = 1)]
public class EMainStatsSO : ScriptableObject
{
  

    public string typeName;
   public float maxHealth;
   public float dragForKnockUp;
   public bool isKnockUp;
   public Sprite sprite;
   public List<StateEnemySO> stateEnnemList = new List<StateEnemySO>(4);
   public StateEnemySO baseState;

   public SpriteRenderer spriteRenderer;
    public int  coeifficentUltimateStrawPoints;
  

}
    
