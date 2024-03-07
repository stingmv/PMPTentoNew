using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PopupAchievementUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI achievementText;

    public void ShowAchievementMaxGoodStreakText(int value)
    { 
        achievementText.SetText($"{value} preguntas seguidas");
    }

    public void ShowAchievementGoodWithoutErrors()
    {
        achievementText.SetText("Completo la ronda sin errores");
    }
}
