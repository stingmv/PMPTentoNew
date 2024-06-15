using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AchievementControlller : MonoBehaviour
{
    [SerializeField] private AchievementData achievementData;
    [SerializeField] public List<int> maxGoodStreakList;//numero maximo de respuestas consecutivas para alcanzar una racha
    [SerializeField] private UnityEvent<int> OnMaxGoodStreakReached;//evento publico en editor
    [SerializeField] private UnityEvent OnMaxGoodWithoutErrorsReached;//evento publico en editor
    [SerializeField] private string achievementOrigin;
    

    //private bool _hasReachedMaxGoodStreak;

    private void OnEnable()
    {
        GameEvents.OnGoodStreaked += GoodStreak;
        GameEvents.OnGoodWithoutErrors += GoodWithoutErrors;
    }

    private void OnDisable()
    {
        GameEvents.OnGoodStreaked -= GoodStreak;
        GameEvents.OnGoodWithoutErrors -= GoodWithoutErrors;
    }

    private void GoodStreak()//racha
    {
        achievementData.AddCounter(0);
    }

    private void GoodWithoutErrors()//racha ronda sin fallas
    {
        achievementData.AddCounter(1);
    }

    public void CheckMaxGoodStreak(int counter)//verificar 
    {
        achievementData.achievementListContainer.achievementList[0].ConsecutiveAnswer = counter;//se llena el campo Consecutive Answer en SO Achievement Data

        for (int i = 0; i < maxGoodStreakList.Count; i++)//recorriendo lista de tipos de racha: 4,6,8 y 10
        {
            if (counter == maxGoodStreakList[i])//si es igual al contador ConsecutiveAnser
            {
                Debug.Log($"Alcanzo Racha de {maxGoodStreakList[i]} preguntas");
                GameEvents.OnGoodStreaked?.Invoke();
                OnMaxGoodStreakReached?.Invoke(maxGoodStreakList[i]);
                achievementData.StreakCounter(maxGoodStreakList[i], SetDateAchievement(), achievementOrigin);//a�ado al contador de rachas de achivement data a traves del metodo StreakCounter

                //_hasReachedMaxGoodStreak = true;
            }

        }
        /*
        if (counter >= maxGoodStreakList[0] && !_hasReachedMaxGoodStreak)//si counter alcanzo o supero maxGoodStreak
        {
            Debug.Log("Alcanzo Racha");
            GameEvents.OnGoodStreaked?.Invoke();
            OnMaxGoodStreakReached?.Invoke(maxGoodStreakList[0]);
            _hasReachedMaxGoodStreak = true;
            
        }*/

    }
    private string SetDateAchievement()
    {
        DateTime lastAchievementDate = DateTime.Now;
        return lastAchievementDate.ToString();
    }

    public void CheckGoodWithoutErrors()
    {
        GameEvents.OnGoodWithoutErrors?.Invoke();
        OnMaxGoodWithoutErrorsReached?.Invoke();
    }

 

}
