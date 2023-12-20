using System;
using System.Collections;
using System.Text;
using Login;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

public class LoginRestApi : MonoBehaviour
{
    [SerializeField] private string url = "https://api-portalweb.bsginstitute.com/api/AspNetUser/authenticate";

    [SerializeField] private ScriptableObjectUser _objectUser;
    [SerializeField] private LoginController _loginController;
    // [SerializeField] private string username = "pruebaxjrivera@bsginstitute.net";
    // [SerializeField] private string password = "BSgrupo123";

    public bool _finishRequest;
    public bool _haveError;
    public IEnumerator PostLoginRoutine(string username, string password)
    {
        _finishRequest = _haveError = false;
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
                _objectUser.userInfo.haveUser = false;
                _loginController._onErrorInLogin?.Invoke("Fallo de comunicación con el servidor, intentelo denuevo");

            }
            else
            {
                _objectUser.userInfo.user = JsonUtility.FromJson<User>(request.downloadHandler.text);
                if (_objectUser.userInfo.user.excepcion.excepcionGenerada)
                {
                    _objectUser.userInfo.haveUser = false;
                    _loginController._onFailedLogin?.Invoke(_objectUser.userInfo.user.excepcion.descripcionGeneral);
                }
                else
                {
                    _objectUser.userInfo.haveUser = true;
                    _loginController._onSuccessLogin?.Invoke();
                }
            }
            PlayerPrefs.SetString("userInfo", JsonUtility.ToJson(_objectUser.userInfo));
            _finishRequest = true;
       }
    }

    
    public void PostLogin(string username, string password)
    {
        StartCoroutine(PostLoginRoutine(username, password));
    }
    [ContextMenu(nameof(PostLogin))]
    public void PostLogin()
    {
        StartCoroutine(PostLoginRoutine("pruebaxjrivera@bsginstitute.net", "BSgrupo123"));
    }
}

[Serializable]
public class DataLogin
{
    public string username;
    public string password;
}