#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Gradient))]
public class GradientEditorScript : Editor {

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        Gradient myGradient = (Gradient)target;
        if(GUILayout.Button("Refresh Gradient"))
        {
            myGradient.RefreshGradient();
        }
    }
}
#endif