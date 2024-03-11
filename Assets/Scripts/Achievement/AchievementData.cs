using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AchievementData", menuName = "ScriptableObjects/AchievementData")]
public class AchievementData : ScriptableObject
{
    [Serializable]
    public class Achievement 
    {
        public string Name;
        public int MaxCounter;
        public int CurrentCounter;
        public int MaxLevel;
        public int CurrentLevel;
        public int MaxCounterDifficulty;
        public int GiftsObtained;
        public List<GiftData> GiftData;
    }

    public List<Achievement> achievementList;

    private void OnEnable()
    {
        LoadDataPlayerPrefs();
    }

    public void AddCounter(int index)
    {
        var achievement = achievementList[index];
        achievement.CurrentCounter++;

        if (achievement.CurrentCounter >= achievement.MaxCounter)
        {
            achievement.CurrentLevel++;
            achievement.CurrentCounter = 0;
            AddMaxCounterDifficulty(index);
            AddGiftsObtained(index);
        }
        SaveDataPlayerPrefs();
    }

    private void AddMaxCounterDifficulty(int index)
    {
        var achievement = achievementList[index];
        achievement.MaxCounter += achievement.MaxCounterDifficulty;
        SaveDataPlayerPrefs();
    }

    private void AddGiftsObtained(int index)
    {
        var achievement = achievementList[index];
        achievement.GiftsObtained++;
        SaveDataPlayerPrefs();
    }

    public void RemoveGiftsObtained(Achievement achievement)
    {
        achievement.GiftsObtained = 0;
        SaveDataPlayerPrefs();
    }

    private void SaveDataPlayerPrefs()
    {
        PlayerPrefs.SetString("AchievData", JsonUtility.ToJson(achievementList));
        PlayerPrefs.Save();
    }

    private void LoadDataPlayerPrefs()
    {
        if (PlayerPrefs.HasKey("AchievData"))
        {
            JsonUtility.FromJsonOverwrite(PlayerPrefs.GetString("AchievData"), achievementList);
        }
    }
}
