using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PopupAchievementUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI achievementText;

    public void ShowAchievementMaxGoodStreakText(int value)
    {
        string text = $"{value} preguntas seguidas";
        achievementText.SetText(text);
        AddMessageNotificationPanel(text);
    }

    public void ShowAchievementGoodWithoutErrors()
    {
        string text = "Completó la ronda sin errores";
        achievementText.SetText(text);
        AddMessageNotificationPanel(text);
    }

    private void AddMessageNotificationPanel(string text)
    {
        NotificationPanel notificationPanel = new NotificationPanel { 
            Title = text,
            Message = text
        };
        GameEvents.AddNotificationPanel?.Invoke(notificationPanel);
    }
}
