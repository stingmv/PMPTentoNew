using TMPro;
using UnityEngine;

public class AchievementUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI counterText;
    [SerializeField] private UnityEngine.UI.Button getGiftButton;

    private AchievementData.Achievement _achievement;

    public void SetValues(AchievementData.Achievement achievement)
    {
        _achievement = achievement;
        nameText.SetText(achievement.Name);
        counterText.SetText($"{achievement.CurrentCounter}/{achievement.MaxCounter}");
        CheckGiftsObtained();
    }

    private void CheckGiftsObtained()
    {
        if (_achievement.GiftsObtained <= 0)
            getGiftButton.gameObject.SetActive(false);
        else
            getGiftButton.gameObject.SetActive(true);
    }

    public void SetGiftObtained()
    {
        GameEvents.OnSetGiftsFromAchievement?.Invoke(_achievement, getGiftButton);
    }
    
}
