using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Question;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class DailyReviewController : MonoBehaviour
{
    [SerializeField] private DailyReviewPMPService _pmpService;
    [SerializeField] private QuestionInformation _questionInformation;
    [SerializeField] private DailyReviewNameListItem _nameListItemPrefab;
    [SerializeField] private List<QuestionData> _session;
    [SerializeField] private Transform _nameListContainer;
    [SerializeField] private ProgressQuestion _progressQuestion;
    [SerializeField] private UnityEvent _onFirstQuestion;
    [SerializeField] private UnityEvent _onLastQuestion;
    [SerializeField] private UnityEvent _onMediumQuestion;
    private List<int> _indexes = new List<int>(){0,1,2,3} ;
    private int _currentIndex;
    private QuestionData _currentQuestion;

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
    private void Start()
    {
        var items = _pmpService.GetAllIncorrectQuestions();
        foreach (Transform child in _nameListContainer.transform)
        {
            Destroy(child.gameObject);
        }

        
        for (int i = 0; i < items.Count; i++)
        {
            var instance = Instantiate(_nameListItemPrefab, _nameListContainer);
            instance.SetData(items[i].pregunta.enunciado);
            
            // _listItems.Add(instance);
        }
        SetData(items.ToArray());
        ConfigurateQuestion();
        if (IsFirst())
        {
            _onFirstQuestion?.Invoke();
        }
        if (IsLast())
        {
            _onLastQuestion?.Invoke();
        }
    }
    public void SetData(QuestionItem[] questions)
    {
        _progressQuestion.CalculateWidth(questions.Length);
        
        for (int i = 0; i < questions.Length; i++)
        {
                
            QuestionData questionData = new QuestionData();
            questionData.questionItem = questions[i];
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
            questionData.idTask = questions[i].idSimuladorPmpTarea;
        }
        
    }
    public void GetPreviousQuestion()
    {
        if (CurrentIndex - 1< 0)
        {
            return;
        }
        _currentQuestion.progressItem.RemoveCurrentItem();
        _onMediumQuestion?.Invoke();
        // var tempQuestion = _session[CurrentIndex - 1];
        CurrentIndex--;
        // return tempQuestion;
        ConfigurateQuestion();
        if (IsFirst())
        {
            _onFirstQuestion?.Invoke();
        }
    }
    public void GetNextQuestion()
    {
            
        if (CurrentIndex + 1 == _session.Count)
        {
            return;
        }
        _currentQuestion.progressItem.RemoveCurrentItem();
        _onMediumQuestion?.Invoke();
        // var tempQuestion = _session[CurrentIndex + 1];
        CurrentIndex++;
        // _onNextQuestion?.Invoke();
        // return tempQuestion;
        ConfigurateQuestion();
        if (IsLast())
        {
            _onLastQuestion?.Invoke();
        }
    }
    public void ConfigurateQuestion()
    {
        _currentQuestion = _session[CurrentIndex];
        _currentQuestion.progressItem.SetCurrentItem();
        _questionInformation.SetData(_currentQuestion);
        // _currentIndex++;
    }
    public bool IsFirst()
    {
        return CurrentIndex <= 0;
    }

    public bool IsLast()
    {
        return CurrentIndex + 1 >= _session.Count;
    }
}
