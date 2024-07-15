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

    [SerializeField] private List<RouletteItem> _rouletteItems;//lista que contendra cada espacio de la ruleta con su elemento a configurar
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

        
    }

    private void OnEnable()
    {
        if (PlayerPrefs.HasKey("UseRoulette"))
        {
            Debug.Log(DateTime.Parse(PlayerPrefs.GetString("UseRoulette")));
            var timeUsedRoulette = DateTime.Parse(PlayerPrefs.GetString("UseRoulette"));
            if (timeUsedRoulette < DateTime.Today)
            {
                //No usó ruleta hoy
                Restart();
            }
            return;
        }
        GenerateRoulette();

    }

    //GENERAR RULETA
    private void GenerateRoulette()
    {
        var itemsLength = _rouletteItems.Count;//cuenta la cantidad de elementos que habran en la ruleta
        Random.InitState(Random.Range(Int32.MinValue, Int32.MaxValue));//inicializa semilla de numeros aleatorios
        
        for (int i = 0; i < itemsLength; i++)
        {                     
            //randomData para obtener la data random a setear en el item
            int randomData = Random.Range(0, _rouletteSO.RouletteItems.Length);
            if (i>0)
            {                
                while (_rouletteItems[i-1].RouletteItemData._ItemRouletteSo== _rouletteSO.RouletteItems[randomData]._ItemRouletteSo)//comparar el power up scriptable object del elemento anterior con el obtenido en el SO rouletteSO debido al randomData
                {
                    Debug.Log("randomData recalculado");
                    randomData = Random.Range(0, _rouletteSO.RouletteItems.Length);//recalcular el randomData
                }
            }

            //En el ultimo item hacer lo mismo y ademas comparar con el primer elemento ya que colinda con el ultimo, y deben estar juntos dos powerups
            if (i==itemsLength-1)
            {
                Debug.Log("Ultima posición");
                while (_rouletteItems[i - 1].RouletteItemData._ItemRouletteSo == _rouletteSO.RouletteItems[randomData]._ItemRouletteSo || _rouletteItems[0].RouletteItemData._ItemRouletteSo == _rouletteSO.RouletteItems[randomData]._ItemRouletteSo)
                {
                    Debug.Log("randomData final recalculado");
                    randomData = Random.Range(0, _rouletteSO.RouletteItems.Length);//recalcular el randomData
                }
            }
            _rouletteItems[i].SetData(_rouletteSO.RouletteItems[randomData]);//con ese numero aleatorio se decide que item de la lista de items a aleatorizar se usara y con el metodo SetData se setea en el item i
            // _rouletteItems.RemoveAt(s);
        }
        /*
        //PARA ESPACIO VACIO        
        var temp =_rouletteItems.FirstOrDefault(x => !x.HaveInformation);//busca el primer elemento de la coleccion donde HaveInformation es false
        if (temp != null)
        {
            temp.ImageItem.color = new Color(0, 0, 0, 0);
            temp.AmountLabel.text = String.Empty;
        }
        */
    }
    
    private void ComputeItemsPosition()
    {
        
    }

    void Update()
    {
        if (_useRoulette)
        {
            return;//en este caso si _useRoulette es true el return hara que termine el codigo inmediatamente
        }
        Quaternion rotacionObjetivo = Quaternion.Euler(0f, 0f, _angleDifference);//angulo de diferencia al girar con el dedo
        if (_isDragging)//si se esta arrastrando
        {
            // _rouletteTransform.rotation = Quaternion.Slerp(_rouletteTransform.rotation, rotacionObjetivo, _speedRotationWithDrag * Time.deltaTime);            
            _rouletteTransform.rotation = rotacionObjetivo;
        }
        else
        {

            if (_canRotate)//evalua si se puede rotar habiendo cumplido los parametros
            {
                _currentRotationSpeed = Mathf.SmoothStep(_currentRotationSpeed, 0f, _deceleration * Time.unscaledDeltaTime);//suaviza la disminucion de velocidad desde _currentRotationSpeed a 0
                if (_currentRotationSpeed <= _minSpeed)//si llega a velocidad minima
                {
                    _currentRotationSpeed = 0;//velocidad a 0
                    _useRoulette = true;
                    CalculateItemSelected();

                    _onSelectedItem?.Invoke();
                }
                //forward representa al eje z
                _rouletteTransform.Rotate(Vector3.forward, _currentRotationSpeed * Math.Sign(_direction) * _velocidadWithoutDrag);//rota la ruleta
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
        Gizmos.DrawRay(_rouletteTransform.position, Input.mousePosition - _rouletteTransform.position);//dibuja rayo desde la posicion de la ruleta hasta donde presione el mouse
        //CalculateItemSelected();
    }
#endif

    //ITEM ESCOGIDO
    private void CalculateItemSelected()//item selected es el item escogido
    {
        //COMPUTE THE ITEM SELECTED
        PlayerPrefs.SetString("UseRoulette", DateTime.Now.ToString());//guardo la fecha en el playerpref
        PlayerPrefs.Save();//guarda todas las preferencias modificadas
        float distance = Single.MaxValue;//setear a un valor maximo extremadamente grande
        selecetdItem = null;
        for (int i = 0; i < _rouletteItems.Count; i++)
        {
            if (distance > (_rouletteItems[i].transform.position - _originPoint.position).sqrMagnitude)
            {
                distance = (_rouletteItems[i].transform.position - _originPoint.position).sqrMagnitude;
                selecetdItem = _rouletteItems[i];//asigna elemento escogido a selecetdItem
            }
        }
        //Update data
        BuyItem();

        //PARTICULA
        //Show particle effects 
        _uiParticle.transform.position = selecetdItem.transform.position;//setea la posicion de la particula en el item seleccionado en la ruleta
        ClearParticles();
        
        var _particleInstantiated = Instantiate(_particlePrefab);//instancia particula
        _particleInstantiated.transform.localPosition = new Vector3(0,0, 0);//setea la posicion de la particula a 0
        _particleInstantiated.SetActive(true);
        _particleInstantiated.transform.localScale = Vector3.one;//setea a 1 la escala de la particula instanciada
        _uiParticle.SetParticleSystemInstance(_particleInstantiated);//metodo para asignar la particula instanciada a _uiParticle
        
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
        _imageReward.sprite = selecetdItem.IconItem;//asigna el Icono del powerup obtenido        
        _finalAmount.text = $"x{selecetdItem.Amount}";
        //_rewardContainer.transform.GetChild(2).gameObject.SetActive(true);
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
        _isDragging = true;//indica que se esta arrastrando
        float currentAngle  = GetAngle();//calcula en angulo actual
        _direction = currentAngle - _lastAngle;//calcula direccion en base a diferencia de angulo actual y ultimo angulo
        _angleDifference = _rouletteTransform.eulerAngles.z + _direction;//giro de ruleta en z al arrastrar + direccion
        _lastAngle = currentAngle;//el angulo actual pasa a ser el ultimo angulo
    }
    public void PointerUp()//se activa desde el event trigger pointer up
    {
        _currentRotationSpeed = Touchscreen.current.delta.magnitude;
        Debug.Log(_currentRotationSpeed);
        if (_currentRotationSpeed < _minSpeedDrag && _currentRotationSpeed >5)
        {
            _onInitRotation?.Invoke();
            Debug.Log(_currentRotationSpeed + " poca velocidad, agregando velocidad personalizada");
            _currentRotationSpeed += 80;
            _canRotate = true;//indica que se puede rotar si cumple lo necesario
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
