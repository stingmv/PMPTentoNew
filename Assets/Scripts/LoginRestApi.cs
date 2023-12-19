using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class LoginRestApi : MonoBehaviour
{
    [SerializeField] private string url = "https://api-portalweb.bsginstitute.com/api/AspNetUser/authenticate";
    [SerializeField] private string username = "pruebaxjrivera@bsginstitute.net";
    [SerializeField] private string password = "BSgrupo123";

    private IEnumerator PostLoginRoutine()
    {
       using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
       {
            DataLogin dataLogin = new DataLogin() { username = username, password = password};
            
            var bodyRaw = Encoding.UTF8.GetBytes(JsonUtility.ToJson(dataLogin));
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            
            request.SetRequestHeader("Content-Type", "application/json");
            
            yield return request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log(request.error);
            }
            else
            {
                PostResult deserializedPostData = JsonUtility.FromJson<PostResult>(request.downloadHandler.text);
                Debug.Log(deserializedPostData);
            }
       }
    }

    [ContextMenu(nameof(PostLogin))]
    public void PostLogin()
    {
        StartCoroutine(PostLoginRoutine());
    }
}

[Serializable]
public class DataLogin
{
    public string username;
    public string password;
}

[Serializable]
public class PostResult
{
    public string id;
    public int idProveedor;
    public int cursos;
    public int idAlumno;
    public int idContacto;
    public string clave;
    public string userName;
    public string token;
    public bool tipoCarrera;
    public string correoEnc;
    public string telEnc;
    public string userAgent;
    public string userIp;
    public PostResultException excepcion;
}

[Serializable]
public class PostResultException
{
    public bool excepcionGenerada;
    public string descripcionGeneral;
    public string error;
}
