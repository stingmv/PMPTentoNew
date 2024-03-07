using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AchievementUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI counterText;
    [SerializeField] private UnityEngine.UI.Button getGiftButton;
    [SerializeField] private Slider counterSlider;
    [SerializeField] private Image[] startLevelImages;
    [SerializeField] private Color starNormalColor;
    [SerializeField] private Color starFillColor;

    private AchievementData.Achievement _achievement;

    public void SetValues(AchievementData.Achievement achievement)
    {
        _achievement = achievement;
        nameText.SetText(achievement.Name);
        counterText.SetText($"{achievement.CurrentCounter}/{achievement.MaxCounter}");
        SetSliderValues();
        FillStarImages();
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

    private void SetSliderValues()
    {
        counterSlider.maxValue = _achievement.MaxCounter;
        counterSlider.value = _achievement.CurrentCounter;
    }

    private void FillStarImages()
    {
        var currentLvl = _achievement.CurrentLevel;

        foreach (var item in startLevelImages)
        {
            item.color = starNormalColor;
        }

        for (int i = 0; i < currentLvl; i++)
        {
            startLevelImages[i].color = starFillColor;
        }
    }
}
