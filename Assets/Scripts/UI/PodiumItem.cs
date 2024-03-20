using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PodiumItem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _username;
    [SerializeField] private TextMeshProUGUI _totalExperience;
    [SerializeField] private Image _imageAvatar;
    [SerializeField] private TextMeshProUGUI _position;

    private int _id;
    public void SetData(string position, string username, string totalExperience, int id)
    {
        _position.text = position;
        _username.text = username;
        _totalExperience.text = totalExperience;
        _id = id;
    }
    public void SetData(string username, string totalExperience, int id)
    {
        _username.text = username;
        _totalExperience.text = totalExperience;
        _id = id;
    }
}
