#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RaceManager))]
public class RaceManagerEditor : Editor
{

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        RaceManager script = (RaceManager) target;
        GUILayout.Space(14);
        GUILayout.Label("Race Controls");
        if (GUILayout.Button("Add players to game"))
        {
            script.AddAllPlayers();
            script.InitializeRace();
        }
        if(GUILayout.Button("Start Race"))
        {
            script.StartRace();
        }
        if(GUILayout.Button("End Race"))
        {
            script.StopRace();
        }

    }

}

#endif