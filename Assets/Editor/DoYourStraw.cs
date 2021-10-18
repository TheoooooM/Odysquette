using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DoYourStraw : EditorWindow
{
    [MenuItem("Tools/StrawMode")]
    public static void OpenShootModeWindow() => GetWindow<DoYourStraw>();
}
