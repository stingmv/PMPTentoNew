using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UIEvents 
{
    #region Login Scene
    
    public static Action PressLoginButton;
    
    #endregion

    #region Main Menu

    // Show the user setting
    public static Action SettingShow;
    // Show the game modes
    public static Action GameModesShow;
    // Show the game challenges
    public static Action GameChallengesShow;
    // Show the choose instructor view
    public static Action ChooseInstructorShow;
    // Show the notifications
    public static Action NotificationShow;
    
    // Hide the user setting
    public static Action SettingHide;
    // Hide the game modes
    public static Action GameModesHide;
    // Hide the game challenges
    public static Action GameChallengesHide;
    // Hide the choose instructor view
    public static Action ChooseInstructorHide;
    // Hide the notifications
    public static Action NotificationHide;

    #endregion

    #region Power up

    public static Action ActivateExplosion;
    public static Action ActivateSmoke;
    public static Action DeactivateSmoke;

    #endregion

    #region Category Mode

    public static Action ShowLoadingView;
    public static Action ShowQuestionView;
    public static Action ShowFinishView;

    #endregion

    #region Learning Mode

    public static Action ShowDomainInformation;

    #endregion
}