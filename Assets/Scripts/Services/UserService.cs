using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class UserService : MonoBehaviour
{
    [SerializeField] private ScriptableObjectUser _scriptableObjectUser;

    private readonly string _urlToUpdate =
        "http://simuladorpmp-servicio.bsginstitute.com/api/ConfiguracionSimulador/ActualizarCaracteristicasGamificacion";

    private readonly string _urlToGetUser = "https://api-portalweb.bsginstitute.com/api/AspNetUser/authenticate";

    private readonly string _urlToGetUserDetail =
        "http://simuladorpmp-servicio.bsginstitute.com/api/ConfiguracionSimulador/ObtenerCaracteristicasGamificacion/";

    private bool _haveError;
    private bool _finishRequest;

    private void OnEnable()
    {
        GameEvents.RequestCoinsChange += GameEvents_RequestCoinsChange;
        GameEvents.RequestExperienceChange += GameEvents_RequestExperienceChange;
        GameEvents.RequestUpdateDetail += GameEvents_RequestUpdateDetail;
    }

    private void OnDisable()
    {
        GameEvents.RequestCoinsChange -= GameEvents_RequestCoinsChange;
        GameEvents.RequestExperienceChange -= GameEvents_RequestExperienceChange;
        GameEvents.RequestUpdateDetail -= GameEvents_RequestUpdateDetail;    }

    private void GameEvents_RequestUpdateDetail()
    {
        StartCoroutine(UpdateUserDetail());
    }

    private void GameEvents_RequestExperienceChange(float obj)
    {
    }

    private void GameEvents_RequestCoinsChange(float obj)
    {
    }

    public void GetUserDetail(int userId)
    {
        StartCoroutine(IGetUserDetail(userId));
    }

    public IEnumerator IGetUserDetail(int userId)
    {
        _finishRequest = _haveError = false;
        using (UnityWebRequest request = new UnityWebRequest(_urlToGetUserDetail + userId, "GET"))
        {
            request.downloadHandler = new DownloadHandlerBuffer();
            AddHeader(request);

            yield return request.SendWebRequest();

            if (request.responseCode >= 400)
            {
                _scriptableObjectUser.userInfo.haveUser = false;
                Debug.Log(request.error);
            }
            else
            {
                try
                {
                    var detail = JsonUtility.FromJson<UserDetail>(request.downloadHandler.text);
                    _scriptableObjectUser.userInfo.user.detail = detail;
                    if (_scriptableObjectUser.userInfo.user.excepcion.excepcionGenerada)
                    {
                        _scriptableObjectUser.userInfo.haveUser = false;
                    }
                    else
                    {
                        if (_scriptableObjectUser.userInfo.user.detail.usernameG != "UserName")
                        {
                            _scriptableObjectUser.userInfo.haveUsername = true;
                        }
                        else
                        {
                            _scriptableObjectUser.userInfo.haveUsername = false;
                        }

                        if (_scriptableObjectUser.userInfo.user.detail.idCaracteristicaGamificacion != 0)
                        {
                            Debug.Log("idcaracteristicaGamificacion 1");
                        }
                    }

                    GameEvents.SuccessGetUserDetail?.Invoke();
                }
                catch (Exception e)
                {
                    Debug.Log(e.Message);
                    _scriptableObjectUser.userInfo.haveUser = false;
                }
            }

            _finishRequest = true;
        }
    }

    public IEnumerator UpdateUserDetail()
    {
        using (UnityWebRequest request = new UnityWebRequest(_urlToUpdate, "POST"))
        {
            UserDetail dataLogin = _scriptableObjectUser.userInfo.user.detail;

            var bodyRaw = Encoding.UTF8.GetBytes(JsonUtility.ToJson(dataLogin));
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();

            AddHeader(request);
            yield return request.SendWebRequest();
            if (request.responseCode >= 400)
            {
                // _buttonChangeUsername.interactable = true;
                Debug.Log(request.error);
            }
            else
            {
                try
                {
                    bool detail = Convert.ToBoolean(request.downloadHandler.text);
                    if (detail)
                    {
                        GameEvents.DetailChanged?.Invoke();
                        Debug.Log(true);
                    }
                    else
                    {
                        Debug.Log(false);
                        // _buttonChangeUsername.interactable = true;
                    }
                }
                catch (Exception e)
                {
                    Debug.Log(request.downloadHandler.text);
                    // _scriptableObjectUser.userInfo.haveUser = false;
                    // _buttonChangeUsername.interactable = true;
                }
            }
        }
    }

    public void GetUSer(string username, string password)
    {
        StartCoroutine(GetUser(username, password));
    }

    public IEnumerator GetUser(string username, string password)
    {
        _finishRequest = _haveError = false;
        using (UnityWebRequest request = new UnityWebRequest(_urlToGetUser, "POST"))
        {
            DataLogin dataLogin = new DataLogin() { username = username, password = password };

            var bodyRaw = Encoding.UTF8.GetBytes(JsonUtility.ToJson(dataLogin));
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();

            AddHeader(request);
            yield return request.SendWebRequest();
            if (request.responseCode == 401)
            {
                _scriptableObjectUser.userInfo.user = JsonUtility.FromJson<User>(request.downloadHandler.text);
                _scriptableObjectUser.userInfo.haveUser = false;
                GameEvents.ErrorGetUser?.Invoke();
            }
            else if (request.responseCode >= 400)
            {
                _scriptableObjectUser.userInfo.haveUser = false;
                GameEvents.ErrorGetUser?.Invoke();
                Debug.Log(request.error);
            }
            else
            {
                try
                {
                    _scriptableObjectUser.userInfo.user = JsonUtility.FromJson<User>(request.downloadHandler.text);
                    if (_scriptableObjectUser.userInfo.user.excepcion.excepcionGenerada)
                    {
                        _scriptableObjectUser.userInfo.haveUser = false;
                        GameEvents.ErrorGetUser?.Invoke();
                    }
                    else
                    {
                        _scriptableObjectUser.userInfo.haveUser = true;
                        GameEvents.SuccessGetUser?.Invoke();
                    }
                }
                catch (Exception e)
                {
                    Debug.Log(e.Message);
                    _scriptableObjectUser.userInfo.haveUser = false;
                    GameEvents.ErrorGetUser?.Invoke();
                }
            }

            _finishRequest = true;
        }
    }

    private void AddHeader(UnityWebRequest request)
    {
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Accept", "application/json");
        request.SetRequestHeader("User-Agent",
            "Mozilla/5.0 (Windows NT 6.1; Unity 3D; ZFBrowser 3.1.0; UnityTests 1.0) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.157 Safari/537.36");
    }
}