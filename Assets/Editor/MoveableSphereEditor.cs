using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SmoothPath)), CanEditMultipleObjects]
public class MoveableSphereEditor : Editor
{
    private List<NodePath> positions;
    private Vector3 newPosition;

    private void Awake()
    {
        SmoothPath moveableSphere = (SmoothPath)target;
        positions = new List<NodePath>();
        for (int i = 0; i < moveableSphere._pathData.ControlPoints.Length; i++)
        {
            var node = new NodePath();
            node.startPoint = moveableSphere._pathData.ControlPoints[i].startPoint;
            node.startTangent = moveableSphere._pathData.ControlPoints[i].startTangent;
            node.endPoint = moveableSphere._pathData.ControlPoints[i].endPoint;
            node.endTangent = moveableSphere._pathData.ControlPoints[i].endTangent;
            positions.Add(node);
        }
    }

    void OnSceneGUI()
    {
        SmoothPath moveableSphere = (SmoothPath)target;
        EditorGUI.BeginChangeCheck();

        // Mostrar una asa para mover la esfera
        if (moveableSphere._pathData.ControlPoints.Length != positions.Count)
        {
            positions.Clear();
            Debug.LogWarning("Actualizando marcadores");
            for (int i = 0; i < moveableSphere._pathData.ControlPoints.Length; i++)
            {
                var node = new NodePath();
                node.startPoint = moveableSphere._pathData.ControlPoints[i].startPoint;
                node.startTangent = moveableSphere._pathData.ControlPoints[i].startTangent;
                node.endPoint = moveableSphere._pathData.ControlPoints[i].endPoint;
                node.endTangent = moveableSphere._pathData.ControlPoints[i].endTangent;
                positions.Add(node);
            }
        }
        for (int i = 0; i < moveableSphere._pathData.ControlPoints.Length; i++)
        {
            if (i - 1 >-1 )
            {
                
                
            }
            else
            {
                positions[i].startPoint = Handles.PositionHandle(moveableSphere._pathData.ControlPoints[i].startPoint, moveableSphere._pathData.ControlPoints[i].startRotation);
                positions[i].startRotation = Handles.RotationHandle(moveableSphere._pathData.ControlPoints[i].startRotation, positions[i].startPoint);

                // positions[i].startTangent = Handles.PositionHandle(moveableSphere._ControlPoints[i].startTangent, Quaternion.identity);
                // positions[i].endTangent = Handles.PositionHandle(moveableSphere._ControlPoints[i].endTangent, Quaternion.identity);
            }
            positions[i].startTangent = Handles.PositionHandle(moveableSphere._pathData.ControlPoints[i].startTangent, Quaternion.identity);
            positions[i].endTangent = Handles.PositionHandle(moveableSphere._pathData.ControlPoints[i].endTangent, Quaternion.identity);
            positions[i].endPoint = Handles.PositionHandle(moveableSphere._pathData.ControlPoints[i].endPoint, moveableSphere._pathData.ControlPoints[i].endRotation);
            positions[i].endRotation = Handles.RotationHandle(moveableSphere._pathData.ControlPoints[i].endRotation, positions[i].endPoint);
        }
        if (EditorGUI.EndChangeCheck())
        {
            // Actualizar la posición de la esfera si se cambia
            // Undo.RecordObject(moveableSphere.transform, "Move Sphere");
            Undo.RecordObject(moveableSphere._pathData, "Move Sphere");
            Debug.Log(moveableSphere._pathData.name);
            AssetDatabase.Refresh();
            for (int i = 0; i < moveableSphere._pathData.ControlPoints.Length; i++)
            {
                moveableSphere._pathData.ControlPoints[i].startPoint = positions[i].startPoint;
                moveableSphere._pathData.ControlPoints[i].startTangent = positions[i].startTangent;
                moveableSphere._pathData.ControlPoints[i].endPoint = positions[i].endPoint;
                moveableSphere._pathData.ControlPoints[i].endTangent = positions[i].endTangent;
                moveableSphere._pathData.ControlPoints[i].endRotation = positions[i].endRotation;
                moveableSphere._pathData.ControlPoints[i].startRotation = positions[i].startRotation;
            }

            EditorUtility.SetDirty(moveableSphere._pathData);
        }
    }
}