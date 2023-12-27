using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class Roulette : MonoBehaviour
{

    [SerializeField] private RectTransform _rouletteTransform; // RectTransform de la ruleta
    [SerializeField] private float _speedRotationWithDrag = 5f; // Ajusta la velocidad de rotación
    [SerializeField] private float _velocidadWithoutDrag = 5f; // Ajusta la velocidad de rotación
    [SerializeField] private float _minSpeed = .02f;
    [SerializeField] private float _minSpeedDrag;
    [SerializeField] private float _deceleration;
    [SerializeField] private UnityEvent _onInitRotation;
    [SerializeField] private UnityEvent _onFailedRotation;
    [SerializeField] private UnityEvent _onSelectedItem;
    private float _lastAngle;
    private float _angleDifference;
    private float _direction;
    private bool _isDragging;
    private float _currentRotationSpeed;
    private bool _canRotate;
    private bool _useRoulette;
    void Update()
    {
        if (_useRoulette)
        {
            return;
        }
        Quaternion rotacionObjetivo = Quaternion.Euler(0f, 0f, _angleDifference);
        if (_isDragging)
        {
            // _rouletteTransform.rotation = Quaternion.Slerp(_rouletteTransform.rotation, rotacionObjetivo, _speedRotationWithDrag * Time.deltaTime);            
            _rouletteTransform.rotation = rotacionObjetivo;
        }
        else
        {
            if (_canRotate)
            {
                _currentRotationSpeed = Mathf.SmoothStep(_currentRotationSpeed, 0f, _deceleration * Time.unscaledDeltaTime);
                if (_currentRotationSpeed <= _minSpeed)
                {
                    _currentRotationSpeed = 0;
                    _useRoulette = true;
                    _onSelectedItem?.Invoke();
                }
                _rouletteTransform.Rotate(Vector3.forward, _currentRotationSpeed * Math.Sign(_direction) * _velocidadWithoutDrag);
            }
            else
            {
                _rouletteTransform.rotation = rotacionObjetivo;
            }
            
        }
    }

    private float GetAngle()
    {
        var mousePosition = Input.mousePosition - _rouletteTransform.position;
        return Mathf.Atan2(mousePosition.y, mousePosition.x) * Mathf.Rad2Deg;
    }
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(_rouletteTransform.position, Input.mousePosition - _rouletteTransform.position);
    }
#endif
    public void StartAngle()
    {
        _lastAngle = GetAngle();
    }
    public void DragAngle()
    {
        _isDragging = true;
        float currentAngle  = GetAngle();
        _direction = currentAngle - _lastAngle;
        _angleDifference = _rouletteTransform.eulerAngles.z + _direction;
        _lastAngle = currentAngle;
    }
    public void PointerUp()
    {
        _currentRotationSpeed = Touchscreen.current.delta.magnitude;
        if (_currentRotationSpeed < _minSpeedDrag && _currentRotationSpeed >5)
        {
            _onInitRotation?.Invoke();
            Debug.Log(_currentRotationSpeed + " poca velocidad, agregando velocidad personalizada");
            _currentRotationSpeed += 80;
            _canRotate = true;
        }
        else
        {
            _onFailedRotation?.Invoke();
            _canRotate = false;
        }
        _isDragging = false;
    }
}
