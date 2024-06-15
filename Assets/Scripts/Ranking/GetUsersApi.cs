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
        Novato,
        Experto,
        Master,
        Leyenda
    }   

    public void GetRankingInformation()//este metodo se llama al presionar en seccion ranking
    {
        
        var tExperience = _user.userInfo.user.detail.totalExperience;//obtenemos experiencia y colocamos en variable
        if (tExperience < 0)
        {
            return;
        }

        switch (tExperience)//Setear titulo de categoria(nivel) 
        {
            case <= 4500:
                GetNovato();
                _categoryTitle.text = "Categoría:\nNovato";
                break;
            case <= 9500:
                GetExperto();
                _categoryTitle.text = "Categoría:\nExperto";
                break;
            case <= 15000:
                GetMaster();
                _categoryTitle.text = "Categoría:\nMaster";
                break;
            default:
                GetLeyenda();
                _categoryTitle.text = "Categoría:\nLeyenda";
                break;           
        }
        GetGlobal();//para que en principio se ejecute el filtro global
    }
    private void Start()
    {
        // Debug.Log(_user.userInfo.user.detail.totalExperience);
        //GetNovato();
    }

    public void GetNovato()//filtro de experiencia en rango de novato
    {
        StopAllCoroutines();
        StartCoroutine(GetData(0, 4500));//con metodo GetData hago la solicitud al endpoint con los parametros de experiencia minima 0 y maxima 2000
    }
    public void GetExperto()
    {
        StopAllCoroutines();
        StartCoroutine(GetData(4501, 9500));
    }
    public void GetMaster()
    {
        StopAllCoroutines();
        StartCoroutine(GetData(9501, 15000));
    }
    public void GetLeyenda()
    {
        StopAllCoroutines();
        StartCoroutine(GetData(15001, 100000));
    }
    public void GetGlobal()
    {
        StopAllCoroutines();
        StartCoroutine(GetData(0, 100000));
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
