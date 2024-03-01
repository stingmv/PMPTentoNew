using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Notifications : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private Transform content;

    private void OnEnable()
    {
        GameEvents.AddNotificationPanel += AddNotification;
    }

    private void OnDisable()
    {
        GameEvents.AddNotificationPanel -= AddNotification;
    }

    private void AddNotification(string text)
    {
        var textChild = prefab.GetComponentInChildren<TextMeshProUGUI>() as TextMeshProUGUI;
        if (textChild != null)
        {
            textChild.SetText(text);
        }
        Instantiate(prefab, content);
    }
}
