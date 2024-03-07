using UnityEngine;
using UnityEngine.Events;

public class AchievementControlller : MonoBehaviour
{
    [SerializeField] private AchievementData achievementData;
    [SerializeField] private int maxGoodStreak;
    [SerializeField] private UnityEvent<int> OnMaxGoodStreakReached;
    [SerializeField] private UnityEvent OnMaxGoodWithoutErrorsReached;

    private bool _hasReachedMaxGoodStreak;

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

    private void GoodStreak() 
    {
        achievementData.AddCounter(0);
    }

    private void GoodWithoutErrors()
    {
        achievementData.AddCounter(1);
    }

    public void CheckMaxGoodStreak(int counter)
    {
        if (counter >= maxGoodStreak && !_hasReachedMaxGoodStreak)
        {
            GameEvents.OnGoodStreaked?.Invoke();
            OnMaxGoodStreakReached?.Invoke(maxGoodStreak);
            _hasReachedMaxGoodStreak = true;
        }
    }

    public void CheckGoodWithoutErrors() 
    {
        GameEvents.OnGoodWithoutErrors?.Invoke();
        OnMaxGoodWithoutErrorsReached?.Invoke();
    }
}
