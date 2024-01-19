using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class PathItem
{
    public SmoothPath smoothPath;
    public bool isInverse;
}

public class PathController : MonoBehaviour
{
    // [SerializeField]PathItem
    [Header("Paths")] 
    [SerializeField] private SmoothPath _mainToRanking;
    [SerializeField] private SmoothPath _mainToStore;
    [SerializeField] private SmoothPath _mainToTrain;
    [SerializeField] private SmoothPath _mainToAchievement;
    [SerializeField] private SmoothPath _storeToTrain;
    [SerializeField] private SmoothPath _storeToAchievement;
    [SerializeField] private SmoothPath _storeToRanking;
    [SerializeField] private SmoothPath _storeToMain;
    [SerializeField] private SmoothPath _achievementToStore;
    [SerializeField] private SmoothPath _achievementToTrain;
    [SerializeField] private SmoothPath _achievementToRanking;
    [SerializeField] private SmoothPath _achievementToMain;
    [SerializeField] private SmoothPath _rankingToStore;
    [SerializeField] private SmoothPath _rankingToTrain;
    [SerializeField] private SmoothPath _rankingToAchievement;
    [SerializeField] private SmoothPath _rankingToMain;

    [SerializeField] private PathState _currentPathState;

    private SmoothPath _toMain;
    private SmoothPath _toStore;
    private SmoothPath _toTrain;
    private SmoothPath _toAchievement;
    private SmoothPath _toRanking;
    private SmoothPath _currentPath;

    public enum PathState
    {
        none,
        mainToRanking,
        mainToStore,
        mainToTrain,
        maintoAchievement,
        storeToTrain,
        storeToMain,
        storeToAchievement,
        storeToRanking,
        achievementToStore,
        achievementToTrain,
        achievementToRanking,
        achievementToMain,
        rankingToStore,
        rankingToTrain,
        rankingToAchievement,
        rankingToMain
    }

    public PathState CurrentPath
    {
        get => _currentPathState;
        set
        {
            _currentPathState = value;
            switch (_currentPathState)
            {
                case PathState.mainToRanking:
                case PathState.storeToRanking:
                case PathState.achievementToRanking:
                    _toMain = _rankingToMain;
                    _toStore = _rankingToStore;
                    _toTrain = _rankingToTrain;
                    _toAchievement = _rankingToAchievement;
                    break;

                case PathState.mainToStore:
                case PathState.achievementToStore:
                case PathState.rankingToStore:
                    _toMain = _storeToMain;
                    _toTrain = _storeToTrain;
                    _toAchievement = _storeToAchievement;
                    _toRanking = _storeToRanking;
                    break;
                
                case PathState.maintoAchievement:
                case PathState.storeToAchievement:
                case PathState.rankingToAchievement:
                    _toMain = _achievementToMain;
                    _toStore = _achievementToStore;
                    _toTrain = _achievementToTrain;
                    _toRanking = _achievementToRanking;
                    break;
                
                case PathState.storeToMain:
                case PathState.achievementToMain:
                case PathState.rankingToMain:
                    _toStore = _mainToStore;
                    _toTrain = _mainToTrain;
                    _toAchievement = _mainToAchievement;
                    _toRanking = _mainToRanking;
                    break;
            }
        }
    }

    private void Start()
    {
        _toMain = null;
        _toAchievement = _mainToAchievement;
        _toRanking = _mainToRanking;
        _toTrain = _mainToTrain;
        _toStore = _mainToStore;
        gameObject.SetActive(false);
    }

    public void ToMain()
    {
        gameObject.SetActive(true);
        CurrentPath = PathState.storeToMain;
        _toMain.StartTransition();
    }

    public void ToStore()
    {
        gameObject.SetActive(true);
        CurrentPath = PathState.mainToStore;
        _toStore.StartTransition();
    }

    public void ToTrain()
    {
        gameObject.SetActive(true);
        CurrentPath = PathState.mainToTrain;
        _toTrain.StartTransition();
    }

    public void ToAchievement()
    {
        gameObject.SetActive(true);
        CurrentPath = PathState.maintoAchievement;
        _toAchievement.StartTransition();
    }

    public void ToRanking()
    {
        gameObject.SetActive(true);
        CurrentPath = PathState.mainToRanking;
        _toRanking.StartTransition();
    }
}