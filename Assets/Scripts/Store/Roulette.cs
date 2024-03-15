using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Roulette : MonoBehaviour
{
    [SerializeField] private RouletteSO _rouletteSO;
    [SerializeField] private ScriptableObjectUser _userSO;

    [SerializeField] private RectTransform _rouletteTransform; // RectTransform de la ruleta
    [SerializeField] private float _speedRotationWithDrag = 5f; // Ajusta la velocidad de rotación
    [SerializeField] private float _velocidadWithoutDrag = 5f; // Ajusta la velocidad de rotación
    [SerializeField] private float _minSpeed = .02f;
    [SerializeField] private float _minSpeedDrag;
    [SerializeField] private float _deceleration;
    [SerializeField] private UnityEvent _onInitRotation;
    [SerializeField] private UnityEvent _onFailedRotation;
    [SerializeField] private UnityEvent _onSelectedItem;
    [SerializeField] private UnityEvent _onResetRoulette;

    [SerializeField] private List<RouletteItem> _rouletteItems;
    [SerializeField] private Transform _originPoint;
    [SerializeField] private UIParticle _uiParticle;
    [SerializeField] private GameObject _particlePrefab;


    [SerializeField] private FadeUI _rewardContainer;
    [SerializeField] private Image _imageReward;
    [SerializeField] private TextMeshProUGUI _coofigurationMessage;
    [SerializeField] private TextMeshProUGUI _finalAmount;
    
    private float _lastAngle;
    private float _angleDifference;
    private float _direction;
    private bool _isDragging;
    private float _currentRotationSpeed;
    private bool _canRotate;
    private bool _useRoulette;
    private RouletteItem selecetdItem;


    private void Start()
    {
        // Modo 1
        // for (int i = 0; i < _rouletteSO.RouletteItems.Length; i++)
        // {
        //     var s = Random.Range(0, Math.Min(4, freeFieldsTotal) + 1);
        //     for (int j = 0; j < s; j++)
        //     {
        //         var randomIndex = Random.Range(0, _rouletteItems.Count); 
        //         
        //         _rouletteItems[randomIndex].SetData(_rouletteSO.RouletteItems[i]);
        //         Debug.Log(_rouletteSO.RouletteItems[i]._ItemRouletteSo.name + " campo: " + _rouletteItems[randomIndex].name);
        //         _rouletteItems.RemoveAt(randomIndex);
        //     }
        //     freeFieldsTotal -= s;
        //     
        // }
        // Modo 2
        GenerateRoulette();

        
    }

    private void OnEnable()
    {
        if (PlayerPrefs.HasKey("UseRoulette"))
        {
            var timeUsedRoulette = DateTime.Parse(PlayerPrefs.GetString("UseRoulette"));
            if (timeUsedRoulette < DateTime.Today)
            {
                // No usó ruleta hoy
                Restart();
            }
        }

        Restart();
    }

    private void GenerateRoulette()
    {
        var itemsLength = _rouletteItems.Count -1;
        for (int i = 0; i < itemsLength; i++)
        {
            Random.InitState(Random.Range(Int32.MinValue, Int32.MaxValue));
            var s = Random.Range(0, _rouletteItems.Count);
            while (_rouletteItems[s].HaveInformation)
            {
                s = Random.Range(0, _rouletteItems.Count);
            }
            var randomData = Random.Range(0, _rouletteSO.RouletteItems.Length);
            _rouletteItems[s].SetData(_rouletteSO.RouletteItems[randomData]);
            // _rouletteItems.RemoveAt(s);
        }
        var temp =_rouletteItems.FirstOrDefault(x => !x.HaveInformation);
        if (temp != null)
        {
            temp.ImageItem.color = new Color(0, 0, 0, 0);
            temp.AmountLabel.text = String.Empty;
        }
    }
    
    private void ComputeItemsPosition()
    {
        
    }

    void Update()
    {
        if (_useRoulette)
        {
            return;
        }
        Quaternion rotacionObjetivo = Quaternion.Euler(0f, 0f, _angleDifference);
        if (_isDragging)
        {
            // _rouletteTransform.rotation = Quaternion.Slerp(_rouletteTransform.rotation, rotacionObjetivo, _speedRotationWithDrag * Time.deltaTime);            
            _rouletteTransform.rotation = rotacionObjetivo;
        }
        else
        {
            if (_canRotate)
            {
                _currentRotationSpeed = Mathf.SmoothStep(_currentRotationSpeed, 0f, _deceleration * Time.unscaledDeltaTime);
                if (_currentRotationSpeed <= _minSpeed)
                {
                    _currentRotationSpeed = 0;
                    _useRoulette = true;
                    CalculateItemSelected();

                    _onSelectedItem?.Invoke();
                }
                _rouletteTransform.Rotate(Vector3.forward, _currentRotationSpeed * Math.Sign(_direction) * _velocidadWithoutDrag);
            }
            else
            {
                _rouletteTransform.rotation = rotacionObjetivo;
            }
            
        }
    }

    private float GetAngle()
    {
        var mousePosition = Input.mousePosition - _rouletteTransform.position;
        return Mathf.Atan2(mousePosition.y, mousePosition.x) * Mathf.Rad2Deg;
    }
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(_rouletteTransform.position, Input.mousePosition - _rouletteTransform.position);
        //CalculateItemSelected();
    }
