using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Handles3D;
using Question;
using ScriptableCreator;
using UI.Button;
using UnityEngine;


public class VideoQuestionModeController : MonoBehaviour
{
    [SerializeField] private DataToRegisterSO _registerExam;
    [SerializeField] private ScriptableObjectSettings _gameSettings;

    [SerializeField] private QuestionController _questionController;
    [SerializeField] private PMPService _pmpService;
    private float _numberOfConsecutiveQuestion;

    private float _experienceAccumulated = 0;
    private float _coinsAccumulated = 0;
    // [Header("Reward")] 
    
    private void Awake()
    {
        _numberOfConsecutiveQuestion = -2;
        _pmpService.Service_GetDomainAndTasks();
    }

    private void Start()
    {
        GetQuestions();
    }
    

    private void OnEnable()
    {
        GameEvents.DomainsRetreived += GameEvents_DomainRetreived;
        GameEvents.TaskRetreived += GameEvents_TaskRetreived;
        GameEvents.GetNameExam += GameEvents_GetNameExam;
        GameEvents.CorrectlyAnswered += GameEvents_CorrectlyAnswered;
        GameEvents.IncorrectlyAnswered += GameEvents_IncorrectlyAnswered;
        GameEvents.GameWon += GameEvents_GameWon;
        GameEvents.GameLost += GameEvents_GameWon;
    }

    private void GameEvents_GameWon()
    {
        UIEvents.ShowFinishView?.Invoke();
    }

    private void GameEvents_IncorrectlyAnswered()
    {
        _numberOfConsecutiveQuestion = -2;
        
    }

    private void GameEvents_CorrectlyAnswered()
    {
        _numberOfConsecutiveQuestion++;
        var clampConsecutive = Mathf.Clamp(_numberOfConsecutiveQuestion, 0, int.MaxValue);
        var exp = 
            // Base experience
            _gameSettings.settingData.MCReward.baseExperience +
            // Bonus by consecutive question
            _gameSettings.settingData.MCReward.aditionalBonusExpConsecutiveQuestion * clampConsecutive +
            // Bonus by achievement
            _gameSettings.settingData.MCReward.aditionalBonusExpForAchievement * 0;
        GameEvents.RequestExperienceChange?.Invoke(exp);
        _experienceAccumulated += exp;

        var coins =
            // Base coins
            _gameSettings.settingData.MCReward.baseCoins +
            // Bonus by consecutive question
            _gameSettings.settingData.MCReward.aditionalBonusCoinsConsecutiveQuestion * clampConsecutive +
            // Bonus by achievement
            _gameSettings.settingData.MCReward.aditionalBonusCoinsForAchievement * 0;
        _coinsAccumulated += coins;
        GameEvents.RequestCoinsChange?.Invoke(coins);
    }

    private void GameEvents_GetNameExam(string obj)
    {
        _pmpService.Service_GetExam();
    }

    private void GameEvents_TaskRetreived(List<Task> obj)
    {
        // _moveItems.SetData(obj);
    }

    private void OnDisable()
    {
        GameEvents.DomainsRetreived -= GameEvents_DomainRetreived;
        GameEvents.TaskRetreived -= GameEvents_TaskRetreived;
        GameEvents.GetNameExam -= GameEvents_GetNameExam;
        GameEvents.CorrectlyAnswered -= GameEvents_CorrectlyAnswered;
        GameEvents.IncorrectlyAnswered -= GameEvents_IncorrectlyAnswered;
        GameEvents.GameWon -= GameEvents_GameWon;
        GameEvents.GameLost -= GameEvents_GameWon;

    }

    private void GameEvents_DomainRetreived(Domains obj)
    {
        for (int i = 0; i < obj.listaDominio.Length; i++)
        {
            // _buttonDomainController.CreateButton(obj.listaDominio[i].nombre, obj.listaDominio[i].id.ToString());
        }
    }

    public void GetTasks()
    {
        // _pmpService.Service_GetTask(int.Parse(_buttonDomainController.CurrentButton.IndexS));
    }

    public void GetQuestions()
    {

        _pmpService.Service_GetQuestions(9682);
        UIEvents.ShowLoadingView?.Invoke();
        // GameEvents.GetNameExam?.Invoke(DateTime.Now.ToString(CultureInfo.CurrentCulture));
    }
}
