using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlosaryChallengeController : MonoBehaviour
{
    [SerializeField] private ButtonsGroup _concepts;
    [SerializeField] private ButtonsGroup _definitions;
    [SerializeField] private TimerGC _timerGC;
    [SerializeField] private float  _maxTime;

    private bool _useTimer;
    

    public bool UseTimer
    {
        get => _useTimer;
        set => _useTimer = value;
    }
    public void Evaluate()
    {
        if (_concepts.OldSelectedButton && _definitions.OldSelectedButton)
        {
            Debug.Log("evaluando");
        }
        // else
        // {
        //     Debug.Log("No Se puede evaluar, no se encuentra pareja");
        // }
    }
    private void Update()
    {
        if (!UseTimer)
        {   
            return;
        }
        // _currentTime += Time.deltaTime / timeToQuestions;
        // if (_currentTime >1)
        // {
        //     UseTimer = false;
        //     GameEvents.GameLost?.Invoke();
        //     return;
        // }
        // _timer.SetValueTimer(1 - _currentTime);
    }
}
