using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class NotificationPanel
{
    public string Title;
    public string Message;
}

[CreateAssetMenu(fileName = "NotificationData", menuName = "ScriptableObjects/NotificationData")]
public class NotificationData : ScriptableObject
{
    public List<NotificationPanel> NotificationList = new List<NotificationPanel>();

    public void AddNotification(NotificationPanel notificationPanel)
    {
        NotificationList.Add(notificationPanel);
    }
}
