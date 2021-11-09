using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "EMainStatsSO", menuName = "EMainStatsSO", order = 1)]
public class EMainStatsSO : ScriptableObject
{
   public string typeName;
   public float maxHealth;
   public float dragforKnockUp;
   public bool isKnockUp;
   public SpriteRenderer spriteRenderer;

}
