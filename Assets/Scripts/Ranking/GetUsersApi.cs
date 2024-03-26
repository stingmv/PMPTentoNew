using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
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
    [SerializeField] private ScriptableObjectUser _user;
    [SerializeField] private TextMeshProUGUI _categoryTitle;
    public enum Categories
    {
        Aficionado,
        Aprendiz,
        Especialista,
        Maestro,
        Experto,
        Elite
    }   

    public void GetRankingInformation()
    {
        var tExperience = _user.userInfo.user.detail.totalExperience;
        if (tExperience < 0)
        {
            return;
        }

        switch (tExperience)
        {
            case <= 2000:
                GetAficionado();
                _categoryTitle.text = "Categoría:\nAficionado";
                break;
            case <= 4500:
                GetAprendiz();
                _categoryTitle.text = "Categoría:\nAprendiz";
                break;
            case <= 7500:
                GetEspecialista();
                _categoryTitle.text = "Categoría:\nEspecialista";
                break;
            case <= 11000:
                GetMaestro();
                _categoryTitle.text = "Categoría:\nMaestro";
                break;
            case <= 15000:
                GetExperto();
                _categoryTitle.text = "Categoría:\nExperto";
                break;
            default:
                GetElite();
                _categoryTitle.text = "Categoría:\nÉlite";
                break;
        }
    }
    private void Start()
    {
        // Debug.Log(_user.userInfo.user.detail.totalExperience);
        // GetAficionado();
    }

    public void GetAficionado()
    {
        StopAllCoroutines();
        StartCoroutine(GetData(0, 2000));
    }
    public void GetAprendiz()
    {
        StopAllCoroutines();
        StartCoroutine(GetData(2001, 4500));
    }
    public void GetEspecialista()
    {
        StopAllCoroutines();
        StartCoroutine(GetData(4501, 7500));
    }
    public void GetMaestro()
    {
        StopAllCoroutines();
        StartCoroutine(GetData(7501, 11000));
    }
    public void GetExperto()
    {
        StopAllCoroutines();
        StartCoroutine(GetData(1101, 15000));
    }
    public void GetElite()
    {
        StopAllCoroutines();
        StartCoroutine(GetData(15001, 100000));
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
        dataUserAll.Users.Clear	();
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
