using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Question;
using ScriptableCreator;
using UnityEngine;

public class TrainingChallengeController : MonoBehaviour
{
    [SerializeField] private DataToRegisterSO _registerSo;
    [SerializeField] private DomainsAndTaskSO _domainsAndTask;
    [SerializeField] private ScriptableObjectSettings _gameSettings;
    [SerializeField] private ScriptableObjectUser _userData;
    
    [SerializeField] private QuestionController _questionController;
    [SerializeField] private PMPService _pmpService;
    [SerializeField] private RewardItemController _rewardItemController;
    
    private float _numberOfConsecutiveQuestion;
    private float _experienceAccumulated = 0;
    private float _coinsAccumulated = 0;
    private void Start()
    {
        GetQuestions();
    }
    
    private void GetQuestions()
    {

        _pmpService.Service_GetQuestions(9682);
        UIEvents.ShowLoadingView?.Invoke();
        // GameEvents.GetNameExam?.Invoke(DateTime.Now.ToString(CultureInfo.CurrentCulture));
    }

    private void OnEnable()
    {
        GameEvents.CorrectlyAnswered += GameEvents_CorrectlyAnswered;
        GameEvents.IncorrectlyAnswered += GameEvents_IncorrectlyAnswered;
        GameEvents.GameWon += GameEvents_GameWon;
        GameEvents.GameLost += GameEvents_GameWon;
    }

    private void OnDisable()
    {
        GameEvents.CorrectlyAnswered -= GameEvents_CorrectlyAnswered;
        GameEvents.IncorrectlyAnswered -= GameEvents_IncorrectlyAnswered;
        GameEvents.GameWon -= GameEvents_GameWon;
        GameEvents.GameLost -= GameEvents_GameWon;
    }

    private void GameEvents_CorrectlyAnswered()
    {
        
        _numberOfConsecutiveQuestion++;
        var clampConsecutive = Mathf.Clamp(_numberOfConsecutiveQuestion, 0, int.MaxValue);
        var exp = 
            // Base experience
            _gameSettings.settingData.DSReward.baseExperience +
            // Bonus by consecutive question
            _gameSettings.settingData.DSReward.aditionalBonusExpConsecutiveQuestion * clampConsecutive +
            // Bonus by achievement
            _gameSettings.settingData.DSReward.aditionalBonusExpForAchievement * 0;
        GameEvents.RequestExperienceChange?.Invoke(exp);
        _experienceAccumulated += exp;

        var coins =
            // Base coins
            _gameSettings.settingData.DSReward.baseCoins +
            // Bonus by consecutive question
            _gameSettings.settingData.DSReward.aditionalBonusCoinsConsecutiveQuestion * clampConsecutive +
            // Bonus by achievement
            _gameSettings.settingData.DSReward.aditionalBonusCoinsForAchievement * 0;
        _coinsAccumulated += coins;
        GameEvents.RequestCoinsChange?.Invoke(coins);

        CheckNumberOfConsecutiveQuestion();
    }
    
    private void GameEvents_IncorrectlyAnswered()
    {
        _numberOfConsecutiveQuestion = -2;
        Debug.Log("GameEvents_IncorrectlyAnswered");

    }
    
    private void GameEvents_GameWon()
    {
        _rewardItemController.AddCoins((int)_coinsAccumulated);
        _rewardItemController.AddExperience((int)_experienceAccumulated);
        Debug.Log("GameEvent_GameWon");
        UIEvents.ShowFinishView?.Invoke();
    }

    public void CheckNumberOfConsecutiveQuestion()
    { 
        if (_numberOfConsecutiveQuestion >= 0) {
            GameEvents.OnGoodStreaked?.Invoke();
        }
    }
}
