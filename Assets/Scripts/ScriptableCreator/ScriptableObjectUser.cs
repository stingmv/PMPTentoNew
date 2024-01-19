using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class User
{
    public string id;
    public int idProveedor;
    public int cursos;
    public int idAlumno;
    public int idContacto;
    public string clave;
    public string userName;
    public string token;
    public bool tipoCarrera;
    public string correoEnc;
    public string telEnc;
    public string userAgent;
    public string userIp;
    public Excepcion excepcion;
}
[Serializable]
public class Excepcion
{
    public bool excepcionGenerada;
    public string descripcionGeneral;
    // public string error;
}

[Serializable]
public class UserInfo
{
    public User user;
    public bool haveUser;
    public bool haveUsername;
    public string username;
    public bool haveInstructor;
    public int idInstructor;
    public float totalExperience;
    public float totalCoins;
}
[CreateAssetMenu(fileName = "User Data", menuName = "User data")]
public class ScriptableObjectUser : ScriptableObject
{
    public UserInfo userInfo;

    private void OnEnable()
    {
        GameEvents.UsernameChanged += GameEvents_NameChanged;
        GameEvents.NewUsername += GameEvents_NewUsername;
        GameEvents.NewInstuctorId += GameEvents_NewInstuctorId;
        GameEvents.RequestExperienceChange += GameEvents_RequestExperienceChange;
        GameEvents.RequestCoinsChange += GameEvents_RequestCoinsChange;

    }

    private void GameEvents_RequestExperienceChange(float obj)
    {
        userInfo.totalExperience += obj;
        PlayerPrefs.SetFloat("TotalExperience", userInfo.totalExperience);
        PlayerPrefs.Save();
        GameEvents.ExperienceChanged?.Invoke(userInfo.totalExperience);
    }

    private void OnDisable()
    {
        GameEvents.UsernameChanged -= GameEvents_NameChanged;
        GameEvents.NewUsername -= GameEvents_NewUsername;
        GameEvents.NewInstuctorId -= GameEvents_NewInstuctorId;
        GameEvents.RequestExperienceChange -= GameEvents_RequestExperienceChange;
        GameEvents.RequestCoinsChange -= GameEvents_RequestCoinsChange;
    }

    private void GameEvents_RequestCoinsChange(float obj)
    {
        userInfo.totalCoins += obj;
        PlayerPrefs.SetFloat("TotalCoins", userInfo.totalCoins);
        PlayerPrefs.Save();
        GameEvents.CoinsChanged?.Invoke(userInfo.totalCoins);
    }

    private void GameEvents_NewInstuctorId(int index)
    {
        userInfo.haveInstructor = true;
        userInfo.idInstructor = index;
        PlayerPrefs.SetInt("HaveInstructor", index);
        PlayerPrefs.Save();

        GameEvents.InstructorSelected?.Invoke();

    }

    private void GameEvents_NameChanged(string username)
    {
        userInfo.username = username;
        PlayerPrefs.SetString("Username",username);
        PlayerPrefs.Save();
    }
    private void GameEvents_NewUsername(string username)
    {
        userInfo.haveUsername = true;
        userInfo.username = username;
        PlayerPrefs.SetString("Username",username);
        PlayerPrefs.SetInt("HaveUsername",1);
        PlayerPrefs.Save();
        GameEvents.UsernameSelected?.Invoke();
    }
}
