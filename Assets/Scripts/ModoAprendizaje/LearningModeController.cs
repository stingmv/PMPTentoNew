using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Handles3D;
using ModoAprendizaje;
using Question;
using ScriptableCreator;
using UI.Button;
using UnityEngine;


public class LearningModeController : MonoBehaviour
{
    [SerializeField] private DataToRegisterSO _registerExam;
    [SerializeField] private DomainsAndTaskSO _domainsAndTask;
    [SerializeField] private ScriptableObjectSettings _gameSettings;
    [SerializeField] private ScriptableObjectUser _userData;

    [SerializeField] private QuestionController _questionController;
    [SerializeField] private PMPService _pmpService;
    [SerializeField] private RewardItemController _rewardItemController;
    [SerializeField] private PlatformController _platformController;
    private float _numberOfConsecutiveQuestion;

    private float _experienceAccumulated = 0;
    private float _coinsAccumulated = 0;
    // [Header("Reward")] 
    
    private void Awake()
    {
        _numberOfConsecutiveQuestion = -2;
        _pmpService.Service_GetDomainAndTasks();
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
        _rewardItemController.AddCoins((int)_coinsAccumulated);
        _rewardItemController.AddExperience((int)_experienceAccumulated);
        Debug.Log("GameEvent_GameWon");
        UIEvents.ShowFinishView?.Invoke();
    }

    private void GameEvents_IncorrectlyAnswered()
    {
        _numberOfConsecutiveQuestion = -2;
        Debug.Log("GameEvents_IncorrectlyAnswered");

    }

    private void GameEvents_CorrectlyAnswered()
    {
        _numberOfConsecutiveQuestion++;
        var clampConsecutive = Mathf.Clamp(_numberOfConsecutiveQuestion, 0, int.MaxValue);
        var exp = 
            // Base experience
            _gameSettings.settingData.MAReward.baseExperience +
            // Bonus by consecutive question
            _gameSettings.settingData.MAReward.aditionalBonusExpConsecutiveQuestion * clampConsecutive +
            // Bonus by achievement
            _gameSettings.settingData.MAReward.aditionalBonusExpForAchievement * 0;
        GameEvents.RequestExperienceChange?.Invoke(exp);
        _experienceAccumulated += exp;

        var coins =
            // Base coins
            _gameSettings.settingData.MAReward.baseCoins +
            // Bonus by consecutive question
            _gameSettings.settingData.MAReward.aditionalBonusCoinsConsecutiveQuestion * clampConsecutive +
            // Bonus by achievement
            _gameSettings.settingData.MAReward.aditionalBonusCoinsForAchievement * 0;
        _coinsAccumulated += coins;
        GameEvents.RequestCoinsChange?.Invoke(coins);
    }

    private void GameEvents_GetNameExam(string obj)
    {
        Debug.Log("GameEvents_GetNameExam");
        _pmpService.Service_GetExam();
    }

    private void GameEvents_TaskRetreived(List<Task> obj)
    {
        Debug.Log("GameEvents_TaskRetreived");

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
        Debug.Log("GameEvents_DomainRetreived");
        // _platformController.SetInstances(obj.listaTarea.Length);
        var ss = -1;
        for (int i = 0; i < obj.listaTarea.Length; i++)
        {
            if (ss != obj.listaTarea[i].idSimuladorPmpDominio)
            {
                //cambio de dominio
                ss = obj.listaTarea[i].idSimuladorPmpDominio;
                _platformController.CreatePlatformInformation();
                i--;
            }
            else
            {
                PlatformItem.PlatformInformation s = new PlatformItem.PlatformInformation
                {
                    id = obj.listaTarea[i].id,
                    attempt = 3,
                    description = obj.listaTarea[i].nombre,
                    index = i + 1,
                    tittle = obj.listaTarea[i].nombre,
                    totalTasks = obj.listaTarea.Length,
                    idDomain = obj.listaTarea[i].idSimuladorPmpDominio
                };
                var item = _platformController.CreatePlatform();
            
                item.Information = s;    
            }
            
        }
        // for (int i = 0; i < obj.listaDominio.Length; i++)
        // {
        //     _buttonDomainController.CreateButton(obj.listaDominio[i].nombre, obj.listaDominio[i].id.ToString());
        // }
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
