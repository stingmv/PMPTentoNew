using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using static DataUserAll;

public class RankingController : MonoBehaviour
{
    [SerializeField] private DataUserAll dataUserAll;
    [SerializeField] private List<PodiumItem> _podio;
    [SerializeField] private Transform _rankingContainer;
    [SerializeField] private PodiumItem _rankingItemPrefab;

    private void OnEnable()
    {
        GameEvents.RankingRetrieved += GameEvents_RankingRetrieved;
    }

    private void OnDisable()
    {
        GameEvents.RankingRetrieved -= GameEvents_RankingRetrieved;
    }

    private void GameEvents_RankingRetrieved()
    {
        DataUsers infoUsers;
        if (dataUserAll.Users.Count > 0)
        {
             infoUsers = dataUserAll.Users[0];
            _podio[0].SetData(infoUsers.userName, infoUsers.totalExperience.ToString(), infoUsers.id);
            if (dataUserAll.Users.Count > 1)
            {
                Debug.Log	("mas de 1");
                infoUsers = dataUserAll.Users[1];
                _podio[1].SetData(infoUsers.userName, infoUsers.totalExperience.ToString(), infoUsers.id);
                if (dataUserAll.Users.Count > 2)
                {
                    infoUsers = dataUserAll.Users[2];
                    _podio[2].SetData(infoUsers.userName, infoUsers.totalExperience.ToString(), infoUsers.id);
                }
                else
                {
                    _podio[2].SetData("", "-", -1);
                }
            }
            else
            {
                _podio[1].SetData("", "-", -1);
                _podio[2].SetData("", "-", -1);    
            }
        }
        else
        {
            _podio[0].SetData("", "-", -1);
            _podio[1].SetData("", "-", -1);
            _podio[2].SetData("", "-", -1);    
        }
        foreach (Transform child in _rankingContainer)
        {
            Destroy(child.gameObject);
        }
        for (int i = 3; i < dataUserAll.Users.Count; i++)
        {
            var item = Instantiate(_rankingItemPrefab, _rankingContainer);
            infoUsers = dataUserAll.Users[i];

            item.SetData(i.ToString(), infoUsers.userName, infoUsers.totalExperience.ToString(), infoUsers.id);
        }
    }
}
