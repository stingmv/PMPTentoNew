using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplaySound : MonoBehaviour
{
    [SerializeField] AudioManager _audioManager;

    private void OnEnable()
    {
        if (!_audioManager)
        {
            _audioManager = GetComponent<AudioManager>();
        }
        UIEvents.PressLoginButton += UIEvents_PressLoginButton;
        GameEvents.CorrectlyAnswered += GameEvents_CorrectlyAnswered;
        GameEvents.IncorrectlyAnswered += GameEvents_IncorrectlyAnswered;
        GameEvents.GameLost += GameEvents_GameLost;
        GameEvents.GameWon += GameEvents_GameWon;
        UIEvents.ActivateSmoke += UIEvents_ActivateSmoke;
        UIEvents.ActivateExplosion += UIEvents_ActivateExplosion;
    }

   


    private void OnDisable()
    {
        UIEvents.PressLoginButton -= UIEvents_PressLoginButton;
        GameEvents.CorrectlyAnswered -= GameEvents_CorrectlyAnswered;
        GameEvents.IncorrectlyAnswered -= GameEvents_IncorrectlyAnswered;
        GameEvents.GameLost -= GameEvents_GameLost;
        GameEvents.GameWon -= GameEvents_GameWon;
    }

    private void UIEvents_PressLoginButton()
    {
        PlayPressedButtonSound();
    }

    private void PlayPressedButtonSound()
    {
        _audioManager.PlaySFXAtPoint(_audioManager.AudioSettings.TapTouchSound, Vector3.zero, 0f, false);
        Debug.Log("play");
    }
    private void PlayCorrectAnswerSound()
    {
        _audioManager.PlaySFXAtPoint(_audioManager.AudioSettings.SuccessSound, Vector3.zero, 0f, false);
        Debug.Log("play correct answer sound");
    }
    private void PlayIncorrectAnswerSound()
    {
        _audioManager.PlaySFXAtPoint(_audioManager.AudioSettings.FailSound, Vector3.zero, 0f, false);
        Debug.Log("play incorrect answer sound");
    }
    private void PlayBombSmokeSound()
    {
        _audioManager.PlaySFXAtPoint(_audioManager.AudioSettings.BombSmokeSound, Vector3.zero, 0f, true);
        Debug.Log("play bomb smoke sound");
    }
    private void PlayBombExplosionSound()
    {
        _audioManager.PlaySFXAtPoint(_audioManager.AudioSettings.BombExplosionSound, Vector3.zero, 0f, false);
        Debug.Log("play bomb explosion sound");
    }
    private void PlayGameWonSound()
    {
        _audioManager.PlayMusic(_audioManager.AudioSettings.GameWonSound, Vector3.zero, 0f, false);
        Debug.Log("play game won sound");
    }
    private void PlayLostSound()
    {
        _audioManager.PlayMusic(_audioManager.AudioSettings.GameLostSound, Vector3.zero, 0f, false);
        Debug.Log("play game lost sound");
    }
    
    private void GameEvents_IncorrectlyAnswered()
    {
        PlayIncorrectAnswerSound();
    }

    private void GameEvents_CorrectlyAnswered()
    {
        PlayCorrectAnswerSound();
    }
    private void UIEvents_ActivateExplosion()
    {
        PlayBombExplosionSound();
    }

    private void UIEvents_ActivateSmoke()
    {
        PlayBombSmokeSound();
    }
    private void GameEvents_GameWon()
    {
        PlayGameWonSound();
    }

    private void GameEvents_GameLost()
    {
        PlayLostSound();
    }
}
