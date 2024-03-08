using System.Collections.Generic;
using UnityEngine;

namespace ScriptableCreator
{
    [CreateAssetMenu(menuName = "Incorrect Questions", fileName = "IncorrectQuestionSO")]
    public class IncorrectQuestionsSO : ScriptableObject
    {
        public List<QuestionItem> IncorrectQuestionsList;

        public void SaveIncorrectQuestion(QuestionItem questionItem)
        {
            if (IncorrectQuestionsList.Exists(x => x.id == questionItem.id))
            {
                return;
            }
            IncorrectQuestionsList.Add(questionItem);   
        }
    }
}