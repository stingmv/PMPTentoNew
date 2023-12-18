using System.Collections;
using System.Collections.Generic;
using TMPro;
using UI;
using UnityEngine;

namespace Question
{
    public class QuestionInformation : MonoBehaviour
    {
        #region Variables

        [SerializeField] private PopupQuestion _popupIncorrect;
        [SerializeField] private PopupQuestion _popupCorrect;
        [SerializeField] private TextMeshProUGUI _question;
        [SerializeField] private Option _opt1;
        [SerializeField] private Option _opt2;
        [SerializeField] private Option _opt3;
        [SerializeField] private Option _opt4;

        public Option Opt1
        {
            get => _opt1;
            set => _opt1 = value;
        }

        public Option Opt2
        {
            get => _opt2;
            set => _opt2 = value;
        }

        public Option Opt3
        {
            get => _opt3;
            set => _opt3 = value;
        }

        public Option Opt4
        {
            get => _opt4;
            set => _opt4 = value;
        }

        #endregion

        #region Unity Methods
        #endregion

        #region Methods

        public void SetData(QuestionData questionData)
        {
            _question.text = questionData.question;
            _opt1.SetData(questionData.options[0],questionData.options[0]);
            _opt2.SetData(questionData.options[1],questionData.options[1]);
            _opt3.SetData(questionData.options[2],questionData.options[2]);
            _opt4.SetData(questionData.options[3],questionData.options[3]);
            EnableOptions();
        }

        public void SetMessage(string message, bool isCorrect)
        {
            if (isCorrect)
            {
                _popupCorrect.SetMessage(message,true);

            }
            else
            {
                _popupIncorrect.SetMessage(message,false);

            }
        }

        // public void SetMessagePower(string message)
        // {
        //     _popupQuestion.SetMessagePower(message);
        // }

        public void  DisableOptions()
        {
            _opt1.DisableOption();
            _opt2.DisableOption();
            _opt3.DisableOption();
            _opt4.DisableOption();
        }
        public void  EnableOptions()
        {
            _opt1.EnableOption();
            _opt2.EnableOption();
            _opt3.EnableOption();
            _opt4.EnableOption();
        }
        
        #endregion

    }

}