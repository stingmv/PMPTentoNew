using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
    public class PopupQuestion : Popup
    {
        #region Variables

        [SerializeField] private TextMeshProUGUI _message;
        [SerializeField] private TextMeshProUGUI _messagePower;
        [SerializeField] private Image _imageBSecondOportunity;
        [SerializeField] private TextMeshProUGUI _labelBSecondOportunity;
        [SerializeField] private EventTrigger _eventTrigger;
        [SerializeField] private Color _correctColor;
        [SerializeField] private Color _incorrectColor;
        
        #endregion

        #region Unity Methods

        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        #endregion

        #region Methods

        public void SetMessage(string message, bool isCorrect)
        {
            if (isCorrect)
            {
                _message.color =_correctColor;
            }
            else
            {
                _message.color = _incorrectColor;
            }
            _message.text = message;
        }

        public void SetMessagePower(string message)
        {
            _messagePower.text = message;
        }

        public void EnableEventTrigger()
        {
            _eventTrigger.enabled = true;
        }

        public void DisableEventTrigger()
        {
            _eventTrigger.enabled = false;
        }

        #endregion

    }

}   