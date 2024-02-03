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
            request.SetRequestHeader("Accept", "application/json");
            request.SetRequestHeader("User-Agent", "Mozilla/5.0 (Windows NT 6.1; Unity 3D; ZFBrowser 3.1.0; UnityTests 1.0) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.157 Safari/537.36");
            
            yield return request.SendWebRequest();
            if (request.responseCode == 401)
            {
                _objectUser.userInfo.user = JsonUtility.FromJson<User>(request.downloadHandler.text);

                _objectUser.userInfo.haveUser = false;
                GameEvents.ErrorLogin?.Invoke(_objectUser.userInfo.user.excepcion.descripcionGeneral);
            }
            else if (request.responseCode >= 400)
            {
                _objectUser.userInfo.haveUser = false;
                _loginController._onErrorInLogin?.Invoke("Fallo de comunicación con el servidor, intentelo denuevo");
                Debug.Log(request.error);
            }
            else
            {
                try
                {
                    _objectUser.userInfo.user = JsonUtility.FromJson<User>(request.downloadHandler.text);
                    if (_objectUser.userInfo.user.excepcion.excepcionGenerada)
                    {
                        _objectUser.userInfo.haveUser = false;
                        GameEvents.FailedLogin?.Invoke(_objectUser.userInfo.user.excepcion.descripcionGeneral);
                        // _loginController._onFailedLogin?.Invoke(_objectUser.userInfo.user.excepcion.descripcionGeneral);
                    }
                    else
                    {
                        _objectUser.userInfo.haveUser = true;
                        GameEvents.SuccessfulLogin?.Invoke(_objectUser.userInfo.user);
                        GameEvents.GetUserExam?.Invoke(_objectUser.userInfo.user.userName);
                        GameEvents.GetNameExam?.Invoke(_objectUser.userInfo.user.idAlumno.ToString());
                        // _loginController._onSuccessLogin?.Invoke();
                    }
                }
                catch (Exception e)
                {
                    Debug.Log(request.downloadHandler.text);
                    _objectUser.userInfo.haveUser = false;
                    _loginController._onErrorInLogin?.Invoke("Se tuvo un error interno, vuelva a intentarlo mas tarde");
                }
                
            }
            // PlayerPrefs.SetString("userInfo", JsonUtility.ToJson(_objectUser.userInfo));
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