using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(Physics_Animation_Blend))]

public class Physics_Animation_Blend_Editor : Editor
{
    public override void OnInspectorGUI()
    {
        Physics_Animation_Blend PAblend = (Physics_Animation_Blend)target;

        if(DrawDefaultInspector())
        {

        }

        if(GUILayout.Button("Map Transforms"))
        {
            PAblend.MapTransforms();
        }
    }
}