#endif
    private void CalculateItemSelected()
    {
        //Compute the item selected
        float distance = Single.MaxValue;
        selecetdItem = null;
        for (int i = 0; i < _rouletteItems.Count; i++)
        {
            if (distance > (_rouletteItems[i].transform.position - _originPoint.position).sqrMagnitude)
            {
                distance = (_rouletteItems[i].transform.position - _originPoint.position).sqrMagnitude;
                selecetdItem = _rouletteItems[i];
            }
        }
        //Update data
        BuyItem();
        //Show particle effects 
        _uiParticle.transform.position = selecetdItem.transform.position;
        ClearParticles();
        var _particleInstantiated = Instantiate(_particlePrefab);
        _particleInstantiated.transform.localPosition = new Vector3(0,0, 0);
        _particleInstantiated.SetActive(true);
        _particleInstantiated.transform.localScale = Vector3.one;
        _uiParticle.SetParticleSystemInstance(_particleInstantiated);
        
        // Show message with reward
        if (!selecetdItem.HaveInformation)
        {
            _coofigurationMessage.text = "Sin suerte hoy, pero mañana hay otro intento. Continúa, sigue jugando";
            _imageReward.gameObject.SetActive(false);
            _rewardContainer.transform.GetChild(2).gameObject.SetActive(false);
            _rewardContainer.gameObject.SetActive(true);
            _rewardContainer.FadeInTransition();
            return;   
        }
        _imageReward.gameObject.SetActive(true);
        _coofigurationMessage.text = "Usted obtuvo las siguiente recompensas:";
        _imageReward.sprite = selecetdItem.ImageItem.sprite;
        _finalAmount.text = $"x{selecetdItem.Amount}";
        _rewardContainer.transform.GetChild(2).gameObject.SetActive(true);
        _rewardContainer.gameObject.SetActive(true);
        _rewardContainer.FadeInTransition();
        
    }
    
    public void BuyItem()
    {
        if (!selecetdItem.HaveInformation)
        {
            return;
        }
        if (selecetdItem.RouletteItemData._ItemRouletteSo.GetType() == typeof(PowerUpItemRoulette))
        {
            var item = selecetdItem.RouletteItemData._ItemRouletteSo as PowerUpItemRoulette;
            switch (item.powerUpSO.nameInPlayerPrefs)
            {
                case "pu_deleteOption":
                    _userSO.userInfo.user.detail.discardOption += item.powerUpSO.amount;
                    break;
                case "pu_moreTime":
                    _userSO.userInfo.user.detail.increaseTime += item.powerUpSO.amount;
                    break;
                case "pu_nextQuestion":
                    _userSO.userInfo.user.detail.skipQuestion += item.powerUpSO.amount;
                    break;
                case "pu_secondOportunity":
                    _userSO.userInfo.user.detail.secondChance += item.powerUpSO.amount;
                    break;
                case "pu_trueOption":
                    _userSO.userInfo.user.detail.findCorrectAnswer += item.powerUpSO.amount;
                    break;
            }
            item.powerUpSO.amount += selecetdItem.Amount;
            GameEvents.RequestUpdateDetail?.Invoke();
            // PlayerPrefs.SetInt(item.powerUpSO.nameInPlayerPrefs, item.powerUpSO.amount);
            // PlayerPrefs.Save();
            // item.powerUpSO.Raise();
        }
        else if (selecetdItem.RouletteItemData._ItemRouletteSo.GetType() == typeof(CoinsItemRoulette))
        {
            _userSO.userInfo.user.detail.totalCoins += selecetdItem.Amount;
            GameEvents.RequestUpdateDetail?.Invoke();
            // GameEvents.RequestCoinsChange?.Invoke(selecetdItem.Amount);
        }
        
        // selecetdItem.RouletteItemData._ItemRouletteSo. _currentItem.PowerUp.amount += _currentItem.Amount;
        // PlayerPrefs.SetInt(_currentItem.PowerUp.nameInPlayerPrefs, _currentItem.PowerUp.amount);
        // PlayerPrefs.Save();
        // _currentItem.PowerUp.Raise();
    }
    
    public void ClearParticles()
    {
        foreach (Transform child in _uiParticle.transform)
        {
            Destroy(child.gameObject);
        }
        
    }
    public void StartAngle()
    {
        _lastAngle = GetAngle();
    }
    public void DragAngle()
    {
        _isDragging = true;
        float currentAngle  = GetAngle();
        _direction = currentAngle - _lastAngle;
        _angleDifference = _rouletteTransform.eulerAngles.z + _direction;
        _lastAngle = currentAngle;
    }
    public void PointerUp()
    {
        _currentRotationSpeed = Touchscreen.current.delta.magnitude;
        Debug.Log(_currentRotationSpeed);
        if (_currentRotationSpeed < _minSpeedDrag && _currentRotationSpeed >5)
        {
            _onInitRotation?.Invoke();
            Debug.Log(_currentRotationSpeed + " poca velocidad, agregando velocidad personalizada");
            _currentRotationSpeed += 80;
            _canRotate = true;
        }
        else if (_currentRotationSpeed >= _minSpeedDrag )
        {
            _onInitRotation?.Invoke();
            _canRotate = true;
        }
        else
        {
            _onFailedRotation?.Invoke();
            _canRotate = false;
        }
        _isDragging = false;
    }

    [ContextMenu("Restart")]
    public void Restart()
    {
        for (int i = 0; i < _rouletteItems.Count(); i++)
        {
            _rouletteItems[i].HaveInformation = false;
        }
        _onResetRoulette?.Invoke();
        _useRoulette = false;
        _canRotate = false;
        GenerateRoulette();
        _rewardContainer.FadeOutTransition();
    }
}
