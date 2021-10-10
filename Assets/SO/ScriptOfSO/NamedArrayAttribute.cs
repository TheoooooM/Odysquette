using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class NamedArrayAttribute : PropertyAttribute
{
    public string type;
   
    public UnityEngine.Color[] colorr = new Color[]{ Color.red*2, Color.magenta*2, new Color(1,0.64f,0f, 1f)*2, Color.cyan*2, Color.gray*2, new Color(0.59f,0.32f,0.31f,1)*2, Color.green*2,  Color.white*2 };
    public NamedArrayAttribute( string type)
    {
        this.type = type;
   
    }
}




