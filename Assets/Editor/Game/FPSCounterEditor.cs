using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Game
{
    [CustomEditor(typeof(FPSCounter))]
    public class FPSCounterEditor : Editor
    {
        private void OnSceneGUI()
        {
            FPSCounter fc = target as FPSCounter;
            Rect sp = SceneView.currentDrawingSceneView.position;
            Rect dest = new Rect(sp.width - 200, sp.height - 110, 180, 60);
            Handles.matrix = Matrix4x4.identity;
            Handles.BeginGUI();
            string s = "FPS: " + Mathf.RoundToInt(fc.FPS);
            DSA.DSAEditorUtils.DrawText(s, dest, 32.0f, Color.black, TextAnchor.LowerRight);
            dest.position += Vector2.one * -2.0f;
            DSA.DSAEditorUtils.DrawText(s, dest, 32.0f, new Color(1.0f, 0.5f, 0.0f), TextAnchor.LowerRight);
            Handles.EndGUI();
        }
    }
}