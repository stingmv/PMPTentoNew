using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ChooseDomainInformation : MonoBehaviour
{
    [SerializeField] private bool _reciveEvents;
    [SerializeField] private UnityEvent _onReciveEvent;
    public void Show()
    {
        UIEvents.ShowDomainInformation?.Invoke();
    }

    private void OnEnable()
    {
        if (_reciveEvents)
        {
            UIEvents.ShowDomainInformation += UIEvents_ShowDomainInformation;
        }
    }

    private void OnDisable()
    {
        if (_reciveEvents)
        {
            UIEvents.ShowDomainInformation -= UIEvents_ShowDomainInformation;
        }
    }

    private void UIEvents_ShowDomainInformation()
    {
        _onReciveEvent?.Invoke();
    }
}
