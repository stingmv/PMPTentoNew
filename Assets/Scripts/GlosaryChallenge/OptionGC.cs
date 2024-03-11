using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OptionGC : MonoBehaviour
{
    [SerializeField] private ButtonsGroup _buttonsGroup;
    [SerializeField] private TextMeshProUGUI _label;
    
    [SerializeField] private Image _image;
    [SerializeField] private Color _correctColor;
    [SerializeField] private Color _incorrectColor;
    [SerializeField] private Color _colorInGC;
    [SerializeField] private EventTrigger _eventTrigger;

    private int _id;
    private bool _selectedInGC;

    public int ID
    {
        get => _id;
        set => _id = value;
    }

    public void SetData(string description, int id)
    {
        _label.text = description;
        ID = id;
    }
    
    public void DisableOption()
    {
        _eventTrigger.enabled = false;
    }
        
    public void EnableOption()
    {
        _label.color = Color.black;
        _image.color = Color.white;
        _eventTrigger.enabled = true;
        ShowOption();
    }
    public void HideOption()
    {
        gameObject.SetActive(false);
    }
    public void ShowOption()
    {
        gameObject.SetActive(true);
    }
    public bool IsDisable()
    {
        return !gameObject.activeInHierarchy;
    }
    public void SendSelectOption()
    {
        _buttonsGroup.SelectButton(this);
        
    }

    public void SelectButton()
    {
        _image.color = _colorInGC;
        _selectedInGC = true;
    }

    public void DeselectButton()
    {
        _image.color = Color.white;
        _selectedInGC = false;
    }
}
