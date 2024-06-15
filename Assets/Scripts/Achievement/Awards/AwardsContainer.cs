using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class AwardsContainer : MonoBehaviour
{
    [SerializeField] GameObject awardImage;
    [SerializeField] TextMeshProUGUI streaksText;
    [SerializeField] TextMeshProUGUI dateText;
    [SerializeField] GameObject awardContent;
    [SerializeField] int typeOfStreak;
    [SerializeField] AchievementData achievementData;

    public void SetValuesAwards()
    {
        ClearAwards();
        FillAwards();   

    }
    
    private void FillAwards()
    {

        switch (typeOfStreak)
        {
            case 4:
                for (int i = 0; i < achievementData.achievementListContainer.achievementList[0].Streak4Questions; i++)
                {
                    Instantiate(awardImage, awardContent.transform);
                    streaksText.text = $"Tienes {achievementData.achievementListContainer.achievementList[0].Streak4Questions} rachas de {typeOfStreak} estrellas";
                    dateText.text = $"<b>Ultima racha</b>: {achievementData.achievementListContainer.achievementList[0].Streak4Date} - {achievementData.achievementListContainer.achievementList[0].lastOriginStreak4}";

                }
                break;
            case 6:
                for (int i = 0; i < achievementData.achievementListContainer.achievementList[0].Streak6Questions; i++)
                {
                    Instantiate(awardImage, awardContent.transform);
                    streaksText.text = $"Tienes {achievementData.achievementListContainer.achievementList[0].Streak6Questions} rachas de {typeOfStreak} estrellas";
                    dateText.text = $"<b>Ultima racha</b>: {achievementData.achievementListContainer.achievementList[0].Streak6Date} - {achievementData.achievementListContainer.achievementList[0].lastOriginStreak6}";
                }
                break;
            case 8:
                for (int i = 0; i < achievementData.achievementListContainer.achievementList[0].Streak8Questions; i++)
                {
                    Instantiate(awardImage, awardContent.transform);
                    streaksText.text = $"Tienes {achievementData.achievementListContainer.achievementList[0].Streak8Questions} rachas de {typeOfStreak} estrellas";
                    dateText.text = $"<b>Ultima racha</b>: {achievementData.achievementListContainer.achievementList[0].Streak8Date} - {achievementData.achievementListContainer.achievementList[0].lastOriginStreak8}";

                }
                break;
            case 10:
                for (int i = 0; i < achievementData.achievementListContainer.achievementList[0].Streak10Questions; i++)
                {
                    Instantiate(awardImage, awardContent.transform);
                    streaksText.text = $"Tienes {achievementData.achievementListContainer.achievementList[0].Streak10Questions} rachas de {typeOfStreak} estrellas";
                    dateText.text = $"<b>Ultima racha</b>: {achievementData.achievementListContainer.achievementList[0].Streak10Date} - {achievementData.achievementListContainer.achievementList[0].lastOriginStreak10}";

                }
                break;
        }
    }

    private void ClearAwards()
    {
        for (int i = awardContent.transform.childCount-1; i >= 0; i--)
        {
            Destroy(awardContent.transform.GetChild(i).gameObject);
        }
    }
}
