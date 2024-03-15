using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AchievementListContainer
{
    public List<AchievementData.Achievement> achievementList = new List<AchievementData.Achievement>();

}
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
        public List<GiftData.Gift> GiftData;
    }

    public AchievementListContainer achievementListContainer;
    private void OnEnable()
    {
        LoadLocalData();
    }

    public void AddCounter(int index)
    {
        var achievement = achievementListContainer.achievementList[index];
        achievement.CurrentCounter++;

        if (achievement.CurrentCounter >= achievement.MaxCounter)
        {
            achievement.CurrentLevel++;
            achievement.CurrentCounter = 0;
            AddMaxCounterDifficulty(index);
            AddGiftsObtained(index);
        }
        SaveLocalData();
    }

    private void AddMaxCounterDifficulty(int index)
    {
        var achievement = achievementListContainer.achievementList[index];
        achievement.MaxCounter += achievement.MaxCounterDifficulty;
        SaveLocalData();
    }

    private void AddGiftsObtained(int index)
    {
        var achievement = achievementListContainer.achievementList[index];
        achievement.GiftsObtained++;
        SaveLocalData();
    }

    public void RemoveGiftsObtained(Achievement achievement)
    {
        achievement.GiftsObtained = 0;
        SaveLocalData();
    }

    private void SaveLocalData()
    {
        // AchievementData data = CreateInstance<AchievementData>();
        // data.achievementList = new List<Achievement>();
        // data.achievementList.AddRange(achievementList);
        //
        // if (FileManager.WriteToFile("SaveData.dat", JsonUtility.ToJson(data)))
        // {
        //     Debug.Log("Save successful");
        // }
        
        PlayerPrefs.SetString("AchieveData", JsonUtility.ToJson(achievementListContainer));
        PlayerPrefs.Save();
    }

    private void LoadLocalData()
    {
        // if (FileManager.LoadFromFile("SaveData.dat", out var json))
        // {
        //     JsonUtility.FromJsonOverwrite(json, this);
        //
        //     Debug.Log("Load complete");
        // }
        if (PlayerPrefs.HasKey("AchieveData"))
        {
            JsonUtility.FromJsonOverwrite(PlayerPrefs.GetString("AchieveData"), achievementListContainer);
        }
        else
        {
            SaveLocalData();
        }
    }
}
