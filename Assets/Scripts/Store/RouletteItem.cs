
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class RouletteItem : MonoBehaviour
{
    [SerializeField] private Transform _originPoint;
    [SerializeField] private RouletteItemData _rouletteItemData;
    [SerializeField] private Image _imageItem;
    [SerializeField] private TextMeshProUGUI _amountLabel;
    private bool _haveInformation;

    public Image ImageItem => _imageItem;
    public TextMeshProUGUI AmountLabel => _amountLabel;
    private int _amount;

    public int Amount => _amount;
    public bool HaveInformation
    {
        get => _haveInformation;
        set => _haveInformation = value;
    }

    public RouletteItemData RouletteItemData
    {
        get => _rouletteItemData;
        set => _rouletteItemData = value;
    }
    public void SetData(RouletteItemData rouletteItemData)
    {
        _rouletteItemData = rouletteItemData;
        _amount = !_rouletteItemData.UseAllAmount ? Random.Range(1, _rouletteItemData.Amount + 1) : _rouletteItemData.Amount;
        _imageItem.sprite = _rouletteItemData._ItemRouletteSo.spritePowerUp;
        _amountLabel.text = $"x{_amount}";
        HaveInformation = true;
    }
}
