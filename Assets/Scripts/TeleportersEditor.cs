using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof (Teleporters))]
public class TeleportersEditor : Editor
{
    Teleporters teleporterSettings;

    public void OnEnable()
    {
        teleporterSettings = (Teleporters) target;
    }

    public override void OnInspectorGUI()
    {
        if (teleporterSettings.teleporters.Count == 0)
        {
            EditorGUILayout.HelpBox("Empty", MessageType.Info);
            GUILayout.Button("Start Adding", EditorStyles.miniButton);
        }
        base.OnInspectorGUI();
    }
}
