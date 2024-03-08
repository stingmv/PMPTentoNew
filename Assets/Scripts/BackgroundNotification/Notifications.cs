using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Notifications : MonoBehaviour
{
    [SerializeField] NotificationData notificationData;
    [SerializeField] private GameObject prefab;
    [SerializeField] private Transform content;

    public void AddNotificationsToPanel()
    {
        ClearChildrenAchievement();
        var notifications = notificationData.NotificationList;
        var textChild = prefab.GetComponentInChildren<TextMeshProUGUI>();

        foreach (var item in notifications)
        {
            textChild.SetText(item.Message);
            Instantiate(prefab, content);
        }
    }

    private void ClearChildrenAchievement()
    {
        for (int i = content.childCount - 1; i >= 0; i--)
        {
            Destroy(content.GetChild(i).gameObject);
        }
    }
}
