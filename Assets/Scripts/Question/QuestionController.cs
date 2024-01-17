using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

namespace Question
{
    [Serializable]
    public class QuestionData
    {
        public string idQuestion;
        public string question;
        public OptionItem[] options = new OptionItem[4];
        public string option1;
        public string option2;
        public string option3;
        public string option4;
        public string idCorrectOption;
        public ProgressItem progressItem;
    }
    public class QuestionController : MonoBehaviour
    {
        #region Variables

        [SerializeField] private QuestionInformation _questionInformation;
        [SerializeField] private UnityEvent _onSelectOption;
        [SerializeField] private UnityEvent _onCorrectOption;
        [SerializeField] private UnityEvent _onIncorrectOption;
        [SerializeField] private ProgressQuestion _progressQuestion;
        [SerializeField] private UnityEvent _onEndQuestions;
        [SerializeField] private UnityEvent _onNextQuestion;
        [SerializeField] private DataToRegisterSO _toRegisterSo;
        private List<QuestionData> _session = new List<QuestionData>();

        private QuestionData _currentQuestion;
        private List<int> _indexes = new List<int>(){0,1,2,3} ;
        private int _currentIndex;

        public int GetCountSession
        {
            get => _session.Count;
        }

        public int CurrentIndex
        {
            get => _currentIndex;
            set => _currentIndex = value;
        }

        public QuestionData CurrentQuestion
        {
            get => _currentQuestion;
            set => _currentQuestion = value;
        }

        #endregion

        #region Unity Methods

        private void OnEnable()
        {
            GameEvents.QuestionReady += GameEvents_QuestionReady;
        }

        private void OnDisable()
        {
            GameEvents.QuestionReady -= GameEvents_QuestionReady;
        }

        private void GameEvents_QuestionReady()
        {
            SetData(_toRegisterSo.questionInformation.listaPreguntas);
            ConfigurateQuestion();
            UIEvents.ShowQuestionView?.Invoke();
        }

        // Start is called before the first frame update
        void Start()
        {
            _progressQuestion.CalculateWidth(10);
            
            // for (int i = 0; i < 10; i++)
            // {
            //     QuestionData questionData = new QuestionData();
            //     var randomValue = Random.value;
            //     questionData.idQuestion = randomValue.ToString();
            //     questionData.question = $"Question nº {randomValue}";
            //     questionData.options[0]. = $"Question {randomValue} option 1";
            //     questionData.options[1] = $"Question {randomValue} option 2";
            //     questionData.options[2] = $"Question {randomValue} option 3";
            //     questionData.options[3] = $"Question {randomValue} option 4";
            //     questionData.idCorrectOption = $"Question {randomValue} option 4";
            //     questionData.progressItem = _progressQuestion.CreateItem();
            //     _session.Add( questionData);
            // }
        }

        
        #endregion

        #region Methods

        public void SetData(QuestionItem[] questions)
        {
            _progressQuestion.CalculateWidth(questions.Length);
            for (int i = 0; i < questions.Length; i++)
            {
                
                QuestionData questionData = new QuestionData();
                questionData.idQuestion = questions[i].pregunta.id.ToString();
                questionData.question = questions[i].pregunta.enunciado;
                
                var randomvalue = Random.Range(0, _indexes.Count);
                questionData.options[_indexes[randomvalue]] = questions[i].pregunta.respuesta[0];
                _indexes.RemoveAt(randomvalue);

                randomvalue = Random.Range(0, _indexes.Count);
                questionData.options[_indexes[randomvalue]] = questions[i].pregunta.respuesta[1];
                _indexes.RemoveAt(randomvalue);

                randomvalue = Random.Range(0, _indexes.Count);
                questionData.options[_indexes[randomvalue]] = questions[i].pregunta.respuesta[2];
                _indexes.RemoveAt(randomvalue);

                questionData.options[_indexes[0]] = questions[i].pregunta.respuesta[3];
                _indexes.RemoveAt(0);

                questionData.idCorrectOption = questions[i].pregunta.respuesta.FirstOrDefault(x => x.correcto == "true")?.id.ToString();
                questionData.progressItem = _progressQuestion.CreateItem();
                _session.Add( questionData);
                _indexes.Add(0);
                _indexes.Add(1);
                _indexes.Add(2);
                _indexes.Add(3);
            }
        }
        public void ConfigurateQuestion()
        {
            _currentQuestion = _session[_currentIndex];
            _currentQuestion.progressItem.SetCurrentItem();
            _questionInformation.SetData(_currentQuestion);
            _currentIndex++;
        }

        public void NextQuestion()
        {
            if (_currentIndex == _session.Count)
            {
                GameEvents.GameWon?.Invoke();
                _onEndQuestions?.Invoke();
                return;
            }
            var tempQuestion = _session[_currentIndex];
            _currentQuestion = tempQuestion;
            _currentQuestion.progressItem.SetCurrentItem();
            _questionInformation.SetData(_currentQuestion);
            _currentIndex++;
            _onNextQuestion?.Invoke();
        }

        public void SetIncorrectQuestion()
        {
            _currentQuestion.progressItem.SetIncorrectSelection();
        }
        public bool ValidateResponse(string id)
        {
            _questionInformation.DisableOptions();
            _onSelectOption?.Invoke();
            if (_currentQuestion.idCorrectOption == id)
            {
                _currentQuestion.progressItem.SetCorrectSelection();
                _questionInformation.SetMessage("¡Correcto! ¡Eres un experto en este tema!", true);
                GameEvents.CorrectlyAnswered?.Invoke();
                _onCorrectOption?.Invoke();
                return true;
            }
            _currentQuestion.progressItem.SetIncorrectSelection();
            _questionInformation.SetMessage("Esa no es la respuesta correcta, pero cada error es una oportunidad de aprendizaje.", false);
            GameEvents.IncorrectlyAnswered?.Invoke();
            _onIncorrectOption?.Invoke();
            return false;
            
        }

        public void SetCurrentQuestionProgress()
        {
            _currentQuestion.progressItem.SetCurrentItem();
        }
        public bool CompareResponse(string id)
        {
            return _currentQuestion.idCorrectOption == id;
        }

        public void ReturnTrueOption()
        {
            var idCorrectOption = _currentQuestion.idCorrectOption;
            if (_questionInformation.Opt1.ID == idCorrectOption)
            {
                ExecuteEvents.Execute<IPointerClickHandler>(_questionInformation.Opt1.gameObject,
                    new PointerEventData(EventSystem.current), ExecuteEvents.pointerClickHandler);
                return;
            }
            if (_questionInformation.Opt2.ID == idCorrectOption)
            {               
                ExecuteEvents.Execute<IPointerClickHandler>(_questionInformation.Opt2.gameObject,
                    new PointerEventData(EventSystem.current), ExecuteEvents.pointerClickHandler);
                return;
            }
            if (_questionInformation.Opt3.ID == idCorrectOption)
            {               
                ExecuteEvents.Execute<IPointerClickHandler>(_questionInformation.Opt3.gameObject,
                    new PointerEventData(EventSystem.current), ExecuteEvents.pointerClickHandler);
                return;
            }
            ExecuteEvents.Execute<IPointerClickHandler>(_questionInformation.Opt4.gameObject,
                new PointerEventData(EventSystem.current), ExecuteEvents.pointerClickHandler);
            return;
        }

        public void SendGameLost()
        {
            GameEvents.GameLost?.Invoke();
        }
        
        #endregion

    }

}