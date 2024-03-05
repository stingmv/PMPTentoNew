using UnityEngine;
using UnityEngine.Events;

public class AchievementControlller : MonoBehaviour
{
    [SerializeField] private AchievementData achievementData;

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

    [ContextMenu(nameof(TestGoodStreak))]
    public void TestGoodStreak()
    {
        GameEvents.OnGoodStreaked?.Invoke();
    }
}
