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
        var info = dataUserAll.Users[0];
        _podio[0].SetData(info.userName, info.totalExperience.ToString(), info.id);
        // info = dataUserAll.Users[1];
        _podio[1].SetData("", "-", -1);
        // info = dataUserAll.Users[2];
        _podio[2].SetData("", "-", -1);
        foreach (Transform child in _rankingContainer)
        {
            Destroy(child.gameObject);
        }
        for (int i = 3; i < dataUserAll.Users.Count; i++)
        {
            var item = Instantiate(_rankingItemPrefab, _rankingContainer);
            info = dataUserAll.Users[i];

            item.SetData(i.ToString(), info.userName, info.totalExperience.ToString(), info.id);
        }
    }
}
