using System;
using System.Collections;
using System.Collections.Generic;
using ScriptableCreator;
using UnityEngine;

public class DailyReviewPMPService : MonoBehaviour
{
    [SerializeField] private IncorrectQuestionsSO _incorrectQuestions;

    public List<QuestionItem> GetAllIncorrectQuestions()
    {
        return _incorrectQuestions.IncorrectQuestionsList;
    }
}
