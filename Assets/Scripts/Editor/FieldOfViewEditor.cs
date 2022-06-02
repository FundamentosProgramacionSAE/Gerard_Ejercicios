#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using AI.Manager;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EnemyLocomotionManager))]
public class FieldOfViewEditor : Editor
{
    private void OnSceneGUI()
    {
        EnemyLocomotionManager fov = (EnemyLocomotionManager) target;
        
        DrawView(fov);
    }
    

    private static void DrawView(EnemyLocomotionManager fov)
    {
        Handles.color = Color.white;

        Handles.DrawWireArc(fov._enemyManager.visionPosition.position, Vector3.up, Vector3.forward, 360, fov._enemyManager.VisionEnemy.ViewRadius);

        Vector3 viewAngleA = fov.DirectionFromAngle(-fov._enemyManager.VisionEnemy.ViewAngle / 2, false);
        Vector3 viewAngleB = fov.DirectionFromAngle(fov._enemyManager.VisionEnemy.ViewAngle / 2, false);

        Handles.color = new Color(0, 0, 0, .5f);
        Handles.DrawSolidArc(fov._enemyManager.visionPosition.position, Vector3.up, viewAngleA, fov._enemyManager.VisionEnemy.ViewAngle, fov._enemyManager.VisionEnemy.ViewRadius);

        Handles.color = Color.cyan;
        Handles.DrawLine(fov._enemyManager.visionPosition.position, fov._enemyManager.visionPosition.position + viewAngleA * fov._enemyManager.VisionEnemy.ViewRadius);
        Handles.DrawLine(fov._enemyManager.visionPosition.position, fov._enemyManager.visionPosition.position + viewAngleB * fov._enemyManager.VisionEnemy.ViewRadius);
    }
    
    
}
#endif