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
        var notifications = notificationData.NotificationListContainer.NotificationList;
        var textChild = prefab.GetComponentInChildren<TextMeshProUGUI>();
        

        foreach (var item in notifications)
        {
            textChild.SetText(item.Message);
            var message = Instantiate(prefab, content);
            message.GetComponent<NotificationMessage>().SetNotificationPanel(item);
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
