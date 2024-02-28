using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AchievementControlller : MonoBehaviour
{
    [SerializeField] private int maxGoodStreakCounter;
    [SerializeField] private int maxGoodWithoutErrors;


    [SerializeField] private UnityEvent<int> OnGoodStreaked;
    [SerializeField] private UnityEvent<int> OnMaxGoodStreaked;

    private int _currentGoodStreakCounter = 0;
    private int _currentGoodWithoutErrors = 0;

    private void OnEnable()
    {
        GameEvents.OnGoodStreaked += GoodStreak;
        GameEvents.OnGoodStreaked += GoodWithoutErrors;
    }

    private void OnDisable()
    {
        GameEvents.OnGoodStreaked -= GoodStreak;
        GameEvents.OnGoodStreaked -= GoodWithoutErrors;
    }

    private void GoodStreak() 
    {
        _currentGoodStreakCounter++;
        OnGoodStreaked?.Invoke(_currentGoodStreakCounter);

        if (_currentGoodStreakCounter >= maxGoodStreakCounter)
        {
            maxGoodStreakCounter++;
            _currentGoodStreakCounter = 0;
            OnMaxGoodStreaked?.Invoke(maxGoodStreakCounter);
        }
    }

    private void GoodWithoutErrors()
    {
        _currentGoodWithoutErrors++;
    }
}
