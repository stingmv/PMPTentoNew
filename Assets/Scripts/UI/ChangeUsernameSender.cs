using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChangeUsernameSender : SenderEvent
{
    private TMP_InputField _usernameField;

    private void OnEnable()
    {
        if (!_usernameField)
        {
            _usernameField = GetComponent<TMP_InputField>();
        }
        
        _usernameField.onEndEdit.AddListener(OnEndEdit);
    }

    private void OnEndEdit(string username)
    {
        Debug.Log("termino de editar username");
        SendEvent();
    }

    protected override void SendEvent()
    {
        GameEvents.UsernameChanged?.Invoke(_usernameField.text);
    }
}
