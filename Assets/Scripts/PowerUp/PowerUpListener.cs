using System;
using ScriptableCreator;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace PowerUp
{
    public class PowerUpListener: MonoBehaviour
    {
        [SerializeField] protected ScripableObjectPowerUp _event;
        [SerializeField] private TextMeshProUGUI _amount;
        [SerializeField] private UnityEvent _response;

        private void Start()
        {
            UpdateValue();
        }

        public void UpdateValue()
        {
            _amount.text = _event.amount.ToString();
        }
        private void OnEnable()
        {
            _event.RegisterListener(this);
        }

        private void OnDisable()
        {
            _event.UnregisterListener(this);
        }

        public void OnEventRaised()
        {
            _response?.Invoke();
        }
    }
}