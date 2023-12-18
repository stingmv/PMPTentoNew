using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(EventTrigger))]
public class ButtonAnimation : MonoBehaviour
{
    [SerializeField] private ScriptableObjectButton _scriptableObjectButton;

    private float _currentTime;
    private Vector3 _startScale;

    private void Start()
    {
        _startScale = transform.localScale;
    }

    public void StartAnimation()
    {
        _currentTime = 0;
        StopAllCoroutines();
        StartCoroutine(IStartAnimation());
    }
    public void StartInverseCustomAnimation()
    {
        _currentTime = 0;
        StopAllCoroutines();
        StartCoroutine(IStartInverseCustomAnimation());
    }
    public void StartInverseAnimation()
    {
        _currentTime = 0;
        StopAllCoroutines();
        StartCoroutine(IStartInverseAnimation());
    }

    IEnumerator IStartAnimation()
    {
        while (_currentTime <= 1)
        {
            _currentTime += Time.deltaTime / _scriptableObjectButton.TimeDuration;
            transform.localScale = _startScale * _scriptableObjectButton.AnimationCurve.Evaluate(_currentTime);
            yield return null;
        }
    }
    IEnumerator IStartInverseCustomAnimation()
    {
        while (_currentTime <= 1)
        {
            _currentTime += Time.deltaTime / _scriptableObjectButton.TimeDuration;
            transform.localScale = _startScale * _scriptableObjectButton.AnimationInverseCurve.Evaluate(1 - _currentTime);
            yield return null;
        }
    }
    IEnumerator IStartInverseAnimation()
    {
        while (_currentTime <= 1)
        {
            _currentTime += Time.deltaTime / _scriptableObjectButton.TimeDuration;
            transform.localScale = _startScale * _scriptableObjectButton.AnimationCurve.Evaluate(1 - _currentTime);
            yield return null;
        }
    }
    
}
