using System;
using System.Collections;
using System.Collections.Generic;
using ScriptableCreator;
using UnityEngine;
using UnityEngine.Serialization;

public class UserManager : MonoBehaviour
{
    private ScriptableObjectUser _userSO;
    [SerializeField] private ScripableObjectPowerUp _powerUpSecondOportunity;
    [SerializeField] private ScripableObjectPowerUp _powerUpTrueOption;
    [SerializeField] private ScripableObjectPowerUp _powerUpDeleteOption;
    [SerializeField] private ScripableObjectPowerUp _powerUpNextQuestion;
    [SerializeField] private ScripableObjectPowerUp _powerUpMoreTime;
    public bool EndFinishLoadData;
    private void OnEnable()
    {
        EndFinishLoadData = false;
        Initialize();
        InitPowerUpData();
    }

    

    void Initialize()
    {
        if (!_userSO)
        {
            _userSO = Resources.Load<ScriptableObjectUser>("User Data");
        }

        if (PlayerPrefs.HasKey("UserInfo") )
        {
            _userSO.userInfo.haveUser= true;
            _userSO.userInfo.user = JsonUtility.FromJson<User>(PlayerPrefs.GetString("UserInfo"));
            GameEvents.GetUserExam?.Invoke(_userSO.userInfo.user.userName);
        }
        else
        {
            _userSO.userInfo.haveUser = false;
            _userSO.userInfo.user = new User();
        }
        if (PlayerPrefs.HasKey("HaveUsername") && PlayerPrefs.HasKey("Username"))
        {
            _userSO.userInfo.haveUsername = true;
            _userSO.userInfo.username = PlayerPrefs.GetString("Username");
        }
        else
        {
            _userSO.userInfo.haveUsername = false;
            _userSO.userInfo.username = "";
        }

        if (PlayerPrefs.HasKey("HaveInstructor"))
        {
            _userSO.userInfo.haveInstructor = true;
            _userSO.userInfo.idInstructor = PlayerPrefs.GetInt("HaveInstructor");
        }
        else
        {
            _userSO.userInfo.haveInstructor = false;
            _userSO.userInfo.idInstructor = -1;
        }
        if (PlayerPrefs.HasKey("TotalExperience"))
        {
            _userSO.userInfo.totalExperience = PlayerPrefs.GetFloat("TotalExperience");
        }
        else
        {
            _userSO.userInfo.totalExperience = 100;
        }
        if (PlayerPrefs.HasKey("TotalCoins"))
        {
            _userSO.userInfo.totalCoins = PlayerPrefs.GetFloat("TotalCoins");
        }
        else
        {
            _userSO.userInfo.totalCoins = 100;
        }
        Debug.Log("terminado 1");
        EndFinishLoadData = true;
    }

    private void InitPowerUpData()
    {
        if (PlayerPrefs.HasKey("pu_secondOportunity"))
        {
            _powerUpSecondOportunity.amount = PlayerPrefs.GetInt("pu_secondOportunity");
        }
        else
        {
            _powerUpSecondOportunity.amount = 5;
            PlayerPrefs.SetInt("pu_secondOportunity", _powerUpSecondOportunity.amount);
            PlayerPrefs.Save();
        }
        if (PlayerPrefs.HasKey("pu_trueOption"))
        {
            _powerUpTrueOption.amount = PlayerPrefs.GetInt("pu_trueOption");
        }
        else
        {
            _powerUpTrueOption.amount = 5;
            PlayerPrefs.SetInt("pu_trueOption", _powerUpTrueOption.amount);
            PlayerPrefs.Save();
        }
        if (PlayerPrefs.HasKey("pu_deleteOption"))
        {
            _powerUpDeleteOption.amount = PlayerPrefs.GetInt("pu_deleteOption");
        }
        else
        {
            _powerUpDeleteOption.amount = 5;
            PlayerPrefs.SetInt("pu_deleteOption", _powerUpDeleteOption.amount);
            PlayerPrefs.Save();
        }
        if (PlayerPrefs.HasKey("pu_nextQuestion"))
        {
            _powerUpNextQuestion.amount = PlayerPrefs.GetInt("pu_nextQuestion");
        }
        else
        {
            _powerUpNextQuestion.amount = 5;
            PlayerPrefs.SetInt("pu_nextQuestion", _powerUpNextQuestion.amount);
            PlayerPrefs.Save();
        }
        if (PlayerPrefs.HasKey("pu_moreTime"))
        {
            _powerUpMoreTime.amount = PlayerPrefs.GetInt("pu_moreTime");
        }
        else
        {
            _powerUpMoreTime.amount = 5;
            PlayerPrefs.SetInt("pu_moreTime", _powerUpMoreTime.amount);
            PlayerPrefs.Save();
        }
    }
}
