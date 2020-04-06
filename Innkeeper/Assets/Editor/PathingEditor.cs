using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TableBehavior))]
public class PathingEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        TableBehavior myTableBehavior = (TableBehavior)target;
    }
}
