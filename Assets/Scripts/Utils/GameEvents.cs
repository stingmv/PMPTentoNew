using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameEvents
{
    // Username changed
    public static Action<string> UsernameChanged;
    // New username
    public static Action<string> NewUsername;

    #region Login events

    public static Action<User> SuccessfulLogin;
    public static Action<string> ErrorLogin;
    public static Action<string> FailedLogin;
    public static Action UsernameSelected;
    public static Action<int> NewInstuctorId;
    public static Action InstructorSelected;

    #endregion

    #region Main menu events

    public static Action<int> InstructorChanged;


    #endregion

    #region Question and options events

    public static Action CorrectlyAnswered;
    public static Action IncorrectlyAnswered;
    public static Action GameWon;
    public static Action GameLost;

    public static Action<Domains> DomainsRetreived;
    public static Action<string> DomainsSaved;
    
    public static Action<List<Task>> TaskRetreived;
    public static Action<int> GetIdDomain;
    public static Action<int> GetIdTask;
    public static Action<string> GetNameExam;
    public static Action<string> GetUserExam;
    public static Action<ResponseOfRegisterExam> ExamCreated;
    public static Action<QuestionInformationExam> QuestionsRetrieved;
    public static Action<int> ExamObtained;
    public static Action QuestionReady;
    
    
    #endregion

    #region Experience

    public static Action<float> RequestExperienceChange;
    public static Action<float> ExperienceChanged;

    #endregion
    
    #region Coins

    public static Action<float> RequestCoinsChange;
    public static Action<float> CoinsChanged;

    #endregion

    #region Learning mode

    public static Action<Vector3> ChosenPlatform;
    public static Action<PlatformItem.PlatformInformation> SendInformation;

    #endregion

    #region Notifications

    public static Action<int> RecoveryAttempt;

    #endregion

    #region Survivor Challenge

    public static Action RecoveryTime;

    #endregion


    #region PowerUps
    

    #endregion
}
