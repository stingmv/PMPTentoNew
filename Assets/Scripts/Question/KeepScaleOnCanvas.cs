using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using UnityEngine;

public class KeepScaleOnCanvas : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private Transform _guideTransform;

    [SerializeField] private Vector3 _initScale;
    [SerializeField] private float  _factor = 2;
    [SerializeField] private RectTransform _topLimit;

    [SerializeField] private float _minHeight;
    [SerializeField] private float _maxHeight;
    [SerializeField] private float _minResolution;
    [SerializeField] private float _maxResolution;
    public float x;

    private void Start()
    {
        _initScale = _guideTransform.localScale;
    }

    private void Update()
    {
        if (_camera != null && _guideTransform != null)
        {
            _guideTransform.position = transform.position;
            var y = transform.position.z - _topLimit.position.z;
            Debug.Log(y);

            x = (y * (_minResolution - _maxResolution) + _maxResolution * _minHeight - _minResolution *  _maxHeight) / (_minHeight - _maxHeight);
            _guideTransform.localScale = Vector3.one * x;
        }
    }
}
