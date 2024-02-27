using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using static DataUserAll;
using static UnityEditor.Progress;
using static UnityEngine.UIElements.UxmlAttributeDescription;

public class RankingController : MonoBehaviour
{
    [SerializeField] private DataUserAll dataUserAll;
    [SerializeField] private UnityEvent<DataUsers> OnFirstPlaceShowedEvent;
    [SerializeField] private UnityEvent<DataUsers> OnSecondPlaceShowedEvent;
    [SerializeField] private UnityEvent<DataUsers> OnThirdPlaceShowedEvent;

    [ContextMenu(nameof(ShowTopThree))]
    public void ShowTopThree()
    {
        List<DataUserAll.DataUsers> users = dataUserAll.Users.OrderByDescending(user => user.totalExperience).ToList();

        int counter = 0;
        foreach (var item in users)
        {
            Debug.Log($"{item.userName} - {item.totalExperience} - {counter}");
            switch (counter)
            {
                case 0:
                    ShowFirstPlace(item);
                    break;
                case 1:
                    ShowSecondPlace(item);
                    break;
                case 2:
                    ShowThirdPlace(item);
                    break;
            }
            counter++;
        }
        counter = 0;
    }

    private void ShowFirstPlace(DataUsers dataUser) 
    {
        OnFirstPlaceShowedEvent?.Invoke(dataUser);
    }

    private void ShowSecondPlace(DataUsers dataUser)
    {
        OnSecondPlaceShowedEvent?.Invoke(dataUser);
    }

    private void ShowThirdPlace(DataUsers dataUser)
    {
        OnThirdPlaceShowedEvent.Invoke(dataUser);
    }
}
