using System;
using System.Collections;
using System.Collections.Generic;
using ScriptableCreator;
using UnityEngine;

namespace PowerUp
{
    public class PowerUpController : MonoBehaviour
    {
        #region Variables

        [SerializeField] private ScripableObjectPowerUp _powerUpSecondOportunity;
        [SerializeField] private ScripableObjectPowerUp _powerUpTrueOption;
        [SerializeField] private ScripableObjectPowerUp _powerUpDeleteOption;
        [SerializeField] private ScripableObjectPowerUp _powerUpNextQuestion;
        [SerializeField] private ScripableObjectPowerUp _powerUpMoreTime;
        #endregion

        #region Unity Methods

        private void OnEnable()
        {
            if (PlayerPrefs.HasKey("pu_secondOportunity"))
            {
                _powerUpSecondOportunity.amount = PlayerPrefs.GetInt("pu_secondOportunity");
            }
            else
            {
                _powerUpSecondOportunity.amount = 5;
            }
            if (PlayerPrefs.HasKey("pu_trueOption"))
            {
                _powerUpTrueOption.amount = PlayerPrefs.GetInt("pu_trueOption");
            }
            else
            {
                _powerUpTrueOption.amount = 5;
            }
            if (PlayerPrefs.HasKey("pu_deleteOption"))
            {
                _powerUpDeleteOption.amount = PlayerPrefs.GetInt("pu_deleteOption");
            }
            else
            {
                _powerUpDeleteOption.amount = 5;
            }
            if (PlayerPrefs.HasKey("pu_nextQuestion"))
            {
                _powerUpNextQuestion.amount = PlayerPrefs.GetInt("pu_nextQuestion");
            }
            else
            {
                _powerUpNextQuestion.amount = 5;
            }
            if (PlayerPrefs.HasKey("pu_moreTime"))
            {
                _powerUpMoreTime.amount = PlayerPrefs.GetInt("pu_moreTime");
            }
            else
            {
                _powerUpMoreTime.amount = 5;
            }
        }

        #endregion

        #region Methods

        public void UseSecondOportunity()
        {
            _powerUpSecondOportunity.amount--;
            PlayerPrefs.SetInt("pu_secondOportunity", _powerUpSecondOportunity.amount);
            PlayerPrefs.Save();
            _powerUpSecondOportunity.Raise();
        }

        public void BuySecondOportunity(int amount )
        {
            _powerUpSecondOportunity.amount += amount;
            PlayerPrefs.SetInt("pu_secondOportunity", _powerUpSecondOportunity.amount);
            PlayerPrefs.Save();
            _powerUpSecondOportunity.Raise();
        }
        public void UseTrueOption()
        {
            _powerUpTrueOption.amount--;
            PlayerPrefs.SetInt("pu_trueOption", _powerUpTrueOption.amount);
            PlayerPrefs.Save();
            _powerUpTrueOption.Raise();
        }

        public void BuyTrueOption(int amount )
        {
            _powerUpTrueOption.amount += amount;
            PlayerPrefs.SetInt("pu_trueOption", _powerUpTrueOption.amount);
            PlayerPrefs.Save();
            _powerUpTrueOption.Raise();
        }
        
        public void UseDeleteOption()
        {
            _powerUpDeleteOption.amount--;
            PlayerPrefs.SetInt("pu_deleteOption", _powerUpDeleteOption.amount);
            PlayerPrefs.Save();
            _powerUpDeleteOption.Raise();
        }

        public void BuyDeleteOption(int amount )
        {
            _powerUpDeleteOption.amount += amount;
            PlayerPrefs.SetInt("pu_deleteOption", _powerUpDeleteOption.amount);
            PlayerPrefs.Save();
            _powerUpDeleteOption.Raise();
        }
        public void UseNextQuestion()
        {
            _powerUpNextQuestion.amount--;
            PlayerPrefs.SetInt("pu_nextQuestion", _powerUpNextQuestion.amount);
            PlayerPrefs.Save();
            _powerUpNextQuestion.Raise();
        }

        public void BuyNextQuestion(int amount )
        {
            _powerUpNextQuestion.amount += amount;
            PlayerPrefs.SetInt("pu_nextQuestion", _powerUpNextQuestion.amount);
            PlayerPrefs.Save();
            _powerUpNextQuestion.Raise();
        }
        
        public void UseMoreTime()
        {
            _powerUpMoreTime.amount--;
            PlayerPrefs.SetInt("pu_moreTime", _powerUpMoreTime.amount);
            PlayerPrefs.Save();
            _powerUpMoreTime.Raise();
        }
        public void BuyMoreTime(int amount )
        {
            _powerUpMoreTime.amount += amount;
            PlayerPrefs.SetInt("pu_moreTime", _powerUpMoreTime.amount);
            PlayerPrefs.Save();
            _powerUpMoreTime.Raise();
        }

        
        #endregion

    }

}