
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AI.States;
using UnityEditor;
using UnityEngine;

public class WaypointSystem : MonoBehaviour
{
    [HideInInspector] public List<Transform> Waypoints = new List<Transform>();
    public PatrolState PatrolState;

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        for (int i = 0; i < Waypoints.Count; i++)
        {
            if(Waypoints[i] == null) Waypoints.RemoveAt(i);
        }

        foreach (var child in gameObject.GetComponentsInChildren<Transform>())
        {
            if (!Waypoints.Contains(child) && child != gameObject.transform)
            {
                Waypoints.Add(child);
            }
        }
        
        if (Waypoints != null && PatrolState)
        {
            GUIStyle style = new GUIStyle();
            style.alignment = TextAnchor.MiddleCenter;
            var startPosition = Waypoints[0].position;
            var previousPosition = startPosition;
            var addMidle = Vector3.zero;
            
            for (int i = 0; i < Waypoints.Count; i++)
            {
               
                Gizmos.color = new Color(0, 0, 1, 0.5f);
                Gizmos.DrawSphere(Waypoints[i].position, 1);
                Gizmos.color = Color.white;
                Gizmos.DrawLine(previousPosition, Waypoints[i].position);
                

                style.normal.textColor = Color.red;
                style.fontSize = 25;
                Handles.Label(Waypoints[i].position + Vector3.up, (i + 1).ToString(), style );

                addMidle += Waypoints[i].position;

                previousPosition = Waypoints[i].position;
            }

            style.normal.textColor = Color.white;
            style.fontSize = 15;
            var middle = addMidle / Waypoints.Count;
            Handles.Label(middle, "Path of " + PatrolState.gameObject.name, style);
            
            if(PatrolState.Recursive) Gizmos.DrawLine(previousPosition, startPosition);
        }
    }
#endif
}

#if UNITY_EDITOR
[CustomEditor(typeof(WaypointSystem))]
public class WaypointSystemEditor : Editor
{
    private WaypointSystem wp;
    private GameObject instance;
    
    private void OnSceneGUI()
    {
        wp = (WaypointSystem) target;
        
        foreach (var t in wp.Waypoints)
        {
            if (t == null) continue;
            
            Undo.RecordObject(t, "Position Waypoints");
            t.position = Handles.PositionHandle(t.position, t.rotation);

        }
        
        
        Handles.BeginGUI();
     
        Rect rect = new Rect(10, 10, 100, 50);
        if (GUI.Button(rect, "Add Waypoint"))
        {
            instance = new GameObject
            {
                transform =
                {
                    parent = wp.gameObject.transform,
                    position = wp.Waypoints[wp.Waypoints.Count - 1].transform.position + new Vector3(5, 0, 5)
                },
                gameObject =
                {
                    name = "Waypoint_" + (wp.Waypoints.Count + 1).ToString()
                }
                
            };
            Undo.RegisterCreatedObjectUndo(instance, "Create object");
            wp.Waypoints.Add(instance.transform);
        }

        if (GUI.Button(new Rect(rect.x, rect.y + 55, 100, 50), "Remove Last"))
        {
            Undo.DestroyObjectImmediate(wp.Waypoints[wp.Waypoints.Count- 1].gameObject);
        }
        Handles.EndGUI();
     
        SceneView.RepaintAll ();
    }
}
#endif
