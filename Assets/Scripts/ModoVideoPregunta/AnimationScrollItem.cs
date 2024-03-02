using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationScrollItem : MonoBehaviour
{
    [SerializeField] private OptionAnimation _option1;
    [SerializeField] private OptionAnimation _option2;
    [SerializeField] private OptionAnimation _option3;
    [SerializeField] private OptionAnimation _option4;
    [SerializeField] private CanvasGroup _question;
    [SerializeField] private AnimationCurve _animationCurve;
    [SerializeField] private float _optionAnimationTime;
    
    [ContextMenu("start animation")]
    public void StartAnimation()
    {
        // StartCoroutine(IStartAnimation());
        _option1.StartAnimation();
        _option2.StartAnimation();
        _option3.StartAnimation();
        _option4.StartAnimation();
    }

    // IEnumerator IStartAnimation()
    // {
    //     float currentTime = 0;
    //     Vector3 lastPosition = _options.transform.localPosition;
    //     Vector3 initPosition = _options.transform.localPosition + new Vector3(0, -100, 0);
    //     Debug.Log(initPosition);
    //     while (currentTime <=1)
    //     {
    //         currentTime += Time.deltaTime / _optionAnimationTime;
    //         _options.alpha = _animationCurve.Evaluate(currentTime);
    //         _options.transform.localPosition = Vector3.Lerp(initPosition, lastPosition, currentTime); 
    //         yield return null;
    //     }
    // }
}