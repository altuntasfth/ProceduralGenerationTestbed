using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PerlinGrapher))]
public class PerlinGrapherHandles : Editor
{
    private void OnSceneGUI()
    {
        PerlinGrapher handle = (PerlinGrapher)target;
        if (handle == null)
            return;   
        
        Handles.color = Color.blue;
        Handles.Label(handle.lr.GetPosition(0) + Vector3.up * 2f, "Layer: " + handle.gameObject.name);
    }
}
