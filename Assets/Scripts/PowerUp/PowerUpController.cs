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
        #endregion

        #region Unity Methods

        

        #endregion

        #region Methods

        public void UseSecondOportunity()
        {
            _powerUpSecondOportunity.amount--;
            _powerUpSecondOportunity.Raise();
        }

        public void BuySecondOportunity(int amount )
        {
            _powerUpSecondOportunity.amount += amount;
            _powerUpSecondOportunity.Raise();
        }
        public void UseTrueOption()
        {
            _powerUpTrueOption.amount--;
            _powerUpTrueOption.Raise();
        }

        public void BuyTrueOption(int amount )
        {
            _powerUpTrueOption.amount += amount;
            _powerUpTrueOption.Raise();
        }
        
        public void UseDeleteOption()
        {
            _powerUpDeleteOption.amount--;
            _powerUpDeleteOption.Raise();
        }

        public void BuyDeleteOption(int amount )
        {
            _powerUpDeleteOption.amount += amount;
            _powerUpDeleteOption.Raise();
        }
        public void UseNextQuestion()
        {
            _powerUpNextQuestion.amount--;
            _powerUpNextQuestion.Raise();
        }

        public void BuyNextQuestion(int amount )
        {
            _powerUpNextQuestion.amount += amount;
            _powerUpNextQuestion.Raise();
        }

        
        #endregion

    }

}