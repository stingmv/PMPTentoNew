using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionAnimation : MonoBehaviour
{
    [SerializeField] private AnimationCurve _animationPositionCurve;
    [SerializeField] private AnimationCurve _animationAphaCurve;
    [SerializeField] private float _positionAnimationTime;
    [SerializeField] private float _alphaAnimationTime;
    [SerializeField] private CanvasGroup _options;
    [SerializeField] private float _delay;

    public void StartAnimation()
    {
        StartCoroutine(IStartAnimationAlpha());
        StartCoroutine(IStartAnimationPosition());
    }

    IEnumerator IStartAnimationPosition()
    {
        Vector3 lastPosition = transform.localPosition;
        Vector3 initPosition = transform.localPosition + new Vector3(0, -100, 0);
        _options.transform.localPosition = initPosition;
        yield return new WaitForSeconds(_delay);
        float currentTime = 0;

        Debug.Log(initPosition);
        while (currentTime <=1)
        {
            currentTime += Time.deltaTime / _positionAnimationTime;
            _options.transform.localPosition = Vector3.Lerp(initPosition, lastPosition, _animationPositionCurve.Evaluate(currentTime)); 
            yield return null;
        }
    }
    IEnumerator IStartAnimationAlpha()
    {
        _options.alpha = 0;
        float currentTime = 0;
        while (currentTime <=1)
        {
            currentTime += Time.deltaTime / _alphaAnimationTime;
            _options.alpha = _animationAphaCurve.Evaluate(currentTime);
            yield return null;
        }
    }
}
