using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AchievementUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI currentGoodStreakText;
    [SerializeField] private TextMeshProUGUI maxGoodStreakText;

    public void SetCurrentGoodStreakText(int value) 
    {
        currentGoodStreakText.SetText(value.ToString());
    }

    public void SetMaxGoodStreakText(int value)
    {
        maxGoodStreakText.SetText(value.ToString());
    }
}
