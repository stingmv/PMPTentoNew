using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PathToInstantiateInstructor : MonoBehaviour
{
    [SerializeField] private float _radious;
    [SerializeField] private float _angle;
    [SerializeField] private int numInstants;

    private float _angleRad = 0;
    private void OnDrawGizmos()
    {
        Handles.DrawWireDisc(Vector3.zero, Vector3.up, _radious);
        _angleRad =Mathf.Deg2Rad *_angle;

        for (int i = 0; i < numInstants; i++)
        {
            Gizmos.DrawSphere(new Vector3(_radious * Mathf.Sin(_angleRad * i), 0, _radious * Mathf.Cos(_angleRad * i)), .1f);
        }
        
    }

    private void Start()
    {
        _angleRad =Mathf.Deg2Rad *_angle;
    }

    public Vector3 GetPositionToInstantiate(int index)
    {
        return new Vector3(_radious * Mathf.Sin(_angleRad * index), 0, _radious * Mathf.Cos(_angleRad * index));
    }
}
