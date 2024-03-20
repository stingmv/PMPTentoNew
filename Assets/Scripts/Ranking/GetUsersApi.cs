using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

[Serializable]
public class DataToRequireRanking
{
    public int MinExperience;
    public int MaxExperience;
}
public class GetUsersApi : MonoBehaviour
{
    [SerializeField] private string URL = "http://simuladorpmp-servicio.bsginstitute.com/api/Gamificacion/ObtenerRankingGamificacion";
    [SerializeField] private DataUserAll dataUserAll;
    [SerializeField] private UserService _userService;

    private void Start()
    {
        StartCoroutine(GetData(0, 20000));
    }

    private IEnumerator GetData(int minExperience, int maxExperience)
    {
        GameEvents.RequestRanking?.Invoke();
        using (UnityWebRequest request = new UnityWebRequest(URL, "POST"))
        {
            DataToRequireRanking dataLogin = new DataToRequireRanking() { MinExperience = minExperience, MaxExperience = maxExperience};
            var bodyRaw = Encoding.UTF8.GetBytes(JsonUtility.ToJson(dataLogin));
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();

            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Accept", "application/json");
            request.SetRequestHeader("User-Agent", "Mozilla/5.0 (Windows NT 6.1; Unity 3D; ZFBrowser 3.1.0; UnityTests 1.0) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.157 Safari/537.36");

            
            yield return request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.LogError(request.error);
            }
            else
            {
                string json = request.downloadHandler.text;
                Debug.Log(json);
                // dataUserAll.Users = JsonUtility.FromJson<DataUserAll.DataUsers>(json).Users;
                SimpleJSON.JSONNode stats = SimpleJSON.JSON.Parse(json);
                SaveDataAllUsers(stats);
            }
        }
    }

    private void SaveDataAllUsers(SimpleJSON.JSONNode stats) {
        for (int i = 0; i < stats.Count; i++) {
            DataUserAll.DataUsers user = new DataUserAll.DataUsers();
            user.id = stats[i]["idAlumno"];
            user.userName = stats[i]["usernameG"];
            user.totalExperience = stats[i]["totalExperience"];
            dataUserAll.Users.Add(user);
        }
        GameEvents.RankingRetrieved?.Invoke();
    }
}
