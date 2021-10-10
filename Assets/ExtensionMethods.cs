using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtensionMethods 
{



   public static Vector3 Bezier(Vector3 p0, Vector3 p1, Vector3 p2, float t)
   {
       return Vector3.Lerp(Vector3.Lerp(p0, p1, t), Vector3.Lerp(p1, p2, t), t);
   }

   public static Vector3 MinMaxNormalize(this Vector3 v )
   {
       return new Vector3(Mathf.Clamp(v.x, 0, 1), Mathf.Clamp(v.y, 0, 1), Mathf.Clamp(v.z, 0, 1));
   }
   

//elelment 0 soit la base position poru qu'on rajoute deux pont par deux point a chaque fois 
}
