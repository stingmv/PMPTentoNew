using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Question
{
    public class OptionVP : MonoBehaviour
    {
        #region Variables

        [SerializeField] private TextMeshProUGUI _label;
        [SerializeField] private QuestionController _questionController;
        [SerializeField] private Image _image;
        [SerializeField] private Sprite _defaultColor;
        [SerializeField] private Sprite _correctColor;
        [SerializeField] private Sprite _incorrectColor;
        [SerializeField] private EventTrigger _eventTrigger;
        
        private string _id;
        public string ID
        {
            get => _id;
            set => _id = value;
        }
        
        public EventTrigger EventTriggerT
        {
            get => _eventTrigger;
            set => _eventTrigger = value;
        }

        #endregion

        #region Unity Methods

        

        #endregion

        #region Methods

        public void SetData(string description, string id)
        {
            _label.text = description;
            _id = id;
        }

        public void CompareResponse()
        {
            if (_questionController.ValidateResponse(_id))
            {
                _image.sprite = _correctColor;
            }
            else
            {
                _image.sprite = _incorrectColor;
            }
            _label.color = Color.white;
        }

        public void DisableOption()
        {
            _eventTrigger.enabled = false;
        }
        
        public void EnableOption()
        {
            _label.color = Color.white;
            _image.sprite = _defaultColor;
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
        #endregion
    }

}