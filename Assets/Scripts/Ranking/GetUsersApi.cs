using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GetUsersApi : MonoBehaviour
{
    [SerializeField] private string URL = "https://9944b841-dde7-4337-abe7-8211f9549960.mock.pstmn.io/msgetusersmpmpg";
    [SerializeField] private DataUserAll dataUserAll;

    private void Start()
    {
        StartCoroutine(GetData());
    }

    private IEnumerator GetData()
    {
        using (UnityWebRequest request = UnityWebRequest.Get(URL))
        {
            yield return request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.LogError(request.error);
            }
            else
            {
                string json = request.downloadHandler.text;
                
                SimpleJSON.JSONNode stats = SimpleJSON.JSON.Parse(json);
                SaveDataAllUsers(stats);
            }
        }
    }

    private void SaveDataAllUsers(SimpleJSON.JSONNode stats) {
        for (int i = 0; i < stats.Count; i++) {
            DataUserAll.DataUsers user = new DataUserAll.DataUsers();
            user.id = stats[i]["id"];
            user.userName = stats[i]["userName"];
            user.totalExperience = stats[i]["totalExperience"];
            user.email = stats[i]["email"];
            user.password = stats[i]["password"];
            dataUserAll.Users.Add(user);
        }
    }
}
