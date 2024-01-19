using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class UserManager : MonoBehaviour
{
    private ScriptableObjectUser _userSO;

    public bool EndFinishLoadData;
    private void OnEnable()
    {
        EndFinishLoadData = false;
        Initialize();
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
}
