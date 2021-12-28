using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GradientFog))]
public class GradientFogEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GradientFog myScript = (GradientFog)target;
        if (GUILayout.Button("Bake Gradient"))
        {
            myScript.BakeGradient();
        }
    }
}
