using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PathToInstantiateInstructor : MonoBehaviour
{
    [SerializeField] private float _radious;
    [SerializeField] private int _sectors;
    [SerializeField] private int numInstants;

    private void OnDrawGizmos()
    {
        Handles.DrawWireDisc(Vector3.zero, Vector3.up, _radious);
        var angle = numInstants / 360;
        for (int i = 0; i < numInstants; i++)
        {
            Gizmos.DrawSphere(new Vector3(_radious * Mathf.Cos(angle * i), 0, _radious * Mathf.Sin(angle)), .1f);
        }
        
    }
}
