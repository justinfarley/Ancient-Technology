using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using TMPro;
using System.Runtime.CompilerServices;

[CustomEditor(typeof(PatrolEnemy))]
public class FieldOfViewEditor : Editor
{
    private void OnSceneGUI()
    {
        PatrolEnemy fov = target as PatrolEnemy;
        Handles.color = Color.white;
        Handles.DrawWireArc(fov.fov.transform.position, Vector3.forward, Vector2.right, 360, fov.fov.viewRadius);

        Vector3 viewAngleA = fov.fov.DirFromAngle((fov.isFacingRight ? -fov.fov.viewAngle / 2 + 90 : -fov.fov.viewAngle / 2 - 90), false);
        Vector3 viewAngleB = fov.fov.DirFromAngle((fov.isFacingRight ? fov.fov.viewAngle / 2 + 90 : fov.fov.viewAngle / 2 - 90), false);

        Handles.DrawLine(fov.fov.transform.position, fov.fov.transform.position + viewAngleA * fov.fov.viewRadius);
        Handles.DrawLine(fov.fov.transform.position, fov.fov.transform.position + viewAngleB * fov.fov.viewRadius);
    }
}
