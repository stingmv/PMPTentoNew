using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlatformItem : MonoBehaviour
{
    [SerializeField] private Color[] _colors;
    [SerializeField] private Image _image;
    [Serializable]
    public struct PlatformInformation
    {
        public int id;
        public int idDomain;
        public int index;
        public string description;
        public string tittle;
        public int attempt;    
        public int totalTasks;    
    }
    private PlatformInformation _information;

    public PlatformInformation Information
    {
        get => _information;
        set
        {
            _information = value;
            _image.color = _colors[_information.idDomain];
        }
    }
    public void SendInformation()
    {
        GameEvents.SendInformation?.Invoke(Information);
    }
}
