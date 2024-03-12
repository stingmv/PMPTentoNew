using System;
using System.Collections;
using System.Collections.Generic;
using ScriptableCreator;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StoreController : MonoBehaviour
{
    [SerializeField] private ScriptableObjectUser _user;
    [Header("Power ups scriptable objects")]
    [SerializeField] private ScripableObjectPowerUp _powerUpSecondOportunity;
    [SerializeField] private ScripableObjectPowerUp _powerUpTrueOption;
    [SerializeField] private ScripableObjectPowerUp _powerUpDeleteOption;
    [SerializeField] private ScripableObjectPowerUp _powerUpNextQuestion;
    [SerializeField] private ScripableObjectPowerUp _powerUpMoreTime;

    [Header("Power ups scriptable objects")]
    [SerializeField] private Sprite _spriteSecondOportunity;
    [SerializeField] private Sprite _spriteTrueOption;
    [SerializeField] private Sprite _spriteDeleteOption;
    [SerializeField] private Sprite _spriteNextQuestion;
    [SerializeField] private Sprite _spriteMoreTime;
    
    [Header("General")] 
    [SerializeField] private Transform _GeneralContainer;
    [SerializeField] private StoreSection _storeSectionPrefab;
    [SerializeField] private StoreItem _storeItemPrefab;
    [SerializeField] private Transform _offset;
    [SerializeField] private TextMeshProUGUI _totalCoinsLabel;
    [SerializeField] private TextMeshProUGUI _totalExperienceLabel;
    [SerializeField] private TextMeshProUGUI _usernameLabel;

    [Header("Pop-up compra")] 
    [SerializeField] private FadeUI _popupCompra;
    [SerializeField] private TextMeshProUGUI _messageCompra;
    [SerializeField] private Image _imageCompra;
    [SerializeField] private TextMeshProUGUI _amountLabel;

    private StoreItem _currentItem;

    public float CoinsFromUser => _user.userInfo.user.detail.totalCoins;
    private void OnEnable()
    {
        _usernameLabel.text = _user.userInfo.user.detail.usernameG;
        _totalCoinsLabel.text = _user.userInfo.user.detail.totalCoins.ToString();
        _totalExperienceLabel.text = _user.userInfo.user.detail.totalExperience.ToString();
        GameEvents.CoinsChanged += GameEvents_CoinsChanged;
        GameEvents.ExperienceChanged += GameEvents_ExperienceChanged;
        var itemInstantiaded = Instantiate(_storeSectionPrefab, _GeneralContainer);
        
        // Segunda oportunidad
        
        itemInstantiaded.SetData("Segunda oportunidad");
        for (int i = 0; i < 3; i++)
        {
            var storeItem = Instantiate(_storeItemPrefab, itemInstantiaded.Container);
            float costItem = 0 ;
            if (i == 0)
            {
                costItem = _powerUpSecondOportunity.unitCost;
            }
            else
            {
                costItem = (_powerUpSecondOportunity.unitCost * (i + 1) - _powerUpSecondOportunity.discount *(i + 1));

            }
            storeItem.SetData(this, costItem, i + 1, _spriteSecondOportunity, _powerUpSecondOportunity);
        }
        
        // Verdadera opcion
        
        itemInstantiaded = Instantiate(_storeSectionPrefab, _GeneralContainer);
        itemInstantiaded.SetData("Opción correcta");
        for (int i = 0; i < 3; i++)
        {
            var storeItem = Instantiate(_storeItemPrefab, itemInstantiaded.Container);
            float costItem = 0 ;
            if (i == 0)
            {
                costItem = _powerUpTrueOption.unitCost;
            }
            else
            {
                costItem = (_powerUpTrueOption.unitCost * (i + 1) - _powerUpTrueOption.discount *(i + 1));

            }
            storeItem.SetData(this, costItem, i + 1, _spriteTrueOption, _powerUpTrueOption);
        }
        
        // Eliminar opcion
        
        itemInstantiaded = Instantiate(_storeSectionPrefab, _GeneralContainer);
        itemInstantiaded.SetData("Eliminar opción");
        for (int i = 0; i < 3; i++)
        {
            var storeItem = Instantiate(_storeItemPrefab, itemInstantiaded.Container);
            float costItem = 0 ;
            if (i == 0)
            {
                costItem = _powerUpDeleteOption.unitCost;
            }
            else
            {
                costItem = (_powerUpDeleteOption.unitCost * (i + 1) - _powerUpDeleteOption.discount *(i + 1));

            }
            storeItem.SetData(this, costItem, i + 1, _spriteDeleteOption, _powerUpDeleteOption);
        }
        
        // Siguiente pregunta
        
        itemInstantiaded = Instantiate(_storeSectionPrefab, _GeneralContainer);
        itemInstantiaded.SetData("Siguiente pregunta");
        for (int i = 0; i < 3; i++)
        {
            var storeItem = Instantiate(_storeItemPrefab, itemInstantiaded.Container);
            float costItem = 0 ;
            if (i == 0)
            {
                costItem = _powerUpNextQuestion.unitCost;
            }
            else
            {
                costItem = (_powerUpNextQuestion.unitCost * (i + 1) - _powerUpNextQuestion.discount *(i + 1));

            }
            storeItem.SetData(this, costItem, i + 1, _spriteNextQuestion, _powerUpNextQuestion);
        }
        
        // Mas Tiempo
        
        itemInstantiaded = Instantiate(_storeSectionPrefab, _GeneralContainer);
        itemInstantiaded.SetData("Mas tiempo");
        for (int i = 0; i < 3; i++)
        {
            var storeItem = Instantiate(_storeItemPrefab, itemInstantiaded.Container);
            float costItem = 0 ;
            if (i == 0)
            {
                costItem = _powerUpMoreTime.unitCost;
            }
            else
            {
                costItem = (_powerUpMoreTime.unitCost * (i + 1) - _powerUpMoreTime.discount *(i + 1));

            }
            storeItem.SetData(this, costItem, i + 1, _spriteMoreTime, _powerUpMoreTime);
        }
        
        Instantiate(_offset, _GeneralContainer);
    }
    
    private void GameEvents_ExperienceChanged(float obj)
    {
        _totalExperienceLabel.text = obj.ToString();
    }

    private void GameEvents_CoinsChanged(float obj)
    {
        _totalCoinsLabel.text = obj.ToString();
        // _totalCoinsLabel.text = _user.userInfo.totalCoins.ToString();
    }

    public void OpenPopUpCompra(StoreItem storeItem)
    {
        _currentItem = storeItem;
        _imageCompra.sprite = _currentItem.SpriteFromImage;
        string pot = _currentItem.Amount > 1 ? "potenciador" : "potenciadores";
        _messageCompra.text = $"Está a punto de comprar {_currentItem.Amount} {pot} para {GetPowerUpName()}";
        _amountLabel.text = $"x{_currentItem.Amount}";
        _popupCompra.gameObject.SetActive(true);
        _popupCompra.FadeInTransition();
    }

    public void BuyItem()
    {
        GameEvents.RequestCoinsChange?.Invoke(-_currentItem.Cost);
        _currentItem.PowerUp.amount += _currentItem.Amount;
        PlayerPrefs.SetInt(_currentItem.PowerUp.nameInPlayerPrefs, _currentItem.PowerUp.amount);
        PlayerPrefs.Save();
        _currentItem.PowerUp.Raise();
    }
    public string GetPowerUpName()
    {
        switch (_currentItem.NamePowerUp)
        {
            case "pu_deleteOption":
                return "descartar alternativa"; 
            case "pu_moreTime":
                return "aumento de tiempo";
            case "pu_nextQuestion":
                return "saltar pregunta";
            case "pu_secondOportunity":
                return "segunda oportunidad";
            case "pu_trueOption":
                return "opción correcta";
            default:
                return "";
        }
    }
}
