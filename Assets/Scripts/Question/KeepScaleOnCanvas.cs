using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using UnityEngine;

public class KeepScaleOnCanvas : MonoBehaviour
{
    [SerializeField] private ScriptableObjectInstructor _instructors;
    [SerializeField] private ScriptableObjectUser _user;
    [SerializeField] private GameObject _instructorPrefab;
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
        _guideTransform =
            Instantiate(_instructors.instructors[_user.userInfo.idInstructor].prefab, transform.position, Quaternion.Euler(new Vector3(8.3f, 180f, 0f)), transform.parent).transform;
        _guideTransform.localRotation= Quaternion.Euler(new Vector3(8.6f, 180f, 0f));
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
