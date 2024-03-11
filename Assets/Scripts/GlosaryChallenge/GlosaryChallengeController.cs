using System;
using System.Collections;
using System.Collections.Generic;
using ScriptableCreator;
using UnityEngine;

public class GlosaryChallengeController : MonoBehaviour
{
    [SerializeField] private ConceptAndDefinitionSO _conceptAndDefinitionSo;
    [SerializeField] private ButtonsGroup _concepts;
    [SerializeField] private ButtonsGroup _definitions;
    [SerializeField] private TimerGC _timerGC;
    [SerializeField] private float  _maxTime;

    private bool _useTimer;
    private List<int> _randomIndices;
    private readonly int _indicesPool = 207;
    private readonly int _maxSelectedIndices = 5;
    private int _numberOfSelectedIndices = 0;
    

    public bool UseTimer
    {
        get => _useTimer;
        set => _useTimer = value;
    }

    private void Start()
    {
        _randomIndices = GenerateRandomList(_indicesPool);
        for (int i = 0; i < _maxSelectedIndices; i++)
        {
            var conceptAndDefinition = _conceptAndDefinitionSo.list[_randomIndices[i]];
            _concepts.Options[i].SetData(conceptAndDefinition.concept, conceptAndDefinition.id);
            _definitions.Options[i].SetData(conceptAndDefinition.definition, conceptAndDefinition.id);
            
        }
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
    
    public List<int> GenerateRandomList(int size)
    {
        // Crea una lista ordenada del 1 al 207
        List<int> numbers = new List<int>();
        for (int i = 1; i <= size; i++)
        {
            numbers.Add(i);
        }

        // Mezcla aleatoriamente la lista utilizando el algoritmo de Fisher-Yates
        System.Random random = new System.Random();
        for (int i = 0; i < numbers.Count; i++)
        {
            int randomIndex = random.Next(i, numbers.Count);
            (numbers[randomIndex], numbers[i]) = (numbers[i], numbers[randomIndex]);
        }
        return numbers;
    }
}
