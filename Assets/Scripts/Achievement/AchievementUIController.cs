using TMPro;
using UnityEngine;

public class AchievementUIController : MonoBehaviour
{
    [SerializeField] private AchievementData achievementData;
    [SerializeField] private GameObject achievementPrefab;
    [SerializeField] private Transform contentAchievements;

    [SerializeField] private GameObject popupContainer;
    [SerializeField] private TextMeshProUGUI popUpGift1Text;
    [SerializeField] private TextMeshProUGUI popUpGift2Text;

    private AchievementData.Achievement _achievement;
    private UnityEngine.UI.Button _button;

    private void OnEnable()
    {
        GameEvents.OnSetGiftsFromAchievement += SetGiftsObtained;
    }

    private void OnDisable()
    {
        GameEvents.OnSetGiftsFromAchievement -= SetGiftsObtained;
    }

    public void SetAllAchievements()
    {
        ClearChildrenAchievement();

        var list = achievementData.achievementListContainer.achievementList;
        for (int i = 0; i < list.Count; i++)
        {
            var item = Instantiate(achievementPrefab, contentAchievements);
            item.GetComponent<AchievementUI>().SetValues(list[i]);
        }
    }

    public void ClearChildrenAchievement()
    {
        for (int i = contentAchievements.childCount - 1; i >= 0; i--)
        {
            Destroy(contentAchievements.GetChild(i).gameObject);
        }
    }

    public void GetGifts()
    {
        achievementData.RemoveGiftsObtained(_achievement);
        _button.gameObject.SetActive(false);
    }

    public void SetGiftsObtained(AchievementData.Achievement achievement, UnityEngine.UI.Button button)
    {
        _achievement = achievement;
        _button = button;

        popupContainer.SetActive(true);
        popupContainer.GetComponent<FadeUI>().FadeInTransition();

        int totalGift1 = achievement.GiftsObtained * achievement.GiftData[0].Amount;
        int totalGift2 = achievement.GiftsObtained * achievement.GiftData[1].Amount;

        popUpGift1Text.SetText(totalGift1.ToString());
        popUpGift2Text.SetText(totalGift2.ToString());
    }
}
