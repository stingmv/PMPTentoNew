using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using JetBrains.Annotations;
using Question;
using ScriptableCreator;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class TrainingChallengeController : MonoBehaviour
{
    [SerializeField] private DataToRegisterSO _registerSo;
    [SerializeField] private DomainsAndTaskSO _domainsAndTask;
    [SerializeField] private DataToRegisterSO _registerExam;
    [SerializeField] private ScriptableObjectSettings _gameSettings;
    [SerializeField] private ScriptableObjectUser _userData;
    // [SerializeField] private TextMeshProUGUI _DomainLabelInQuestions;
    [SerializeField] private TextMeshProUGUI _TaskLabelInQuestions;
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

        // _pmpService.Service_GetQuestions(9682);
        UIEvents.ShowLoadingView?.Invoke();
        // GameEvents.GetNameExam?.Invoke(DateTime.Now.ToString(CultureInfo.CurrentCulture));
        var task = _domainsAndTask.DomainContainer.listaTarea[
            Random.Range(
                0,
                _domainsAndTask.DomainContainer.listaTarea.Length
            )
        ];
        var response = _pmpService.Service_GetDomainAndTaskNames(task.id);
        // _DomainLabelInQuestions.text = response.Item1;
        _TaskLabelInQuestions.text = $"Tarea: {response.Item2}";
        _registerExam.dataToRegisterExam.IdSimuladorPmpTarea = task.id;
        _registerExam.dataToRegisterExam.IdSimuladorPmpDominio = _domainsAndTask.DomainContainer.listaDominio.FirstOrDefault( x => x.id == task.idSimuladorPmpDominio)!.id;
        GameEvents.GetNameExam?.Invoke($"ModoAprendizaje-{_userData.userInfo.user.detail.usernameG}-{task.id}-{task.idSimuladorPmpDominio}-{DateTime.Now.ToString(CultureInfo.CurrentCulture)}");

    }

    private void OnEnable()
    {
        GameEvents.CorrectlyAnswered += GameEvents_CorrectlyAnswered;
        GameEvents.IncorrectlyAnswered += GameEvents_IncorrectlyAnswered;
        GameEvents.GetNameExam	+= GameEvents_GetNameExam;
        GameEvents.GameWon += GameEvents_GameWon;
        GameEvents.GameLost += GameEvents_GameWon;
    }
    

    private void GameEvents_GetNameExam(string obj)
    {
        _pmpService.Service_GetExam();
    }

    private void OnDisable()
    {
        GameEvents.CorrectlyAnswered -= GameEvents_CorrectlyAnswered;
        GameEvents.IncorrectlyAnswered -= GameEvents_IncorrectlyAnswered;
        GameEvents.GetNameExam	-= GameEvents_GetNameExam;
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
